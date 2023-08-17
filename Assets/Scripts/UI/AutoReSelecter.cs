using UnityEngine;
using UnityEngine.EventSystems;

namespace Solace {
  [RequireComponent(typeof(EventSystem))]
  public class AutoReSelecter : MonoBehaviour {
    private EventSystem eventSystem;
    private GameObject lastSelectedObject;

    void Awake() {
      eventSystem = gameObject.GetComponent<EventSystem>();
    }

    void Update() {
      if (eventSystem.currentSelectedGameObject == null) {
        eventSystem.SetSelectedGameObject(lastSelectedObject);
      } else {
        lastSelectedObject = eventSystem.currentSelectedGameObject;
      }
    }
  }
}
