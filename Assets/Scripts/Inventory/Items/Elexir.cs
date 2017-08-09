using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elexir : Item
{
    public Elexir(int id, string name, Sprite[] icons, string description)
    {
        this.id = id;
        this.name = name;
        this.icons = icons;
        this.description = description;
    }

}
