using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[Serializable]
public abstract class Item : MonoBehaviour
{
    // шаблон для всех предметов

    public enum ItemType
    {
        Weapon,Armor,SpellBook,Elexir,OtherStuff
    }
    public enum Character
    {
        Bishop, apprentice, hunter, All
    }

    public int id; // id предмета 
    public string name; 
    public Sprite[] icons;
    public string description; // описание предмета
    public ItemType itemType; // тип предмета 
    public Character character; // какой персонаж может носить 
    public GameObject prefab; // ссылка на префаб 
    public string prefabPath; // путь к префабу 
	public bool vertical = true; // ориентация предмета в инвентаре
    public int taking_slots;// количество слотов в инвентаре

	void Start()
	{
		icons = new Sprite[taking_slots];
		for (int i = 0; i < taking_slots; i++) 
		{
			icons [i] = ItemDataBase.dataBase.emptyIcon;
		}


	}

    public void ClickAction()
    {

    }


}
