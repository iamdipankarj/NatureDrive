using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace LastUI {
  public class SwitchHandler : MonoBehaviour {
    private const float AnimationSpeed = 9f;

    [Header("References")]
    [SerializeField] RectTransform HandleImage;
    [SerializeField] TextMeshProUGUI indicator;

    Toggle toggle;

    Vector2 handlePosition;
    Vector2 positionOnState;
    Vector2 positionOffState;

    private Coroutine toggleCoroutine;

    private IEnumerator DoToggleOn() {
      float time = 0;
      while (time < 1) {
        Vector2 targetAnchor = Vector2.Lerp(positionOffState, positionOnState, time);
        HandleImage.anchoredPosition = targetAnchor;
        yield return null;
        time += Time.deltaTime * AnimationSpeed;
      }
    }

    private IEnumerator DoToggleOff() {
      float time = 0;
      while (time < 1) {
        Vector2 targetAnchor = Vector2.Lerp(positionOnState, positionOffState, time);
        HandleImage.anchoredPosition = targetAnchor;
        yield return null;
        time += Time.deltaTime * AnimationSpeed;
      }
    }

    void Awake() {
      toggle = GetComponent<Toggle>();

      handlePosition = HandleImage.anchoredPosition;
      positionOnState = handlePosition * -1;
      positionOffState = handlePosition;

      toggle.onValueChanged.AddListener(OnSwitch);

      OnSwitch(toggle.isOn);
    }

    public void OnSwitch(bool on) {
      indicator.text = on ? "ON" : "OFF";
      //if (toggleCoroutine != null) {
      //  StopCoroutine(toggleCoroutine);
      //}
      //toggleCoroutine = StartCoroutine(on ? DoToggleOn() : DoToggleOff());


      HandleImage.anchoredPosition = on ? handlePosition * -1 : handlePosition ; // no anim
      //HandleImage.DOAnchorPos(on ? handlePosition * -1 : handlePosition, AnimationSpeed).SetEase(Ease.InSine);
    }

    private void OnDisable() {
      if (toggleCoroutine != null) {
        StopCoroutine(toggleCoroutine);
      }
    }

    void OnDestroy() {
      toggle.onValueChanged.RemoveListener(OnSwitch);
    }
  }
}