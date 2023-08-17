using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Solace {
  public class HorizontalSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Transform optionsContainer;
    public TMP_Text placeholder;
    public TMP_Text optionText;

    private TextMeshProUGUI currentSelection;
    private bool isFocused = false;

    public Button leftCaret;
    public Button rightCaret;

    public int currentIndex = 1;

    public List<string> options = new() {
      "2160x1920",
      "1920x1080",
      "1024x768"
    };

    public void OnPointerEnter(PointerEventData eventData) {
      currentSelection.color = ColorManager.accentColor;
      isFocused = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
      currentSelection.color = ColorManager.defaultColor;
      isFocused = false;
    }

    private void ChangeToActiveColor(TextMeshProUGUI text) {
      text.color = ColorManager.accentColor;
    }

    private void ChangeToDefaultColor(TextMeshProUGUI text) {
      text.color = ColorManager.defaultColor;
    }

    void Start() {
      ClearAll();
      AddListeners();
      foreach (var option in options) {
        TMP_Text text = Instantiate(optionText);
        text.name = option;
        text.SetText(option);
        text.transform.SetParent(optionsContainer);
        text.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
      }
      SetActiveIndex(currentIndex);
    }

    private void AddListeners() {
      leftCaret.onClick.AddListener(OnLeftCaretClick);
      rightCaret.onClick.AddListener(OnRightCaretClick);
    }

    private void OnLeftCaretClick() {
      currentIndex--;
      if (currentIndex < 0) {
        currentIndex = optionsContainer.childCount - 1;
        SetActiveIndex(currentIndex);
      } else {
        SetActiveIndex(currentIndex);
      }
    }

    private void OnRightCaretClick() {
      currentIndex++;
      if (currentIndex > optionsContainer.childCount - 1) {
        currentIndex = 0;
        SetActiveIndex(currentIndex);
      } else {
        SetActiveIndex(currentIndex);
      }
    }

    private void RemoveListeners() {
      leftCaret.onClick.RemoveListener(OnLeftCaretClick);
      rightCaret.onClick.RemoveListener(OnRightCaretClick);
    }

    private void SetActiveIndex(int index) {
      for (int i = 0; i < options.Count; i++) {
        Transform child = optionsContainer.GetChild(i);
        if (child != null) {
          if (i == index) {
            child.gameObject.SetActive(true);
            currentSelection = child.gameObject.GetComponent<TextMeshProUGUI>();
            if (isFocused) {
              ChangeToActiveColor(currentSelection);
            }
          } else {
            child.gameObject.SetActive(false);
            if (isFocused) {
              ChangeToDefaultColor(child.gameObject.GetComponent<TextMeshProUGUI>());
            }
          }
        }
      }
    }

    private void ClearAll() {
      placeholder.gameObject.SetActive(false);
      foreach (Transform child in optionsContainer) {
        Destroy(child.gameObject);
      }
    }

    private void OnDisable() {
      RemoveListeners();
    }
  }
}
