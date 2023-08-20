using TMPro;
using UnityEngine;

namespace Solace {
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class VehicleSpeedUIController : MonoBehaviour {
    private TextMeshProUGUI textComponent;
    void Start() {
      textComponent = GetComponent<TextMeshProUGUI>();
      CarController.DidUpdateCarSpeed += OnCarSpeedChange;
    }

    private void OnDisable() {
      CarController.DidUpdateCarSpeed -= OnCarSpeedChange;
    }

    private void OnCarSpeedChange(int speed) {
      textComponent.text = speed.ToString();
    }
  }
}
