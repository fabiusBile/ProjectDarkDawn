using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public  class ItemDataBase : MonoBehaviour
{
    public  List<Item> itemDataBase;
    public  GameObject weaponPrefab;
    public  GameObject armorPrefab;
    public  GameObject slotPrefab;
    [HideInInspector]
    public  Sprite emptyIcon;
    public static ItemDataBase dataBase;
    public GameObject myPrefub;
    [ExecuteInEditMode]
    private void Awake()
    {
        dataBase = GameObject.FindGameObjectWithTag("ItemDataBase").GetComponent<ItemDataBase>();
    }

    private void OnEnable()
    {
        dataBase = GameObject.FindGameObjectWithTag("ItemDataBase").GetComponent<ItemDataBase>();
    }

    public void Add(Item it)
    {
            itemDataBase.Add(it);
            ReplacePrefab();
    }
    
    public void Remove(Item it)
    {
        itemDataBase.Remove(it);
        ReplacePrefab();
    }

    public static Item GetItemById(int id)
    {
        if(id> dataBase.itemDataBase.Count)
        {
            Debug.LogError("id > dataBase.count");
            return null;
        }
        foreach(Item item in dataBase.itemDataBase)
        {
            if (item.id == id) return item;
        }
        return null;
    }

    public static int GetNextId()
    {
        return dataBase.itemDataBase.Count;
    }
    
    public bool Contain(Item item)
    {
        foreach ( Item it in itemDataBase)
        {
            if (it.name == item.name) return true;
        }
        return false;
    }

    public void ReplacePrefab()
    {
        PrefabUtility.ReplacePrefab(this.gameObject, PrefabUtility.GetPrefabParent(this.gameObject), ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
    }
}

        

