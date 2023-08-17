using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.InputSystem;
using System;

namespace Solace {
  public class HorizontalSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
    public TMP_Text selection;
    public Image leftCaret;
    public Image rightCaret;

    private Sprite leftCaretSprite;
    private Sprite rightCaretSprite;
    private Sprite leftCaretActiveSprite;
    private Sprite rightCaretActiveSprite;

    private bool isFocused = false;

    SolaceInputActions inputActions;

    public int currentIndex = 1;

    public List<string> options = new() {
      "2160x1920",
      "1920x1080",
      "1024x768"
    };

    private void Awake() {
      inputActions = new();
    }

    void Start() {
      UpdateSelectionText(currentIndex);
      leftCaretSprite = Resources.Load<Sprite>("Sprites/CaretLeft");
      rightCaretSprite = Resources.Load<Sprite>("Sprites/CaretRight");
      leftCaretActiveSprite = Resources.Load<Sprite>("Sprites/CaretLeftActive");
      rightCaretActiveSprite = Resources.Load<Sprite>("Sprites/CaretRightActive");
    }

    private void OnNavigatePeformed(InputAction.CallbackContext context) {
      float value = context.ReadValue<float>();
      if (value < 0f) {
        OnLeftCaretClick();
      } else if (value > 0f) {
        OnRightCaretClick();
      }
    }

    public void OnPointerEnter(PointerEventData eventData) {
      selection.color = ColorManager.accentColor;
      ActivateCarets(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
      selection.color = ColorManager.defaultColor;
      ActivateCarets(false);
    }

    private void ActivateCarets(bool active) {
      if (active) {
        isFocused = true;
        leftCaret.sprite = leftCaretActiveSprite;
        rightCaret.sprite = rightCaretActiveSprite;
      } else {
        isFocused = false;
        leftCaret.sprite = leftCaretSprite;
        rightCaret.sprite = rightCaretSprite;
      }
    }

    public void OnSelect(BaseEventData eventData) {
      selection.color = ColorManager.accentColor;
      ActivateCarets(true);
      EnableInputActions();
    }

    public void OnDeselect(BaseEventData eventData) {
      selection.color = ColorManager.defaultColor;
      ActivateCarets(false);
      DisableInputActions();
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

    private void OnSubmitPerformed(InputAction.CallbackContext context) {
      if (isFocused) {
        OnRightCaretClick();
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

    private void UpdateSelectionText(int index) {
      string text = options.ElementAt(index);
      selection.text = text;
    }

    private void EnableInputActions() {
      inputActions.Enable();
      inputActions.UI.Navigate.performed += OnNavigatePeformed;
      inputActions.UI.Submit.performed += OnSubmitPerformed;
      inputActions.UI.Click.performed += OnSubmitPerformed;
    }

    private void DisableInputActions() {
      inputActions.Disable();
      inputActions.UI.Navigate.performed -= OnNavigatePeformed;
      inputActions.UI.Submit.performed -= OnSubmitPerformed;
      inputActions.UI.Click.performed -= OnSubmitPerformed;
    }

    private void OnDisable() {
      DisableInputActions();
    }
  }
}
