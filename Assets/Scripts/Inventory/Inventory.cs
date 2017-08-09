using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Inventory : MonoBehaviour
{
    // Во время старта добавляем каждый слот в массив и задаем его id равный индексу в массиве
    // 

    public static List<Slot> inventory = new List<Slot>(); // список слотов
    public static List<slotGroup> slotGropus = new List<slotGroup>(); // список групп слотов. Нужен для предметов, занимающих несколько слотов( с 1 слотом тоже слот группа)
    public static int X = 16; // количество ячеек по горизонтали
    public static draggingGroup draggingGr;
#region structs
    public struct slotGroup
    {
        public int[] slots;

    }

    public struct draggingGroup
    {
        public Slot[] sg; // массив для создания слотов
        public Vector3 lastPos; // последняя позиция слота
        public Vector3 delta; // смещение мыши относительно посленей позиции слота 
        public bool dragging; 
        public bool vertical;
        public int fromSlot;
        public Vector3 slotDelta; // для правильного расположения слотов
    }
#endregion

    private void Awake()
    {
        Slot tmpSlot;
        Transform slotPanel = transform.Find("SlotPanel").transform;
        draggingGr = new draggingGroup();
        draggingGr.dragging = false;
        for(int i=0; i<slotPanel.childCount;i++)
        {
            tmpSlot = slotPanel.GetChild(i).GetComponent<Slot>();
            tmpSlot.slotId = i;
            inventory.Add(tmpSlot);
        }

    }

    // добавляет предмет в инвентарь 
    public static void Add(int id)
    {
        Item it = ItemDataBase.GetItemById(id);
        int taking_slots = it.taking_slots;
        slotGroup sg = findSlots(taking_slots, it.vertical); // получаем полходяшие слоты
        if (sg.slots[0] == -1)
        {
            // нет места 
            return;
        }
        for (int j = 0; j < taking_slots; j++)
        {
            inventory[sg.slots[j]].Set(id, j);
        }
        slotGropus.Add(sg);
    }

    // находит индекс слота, который подходит для добавления нового предмета
    public static slotGroup findSlots(int taking_slots, bool vertical)
    {
        slotGroup sg = new slotGroup();
        sg.slots = new int[taking_slots];
        if (taking_slots == 1)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemId == -1)
                {
                    sg.slots[0] = i;
                    return sg;
                }
            }
        }
        else if (taking_slots > 1 && taking_slots < 4)
        {


            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemId == -1) // слот не занят
                {
                    if (vertical && i + X * (taking_slots - 1) < inventory.Count) // у предмета вертикальная ориентация и не выходит за рамки массива
                    {
                        bool add = true;
                        for (int j = 0; j < taking_slots; j++)
                        {
                            sg.slots[j] = i + X * j;
                            if (inventory[i+X*j].itemId != -1)
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
   //                         for (int j = 0; j < taking_slots; j++) Debug.Log(sg.slots[j]);
                            return sg;
                        }
                    }
                    else if (!vertical && i + (taking_slots - 1) < inventory.Count)
                    {
                        bool add = true;
                        //проверка на "край" инвентаря 
                        int y = i / X + 1; // получаем номер строки
                        int edge = X * y; // край строки
                        if(edge-i >= taking_slots)
                        {
                            for (int j = 0; j < taking_slots; j++)
                            {
                                sg.slots[j] = i + j;
                                if (inventory[i + j].itemId != -1)
                                {
                                    add = false;
                                }
                            }
                        }
                        else
                        {
                            add = false;
                        }
                        
                        if(add)
                        {
                            return sg;
                        }
                    }
                }
            }
        }
        else if( taking_slots == 4)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                //проверка на "край" инвентаря 
                int y = i / X + 1; // получаем номер строки
                int edge = X * y; // край строки
                if (edge - i >= 1 && i+X+1 < inventory.Count)
                    if (inventory[i].itemId == -1 && inventory[i+1].itemId == -1 && inventory[i+X].itemId == -1 && inventory[i+X+1].itemId == -1)
                     {
                        sg.slots[0] = i;
                        sg.slots[1] = i + 1;
                        sg.slots[2] = i + X;
                        sg.slots[3] = i + X + 1;
                        return sg;
                     }
            }
        }
        sg.slots[0] = -1;
        return sg;
    }
    

    public static slotGroup findGroupById(int slotId)
    {
        for(int i = 0; i<slotGropus.Count; i++)
        {
            for(int j = 0; j < slotGropus[i].slots.Length;j++)
            {
                if (slotGropus[i].slots[j] == slotId) return slotGropus[i];
            }
        }

        Debug.Log("Slot is not in slot's Grop");
        slotGroup sg = new slotGroup();
        sg.slots = new int[1] { -1 };
        return sg;
    }

    public static Slot findSlotByPosition(Vector3 pos)
    {
        float closesDist = 1010;
        Slot rs = null;
        float dist;
        foreach(Slot s in inventory)
        {
            dist = (s.transform.position - Input.mousePosition).magnitude;
            if(dist < closesDist)
            {
                rs = s;
                closesDist = dist; 
            }

        }
        if (closesDist > 70f) return null;
        return rs;
    }

