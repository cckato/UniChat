using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

public class TcpServer : MonoBehaviour
{
    //------------------------------
    // Constants
    //------------------------------
    private const int port = 5566;

    //------------------------------
    // Fields
    //------------------------------
    [SerializeField]
    Text
        debugText;
    [SerializeField]
    Button
        sendButton;

    private string
        address = "";
    private Socket listener = null;
    private Socket socket = null;
    private bool isListening = false;
    private bool isConnected = false;
    private int count = 0;

    //------------------------------
    // Life Cycle
    //------------------------------
    void Start()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress hostAddress = hostEntry.AddressList [0];
        Debug.Log(hostEntry.HostName);
        address = hostAddress.ToString();
    }

    void Update()
    {
        if (isListening == false)
        {
            StartListener();
        } else if (isConnected == false)
        {
            AcceptClient();
        } else
        {
            ReceiveFromClient();
        }

    }

    //------------------------------
    // Server Logics.
    //------------------------------
    void StartListener()
    {
        listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, port));
        listener.Listen(1);
        isListening = true;
        debugText.text = "Listening...(" + address + ")";
    }

    void AcceptClient()
    {
        if (listener != null && listener.Poll(0, SelectMode.SelectRead))
        {
            Debug.Log("Connecting from client...");
            socket = listener.Accept();
            Debug.Log("Connected from client");
            isConnected = true;
            debugText.text = "Connected!!!";
            sendButton.interactable = true;
        }
    }

    void ReceiveFromClient()
    {
        if (socket == null)
        {
            return;
        }

        byte[] buffer = new byte[1400];
        int recvSize = socket.Receive(buffer, buffer.Length, SocketFlags.None);
        if (recvSize > 0)
        {
            string message = System.Text.Encoding.UTF8.GetString(buffer);
            Debug.Log(message);
            debugText.text = message;
        }
    }


    //------------------------------
    // User touch actions.
    //------------------------------
    public void OnClickSendButton()
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("This is server: " + count);
        socket.Send(buffer, buffer.Length, SocketFlags.None);

        count++;
    }


}
