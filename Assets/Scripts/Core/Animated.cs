using UnityEngine;
using System.Collections;

public class Animated : MonoBehaviour {
	public string spriteSheet;
	Sprite[] sprites;
	SpriteRenderer renderer;
	void Awake(){
		sprites = Resources.LoadAll<Sprite>(spriteSheet);
		renderer = GetComponent<SpriteRenderer> ();
	}
	// Update is called once per frame
	void LateUpdate () {
		
		string spriteName =  renderer.sprite.name.Substring(10,renderer.sprite.name.Length-10);
		Debug.Log (spriteName);
		Sprite newSprite = sprites [int.Parse (spriteName)];
		renderer.sprite = newSprite;
	}
}
