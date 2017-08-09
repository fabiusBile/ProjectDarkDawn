using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    public enum ArmorType
    {
        Simple, Magic, Else
    }
    public float defence;
    public ArmorType armorType;

    public Armor(int id, string name, Sprite[] icons, string description, ArmorType at, float armor)
    {
        this.id = id;
        this.name = name;
        this.icons = icons;
        this.description = description;
        this.armorType = at;
        this.defence = armor;
    }
}
