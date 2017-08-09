using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using System;
public class DataBaseExplorer : EditorWindow
{

    Vector2 scrollPos;
	bool showWeapon = true, showArmor =  false, showOther = false;
    [MenuItem("Items/DataBase")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(DataBaseExplorer));
        window.maxSize = window.maxSize;
        window.minSize = window.minSize;
        window.ShowPopup();
    }

    private void OnGUI()
    {
		// Фильтры
		EditorGUILayout.BeginHorizontal (GUILayout.Width(100));
		EditorGUILayout.BeginVertical();
		showWeapon = EditorGUILayout.Toggle ("Weapon", showWeapon);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical (GUILayout.Width(100));
		showArmor = EditorGUILayout.Toggle ("Armor", showArmor);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical (GUILayout.Width(100));
		showOther = EditorGUILayout.Toggle ("Other", showOther);
		EditorGUILayout.EndVertical();

		// Шапка таблицы
		EditorGUILayout.EndHorizontal ();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("ID", GUILayout.Width(40),GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Icon", GUILayout.Width(150), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Name", GUILayout.Width(150), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("States", GUILayout.Width(50), GUILayout.ExpandWidth(false));
		EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Prefab", GUILayout.Width(100), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Description", GUILayout.Width(250), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Delete", GUILayout.Width(150), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        string description = "", name = "";
		Sprite[] icons;
        bool changed = false;
		bool showItem = false;
		float states = 0;
        string statesName = "";

		int taking_slots;
        float iconWidth = 0;
		// дописать статы
        foreach (Item it in ItemDataBase.dataBase.itemDataBase.ToArray())
        {
            showItem = false;
			if (it.itemType == Item.ItemType.Weapon && showWeapon) 
			{
				showItem = true;
				Weapon w = (Weapon)it;
				states = w.attack;
                statesName = "Attack";
			}
            else if (it.itemType == Item.ItemType.Armor && showArmor) 
			{
				showItem = true;
			    Armor ar = (Armor)it;
				states = ar.defence;
                statesName = "Armor";
			} 
            else if (it.itemType == Item.ItemType.OtherStuff && showOther) 
			{
				showItem = true;
			}
		if (showItem) 
		{
				
			
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            Texture2D texture;
            if (it.icons[0] == null) texture = ItemDataBase.dataBase.emptyIcon.texture; // иконки дописать 
            else texture = it.icons[0].texture;

            // ID
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(it.id.ToString(), GUILayout.Width(40));
            EditorGUILayout.EndVertical();

            // Иконка
            EditorGUILayout.BeginVertical(GUILayout.Width(150));

			icons = it.icons;
			taking_slots = it.taking_slots;
            iconWidth = 120 / taking_slots;
				if (taking_slots == 1) 
				{
					icons [0] = (Sprite)EditorGUILayout.ObjectField (it.icons [0], typeof(Sprite), GUILayout.Width(40),GUILayout.Height(40));
				} 
				// Если расположение может быть вертикальным или горизонтальным
				else if (taking_slots > 1 && taking_slots < 4) 
				{
					EditorGUILayout.BeginVertical ();
					// вертикальное расположение
					if (it.vertical) 
					{
                        GUILayout.BeginVertical();
						for (int i = 0; i < taking_slots; i++) 
						{
                            EditorGUILayout.BeginHorizontal();
                            icons [i] = (Sprite)EditorGUILayout.ObjectField (it.icons [i], typeof(Sprite), GUILayout.Height(40),GUILayout.Width(40));
                            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(10));
                            EditorGUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    } 
					// горизонтальное 
					else 
					{
						EditorGUILayout.BeginHorizontal ();
                        for (int i = 0; i < taking_slots; i++)
                        {
                            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(40));
                        }
                        EditorGUILayout.EndHorizontal ();

						EditorGUILayout.BeginHorizontal ();
                        for (int i = 0; i < taking_slots; i++)
                        {
                            icons[i] = (Sprite)EditorGUILayout.ObjectField(icons[i], typeof(Sprite), GUILayout.Width(40), GUILayout.Height(40));
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
					EditorGUILayout.LabelField ("0", GUILayout.Width (40));
					EditorGUILayout.LabelField ("1", GUILayout.Width (40));
					EditorGUILayout.EndHorizontal ();

					EditorGUILayout.BeginHorizontal ();
                    icons[0] = (Sprite)EditorGUILayout.ObjectField(icons[0], typeof(Sprite), GUILayout.Width(40), GUILayout.Height(40));
                    icons[1] = (Sprite)EditorGUILayout.ObjectField(icons[1], typeof(Sprite), GUILayout.Width(40), GUILayout.Height(40));
                    EditorGUILayout.EndHorizontal ();

                    EditorGUILayout.BeginHorizontal ();
                    icons[2] = (Sprite)EditorGUILayout.ObjectField(icons[2], typeof(Sprite), GUILayout.Width(40), GUILayout.Height(40));
                    icons[3] = (Sprite)EditorGUILayout.ObjectField(icons[3], typeof(Sprite), GUILayout.Width(40), GUILayout.Height(40));
                    EditorGUILayout.EndHorizontal ();

					EditorGUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("2", GUILayout.Width (40));
					EditorGUILayout.LabelField ("3", GUILayout.Width (40));
					EditorGUILayout.EndHorizontal ();
				}
            if (icons[0] != it.icons[0] && icons[0] != null)
            {
                it.icons[0] = icons[0];
                changed = true;
            }
            EditorGUILayout.EndVertical();

            //имя предмета
            EditorGUILayout.BeginVertical();
            name = EditorGUILayout.DelayedTextField(it.name, GUILayout.Width(150));
            if (name.Length > 5 && name != it.name)
            {
                it.name = name;
                changed = true;
            }

            EditorGUILayout.EndVertical();

			// Статы
			EditorGUILayout.BeginVertical();
				states = EditorGUILayout.DelayedFloatField(states,GUILayout.Width(50));
				EditorGUILayout.LabelField (statesName, GUILayout.Width(50));
                if(it.itemType == Item.ItemType.Weapon)
                {
                   Weapon w = (Weapon)it;
                   if (states != w.attack)
                   {
                       w.attack = states;
                       changed = true;
                   }
                }
                else if (it.itemType == Item.ItemType.Armor)
                {
                        Armor ar = (Armor)it;
                        if(states != ar.defence)
                        {
                            ar.defence = states;
                            changed = true;
                        }
                }


			EditorGUILayout.EndVertical();
            // префаб
            EditorGUILayout.BeginVertical();
            EditorGUILayout.ObjectField(it, typeof(Item), it, GUILayout.Width(100));
            EditorGUILayout.EndVertical();
            // описание
            EditorGUILayout.BeginVertical();
            description = EditorGUILayout.DelayedTextField(it.description, GUILayout.Width(250), GUILayout.Height(50));
            if (description.Length > 5 && description != it.description)
            {
                it.description = description;
                changed = true;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Delete", GUILayout.Width(150)))
            {
                FileUtil.DeleteFileOrDirectory(Application.dataPath.Replace("Assets", "") + it.prefabPath);
                ItemDataBase.dataBase.Remove(it);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            if (changed) ItemDataBase.dataBase.ReplacePrefab();
            changed = false;
		}
        }
        EditorGUILayout.EndScrollView();

    }
}

