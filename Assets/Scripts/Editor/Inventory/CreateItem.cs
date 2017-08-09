using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreateItem : EditorWindow
{
    // Класс, описывающий окно создания предмета
    string itemName = "";
    string path = "Assets/Prefabs/Items/";
    string folder = "";
    string description = "";
	int taking_slots = 1;
	bool vertical = true;

    Item.ItemType itemType = Item.ItemType.Armor;
	Sprite[] icons;
    Button button;
    Item.Character character = Item.Character.All;
    WeaponForm WF = new WeaponForm();
    ArmorForm AF = new ArmorForm();
    [MenuItem("Items/Add")]


    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateItem));
    }


# region Item's structures

    // структура данных, необходимых для создания оружия
    private struct WeaponForm
    {
        public float attack;
        public Weapon.WeaponType weaponType;

        WeaponForm(float a, float wt)
        {
            attack = 0;
			weaponType = Weapon.WeaponType.oneHanded_meele;
        }
    }

    private struct ArmorForm
    {
        public float armor;
        public Armor.ArmorType armorType;
    }

#endregion

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Add Item", EditorStyles.boldLabel);

		// Назначаем параметры
        itemName = EditorGUILayout.TextField("Item Name", itemName); // имя
        itemType =(Item.ItemType)EditorGUILayout.EnumPopup("ItemType", itemType); // тип предмета 
        character = (Item.Character)EditorGUILayout.EnumPopup("Character", character); // тип предмета 
		//===============================================================================================================================================
		if(icons == null) icons = new Sprite[taking_slots];
		if (taking_slots == 1) 
		{
			icons [0] = (Sprite)EditorGUILayout.ObjectField ("Sprite ", icons [0], typeof(Sprite), allowSceneObjects: true);
		} 
		// Если расположение иконок может быть вертикальным или горизонтальным
		else if (taking_slots > 1 && taking_slots < 4) 
		{
			EditorGUILayout.BeginVertical ();
			vertical = EditorGUILayout.Toggle ("Vertical Orientation", vertical);
			// вертикальное расположение
			if (vertical) 
			{
				for (int i = 0; i < taking_slots; i++) 
				{
					icons [i] = (Sprite)EditorGUILayout.ObjectField ("Sprite " + i.ToString (), icons [i], typeof(Sprite), allowSceneObjects: true);
				}	
			} 
			// горизонтальное 
			else 
			{
				EditorGUILayout.BeginHorizontal ();
				for (int i = 0; i < taking_slots; i++) 
				{
					EditorGUILayout.LabelField (i.ToString (), GUILayout.Width (50));
				}
				EditorGUILayout.EndHorizontal ();

				EditorGUILayout.BeginHorizontal ();
				for (int i = 0; i < taking_slots; i++) 
				{
					icons [i] = (Sprite)EditorGUILayout.ObjectField (icons [i], typeof(Sprite), GUILayout.Width (50), GUILayout.Height (50));
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.Space ();
			EditorGUILayout.EndVertical ();
		} 
		// Занимает 4 слота 
		else if (taking_slots == 4) 
		{
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("0", GUILayout.Width (50));
			EditorGUILayout.LabelField ("1", GUILayout.Width (50));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			icons [0] = (Sprite)EditorGUILayout.ObjectField (icons [0], typeof(Sprite), GUILayout.Width (50), GUILayout.Height (50));
			icons [1] = (Sprite)EditorGUILayout.ObjectField (icons [1], typeof(Sprite), GUILayout.Width (50), GUILayout.Height (50));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			icons [2] = (Sprite)EditorGUILayout.ObjectField (icons [2], typeof(Sprite), GUILayout.Width (50), GUILayout.Height (50));
			icons [3] = (Sprite)EditorGUILayout.ObjectField (icons [3], typeof(Sprite), GUILayout.Width (50), GUILayout.Height (50));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("2", GUILayout.Width (50));
			EditorGUILayout.LabelField ("3", GUILayout.Width (50));
			EditorGUILayout.EndHorizontal ();
		}
        // задаем дополнительные параметры в зависимости от выбранного типа предмета
        switch (itemType)
        {
            case Item.ItemType.Weapon:

                WF.attack = EditorGUILayout.FloatField("Attack", WF.attack);
                WF.weaponType = (Weapon.WeaponType)EditorGUILayout.EnumPopup("Weapon Type", WF.weaponType);
                folder = "Weapons"; // Папка для сохранения префаба
                break;

            case Item.ItemType.Armor:

                AF.armor = EditorGUILayout.FloatField("Armor", AF.armor);
                AF.armorType = (Armor.ArmorType)EditorGUILayout.EnumPopup("Armor Type", AF.armorType);
                folder = "Armor"; // Папка для сохранения префаба
                break;
            case Item.ItemType.SpellBook:
                folder = "SpellBooks";
                break;
        }
        EditorGUILayout.LabelField("Description");
        EditorGUILayout.BeginVertical(EditorStyles.textArea);
        description = EditorGUILayout.TextField(description, EditorStyles.largeLabel);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();

		taking_slots = EditorGUILayout.DelayedIntField ("Take inventory slots", taking_slots);
		if(taking_slots != icons.Length) icons = new Sprite[taking_slots];
			
        // Сохраняем предемет с выбранными параметрами
        if (GUILayout.Button("Add"))
        {
            Debug.Log("Add Object");
            //GameObject newItem = PrefabUtility.CreateEmptyPrefab("Assets/Prefabs/Items/"+folder+"/"+itemName+".prefab") as GameObject;
            switch(itemType)
            {
			case Item.ItemType.Weapon:

				Weapon prefabWeapon = GameObject.Instantiate (ItemDataBase.dataBase.weaponPrefab).GetComponent<Weapon> ();
				prefabWeapon.name = itemName;
				prefabWeapon.itemType = itemType;
				prefabWeapon.icons = icons;
				prefabWeapon.weaponType = WF.weaponType;
                prefabWeapon.attack = WF.attack;
				prefabWeapon.gameObject.name = itemName;
				prefabWeapon.description = description;
				prefabWeapon.vertical = vertical;
                prefabWeapon.taking_slots = taking_slots;
                prefabWeapon.character = character;

                    if(!ItemDataBase.dataBase.Contain(prefabWeapon))
                    {
                        prefabWeapon.prefabPath = path + folder + "/" + itemName + ".prefab";
                        prefabWeapon.id = ItemDataBase.GetNextId();
                        prefabWeapon.prefab = PrefabUtility.CreatePrefab(path + folder + "/" + itemName + ".prefab", prefabWeapon.gameObject, ReplacePrefabOptions.Default);
                        ItemDataBase.dataBase.Add(prefabWeapon.prefab.GetComponent<Item>());
                    }
                    DestroyImmediate(prefabWeapon.gameObject);
                    break;
                case Item.ItemType.Armor:
                    Armor prefabArmor = GameObject.Instantiate(ItemDataBase.dataBase.armorPrefab).GetComponent<Armor>();
                    prefabArmor.name = itemName;
                    prefabArmor.itemType = itemType;
                    prefabArmor.icons = icons;
                    prefabArmor.armorType = AF.armorType;
                    prefabArmor.defence = AF.armor;
                    prefabArmor.gameObject.name = itemName;
                    prefabArmor.description = description;
                    prefabArmor.vertical = vertical;
                    prefabArmor.taking_slots = taking_slots;
                    prefabArmor.character = character;

                    if (!ItemDataBase.dataBase.Contain(prefabArmor))
                    {
                        prefabArmor.prefabPath = path + folder + "/" + itemName + ".prefab";
                        prefabArmor.id = ItemDataBase.GetNextId();
                        prefabArmor.prefab = PrefabUtility.CreatePrefab(path + folder + "/" + itemName + ".prefab", prefabArmor.gameObject, ReplacePrefabOptions.Default);
                        ItemDataBase.dataBase.Add(prefabArmor.prefab.GetComponent<Item>());
                    }

                    break;
            } 

        }
        EditorGUILayout.EndVertical();

    }

}
