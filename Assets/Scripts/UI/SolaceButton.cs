using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Solace {
  public class SolaceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ISubmitHandler {
    private TextMeshProUGUI textObj;
    private Color defaultColor;

    void Start() {
      textObj = GetComponent<TextMeshProUGUI>();
      defaultColor = textObj.color;
    }

    public void OnPointerEnter(PointerEventData eventData) {
      textObj.color = ColorManager.accentColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
      textObj.color = defaultColor;
    }

    public void OnSelect(BaseEventData eventData) {
      textObj.color = ColorManager.accentColor;
      AudioManager.instance.PlayClickClip();
    }

    public void OnDeselect(BaseEventData eventData) {
      textObj.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData) {
      
    }

    public void OnSubmit(BaseEventData eventData) {
      AudioManager.instance.PlayFocusClip();
    }
  }
}
