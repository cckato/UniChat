using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;

public class MyCell : TableViewCell
{
    public int index;
    public Text textLabel;
    public Button button;

    public void SetIndex(int index)
    {
        this.index = index;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            MyTableViewController tableViewController = GameObject.FindGameObjectWithTag("TableViewController").GetComponent<MyTableViewController>();
            if (tableViewController != null)
            {
                tableViewController.OnClickTableCell(this, index);
            }
        });

    }

}