#region Drag/Drop
    public static void BeginDrag(int slotId)
    {
        if(inventory[slotId].itemId != -1 )
        {
            draggingGr.dragging = true;
            Item it = ItemDataBase.GetItemById(inventory[slotId].itemId);
            draggingGr.sg = new Slot[it.taking_slots];
            draggingGr.lastPos = Input.mousePosition;
            draggingGr.delta = draggingGr.lastPos - Input.mousePosition;
            draggingGr.vertical = it.vertical;
            draggingGr.fromSlot = slotId;
            if (it.taking_slots == 1)
            {
                draggingGr.sg[0] = GameObject.Instantiate(ItemDataBase.dataBase.slotPrefab, Input.mousePosition, Quaternion.identity, GameObject.Find("Inventory").transform).GetComponent<Slot>();
                draggingGr.sg[0].gameObject.name = " Dragging Slot ";
                draggingGr.sg[0].Set(it.id);
            }
            else if (it.taking_slots > 1 && it.taking_slots < 4)
            {
                if (draggingGr.vertical) draggingGr.slotDelta = new Vector3(0, -50, 0);
                else draggingGr.slotDelta = new Vector3(50, 0, 0);

                for (int i = 0; i < it.taking_slots; i++)
                {
                    draggingGr.sg[i] = GameObject.Instantiate(ItemDataBase.dataBase.slotPrefab, Input.mousePosition + draggingGr.slotDelta * i, Quaternion.identity, GameObject.Find("Inventory").transform).GetComponent<Slot>();
                    draggingGr.sg[i].gameObject.name = " Dragging Slot ";
                    draggingGr.sg[i].Set(it.id, i);
                }
            }
            else if(it.taking_slots == 4)
            {
                draggingGr.sg[0] = GameObject.Instantiate(ItemDataBase.dataBase.slotPrefab, Input.mousePosition, Quaternion.identity, GameObject.Find("Inventory").transform).GetComponent<Slot>();
                draggingGr.sg[0].gameObject.name = " Dragging Slot ";
                draggingGr.sg[0].Set(it.id, 0);

                draggingGr.sg[1] = GameObject.Instantiate(ItemDataBase.dataBase.slotPrefab, Input.mousePosition + Vector3.right * 50, Quaternion.identity, GameObject.Find("Inventory").transform).GetComponent<Slot>();
                draggingGr.sg[1].gameObject.name = " Dragging Slot ";
                draggingGr.sg[1].Set(it.id, 1);

                draggingGr.sg[2] = GameObject.Instantiate(ItemDataBase.dataBase.slotPrefab, Input.mousePosition + -Vector3.up * 50, Quaternion.identity, GameObject.Find("Inventory").transform).GetComponent<Slot>();
                draggingGr.sg[2].gameObject.name = " Dragging Slot ";
                draggingGr.sg[2].Set(it.id, 2);

                draggingGr.sg[3] = GameObject.Instantiate(ItemDataBase.dataBase.slotPrefab, Input.mousePosition + Vector3.right * 50 + Vector3.down * 50, Quaternion.identity, GameObject.Find("Inventory").transform).GetComponent<Slot>();
                draggingGr.sg[3].gameObject.name = " Dragging Slot ";
                draggingGr.sg[3].Set(it.id, 3);


            }
        }
        

    }

    public static void Drop()
    {
        draggingGr.dragging = false;
        for(int i = 0; i<draggingGr.sg.Length; i++)
        {
            GameObject.Destroy(draggingGr.sg[i].gameObject);
        }
        Slot s = findSlotByPosition(Input.mousePosition);
        if(s != null)
        {
            MoveItem(draggingGr.fromSlot, s.slotId);
        }
  
    }
