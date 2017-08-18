using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using Assets.UltimateIsometricToolkit.Scripts.Utils;

using UnityEngine;
using UnityEngine.UI;
public class EnemyController : LivingEntity, IController {

	IsoTransform target;
	Moveable moveable;

	[SerializeField]
	Transform hpBarPivot;

	Entity targetEntity;
	// Use this for initialization

	Weapon weapon;

	Animator[] animators;

	IsoTransform isoTransform;
	void Start () {
		moveable = GetComponent<Moveable> ();
		hpBar.maxValue = hp;
		hpBar.value = hp;
		weapon = GetComponent<Weapon> ();
		isoTransform = GetComponent<IsoTransform> ();
		animators = this.transform.GetChild (0).GetComponentsInChildren<Animator> ();

	}

	void OnTriggerEnter(Collider collider){
		if (collider.transform.tag == "Player"){
			target = collider.GetComponent<IsoTransform> ();
			targetEntity = collider.GetComponent<Entity> ();
		}
	}

	// Update is called once per frame
	void Update () {
		
		hpBar.transform.position = Camera.main.WorldToScreenPoint (hpBarPivot.position);

		if (target != null ) {
			if (Vector3.Distance (isoTransform.Position, target.Position) <= weapon.Range) {
				if (weapon.CanAttack) {
					weapon.StartAttack (target.Position);
				}
				moveable.Stop ();
			} else {
				moveable.Move (target.Position);
			}
		}
	}

	#region IController implementation

		public void StopAttack ()
		{
			weapon.EndAttack ();
			foreach (Animator animator in animators) {
				animator.SetBool ("Attack", false);
			}
		}

	#endregion

	public override void Die ()
	{
		Debug.Log ("Enemy is dead!");
		GameObject.Destroy (hpBar.gameObject);
		GameObject.Destroy (gameObject);
	}
}
