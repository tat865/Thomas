using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCycle : MonoBehaviour {

    public float spinSpeed;

	void Update ()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, spinSpeed) * Time.deltaTime);
    }
}
