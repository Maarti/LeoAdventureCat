using UnityEngine;

public interface ISentinel
{
	GameObject CheckLoS ();
}

public interface IAttackable
{
	void Attack ();
}

public interface IDefendable
{
	void Defend (float damage, bool bump);
}

public interface IPatroller
{
	void Patrol ();
}