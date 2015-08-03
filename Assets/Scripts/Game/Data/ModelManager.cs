using UnityEngine;
using System.Collections;

public class ModelManager : SingletonMonoBehaviour<ModelManager>
{
    //-------------------------------------
    // Fields
    //-------------------------------------
    private ChatRoomModel chatRoomModel;

    public ChatRoomModel GetChatRoomModel()
    {
        if (chatRoomModel == null)
        {
            chatRoomModel = GetComponent<ChatRoomModel>();
        }
        return chatRoomModel;
    }
}