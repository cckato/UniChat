using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class TcpClient : MonoBehaviour
{
    //------------------------------
    // Constants
    //------------------------------
    private const int port = 5555;

    //------------------------------
    // Fields
    //------------------------------
    [SerializeField]
    TransportTCP
        transportTcp;
    [SerializeField]
    Text
        debugText;
    [SerializeField]
    Button
        sendButton;
    [SerializeField]
    Button
        connectButton;
    [SerializeField]
    Button
        disconnectButton;
    [SerializeField]
    InputField
        addressField;

    private string address = "";
    private int count = 0;

    //------------------------------
    // Life Cycle
    //------------------------------
    void Start()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress hostAddress = hostEntry.AddressList [0];
        Debug.Log(hostEntry.HostName);
        addressField.text = hostAddress.ToString();

        transportTcp.RegisterEventHandler(EventCallback);
    }

    void Update()
    {
        if (transportTcp.IsConnected())
        {
            byte[] buffer = new byte[1400];
            int recvSize = transportTcp.Receive(ref buffer, buffer.Length);
            if (recvSize > 0)
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer);
                debugText.text = message;
            }
        }
    }
    //------------------------------
    // EventDelegate
    //------------------------------
    public void EventCallback(NetEventState state)
    {
        Debug.Log("EventCallback!!!");
        switch (state.type)
        {
            case NetEventType.Connect:
                if (state.result == NetEventResult.Success)
                {
                    debugText.text = "Connected!!";
                    sendButton.interactable = true;
                    disconnectButton.interactable = true;
                } else
                {
                    debugText.text = "Connection Failed...";
                    connectButton.interactable = true;
                }
                break;
            case NetEventType.Disconnect:
                debugText.text = "Disconnect...";
                connectButton.interactable = true;
                sendButton.interactable = false;
                disconnectButton.interactable = false;
                break;
            default:
                break;
        }
    }

    //------------------------------
    // User touch events.
    //------------------------------
    public void OnClickSendButton()
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("This is client: " + count);
        transportTcp.Send(buffer, buffer.Length);

        count++;
    }

    public void OnClickConnectButton()
    {
        if (string.IsNullOrEmpty(addressField.text))
        {
            return;
        } 

        address = addressField.text;
        connectButton.interactable = false;
        debugText.text = "Connecting...";

        transportTcp.Connect(address, port);

    }

    public void OnClickDisconnectButton()
    {
        transportTcp.Disconnect();
    }
}



