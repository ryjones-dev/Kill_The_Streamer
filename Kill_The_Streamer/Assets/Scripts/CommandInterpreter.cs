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
    private static CommandInterpreter s_instance; // Singleton instance

    private Queue<CommandNetworkInfo> m_commandQueue; // A queue of commands viewers send
    private Mutex m_commandQueueMut; // A mutex to lock the queue for access by the network thread

    private void Awake()
    {
        // Initializes the singleton
        if(s_instance == null)
        {
            s_instance = this;

            // Initializes the command queue and mutex
            m_commandQueue = new Queue<CommandNetworkInfo>();
            m_commandQueueMut = new Mutex();
        }
        else
        {
            // Destroys the component if there is already a singleton instance
            Destroy(this);
        }
    }

    // Adds a command to the queue
    public static void AddCommand(CommandNetworkInfo p_commandInfo)
    {
        // TODO: Add logic to only allow valid commands in the queue
        s_instance.m_commandQueueMut.WaitOne();
        s_instance.m_commandQueue.Enqueue(p_commandInfo);
        s_instance.m_commandQueueMut.ReleaseMutex();
    }

    private void Update()
    {
        if(m_commandQueue.Count > 0)
        {
            // Dequeues the queue automatically
            while(m_commandQueue.Count > 0)
            {
                // Locks the mutex and dequeues an item from the queue
                m_commandQueueMut.WaitOne();
                CommandNetworkInfo commandInfo = m_commandQueue.Dequeue();
                m_commandQueueMut.ReleaseMutex();

                // Gets the command and its parameters in a string array
                string[] command = commandInfo.command.Split(' ');

                // Gets the spawn location from the first parameter
                int spawnLocation = -1;
                if (command.Length > 1)
                {
                    int.TryParse(command[1], out spawnLocation);
                }

                // Trims the command to avoid issues with white space
                command[0] = command[0].Trim();

                // Creates the appropriate enemy based on the command
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
        // Closes the mutex when the game closes
        s_instance.m_commandQueueMut.Close();
    }
}
