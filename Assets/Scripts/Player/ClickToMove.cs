using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {
	// Update is called once per frame
	Moveable mov;
	void Start(){
		mov = gameObject.GetComponent<Moveable>();
	}
	void Update () {
		if (Input.GetButtonDown ("Fire2")) 
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 5f;

			Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

			Collider2D[] col = Physics2D.OverlapPointAll(v);

			if(col.Length > 0){
				mov.Move (col [0].transform.position);
			}
		}
	}
}
