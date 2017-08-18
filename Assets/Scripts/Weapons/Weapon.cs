using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using UnityEditor.Animations;


public class CantAttackException : UnityException{
	public CantAttackException() : base() { }
	public CantAttackException(string message) : base(message) { }
	public CantAttackException(string message, System.Exception inner) : base(message, inner) { }

	protected CantAttackException(System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) { }
}


abstract public class Weapon : MonoBehaviour {


	[SerializeField]
	protected float rangeModifier=1;

	[SerializeField]
	protected float range;

	public float Range {
		get {
			return range*rangeModifier;
		}
	}

	[SerializeField]
	public float cooldown = 0;

	bool canAttack = true;

	public bool CanAttack {
		get {
			return canAttack;
		}
	}

	private bool doDamage = false;

	public bool DoDamage {
		get {
			return DoDamage;
		}
	}

	public Vector3 target;

	Animator[] animators;

	public void Start(){
		
		animators=this.transform.root.GetComponentsInChildren<Animator> ();

	}

	public void StartAttack(Vector3 target){
		if (canAttack) {
			this.target =  target;
			SetAttackAnim ();
		} else {			
			throw new CantAttackException ("Cant attack now, wait for cooldown");
		}
	}

	protected void SetAttackAnim(){
		foreach (Animator anim in animators) {
			anim.SetBool ("Attack", true);
		}
	}
		
	public void Update(){
		if (doDamage)
			DoAttack ();
	}

	protected virtual void DoAttack(){}

	protected virtual void AfterAttack(){}

	void StartDoDamage(){
		doDamage = true;
	}

	public void EndAttack(){
		StartCoroutine (MakeCooldown());
		foreach (Animator anim in animators) {
			anim.SetBool ("Attack", false);
		}
		doDamage = false;
	}

	IEnumerator MakeCooldown(){
		yield return new WaitForSeconds(cooldown);
		canAttack = true;
	}
}
