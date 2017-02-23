using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInterpreter : MonoBehaviour
{
    private static CommandInterpreter s_instance;

    private Queue<string> m_commandQueue;

    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddCommand(string p_command)
    {
        s_instance.m_commandQueue.Enqueue(p_command);
    }

    private void Update()
    {
        if(s_instance.m_commandQueue.Count > 0)
        {
            while(s_instance.m_commandQueue.Count > 0)
            {
                string commandString = s_instance.m_commandQueue.Dequeue();
                string[] command = commandString.Split(' ');

                switch(command[0])
                {
                    case "TheIlluminati":
                        if(command.Length > 1)
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
