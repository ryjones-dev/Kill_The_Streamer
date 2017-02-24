using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public struct CommandNetworkInfo
{
    public string username;
    public string command;
}

public class CommandInterpreter : MonoBehaviour
{
    private static CommandInterpreter s_instance;

    public static Queue<CommandNetworkInfo> s_commandQueue;
    public static Mutex s_commandQueueMut;

    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;

            s_commandQueue = new Queue<CommandNetworkInfo>();
            s_commandQueueMut = new Mutex();
        }
        else
        {
            Destroy(this);
        }
    }

    public static void AddCommand(CommandNetworkInfo p_commandInfo)
    {
        s_commandQueue.Enqueue(p_commandInfo);
    }

    private void Update()
    {
        if(s_commandQueue.Count > 0)
        {
            while(s_commandQueue.Count > 0)
            {
                s_commandQueueMut.WaitOne();
                CommandNetworkInfo commandInfo = s_commandQueue.Dequeue();
                s_commandQueueMut.ReleaseMutex();
                string[] command = commandInfo.command.Split(' ');

                // Gets the spawn location from the second parameter
                int spawnLocation = -1;
                if (command.Length > 1)
                {
                    int.TryParse(command[1], out spawnLocation);
                }

                command[0] = command[0].Trim();

                switch (command[0])
                {
                    case "TheIlluminati":
                        EnemyManager.CreateEnemy(EnemyType.BooEnemy, commandInfo.username, spawnLocation - 1);
                        break;

                    case "Kappa":
                        EnemyManager.CreateEnemy(EnemyType.SeekEnemy, commandInfo.username, spawnLocation - 1);
                        break;

                    case "FrankerZ":
                        EnemyManager.CreateEnemy(EnemyType.GhostEnemy, commandInfo.username, spawnLocation - 1);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        CommandInterpreter.s_commandQueueMut.Close();
    }
}
