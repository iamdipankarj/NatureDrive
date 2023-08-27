using System;
using TMPro;
using UnityEngine;

namespace Solace {
  public class Stopwatch : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI timerText;
    private bool timerActive = true;
    private float currentTime = 0f;
    private TimeSpan timeSpan;

    public void StartTimer() {
      timerActive = true;
    }

    public void StopTimer() {
      timerActive = false;
    }

    void Start() {
    
    }

    void Update() {
      if (timerActive) {
        currentTime += Time.deltaTime;
      }
      timeSpan = TimeSpan.FromSeconds(currentTime);
      timerText.text = timeSpan.ToString("mm\\:ss\\:ff");
    }
  }
}
