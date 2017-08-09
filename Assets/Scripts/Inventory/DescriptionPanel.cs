using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanel : MonoBehaviour
{
    public static DescriptionPanel panel;
    Text itemName;
    Image icon;
    Text description;
    float deltaDistance = 30f;
    float xSize, ySize;
    float timeToHide = 0.1f, timer=0f;
    bool isHiding;
    private void Awake()
    {
        panel = this.gameObject.GetComponent<DescriptionPanel>();
        itemName = transform.Find("Name").GetComponent<Text>();
        icon = transform.Find("Icon").GetComponent<Image>();
        description = transform.Find("Description").GetChild(0).GetComponent<Text>();
        if (panel.gameObject.active) panel.gameObject.SetActive(false);
        xSize = transform.GetComponent<RectTransform>().rect.size.x/2;
        ySize = transform.GetComponent<RectTransform>().rect.size.y/2;
    }

    public static void Show(int id,Vector3 slotPos)
    {
        if(!Inventory.draggingGr.dragging)
        {
            Item item = ItemDataBase.GetItemById(id);
            panel.itemName.text = item.name;
            panel.icon.sprite = item.icons[0];
            panel.description.text = item.description;
            float xPos = slotPos.x + panel.xSize + panel.deltaDistance;
            float yPos = slotPos.y - panel.ySize - panel.deltaDistance;
            if (xPos + panel.xSize > Screen.width) xPos -= 2 * (panel.xSize + panel.deltaDistance);
            if (yPos - panel.ySize < 0) yPos += 2 * (panel.ySize + panel.deltaDistance);
            panel.transform.position = new Vector3(xPos, yPos);
            if (panel.isHiding)
            {
                panel.isHiding = false;
            }
            else
            {
                panel.gameObject.SetActive(true);
            }
        }
        

    }

    public static void Hide()
    {
        panel.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(isHiding)
        {
            timer += Time.fixedDeltaTime;
            if(timer >= timeToHide)
            {
                panel.gameObject.SetActive(false);
            }
        }
        else
        {
            if (timer > 0) timer = 0f;
        }
    }


}
