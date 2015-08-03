using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

using LitJson;
using System.Text;

public class ChatRoomModel : Fictbox.ObservableModel<bool>
{
    //-------------------------------------
    // Fields
    //-------------------------------------
    public List<ChatRoom> Rooms { get; set; }

    //-------------------------------------
    // Logics
    //-------------------------------------
    public void Fetch()
    {
        StartCoroutine("Get");
    }

    IEnumerator Get()
    {
        string url = "https://fierce-dawn-6543.herokuapp.com/users.json";
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
            List<User> users = JsonMapper.ToObject<List<User>>(www.text);

            List<ChatRoom> data = new List<ChatRoom>();
            foreach (User u in users)
            {
                data.Add(ChatRoom.Create(u.name));
            }
            Rooms = data;
            NotifyObservers(true);
        }
    }

    public void SaveNewRoom(ChatRoom room)
    {
        Debug.Log("SaveNewRoom");
        StartCoroutine("Post", room);
    }

    IEnumerator Post(ChatRoom room)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json; charset=UTF-8");

        JsonData data = new JsonData();
        data ["name"] = room.Name;

        string postJsonStr = data.ToJson();
        byte[] postBytes = Encoding.Default.GetBytes(postJsonStr);

        Debug.Log("post: " + postJsonStr);

        string url = "https://fierce-dawn-6543.herokuapp.com/users";
        WWW www = new WWW(url, postBytes, header);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("Post Success");
        } else
        {
            Debug.LogError(www.error);
        }

    }
}






