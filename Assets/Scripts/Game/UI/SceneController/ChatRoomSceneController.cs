using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Tacticsoft;

public class ChatRoomSceneController : MonoBehaviour, ITableViewDataSource
{

    public TableView tableView;
    public ChatCell cellPrefab;

    public Text titleLabel;
    private int numInstancesCreated = 0;

    void Start()
    {
        GameObject intentObj = GameObject.FindGameObjectWithTag("Intent");
        if (intentObj != null)
        {
            Intent intent = intentObj.GetComponent<Intent>();
            int row = intent.getInteger("row");
            ChatRoom room = ModelManager.Instance.GetChatRoomModel().Rooms [row];
            titleLabel.text = room.Name;
            DestroyObject(intentObj);
        }
    }

    public void OnClickBuckButton()
    {
        Application.LoadLevel("Chat");
    }


    #region ITableViewDataSource

    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        return 20;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
        return (cellPrefab.transform as RectTransform).rect.height;
    }

    //Will be called by the TableView when a cell needs to be created for display
    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        ChatCell cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as ChatCell;

        if (cell == null)
        {
            cell = (ChatCell)GameObject.Instantiate(cellPrefab);
            cell.name = "ChatCellInstance_" + (++numInstancesCreated).ToString();
        }
        return cell;
    }

    #endregion

}
