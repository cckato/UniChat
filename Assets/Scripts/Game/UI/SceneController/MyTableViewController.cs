using UnityEngine;
using System.Collections;
using Tacticsoft;
using System.Collections.Generic;
using UnityEngine.UI;

public class MyTableViewController : Fictbox.SceneControllerBehaviour, ITableViewDataSource
{
    public TableView tableView;
    public MyCell cellPrefab;
    public GameObject modalCanvas;
    public GameObject intentPrefab;
    public InputField inputField;


    private int numInstancesCreated = 0;

    private ChatRoomModel chatRoomModel;


    void Awake()
    {
        base.Awake();
        chatRoomModel = ModelManager.Instance.GetChatRoomModel();
        tableView.dataSource = this;
    }


    void OnEnable()
    {
        chatRoomModel.Updated += OnResourceUpdated;
    }

    void Start()
    {
        RefreshViews();
    }

    void OnDisable()
    {
        chatRoomModel.Updated -= OnResourceUpdated;
    }

    void RefreshViews()
    {
        if (chatRoomModel.Rooms == null)
        {
            chatRoomModel.Fetch();
            return;
        }
        tableView.ReloadData();
        if (tableView.scrollableHeight > 0)
        {
            tableView.scrollY = 0.1f;
        }
    }


    public void OnResourceUpdated(bool succeeded)
    {
        if (succeeded)
        {
            RefreshViews();
        }
    }

    #region ITableViewDataSource


    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        return (chatRoomModel.Rooms != null) ? chatRoomModel.Rooms.Count : 0;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
        return (cellPrefab.transform as RectTransform).rect.height;
    }

    //Will be called by the TableView when a cell needs to be created for display
    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        MyCell cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as MyCell;

        if (cell == null)
        {
            cell = (MyCell)GameObject.Instantiate(cellPrefab);
            cell.name = "VisibleCounterCellInstance_" + (++numInstancesCreated).ToString();
        }

        ChatRoom room = chatRoomModel.Rooms [row];
        cell.SetIndex(row);
        cell.textLabel.text = room.Name;
        return cell;
    }

    #endregion

    public void OnClickTableCell(TableViewCell cell, int row)
    {
        GameObject intentObj = GameObject.Instantiate(intentPrefab);
        Intent intent = intentObj.GetComponent<Intent>();
        intent.putInteger("row", row);
        Application.LoadLevel("ChatRoom");
    }

    public void OnClickToolbarRightButton()
    {
        Debug.Log("OnClickToolbarRightButton");
        modalCanvas.SetActive(true);
    }

    public void OnClickModalCancelButton()
    {
        modalCanvas.SetActive(false);
    }

    public void OnClickModalSendButton()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            return;
        }

        chatRoomModel.SaveNewRoom(ChatRoom.Create(inputField.text));
    }

}


