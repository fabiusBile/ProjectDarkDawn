using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Weapon : Item
{
    public float attack = 0f;
    // типы оружия
    public  enum WeaponType
    {
		oneHanded_meele,oneHanded_range,oneHanded_pair,twoHanded_meele,twoHanded_range
    }
   public WeaponType weaponType;
    public Weapon( int id, string name, Sprite[] icons, string description, WeaponType wt, float attack)
    {
        this.id = id;
        this.name = name;
		this.icons = icons;
        this.description = description;
        this.itemType = ItemType.Weapon;
        this.weaponType = wt;
        this.attack = attack;
        
    }

}
