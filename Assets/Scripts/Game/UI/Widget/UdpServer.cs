using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class UdpServer : MonoBehaviour
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
    [SerializeField]
    Button
        serverButton;

    private string
        address = "";
    private Socket socket = null;
    private int count = 0;
    private bool initialized = false;

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
        if (initialized)
        {
            ReceiveFromClient();
        }
    }

    void ReceiveFromClient()
    {
        byte[] buffer = new byte[1400];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint senderRemote = (EndPoint)sender;

        if (socket.Poll(0, SelectMode.SelectRead))
        {
            int recvSize = socket.ReceiveFrom(buffer, SocketFlags.None, ref senderRemote);
            if (recvSize > 0)
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer);
                Debug.Log(message);
                debugText.text = message;
            }

        }

    }

    void Connect()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Any, port));
        sendButton.interactable = true;
        initialized = true;
        debugText.text = "Listening...";
        sendButton.interactable = false;
        serverButton.interactable = false;
    }

    public void OnClickServer()
    {
        Connect();
    }

    public void OnClickSend()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("[UDP] this is client: " + count);
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(address), port);
        socket.SendTo(buffer, buffer.Length, SocketFlags.None, endpoint);

        count++;
    }



}




