using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatRoomSceneController : MonoBehaviour
{

    public Text titleLabel;

    void Start()
    {
        GameObject intentObj = GameObject.FindGameObjectWithTag("Intent");
        if (intentObj != null)
        {
            Intent intent = intentObj.GetComponent<Intent>();
            int row = intent.getInteger("row");
            titleLabel.text = "# " + row + " Room";
            DestroyObject(intentObj);
        }
    }

    public void OnClickBuckButton()
    {
        Application.LoadLevel("Chat");
    }

}
