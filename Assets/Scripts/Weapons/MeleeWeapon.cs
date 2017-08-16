using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon {

	[SerializeField]
	float damage;

	[SerializeField]
	float range;

	[SerializeField]
	float radius;

	[SerializeField]
	float attackStartDistance;

	override protected void DoAttack ()
	{
		Debug.Log ("I attack!");



		LayerMask layerMask = 1 << LayerMask.NameToLayer ("floor");
		layerMask = ~layerMask;
		Vector3 direction = target - transform.position;
		direction.Normalize ();

		Vector3 attackStartPos = transform.position + direction * attackStartDistance;

		RaycastHit[] hits = Physics.SphereCastAll(attackStartPos,radius,direction, range,layerMask);
		foreach (RaycastHit hit in hits){

			Transform hitTransform = hit.collider.transform;
			if (hitTransform != this.transform) {
				Vector3 _direction = hit.collider.transform.position - attackStartPos;
				Debug.DrawRay (attackStartPos, _direction, Color.red, 1f);
				Debug.Log (hit.collider.name);
				try {
					Entity en = hitTransform.GetComponent<Entity>();
					if (en is ILiving){
						((ILiving) en).TakeDamage(damage);
					} 
				} catch {
					continue;
				}
			}

		}
		base.DoAttack ();
	}

}
