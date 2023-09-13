using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MSVehicle;

[RequireComponent(typeof(MSVehicleController))]
public class MSAIFollowTargetExample : MonoBehaviour {

	[Tooltip("The target the vehicle should follow. The vehicle will try to follow the target whenever it moves.")]
	public Transform _target;
	[Range(10,420)][Tooltip("The maximum speed the vehicle will reach while trying to follow the target.")]
	public float vehicleSpeedKMh = 38;

	MSVehicleController _AILogicComponent;
	float logicAiVerticalInput;
	float vehicleCurrentSpeed;
	float speedToFollow;

	void Start () {
		_AILogicComponent = GetComponent<MSVehicleController> ();
	}

	void Update () {
		if (_target) {
			_AILogicComponent._AISettings.AIIsActive = true;

			//horizontal input
			Vector3 _AIPointPos = new Vector3 (_target.position.x, 0, _target.position.z); //ignore Y axis
			Vector3 _VehiclePos = new Vector3 (transform.position.x, 0, transform.position.z); //ignore Y axis
			Vector3 direction = (_AIPointPos - _VehiclePos).normalized; 
			Vector3 _vehicleForword = new Vector3 (transform.forward.x, 0, transform.forward.z);
			float _AIPointSingleAngle = Vector3.SignedAngle (direction, _vehicleForword, Vector3.up);
			_AILogicComponent._AISettings._AIHorizontalInput = Mathf.Clamp (-_AIPointSingleAngle / _AILogicComponent._AISettings.vehicleMaxSteerAngle, -1, 1);

			//vertical input
			vehicleCurrentSpeed = _AILogicComponent.KMh;
			float distanceToTarget = Vector3.Distance (transform.position, _target.position);
			speedToFollow = (vehicleSpeedKMh - (10/distanceToTarget));
			float firstInterval = speedToFollow - 2.5f;

			if (vehicleCurrentSpeed < firstInterval) {
				logicAiVerticalInput += Time.deltaTime * 0.25f;
				logicAiVerticalInput = Mathf.Clamp(logicAiVerticalInput, -1.0f, 1.0f);
			}
			if (vehicleCurrentSpeed >= firstInterval && vehicleCurrentSpeed < speedToFollow) {
				logicAiVerticalInput = 0;
			}
			if (vehicleCurrentSpeed >= speedToFollow) {
				logicAiVerticalInput -= Time.deltaTime * 0.25f;
				logicAiVerticalInput = Mathf.Clamp(logicAiVerticalInput, -1.0f, 1.0f);
			}
			_AILogicComponent._AISettings._AIVerticalInput = logicAiVerticalInput;
		} else {
			_AILogicComponent._AISettings.AIIsActive = false;
			_AILogicComponent._AISettings._AIVerticalInput = 0;
			_AILogicComponent._AISettings._AIHorizontalInput = 0;
		}
			
		/*
		//OTHER USEFUL FUNCTIONS
		_AILogicComponent.AIHornInput(); //vehicle horn
		_AILogicComponent.AISuspensionHeight(); //change suspension Height
		_AILogicComponent.AIMainLights();
		_AILogicComponent.AIHeadLights();
		_AILogicComponent.AIFlashesRightAlert();
		_AILogicComponent.AIFlashesLeftAlert();
		_AILogicComponent.AIWarningLight ();
		_AILogicComponent.AIExtraLights ();
		*/
	}
}
