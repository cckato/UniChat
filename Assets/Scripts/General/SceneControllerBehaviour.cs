using UnityEngine;
using System.Collections;

namespace Fictbox
{

    public class SceneControllerBehaviour : MonoBehaviour
    {

        protected void Awake()
        {
            if (GameObject.FindGameObjectWithTag("ModelManager") == null)
            {
                GameObject modelManagerPrefab = (GameObject)Resources.Load("Prefab/Models/ModelManager");
                GameObject obj = Instantiate(modelManagerPrefab);
                DontDestroyOnLoad(obj);
            }
        }

    }
}

