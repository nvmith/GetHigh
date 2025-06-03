using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hand : MonoBehaviour
{
	float angle;
	Vector2 target, mouse;

	void Start()
    {
		target = transform.position;
	}

    void Update()
    {
		LookMouse();
	}

	void LookMouse()
	{
		if (InGameManager.Instance.player.isAttack) return;

		mouse = CameraController.Instance.Pointer;
		angle = CameraController.Instance.PlayerAngle;
		if (InGameManager.Instance.player.IsReverse)
		{
			angle *= -1;
            this.transform.rotation = Quaternion.AngleAxis(180 - angle, Vector3.forward);
        }
		else this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
