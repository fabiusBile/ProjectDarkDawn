using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEditor;


public class Animated : MonoBehaviour {
	string spritePath;
	Sprite[] sprites;
	SpriteRenderer sRenderer;
	public Texture2D spriteSheet;
	PlayerController player;

	void Awake(){
		spritePath = AssetDatabase.GetAssetPath (spriteSheet);
		spritePath = spritePath.Replace (".png", "");
		spritePath = spritePath.Replace ("Assets/Resources/", "");
		sprites = Resources.LoadAll<Sprite>(spritePath);

		sRenderer = GetComponent<SpriteRenderer> ();
		player = transform.parent.parent.GetComponent<PlayerController> ();
	}
	// Update is called once per frame
	void LateUpdate () {

		// Так как для всех спрайтов персонажей и экипировки используются атласы с одинаковым расположением спрайтов
		// вместо необходимости заново настраивать анимацию для каждого предмета во время игры выбираются спрайты с тем же номером, что и у
		// тела персонажа
		if (sprites.Length > 0) {
			string baseSpriteName = sRenderer.sprite.name;

			// Отделение номера спрайта от его имени

			string spriteName = Regex.Match (baseSpriteName, @"\d*$").Value;

			//Выбор из атласа спрайта с тем же номером, что и у тела в данный кадр анимации 

			Sprite newSprite = sprites [int.Parse (spriteName)];
			sRenderer.sprite = newSprite;
		}

	}
	public void StopAttack(){
		player.StopAttack ();
	}
}
