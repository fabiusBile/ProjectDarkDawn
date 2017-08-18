using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : Entity{

	[SerializeField]
	protected float hp;

	[SerializeField]
	protected float maxHp;

	[SerializeField]
	protected Slider hpBar;


	public float HP {
		get {
			return hp;
		}
	}

	public float MaxHp {
		get {
			return maxHp;
		}
	}

	virtual public void Start(){
		if (hpBar != null) {
			hpBar.maxValue = maxHp;
			hpBar.value = hp;
		}
	}

	public void TakeDamage (float damageAmount)
	{
		if (hp > 0) {
			hp -= damageAmount;
		}

		if (hpBar != null) {
			hpBar.value = hp;
		}

		if (hp <= 0) {
			Die ();
		}

	}

	public abstract void Die ();

}
