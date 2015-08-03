using UnityEngine;
using System.Collections;

public class ChatRoom
{
    public string Name { get; set; }

    public static ChatRoom Create(string name)
    {
        ChatRoom r = new ChatRoom();
        r.Name = name;
        return r;
    }
}
