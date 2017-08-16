using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using UnityEngine;

public class EnemyController : Entity, ILiving {

	IsoTransform target;
	Moveable moveable;
	[SerializeField]
	private float hp;
	Entity targetEntity;
	// Use this for initialization
	void Start () {
		moveable = GetComponent<Moveable> ();
	}

	void OnTriggerEnter(Collider collider){
		if (collider.transform.tag == "Player"){
			target = collider.GetComponent<IsoTransform> ();
			targetEntity = collider.GetComponent<Entity> ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (target != null) {
			moveable.Move (target.Position);
			if (targetEntity is ILiving) {
				ILiving living = (ILiving)targetEntity;
				living.TakeDamage (0.01f);
			}
		}
	}

	#region ILiving implementation

	public void TakeDamage (float damageAmount)
	{
		hp -= damageAmount;

		if (hp <= 0) {
			Die ();
		}
	}

	public void Die ()
	{
		Debug.Log ("Enemy is dead!");
		GameObject.Destroy (gameObject);
	}

	public float Hp {
		get {
			return hp;
		}
	}


	#endregion
}
