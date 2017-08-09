using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
{
    public int slotId;
    public int itemId =-1; // нет никакого предмета
    public void Set(int id)
    {
        itemId = id;
        Sprite sprite = ItemDataBase.GetItemById(id).icons[0];
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
    //  устанавливает слот и иконку в слоте 
    public void Set(int id, int i)
    {
        itemId = id;
        Sprite sprite = ItemDataBase.GetItemById(id).icons[i];
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        
    }

    public void Clear()
    {
        itemId = -1;
        transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    #region Реализация интерфейсов
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemId > -1)
        {
            DescriptionPanel.Show(itemId,this.transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DescriptionPanel.Hide();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        Inventory.BeginDrag(slotId);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    { 
        Inventory.Drop();
    }




    #endregion
}
