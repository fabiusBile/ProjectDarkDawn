using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
public class Animated : MonoBehaviour {
	public string spriteSheet;
	Sprite[] sprites;
	SpriteRenderer sRenderer;
	public bool isWeapon;
	public bool isClothe;
	PlayerController player;

	void Awake(){
		sprites = Resources.LoadAll<Sprite>(spriteSheet);
		sRenderer = GetComponent<SpriteRenderer> ();
		player = transform.parent.parent.GetComponent<PlayerController> ();
	}
	// Update is called once per frame
	void LateUpdate () {

		// Так как для всех спрайтов персонажей и экипировки используются атласы с одинаковым расположением спрайтов
		// вместо необходимости заново настраивать анимацию для каждого предмета во время игры выбираются спрайты с тем же номером, что и у
		// тела персонажа

		string baseSpriteName = sRenderer.sprite.name;

		// Отделение номера спрайта от его имени

		string spriteName = Regex.Match (baseSpriteName, @"\d*$").Value;

		//Выбор из атласа спрайта с тем же номером, что и у тела в данный кадр анимации 

		Sprite newSprite = sprites [int.Parse (spriteName)];
		sRenderer.sprite = newSprite;
	}
	public void StopAttack(){
		player.StopAttack ();
	}
}
