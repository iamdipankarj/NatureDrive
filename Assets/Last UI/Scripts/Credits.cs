using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour {
  public float speed = 100f;
  public float creditsPosBegin = -825f;
  public float boundaryTextEnd = 825f;

  RectTransform creditsRect;

  [SerializeField]
  private bool isLooping = false;

  void OnEnable() {
    creditsRect = GetComponent<RectTransform>();
    StartCoroutine(AutoScroll());
  }

  void OnDisable() {
    creditsRect.localPosition = new Vector3(creditsRect.localPosition.x, creditsPosBegin, creditsRect.localPosition.z);
  }

  IEnumerator AutoScroll() {
    while (creditsRect.localPosition.y < boundaryTextEnd) {
      creditsRect.Translate(speed * Time.deltaTime * Vector3.up);
      if (creditsRect.localPosition.y > boundaryTextEnd) {
        if (isLooping) {
          creditsRect.localPosition = Vector3.up * creditsPosBegin;
        } else {
          break;
        }
      }
      yield return null;
    }
  }
}