#endregion

    private static void MoveItem(int fromSlot, int toSlot)
    {
        Item it = ItemDataBase.GetItemById(inventory[fromSlot].itemId);
        if(IsAbleToMove(it.id, fromSlot, toSlot))
        {
            slotGroup sg = findGroupById(fromSlot);
            int[] delta = new int[it.taking_slots];
            int zeroId = sg.slots[0];
            for (int i = 0; i < it.taking_slots; i++) // перемещение
            {
                delta[i] = sg.slots[i] - zeroId;// получаем отношение id слотов в прошлой группе
                inventory[sg.slots[i]].Clear();
                sg.slots[i] = toSlot + delta[i];
            }
            for(int i = 0; i< it.taking_slots; i++)
            {
                inventory[sg.slots[i]].Set(it.id, i);
            }
        }
    }

    // проверка на возможность поместить предмет в данный слот
    private static bool IsAbleToMove(int itemId, int fromSlot, int toSlot)
    {
        Item it = ItemDataBase.GetItemById(itemId);
        int taking_slots = it.taking_slots;
        bool vertical = it.vertical;
        slotGroup fromSG = findGroupById(fromSlot);
        if (taking_slots == 1)
        {
            if (inventory[toSlot].itemId == -1)
            {
                return true;
            }

        }
        else if (taking_slots > 1 && taking_slots < 4)
        {
                if (inventory[toSlot].itemId == -1 || findGroupById(toSlot).slots == fromSG.slots) // слот не занят
                {
                    if (vertical && toSlot + X * (taking_slots - 1) < inventory.Count) // у предмета вертикальная ориентация и не выходит за рамки массива
                    {
                        bool add = true;
                        for (int j = 0; j < taking_slots; j++)
                        {
                            if (!(add && inventory[toSlot + X * j].itemId == -1 || (inventory[toSlot + X * j].itemId != -1 && findGroupById(toSlot + X *j).slots == fromSG.slots)))
                            {
                            add = false;
                            }
                       
                        }
                        return add;
                    }
                    else if (!vertical && toSlot + (taking_slots - 1) < inventory.Count)
                    {
                        bool add = true;
                        //проверка на "край" инвентаря 
                        int y = toSlot / X + 1; // получаем номер строки
                        int edge = X * y; // край строки
                        if (edge - toSlot >= taking_slots)
                        {
                            for (int j = 0; j < taking_slots; j++)
                            {
                            if (!(add && inventory[toSlot + j].itemId == -1 || (inventory[toSlot + j].itemId != -1 && findGroupById(toSlot + j).slots == fromSG.slots)))
                            {
                                    add = false;
                                }
                            }
                        }
                        return add;
                    }
                }
        }
        else if (taking_slots == 4)
        {
            //проверка на "край" инвентаря 
            int y = toSlot / X + 1; // получаем номер строки
            int edge = X * y; // край строки
            if (edge - toSlot >= 1 && toSlot + X + 1 < inventory.Count)
            {
                bool add = true;
                if (!(add && inventory[toSlot].itemId == -1 || (inventory[toSlot].itemId != -1 && findGroupById(toSlot).slots == fromSG.slots))) add = false;
                if (!(add && inventory[toSlot+1].itemId == -1 || (inventory[toSlot + 1].itemId != -1 && findGroupById(toSlot + 1).slots == fromSG.slots))) add = false;
                if (!(add && inventory[toSlot+X].itemId == -1 || (inventory[toSlot + X].itemId != -1 && findGroupById(toSlot + X).slots == fromSG.slots))) add = false;
                if (!(add && inventory[toSlot + X+1].itemId == -1 || (inventory[toSlot + X+1].itemId != -1 && findGroupById(toSlot + X+1).slots == fromSG.slots))) add = false;
                //if (inventory[toSlot].itemId == -1 && inventory[toSlot + 1].itemId == -1 && inventory[toSlot + X].itemId == -1 && inventory[toSlot + X + 1].itemId == -1)
                //{
                //    return true;
                //}
                return add;
            }
           

        }
        return false;
    }

    public void Update()
    {
        if(draggingGr.dragging)
        {
            draggingGr.delta = Input.mousePosition - draggingGr.lastPos;
            for(int i = 0; i < draggingGr.sg.Length; i++)
            {
                draggingGr.sg[i].transform.position += draggingGr.delta;
            }
            draggingGr.lastPos = Input.mousePosition;
        }
    }
}
