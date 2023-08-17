using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.InputSystem;

namespace Solace {
  public class HorizontalSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler {
    public TMP_Text selection;
    public Image leftCaret;
    public Image rightCaret;

    private Sprite leftCaretSprite;
    private Sprite rightCaretSprite;
    private Sprite leftCaretActiveSprite;
    private Sprite rightCaretActiveSprite;

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
      UpdateSelectionText();
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
      RegisterInputActions();
    }

    public void OnPointerExit(PointerEventData eventData) {
      selection.color = ColorManager.defaultColor;
      ActivateCarets(false);
      DeregisterInputActions();
    }

    private void ActivateCarets(bool active) {
      if (active) {
        leftCaret.sprite = leftCaretActiveSprite;
        rightCaret.sprite = rightCaretActiveSprite;
      } else {
        leftCaret.sprite = leftCaretSprite;
        rightCaret.sprite = rightCaretSprite;
      }
    }

    public void OnSelect(BaseEventData eventData) {
      selection.color = ColorManager.accentColor;
      ActivateCarets(true);
      RegisterInputActions();
      AudioManager.instance.PlayClickClip();
    }

    public void OnDeselect(BaseEventData eventData) {
      selection.color = ColorManager.defaultColor;
      ActivateCarets(false);
      DeregisterInputActions();
    }

    private void OnLeftCaretClick() {
      currentIndex--;
      if (currentIndex < 0) {
        currentIndex = options.Count - 1;
        UpdateSelectionText();
      } else {
        UpdateSelectionText();
      }
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context) {
      OnRightCaretClick();
    }

    private void OnRightCaretClick() {
      currentIndex++;
      if (currentIndex > options.Count - 1) {
        currentIndex = 0;
        UpdateSelectionText();
      } else {
        UpdateSelectionText();
      }
    }

    public void UpdateSelectionText() {
      if (currentIndex >= 0 && currentIndex < options.Count) {
        string text = options.ElementAt(currentIndex);
        selection.text = text;
      }
    }

    public string GetCurrentSelected() {
      return options.ElementAt(currentIndex);
    }

    private void RegisterInputActions() {
      inputActions.UI.Navigate.performed += OnNavigatePeformed;
      inputActions.UI.Submit.performed += OnSubmitPerformed;
      inputActions.UI.Click.performed += OnSubmitPerformed;
    }

    private void DeregisterInputActions() {
      inputActions.UI.Navigate.performed -= OnNavigatePeformed;
      inputActions.UI.Submit.performed -= OnSubmitPerformed;
      inputActions.UI.Click.performed -= OnSubmitPerformed;
    }

    private void OnEnable() {
      inputActions.Enable();
    }

    private void OnDisable() {
      inputActions.Disable();
      DeregisterInputActions();
    }

    public void OnSubmit(BaseEventData eventData) {
      AudioManager.instance.PlayFocusClip();
    }
  }
}
