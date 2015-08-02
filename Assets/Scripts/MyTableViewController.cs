using UnityEngine;
using System.Collections;
using Tacticsoft;

public class MyTableViewController : MonoBehaviour, ITableViewDataSource
{
    public TableView tableView;
    public MyCell cellPrefab;
    public GameObject modalCanvas;
    public GameObject intentPrefab;

    private int numInstancesCreated = 0;

    IEnumerator Start()
    {
        tableView.dataSource = this;
        yield return new WaitForSeconds(1f);
        tableView.ReloadData();
    }



    #region ITableViewDataSource


    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        return 20;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
//        cellHeight = (tableView.transform as RectTransform).rect.height * 0.13f;
//        Debug.Log("CellHeight: " + cellHeight);
//        return cellHeight;
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
        cell.SetIndex(row);
        cell.textLabel.text = "item #" + row;
        return cell;
    }

    #endregion

    public void OnClickTableCell(TableViewCell cell, int row)
    {
        Debug.Log("OnClick!! " + row);
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

}


