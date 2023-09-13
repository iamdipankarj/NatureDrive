using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MSVehicle;

public class UI_ChangeVehiclesMSVS : MonoBehaviour {

	MSSceneController controls; //main scene Controller
	bool error;

	Button nextVehicle;
	Button previousVehicle;

	bool playerIsNull;
	int nextVehicleInt;
	int nextIndex;

	bool canCheckUI;

	void Start () {
		controls = GameObject.FindObjectOfType (typeof(MSSceneController)) as MSSceneController;
		if (!controls) {
			Debug.LogError ("There must be an object with the 'MSSceneController' component so that vehicles can be managed.");
			error = true;
			this.transform.gameObject.SetActive (false);
			return;
		}
		if (controls.error) {
			error = true;
			this.transform.gameObject.SetActive (false);
			return;
		}

		// UI.Find interactive
		nextVehicle = transform.Find ("Canvas/Default/nextVehicle").GetComponent<Button> ();
		previousVehicle = transform.Find ("Canvas/Default/previousVehicle").GetComponent<Button> ();

		if (nextVehicle) {
			nextVehicle.onClick = new Button.ButtonClickedEvent ();
			nextVehicle.onClick.AddListener (() => NextVehicle ());
		}
		if (previousVehicle) {
			previousVehicle.onClick = new Button.ButtonClickedEvent ();
			previousVehicle.onClick.AddListener (() => PreviousVehicle ());
		}

		canCheckUI = false;
		StartCoroutine ("YieldToCheckUI");

		CheckIfPlayerIsNull ();
		EnableUI (true);
	}

	void CheckIfPlayerIsNull(){
		playerIsNull = false;
		if (!controls.player) {
			playerIsNull = true;
		}
	}

	IEnumerator YieldToCheckUI(){
		yield return new WaitForSeconds (0.2f);
		canCheckUI = true;
	}

	void Update () {
		if (canCheckUI) {
			bool inside = false;
			if (controls.vehicleCode._vehicleState == MSVehicleController.ControlState.isPlayer) {
				inside = true;
			}
			EnableUI (inside);
		}
	}

	void EnableUI(bool enable){
		if (controls.vehicles.Count < 2) {
			nextVehicle.gameObject.SetActive(false);
			previousVehicle.gameObject.SetActive (false);
		} else {
			nextVehicle.gameObject.SetActive(enable);
			previousVehicle.gameObject.SetActive (enable);
		}
	}

	public void PreviousVehicle(){
		if (!error && controls.vehicles.Count > 1) {
			CheckIfPlayerIsNull ();
			//
			if (playerIsNull) {
				controls.currentVehicle--;
				EnableVehicle (controls.currentVehicle + 1);
			} else {
				if (!controls.player.gameObject.activeInHierarchy) {
					controls.currentVehicle--;
					EnableVehicle (controls.currentVehicle + 1);
				}
			}
		}
	}
	 
	public void NextVehicle(){
		if (!error && controls.vehicles.Count > 1) {
			CheckIfPlayerIsNull ();
			//
			if (playerIsNull) {
				controls.currentVehicle++;
				EnableVehicle (controls.currentVehicle - 1);
			} else {
				if (!controls.player.gameObject.activeInHierarchy) {
					controls.currentVehicle++;
					EnableVehicle (controls.currentVehicle - 1);
				}
			}
		}
	}

	void EnableVehicle(int index){
		//index = value that was in variable 'controls.currentVehicle'
		controls.currentVehicle = Mathf.Clamp (controls.currentVehicle, 0, controls.vehicles.Count - 1);
		if (index != controls.currentVehicle) {
			if (controls.vehicles [controls.currentVehicle]) {
				if (controls.vehicles [controls.currentVehicle].activeInHierarchy) {
					//change vehicle
					controls.vehicles [index].GetComponent<MSVehicleController> ().ExitTheVehicle();
					controls.vehicles [controls.currentVehicle].GetComponent<MSVehicleController> ().EnterInVehicle (true);//true = isplayer
				} 
				else {
					nextVehicleInt = controls.currentVehicle - index;
					nextIndex = -1;
					// 
					if (nextVehicleInt > 0) { //next Vehicle
						for (int x = controls.currentVehicle; x < controls.vehicles.Count; x++) {
							if (controls.vehicles [x].activeInHierarchy) {
								nextIndex = x;
								break;
							}
						}  
					} else { //previous Vehicle
						for (int x = controls.currentVehicle; x >= 0; x--) { 
							if (controls.vehicles [x].activeInHierarchy) {
								nextIndex = x; 
								break;
							}
						}
					}
					//

					//enable vehicle in nextIndex element of array
					if (nextIndex >= 0) {
						controls.currentVehicle = nextIndex;
						if (controls.vehicles [controls.currentVehicle]) {
							//change vehicle
							controls.vehicles [index].GetComponent<MSVehicleController> ().ExitTheVehicle ();
							controls.vehicles [controls.currentVehicle].GetComponent<MSVehicleController> ().EnterInVehicle (true);//true = isplayer
						} else {
							controls.currentVehicle = index;
						}
					} else {
						controls.currentVehicle = index;
					}
				}
				//
				canCheckUI = false;
				StartCoroutine ("YieldToCheckUI");
			}
		}
	}
}
