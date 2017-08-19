using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Utils;
using Assets.UltimateIsometricToolkit.Scripts.Core;

public class MeleeWeapon : Weapon {

	[SerializeField]
	float damage;
	[SerializeField]
	float damageModifier = 1;

	[SerializeField]
	float radius;
	[SerializeField]
	float radiusModifier = 1;


	List<LivingEntity> attackedTargets;
	public float Radius {
		get {
			return radius * radiusModifier;
		}
	}

	public float Damage {
		get {
			return damage*damageModifier;
		}
	}

	protected bool AttackStarted = false;


	public void Update(){
		if (AttackStarted)
			DoAttack ();
	}

	[SerializeField]
	float attackStartDistance;


	IsoTransform isoTransform;

	void Start(){
		isoTransform = transform.root.GetComponent<IsoTransform> ();
		base.Start ();
	}

	void StartDoDamage(){
		AttackStarted = true;
		attackedTargets = new List<LivingEntity> ();
	}

	override public void EndAttack(){
		base.EndAttack ();
		AttackStarted = false;
	}

	override protected void DoAttack ()
	{
		LayerMask layerMask = 1 << LayerMask.NameToLayer ("floor");
		layerMask = ~layerMask;
		Vector3 direction = target - isoTransform.Position;

		direction.Normalize ();
		//direction.y = 1;

		Vector3 attackStartPos = isoTransform.Position + direction * attackStartDistance;
		//attackStartPos.y = 1;


		Vector3 _direction = Isometric.IsoToScreen (direction);
		Vector3 _attackStartPos = Isometric.IsoToScreen (attackStartPos);
		RaycastHit[] hits = Physics.SphereCastAll(_attackStartPos,Radius,_direction, Range,layerMask);
		Debug.DrawRay (_attackStartPos, _direction*Range, Color.red,2f);

		//Vector3 cylDir = _direction;
		//cylDir.y = 0;
		//DebugDraw.DrawCylinder (_attackStartPos, Quaternion.LookRotation(cylDir)*Quaternion.Euler(Vector3.right * 90), new Vector3(Radius,Range,Radius), Color.cyan);
		foreach (RaycastHit hit in hits){

			Transform hitTransform = hit.collider.transform;

			if (hitTransform != this.transform.root) {
				//Vector3 __direction = hit.collider.transform.position - attackStartPos;
				Debug.Log (hit.collider.name);
				try {
					LivingEntity en = hitTransform.GetComponent<LivingEntity>();
					if (!attackedTargets.Contains(en)){
						en.TakeDamage(Damage);
						attackedTargets.Add(en);
					}
				} catch {
					continue;
				}
			}

		}
		base.DoAttack ();
	}

}
