using UnityEngine;
using System.Collections;

public class Animated : MonoBehaviour {
	public string spriteSheet;
	Sprite[] sprites;
	SpriteRenderer sRenderer;
	void Awake(){
		sprites = Resources.LoadAll<Sprite>(spriteSheet);
		sRenderer = GetComponent<SpriteRenderer> ();
	}
	// Update is called once per frame
	void LateUpdate () {
		
		string spriteName =  sRenderer.sprite.name.Substring(10,sRenderer.sprite.name.Length-10);
		Sprite newSprite = sprites [int.Parse (spriteName)];
		sRenderer.sprite = newSprite;
	}
}
