using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
	[SerializeField]
	private AI enemy;
	private Transform enemyTransform;

	Vector2 playerVec2;
	float angle;

	void Start()
	{
		enemyTransform = enemy.transform;
	}

	void Update()
	{
		LookMouse();
	}

	void LookMouse()
	{
		if (enemy.IsAttack) return;
		
		playerVec2 = (Vector2)InGameManager.Instance.player.transform.position
			- (Vector2)enemyTransform.position;
		angle = Mathf.Atan2(playerVec2.y, playerVec2.x) * Mathf.Rad2Deg;

		if (enemyTransform.localScale.x == -1)
		{
			angle *= -1;
			this.transform.rotation = Quaternion.AngleAxis(180 - angle, Vector3.forward);
		}
		else this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
