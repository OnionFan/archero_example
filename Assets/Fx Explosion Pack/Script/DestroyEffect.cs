using System;
using UnityEngine;
using System.Collections;
using MOBRitual.Utils;

public class DestroyEffect : MonoBehaviour
{

	private float fixedTime;
	
	void Update ()
	{
		if (fixedTime < Time.unscaledTime)
		{
			ObjectPool.Recycle(gameObject);
		}
	}

	private void OnEnable()
	{
		fixedTime = Time.unscaledTime + 10.0f;
	}
}
