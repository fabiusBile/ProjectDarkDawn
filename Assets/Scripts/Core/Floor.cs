using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

	[System.Serializable]
	public class FloorLook{
		public float probability;
		public Sprite sprite;

		[HideInInspector]
		public float probabilitySum;
	}
	public FloorLook[] floorLooks;

	// Use this for initialization
	void Start () {
		if (floorLooks.Length > 0) {
			floorLooks [0].probabilitySum = floorLooks [0].probability;
			float totalSum = floorLooks [0].probability;

			for (int i = 1; i != floorLooks.Length; i++) {
				floorLooks [i].probabilitySum = floorLooks[i - 1].probabilitySum + floorLooks[i].probability;
				totalSum += floorLooks [i].probabilitySum;
			}

			float random = Random.value * 100;

			foreach (FloorLook floorLook in floorLooks) {
				if (random < floorLook.probabilitySum) {
					GetComponent<SpriteRenderer> ().sprite = floorLook.sprite;
					break;
				}
			}
		}
	}
		

}
