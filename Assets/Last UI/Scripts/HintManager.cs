using TMPro;
using UnityEngine;

namespace LastUI {
  public class HintManager : MonoBehaviour {
    public string[] hints;

    [SerializeField]
    private TextMeshProUGUI hintText;

    private void Start() {
      hintText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
      string randomHint = hints[Random.Range(0, hints.Length)];

      hintText.text = randomHint;
    }
  }
}