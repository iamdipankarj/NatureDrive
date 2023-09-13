using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLookAt : MonoBehaviour {

	public Transform target;

	void Update () {
		if (target) {
			transform.LookAt (target);
		}
	}
}
