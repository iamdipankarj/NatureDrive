using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRuteExample : MonoBehaviour {

	public List<Vector3> positions = new List<Vector3> ();
	public float moveSpeed = 10;

	int index = 0;

	void Update () {
		if (positions.Count > 0) {
			transform.position = Vector3.MoveTowards (transform.position, positions [index], Time.deltaTime * moveSpeed);
			if (Vector3.Distance (transform.position, positions [index]) < 0.1f) {
				index++;
				if (index >= positions.Count) {
					index = 0;
				}
			}
		}
	}

	void OnDrawGizmosSelected (){
		if (positions.Count > 0) {
			Gizmos.color = Color.green;
			for (int x = 0; x < positions.Count; x++) {
				Gizmos.DrawSphere (positions[x], 0.5f);
			}
		}
		if (positions.Count > 1) {
			Gizmos.color = Color.red;
			for (int x = 1; x < positions.Count; x++) {
				Gizmos.DrawLine (positions [x-1], positions [x]);
			}
			Gizmos.DrawLine (positions [0], positions [positions.Count -1]);
		}
	}
}
