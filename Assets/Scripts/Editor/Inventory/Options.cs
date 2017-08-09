using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Options : EditorWindow
{
    public static GameObject dataBase;
    public static GameObject weaponPrefab;
    public static Sprite emptyIcon;
    [MenuItem("Items/Options")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Options));
        dataBase = (GameObject)GameObject.FindGameObjectWithTag("ItemDataBase");
    }

    public void OnGUI()
    {
        dataBase = ItemDataBase.dataBase.gameObject;
        ItemDataBase db = dataBase.GetComponent<ItemDataBase>();
        weaponPrefab = (GameObject)EditorGUILayout.ObjectField("WeaponPrefab", db.weaponPrefab, typeof(GameObject), allowSceneObjects:true);
        if(weaponPrefab != db.weaponPrefab)
        {
            db.weaponPrefab = weaponPrefab;
            db.ReplacePrefab();
        }
        dataBase = (GameObject)EditorGUILayout.ObjectField("ItemDataBase", dataBase, typeof(GameObject), allowSceneObjects: true);
        emptyIcon = (Sprite)EditorGUILayout.ObjectField("MissingIcon", db.emptyIcon,typeof(Sprite),allowSceneObjects:true);
        if(emptyIcon != db.emptyIcon)
        {
            db.emptyIcon = emptyIcon;
            db.ReplacePrefab();
        }
        // дописать префабы для всех остальных шаблонов предметов
    }
}
