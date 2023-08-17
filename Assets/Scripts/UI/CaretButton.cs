using UnityEngine;
using UnityEngine.EventSystems;

namespace Solace {
  public class CaretButton : MonoBehaviour, ISelectHandler  {
    public void OnSelect(BaseEventData eventData) {
      ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

    // Start is called before the first frame update
    void Start() {
    
    }

    // Update is called once per frame
    void Update() {
    
    }
  }
}
