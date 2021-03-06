﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
public class ClickToMove : MonoBehaviour {
	Moveable mov;
	public Tile curPos;
	void Start(){
		mov = gameObject.GetComponent<Moveable>();

	}
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Tile> ()) {
			curPos = other.GetComponent<Tile> ();
		}
	}
	void Update () {



		//При нажатии ПКМ
		if (Input.GetButtonDown ("Fire2")) 
		{
			Vector3 mousePosition = Input.mousePosition; 
			mousePosition.z = 5f;

			Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition); 
			Vector2 start = Camera.main.ScreenToWorldPoint(transform.position); 
			Collider2D[] col = Physics2D.OverlapPointAll(v); //Определеяем место клика
			Collider2D[] startCol = Physics2D.OverlapPointAll(start);
			if (col.Length > 0) {
				if (col [0].GetComponent<IsoTransform> ()) {
					mov.Move (col [0].GetComponent<IsoTransform> ().Position);
				}
			}
//			if(col.Length > 0){
//				Transform[,] tm = GameObject.Find ("Level").GetComponent<LevelGeneration.LevelGeneration> ().transformMap;
//				List<Point> path = AStarPathfinder.FindPath(GameObject.Find("Level").GetComponent<LevelGeneration.LevelGeneration>().map,curPos.pos, col[0].GetComponent<Tile>().pos);
//				foreach (Point p in path) {
//					tm [p.x, p.y].GetComponent<SpriteRenderer> ().color = Color.red;
//				}
//				mov.Move (col [0].transform.position); //Если под место клика был тайл - двигаемся в эту точку 
//			}
		}
	}
}
