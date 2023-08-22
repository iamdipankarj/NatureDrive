using UnityEngine;
using UnityEngine.EventSystems;

namespace Solace {
  public class CaretButton : MonoBehaviour, ISelectHandler  {
    public void OnSelect(BaseEventData eventData) {
      ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    void Start() {
    
    }

    void Update() {
    
    }
  }
}
