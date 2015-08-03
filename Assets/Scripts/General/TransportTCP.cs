using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SpicyPixel.Threading;
using SpicyPixel.Threading.Tasks;

public class TransportTCP : ConcurrentBehaviour
{
    private Socket socket = null;
    private bool isConnected = false;
    private PacketQueue sendQueue;
    private PacketQueue recvQueue;

    protected bool threadLoop = false;
    protected Thread thread = null;

    public delegate void EventHandler(NetEventState state);
    private EventHandler handler;
    //-------------------------------
    // LifeCycle
    //-------------------------------
    void Start()
    {
        sendQueue = new PacketQueue();
        recvQueue = new PacketQueue();
    }


    //-------------------------------
    // Public Logic
    //-------------------------------
    public bool Connect(string address, int port)
    {
        bool ret = false;
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            socket.Connect(address, port);
            ret = LaunchThread();
        } catch
        {
            socket = null;
        }

        if (ret == true)
        {
            isConnected = true;
        } else
        {
            isConnected = false;
        }

        if (handler != null)
        {
            NetEventState state = new NetEventState();
            state.type = NetEventType.Connect;
            state.result = (isConnected == true) ? NetEventResult.Success : NetEventResult.Failure;
            handler(state);
        }


        return isConnected;
    }

    public void Disconnect()
    {
        Debug.Log("Disconnect()");

        isConnected = false;

        if (socket != null)
        {

            try
            {
                socket.Shutdown(SocketShutdown.Both);
            } catch
            {
            }
            socket.Close();
            socket = null;

            if (thread.IsAlive)
            {
                try
                {
                    threadLoop = false;
                    thread.Abort();
                } catch
                {

                }
            }

            if (handler != null)
            {
                Debug.Log("handler is not null.");
                NetEventState state = new NetEventState();
                state.type = NetEventType.Disconnect;
                state.result = NetEventResult.Success;
                handler(state);
            } else
            {
                Debug.Log("handler is null...");
            }
        }
    }

    public int Send(byte[] data, int size)
    {
        if (sendQueue == null)
        {
            return 0;
        }

        return sendQueue.Enqueue(data, size);
    }

    public int Receive(ref byte[] buffer, int size)
    {
        if (recvQueue == null)
        {
            return 0;
        }
        return recvQueue.Dequeue(ref buffer, size);
    }

    public bool IsConnected()
    {
        return isConnected;
    }

    public void RegisterEventHandler(EventHandler handler)
    {
        this.handler += handler;
    }

    public void UnregisterEventHandler(EventHandler handler)
    {
        this.handler -= handler;
    }


    //-------------------------------
    // Thread Logic
    //-------------------------------
    bool LaunchThread()
    {
        try
        {
            threadLoop = true; 
            thread = new Thread(new ThreadStart(Dispatch));
            thread.Start();
        } catch
        {
            Debug.Log("Cannot launch thread.");
            return false;
        }
        return true;
    }

    public void Dispatch()
    {
        Debug.Log("Dispatch!!!!");
        while (threadLoop)
        {
            if (socket != null && isConnected == true)
            {
                DispatchSend();
                DispatchReceive();
            }

            Thread.Sleep(5);
        }
    }

    void DispatchSend()
    {
        try
        {
            if (socket.Poll(0, SelectMode.SelectWrite))
            {
                byte[] buffer = new byte[1400];

                int sendSize = sendQueue.Dequeue(ref buffer, buffer.Length);
                while (sendSize > 0)
                {
                    socket.Send(buffer, sendSize, SocketFlags.None);
                    sendSize = sendQueue.Dequeue(ref buffer, buffer.Length);
                }
            }
        } catch
        {
            return;
        }
    }

    void DispatchReceive()
    {
        try
        {
            while (socket.Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[1400];
                int recvSize = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                if (recvSize == 0)
                {
                    if (isConnected)
                    {
                        Debug.Log("Disconnect recv");
                        isConnected = false;
                        taskFactory.StartNew(Disconnect);
                    }
                } else if (recvSize > 0)
                {
                    recvQueue.Enqueue(buffer, recvSize);
                }
            }
        } catch
        {
            return;
        }
    }



}









