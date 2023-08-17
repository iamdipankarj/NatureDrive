using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

namespace Solace {
  public class HorizontalSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public TMP_Text selection;
    public Button leftCaret;
    public Button rightCaret;

    public int currentIndex = 1;

    public List<string> options = new() {
      "2160x1920",
      "1920x1080",
      "1024x768"
    };

    public void OnPointerEnter(PointerEventData eventData) {
      selection.color = ColorManager.accentColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
      selection.color = ColorManager.defaultColor;
    }

    void Start() {
      //ClearAll();
      AddListeners();
      //foreach (var option in options) {
      //  TMP_Text text = Instantiate(optionText);
      //  text.name = option;
      //  text.SetText(option);
      //  text.transform.SetParent(optionsContainer);
      //  text.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
      //}
      UpdateSelectionText(currentIndex);
    }

    private void AddListeners() {
      leftCaret.onClick.AddListener(OnLeftCaretClick);
      rightCaret.onClick.AddListener(OnRightCaretClick);
    }

    private void OnLeftCaretClick() {
      currentIndex--;
      if (currentIndex < 0) {
        currentIndex = options.Count - 1;
        UpdateSelectionText(currentIndex);
      } else {
        UpdateSelectionText(currentIndex);
      }
    }

    private void OnRightCaretClick() {
      currentIndex++;
      if (currentIndex > options.Count - 1) {
        currentIndex = 0;
        UpdateSelectionText(currentIndex);
      } else {
        UpdateSelectionText(currentIndex);
      }
    }

    private void RemoveListeners() {
      leftCaret.onClick.RemoveListener(OnLeftCaretClick);
      rightCaret.onClick.RemoveListener(OnRightCaretClick);
    }

    private void UpdateSelectionText(int index) {
      string text = options.ElementAt(index);
      selection.text = text;
      //for (int i = 0; i < options.Count; i++) {
      //  Transform child = optionsContainer.GetChild(i);
      //  if (child != null) {
      //    if (i == index) {
      //      child.gameObject.SetActive(true);
      //      currentSelection = child.gameObject.GetComponent<TextMeshProUGUI>();
      //      if (isFocused) {
      //        ChangeToActiveColor(currentSelection);
      //      }
      //    } else {
      //      child.gameObject.SetActive(false);
      //      if (isFocused) {
      //        ChangeToDefaultColor(child.gameObject.GetComponent<TextMeshProUGUI>());
      //      }
      //    }
      //  }
      //}
    }

    //private void ClearAll() {
    //  foreach (Transform child in optionsContainer) {
    //    Destroy(child.gameObject);
    //  }
    //}

    private void OnDisable() {
      RemoveListeners();
    }
  }
}
