using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Suspension mesh - MS Vehicle System 
public class MSVSSuspensionSpringMesh : MonoBehaviour {

	[Header("Spring mesh (Z-Axis compression)")]
	[Tooltip("In this variable must be associated the 'spring mesh' linked to this wheel. The mesh should have its pivot on the top of the spring, and it should stretch on the Z axis to work properly.")]
	public GameObject _springMesh;
	[Header("WheelCollider for this spring")]
	[Tooltip("In this variable must be associated the 'wheelCollider' component linked to this spring.")]
	public WheelCollider _wheelCollider;
	//
	[Space(10)][Range(-3.0f, 3.0f)][Tooltip("Here it is possible to set the point where the spring will be supported. It is the point where the spring will be pointing.")]
	public float _springPointOffset = 0;
	[Tooltip("Use this variable to adjust the size of the spring. Sometimes the object will get bigger or smaller than desired, and in this variable it is possible to change the scale.")]
	public float _sizeAdjustment = 1;
	[Tooltip("If this variable is true, the spring will point to the configured green point. If this variable is false, the spring will just stretch, but it will stand still in the position where it was placed.")]
	public bool lookAt = true;

	void OnValidate(){
		if (_sizeAdjustment < 0.01f) {
			_sizeAdjustment = 0.01f;
		}
	}

	void Update () {
		if (_wheelCollider) {
			Vector3 _wheelColliderPosition;
			Quaternion _wheelColliderRotation;
			_wheelCollider.GetWorldPose (out _wheelColliderPosition, out _wheelColliderRotation);
			Vector3 _springFinalPoint = _wheelColliderPosition + _wheelCollider.transform.right * _springPointOffset;
			//
			if (_springMesh) {
				if (lookAt) {
					_springMesh.transform.LookAt (_springFinalPoint);
				}
				float distance = Vector3.Distance(_springMesh.transform.position, _springFinalPoint) * _sizeAdjustment;
				_springMesh.transform.localScale = new Vector3 (_springMesh.transform.localScale.x, _springMesh.transform.localScale.y, distance);
			}
		}
	}

	void OnDrawGizmosSelected(){
		if (_wheelCollider) {
			Vector3 _wheelColliderPosition;
			Quaternion _wheelColliderRotation;
			_wheelCollider.GetWorldPose (out _wheelColliderPosition, out _wheelColliderRotation);
			Vector3 _springFinalPoint = _wheelColliderPosition + _wheelCollider.transform.right * _springPointOffset;
			//
			Gizmos.color = Color.red;
			Gizmos.DrawLine (_wheelColliderPosition, _springFinalPoint);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere (_springFinalPoint, 0.05f);

		}
	}
}
