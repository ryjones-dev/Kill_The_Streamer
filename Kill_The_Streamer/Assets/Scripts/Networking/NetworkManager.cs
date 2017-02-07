﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;



public class NetworkManager : MonoBehaviour {

    public static NetworkManager s_manager;
    public static string s_oauth; //Oauth of the bot listening to the channel
    public static string s_username; //Username of the bot listening to the channel
    public static string s_channel; //Channel being listened to.

    public Socket m_socket;
    public bool m_connected;
    public Thread m_thread;

    private bool ConnectToServer()
    {
        byte[] bytes = new byte[1024];

        try
        {
            Debug.Log("Connnecting...");
            // Connects to twitch
            IPHostEntry ipHostInfo = Dns.GetHostEntry("irc.twitch.tv");
            Debug.Log(ipHostInfo.AddressList[0].ToString());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6667);

            m_socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            m_socket.Connect(remoteEP);

            Debug.Log("Connected");
            m_connected = true;

            //Handshake to authenticate.
            SendData("PASS " + s_oauth);
            SendData("NICK " + s_username);
            SendData("USER " + s_username + " 8 * :TwitchLink Client");

            //Connect to the channel
            SendData("JOIN #" + s_channel);
            m_thread = new Thread(new ThreadStart(Listener));
            m_thread.Start();


        }

        catch (Exception e)
        {
            Debug.Log("Error: " + e);
        }



        return false;
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
            //Debug.Log(output);
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
                    }

                }
                

            }
        }
    }

    public void Disconnect()
    {
        
        m_socket.Shutdown(SocketShutdown.Both);
        m_socket.Close();
    }

    // Use this for initialization
    void Start () {
        string[] config;
        config = System.IO.File.ReadAllLines("config.txt");
        s_oauth = "oauth:" + config[0].Trim(' ').Remove(0, 7);//"oauth: " - 7 characters
        s_username = config[1].Trim(' ').Remove(0, 9).ToLower();//"botname: " - 9 characters
        s_channel = config[2].Trim(' ').Remove(0, 9).ToLower();//"channel: " - 9 characters
        
        if (!s_manager)
        {
            s_manager = this;
        }

        ConnectToServer();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnApplicationQuit()
    {
        m_thread.Abort();
        Disconnect();
    }
}
