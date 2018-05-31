using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phasing : MonoBehaviour {

	public float startDelay;
	public float phaseIncrement;

	void Start()
	{
		InvokeRepeating("Phase", startDelay, phaseIncrement);
	}
	void Phase()
	{
		if(gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
		}
	}
		
}
