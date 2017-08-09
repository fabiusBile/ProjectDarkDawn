using System.Collections;
using System.Collections.Generic;

public interface ILiving {

	float Hp { get; }

	void TakeDamage (float damageAmount);

	void Die ();
}
