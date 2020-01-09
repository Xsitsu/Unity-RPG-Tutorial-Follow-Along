using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
	public float attackSpeed = 1f;
	private float attackCooldown = 0f;

	public float attackDelay = 0.6f;

	public event System.Action OnAttack;

	CharacterStats stats;

	void Start()
	{
		stats = GetComponent<CharacterStats>();
	}

	void Update()
	{
		attackCooldown -= Time.deltaTime;
	}

	public void Attack(CharacterStats targetStats)
	{
		if (attackCooldown < 0f)
		{
			StartCoroutine(DoDamage(targetStats, attackDelay));

			if (OnAttack != null)
			{
				OnAttack();
			}

			attackCooldown = 1f / attackSpeed;
		}
	}

	IEnumerator DoDamage(CharacterStats targetStats, float delay)
	{
		yield return new WaitForSeconds(delay);
		targetStats.TakeDamage(stats.damage.GetValue());
	}
}
