using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCell : MonoBehaviour
{
    public TMPro.TMP_Text item_name;
    private PersonalizationWindow window;
    private int index;

    public void SetItem(string naming, PersonalizationWindow window, int index)
    {
        item_name.text = naming;
        this.window = window;
        this.index = index;
    }

    public void Clicked()
    {
        Debug.Log(index + "is checking window...");
        if (!window) return;
        Debug.Log(index + "was clicked!");
        window.ItemCellClicked(index);
    }

    public void Drag(bool drag)
    {
        if (!window) return;
        window.ItemDrag(index, drag);
    }
}
