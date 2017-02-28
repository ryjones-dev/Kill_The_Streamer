using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public struct EnemyNetworkInfo
{
    public string name;
    public EnemyType type;
    public Direction direction;
    public Vector3 position;
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left,
    Random
}

public class NetworkManager : MonoBehaviour {

    public static NetworkManager s_manager;
    public static string s_oauth; //Oauth of the bot listening to the channel
    public static string s_username; //Username of the bot listening to the channel
    public static string s_channel; //Channel being listened to.
	public bool DISABLE; //For debugging purposes, allows you to disable the network manager.

    public Socket m_socket;
    public bool m_connected;
    public Thread m_thread;

    public float m_timeToPing;
    public bool m_waitingForPong;

    public GameObject m_reconnectingTextObj;
    private Text m_reconnectingText;

    private bool ConnectToServer()
    {

        Debug.Log("Connnecting...");
        // Connects to twitch
        IPHostEntry ipHostInfo = Dns.GetHostEntry("irc.twitch.tv");
        Debug.Log(ipHostInfo.AddressList[0].ToString());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6667);

        m_socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        try
        {
            m_socket.Connect(remoteEP);
        }
        catch(SocketException e)
        {
            Debug.Log("Couldn't Connect...");
            Debug.Log("Error Code: " + e.ErrorCode);
            return false;
        }

        Debug.Log("Connected");
        m_connected = true;
        m_reconnectingText.enabled = false;
        Time.timeScale = 1.0f;
        m_timeToPing = 5.0f;
        m_waitingForPong = false;

        //Handshake to authenticate.
        SendData("PASS " + s_oauth);
        SendData("NICK " + s_username);
        SendData("USER " + s_username + " 8 * :TwitchLink Client");

        //Connect to the channel
        SendData("JOIN #" + s_channel);
        m_thread = new Thread(new ThreadStart(Listener));
        m_thread.Start();

        return true;
    }

    private void SendData(string output)
    {
        if (m_connected)
        {
            byte[] msg = Encoding.ASCII.GetBytes(output + "\n");
            m_socket.Send(msg);
        }
        else
        {
            Debug.Log("Error: Attempt to send data but not connected.");
        }
    }

    public void MessageChannel(string message)
    {
        SendData("PRIVMSG #" + s_channel + " :" + message);
    }

    public static void Listener()
    {
        byte[] bytes = new byte[4096];
        int bytesRec;
        string output;
        int endOfName;
        string name;
        string message;
        int nameLength;
        int inputLength;
        int channelNameLength = s_channel.Length;
        
        while (s_manager.m_connected)
        {
            bytesRec = s_manager.m_socket.Receive(bytes);
            output = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            s_manager.m_timeToPing = 5.0f;
            s_manager.m_waitingForPong = false;
            // Handle PING case
            if (output.StartsWith("PING")){
                s_manager.SendData("PONG :tmi.twitch.tv");
            }
            else
            {
                endOfName = output.IndexOf('!');
                if (endOfName != -1)
                {
                    name = output.Substring(1, endOfName - 1);
                    nameLength = name.Length;
                    inputLength = 29 + 3 * nameLength + channelNameLength;
                    if (inputLength < output.Length)
                    {
                        message = output.Substring(inputLength);
                        Debug.Log(name + ": " + message);
                    
                        string[] parts = message.Split(' ');
                        EnemyType enemyType;
                        Direction direction;

                        switch (parts[0].ToLower().Trim())
                        {
                            case "kappa":
                            case "frankerz":
                            case "mrdestructoid":
                            case "pjsalt":
                            case "theilluminati":
                            case "pogchamp":
                            case "smorc":
                                enemyType = EnemyType.BooEnemy;
                                break;

                                //One part commands
                            case "!start9":
                                Debug.Log("IMPLEMENT START9");
                                continue;
                            default:
                                continue;
                        }
                        if (parts.Length == 1)
                        {
                            direction = Direction.Random;
                        }
                        else
                        {
                            switch (parts[1].ToLower().Trim())
                            {
                                case "up":
                                    direction = Direction.Up;
                                    break;
                                case "right":
                                    direction = Direction.Right;
                                    break;
                                case "down":
                                    direction = Direction.Down;
                                    break;
                                case "left":
                                    direction = Direction.Left;
                                    break;
                                default:
                                    direction = Direction.Random;
                                    break;
                            }
                        }
                        EnemyNetworkInfo info = new EnemyNetworkInfo();

                        info.name = name;
                        info.type = enemyType;
                        info.direction = direction;

                        info.position = new Vector3(0, 0, -3);

                        EnemyManager.s_enemyQueueMut.WaitOne();
                        EnemyManager.s_enemyQueue.Enqueue(info);
                        EnemyManager.s_enemyQueueMut.ReleaseMutex();
                    }
                }
            }
        }
    }

    public void Disconnect()
    {
        m_socket.Shutdown(SocketShutdown.Both);
        m_socket.Close();
		Debug.Log ("Disconnected.");
        m_connected = false;
    }

    // Use this for initialization
    void Start () {
		if (!DISABLE) {
            m_reconnectingText = m_reconnectingTextObj.GetComponent<Text>();


			try {
				string[] config;
				config = System.IO.File.ReadAllLines ("config.txt");
				s_oauth = "oauth:" + config [0].Trim (' ').Remove (0, 7);//"oauth: " - 7 characters
				s_username = config [1].Trim (' ').Remove (0, 9).ToLower ();//"botname: " - 9 characters
				s_channel = config [2].Trim (' ').Remove (0, 9).ToLower ();//"channel: " - 9 characters
	        
				if (!s_manager) {
					s_manager = this;
				}

				ConnectToServer ();
	        
			} catch (Exception e) {
				Debug.Log ("Error: " + e);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (m_connected)
        {
            m_timeToPing -= Time.deltaTime;
            if (m_timeToPing <= 0.0f)
            {
                if (m_waitingForPong)
                {
                    if (m_timeToPing <= -1.0f)
                    {

                        if (m_thread.IsAlive)
                        {
                            m_thread.Abort();
                            Debug.Log("Thread Aborted");
                        }
                        if (m_connected)
                        {
                            Disconnect();
                        }

                        Debug.Log("CONNECTION LOST, PING TIMED OUT");

                        Time.timeScale = 0;
                        m_reconnectingText.enabled = true;
                    }
                }
                else
                {
                    Debug.Log("Checking if active...");
                    m_waitingForPong = true;
                    SendData("PING :tmi.twitch.tv");
                }
            }
        }
        else
        {
            ConnectToServer();
        }

	}

    void OnApplicationQuit()
    {
		if (m_thread.IsAlive) {
			m_thread.Abort ();
			Debug.Log ("Thread Aborted");
		}
		if (m_connected) {
			Disconnect ();
		}
		EnemyManager.s_enemyQueueMut.Close ();

    }
}
