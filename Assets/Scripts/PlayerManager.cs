using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;
	void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one instance of PlayerManager found!");
			return;
		}

		instance = this;
	}

	public GameObject player;
}
