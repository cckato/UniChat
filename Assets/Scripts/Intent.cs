using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intent : MonoBehaviour
{

    Dictionary<string, int> intDic = new Dictionary<string, int>();

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void putInteger(string name, int value)
    {
        intDic.Add(name, value);
    }

    public int getInteger(string name)
    {
        return intDic [name];
    }


}
