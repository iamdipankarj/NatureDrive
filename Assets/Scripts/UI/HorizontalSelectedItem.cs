using System.Collections;
using TMPro;
using UnityEngine;

namespace Solace {
  public enum FadeDirection {
    TO_LEFT,
    TO_RIGHT
  }

  public class HorizontalSelectedItem : MonoBehaviour {
    private const float destroyDuration = 0.8f;
    private Coroutine disableCoroutine;

    void Start() {
    
    }

    public void FadeOut() {
      disableCoroutine = StartCoroutine(DisableFaded());
    }

    private IEnumerator DisableFaded() {
      float time = 0;
      TextMeshPro renderer = GetComponent<TextMeshPro>();
      Color startColor = renderer.material.color;
      Color endColor = new(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0f);
      while (time < 1) {
        renderer.material.color = Color.Lerp(startColor, endColor, time);
        yield return null;
        time += Time.deltaTime * destroyDuration;
      }
      gameObject.SetActive(false);
    }

    private void OnDisable() {
      if (disableCoroutine != null) {
        StopCoroutine(disableCoroutine);
      }
    }

    void Update() {
    
    }
  }
}
