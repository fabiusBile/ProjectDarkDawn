using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
public class CantAttackException : UnityException{
	public CantAttackException() : base() { }
	public CantAttackException(string message) : base(message) { }
	public CantAttackException(string message, System.Exception inner) : base(message, inner) { }

	protected CantAttackException(System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) { }
}


abstract public class Weapon : MonoBehaviour {


	[SerializeField]
	public float cooldown = 0;

	bool canAttack = true;

	public bool CanAttack {
		get {
			return canAttack;
		}
	}


	public Vector3 target;

	Animator[] animators;

	public void Awake(){
		animators=this.transform.GetChild (0).GetComponentsInChildren<Animator> ();
	}

	public void StartAttack(Vector3 target){
		if (canAttack) {
			this.target =  Isometric.IsoToScreen(target);
			BeforeAttack ();
			DoAttack ();
			AfterAttack ();
			canAttack = false;
		} else {			
			throw new CantAttackException ("Cant attack now, wait for cooldown");
		}
	}

	protected void SetAttackAnim(){
		foreach (Animator anim in animators) {
			anim.SetBool ("Attack", true);
		}
	}

	protected virtual void BeforeAttack(){
		SetAttackAnim ();
	}

	protected virtual void DoAttack(){}

	protected virtual void AfterAttack(){}

	public void EndAttack(){
		StartCoroutine (MakeCooldown());
	}

	IEnumerator MakeCooldown(){
		yield return new WaitForSeconds(cooldown);
		canAttack = true;
	}
}
