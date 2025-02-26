using UnityEngine;
using UnityEngine.UI;

namespace LastUI {
  [RequireComponent(typeof(Button))]
  public class StateSwitcher : MonoBehaviour {

    public CanvasType ChangeCanvasTo;

    StateManager stateManager;
    Button menuButton;

    private void Start() {
      menuButton = GetComponent<Button>();
      menuButton.onClick.AddListener(OnButtonClicked);
      stateManager = StateManager.instance;
    }

    public void OnButtonClicked() {
      StartCoroutine(stateManager.PlayNextCanvasAnimation(ChangeCanvasTo));
    }
  }
}