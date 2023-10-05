using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;
using Solace;

namespace LastUI {
  public class UIInputManager : MonoBehaviour {
    //private LastUIInputActions lastuiInputActions;
    private StateManager stateManager;

    private Player player;

    [Tooltip("Assign whatever button you want it to start with selected in here.")]
    public Button FirstSelectedButton;

    [HideInInspector] public GameObject SelectedButton;

    private void OnEnable() {
      //lastuiInputActions.Enable();
    }

    private void OnDisable() {
      //lastuiInputActions.Disable();
    }

    public void Awake() {
      player = ReInput.players.GetPlayer(0);
      //lastuiInputActions = new LastUIInputActions();
      stateManager = StateManager.instance;
      SelectedButton = FirstSelectedButton.gameObject;
    }

    void Start() {
      FirstSelectedButton.Select();
      //lastuiInputActions.LastUI.Cancel.performed += ctx => IfCancelPressed();
      //lastuiInputActions.LastUI.Cancel.performed += ctx => Debug.Log("Go Back Pressed");
      //lastuiInputActions.LastUI.Navigate.performed += ctx => ChangeSliderValue(ctx.ReadValue<Vector2>());
      //lastuiInputActions.LastUI.Submit.performed += ctx => SubmitPerformed();
    }

    private void Update() {
      SelectedButton = EventSystem.current.currentSelectedGameObject;
      if (player.GetButtonDown("UICancel")) {
        Debug.Log("Go Back Pressed");
        IfCancelPressed();
      }
      if (SelectedButton != null) {
        if (player.GetButtonDown("UIHorizontalLeft")) {
          ChangeSliderValue(-1);
        } else if (player.GetButtonDown("UIHorizontalRight")) {
          ChangeSliderValue(1);
        }
      }
      if (player.GetButtonDown("UISubmit")) {
        SubmitPerformed();
      }
    }

    void IfCancelPressed() {
      if (stateManager.ActiveCanvas.canvasType.canGoPreviousCanvas == true) {
        StartCoroutine(stateManager.PlayPreviousCanvasAnimation());
      }
    }

    void ChangeSliderValue(float direction) {
      if (SelectedButton.TryGetComponent(out ItemController controller)) {
        if (controller.itemType == ItemController.ItemTypes.HorizontalSelector) {
          if (direction == -1) {
            SelectedButton.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.Invoke();
          }
          if (direction == 1) {
            SelectedButton.transform.GetChild(0).GetChild(1).GetComponent<Button>().onClick.Invoke();
          }
        }
      }
    }

    void SubmitPerformed() {
      if (SelectedButton.TryGetComponent(out ItemController controller)) {
        if (controller.itemType == ItemController.ItemTypes.Toggle) {
          if (SelectedButton.GetComponentInChildren<Toggle>().isOn == true) {
            SelectedButton.GetComponentInChildren<Toggle>().isOn = false;
          } else {
            SelectedButton.GetComponentInChildren<Toggle>().isOn = true;
          }
        }
      }
    }

    public void SelectObject(Selectable select) {
      select.Select();
    }
  }
}