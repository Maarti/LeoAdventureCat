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
	void Defend (float damage, float bumpelocity);
}

public interface IPatroller
{
	void Patrol ();
}