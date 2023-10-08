using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LastUI {
  public class StateManager : MonoBehaviour {
    public static StateManager instance;
    [Tooltip("You need to add all of your states in here.")]
    [Header("List of States")]
    [SerializeField]
    public List<GameObject> States = new();

    [Tooltip("Assign starting state in here.")]
    public CanvasType FirstCanvas;

    public Canvas mainCanvas;
    private Animator canvasAnimator;

    List<StateController> canvasControllerList;

    [HideInInspector]
    public StateController ActiveCanvas;
    [HideInInspector]
    public StateController PreviousCanvas;

    private InspectManager inspectManager;

    private void Awake() {
      if (instance == null) {
        instance = this;
      } else {
        Destroy(gameObject);
      }
      //DontDestroyOnLoad(gameObject);
      canvasAnimator = mainCanvas.GetComponent<Animator>();
    }

    private void Start() {
      foreach (GameObject states in States) {
        states.SetActive(true);
      }
      inspectManager = FindObjectOfType<InspectManager>();
      canvasControllerList = mainCanvas.GetComponentsInChildren<StateController>().ToList();
      canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
      StartCoroutine(PlayNextCanvasAnimation(FirstCanvas));
    }

    private void OnEnable() {
      StartCoroutine(PlayNextCanvasAnimation(FirstCanvas));
    }

    public void GoToNextCanvas(CanvasType _type) {
      if (ActiveCanvas != null) {
        ActiveCanvas.gameObject.SetActive(false);
      }
      inspectManager.DeactiveInspector();

      StateController NextCanvas = canvasControllerList.Find(x => x.canvasType == _type);
      if (NextCanvas != null) {

        PreviousCanvas = ActiveCanvas;
        NextCanvas.gameObject.SetActive(true);
        ActiveCanvas = NextCanvas;
        NextCanvas.GetComponent<StateController>().StartSelectable.Select();
      } else {
        Debug.LogWarning("The next canvas was not found!");
      }
    }

    public void GoToPreviousCanvas() {
      if (ActiveCanvas != null) {
        ActiveCanvas.gameObject.SetActive(false);
      }

      inspectManager.DeactiveInspector();

      StateController NextCanvas = canvasControllerList.Find(x => x.canvasType == ActiveCanvas.previousCanvas);

      Debug.Log(NextCanvas);

      if (ActiveCanvas.canvasType.canGoPreviousCanvas == true) {
        PreviousCanvas = ActiveCanvas;
        NextCanvas.gameObject.SetActive(true);
        ActiveCanvas = NextCanvas;
        NextCanvas.GetComponent<StateController>().StartSelectable.Select();

        //Debug.Log("Can go previous canvas.");

      } else {
        //Debug.Log("Can't go previous canvas.");
      }
      //Debug.Log("Go Back Performed");
    }



    public IEnumerator PlayNextCanvasAnimation(CanvasType _type) {
      canvasAnimator.Play("out_canvas");
      yield return new WaitForSeconds(0.1f);
      GoToNextCanvas(_type);
      canvasAnimator.Play("in_canvas");
    }

    public IEnumerator PlayPreviousCanvasAnimation() {
      canvasAnimator.Play("out_canvas");
      yield return new WaitForSeconds(0.1f);
      GoToPreviousCanvas();
      canvasAnimator.Play("in_canvas");
    }

    public void LeaveGame() {
      Application.Quit();
      Debug.Log("When you build, your game will close when submit this button.");
    }
  }
}