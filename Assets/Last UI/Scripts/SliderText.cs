using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LastUI {
  public class SliderText : MonoBehaviour {

    public TextMeshProUGUI sliderValue;
    public Slider slider;
    public bool specialEnd = false;
    public float endCap = 90f;

    public void Update() {
      if (specialEnd && slider.value > endCap) {
        sliderValue.text = "Uncapped";
      } else {
        sliderValue.text = slider.value.ToString("0");
        if (slider.value >= slider.maxValue - 0.05f || slider.value <= slider.minValue + 0.05f) {
          sliderValue.text = slider.value.ToString("0");
        }
      }
    }
  }
}