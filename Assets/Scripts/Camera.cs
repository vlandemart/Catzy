using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
	[SerializeField]
	private Vector2 maxPos;
	[SerializeField]
	private Vector2 minPos;

	private void Update()
	{
		Vector3 desiredPos = Player.Instance.transform.position;
		desiredPos.x = Mathf.Clamp(desiredPos.x, minPos.x, maxPos.x);
		desiredPos.y = Mathf.Clamp(desiredPos.y, minPos.y, maxPos.y);
		desiredPos.z = -10;
		transform.position = desiredPos;
	}
}
