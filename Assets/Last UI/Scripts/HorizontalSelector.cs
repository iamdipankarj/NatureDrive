using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LastUI {

  //This script handles horizontal selector.
  public class HorizontalSelector : MonoBehaviour {
    private TextMeshProUGUI text;
    private int m_Index = 0;
    public delegate void SelectionHandler(int index);
    public event SelectionHandler OnValueChanged;

    public int index {
      get {
        return m_Index;
      }
      set {
        m_Index = value;
        text.text = data[m_Index];
      }
    }

    public int defalutValueIndex = 0;

    public List<string> data = new();

    public string value {
      get {
        return data[m_Index];
      }
    }

    void Start() {
      text = transform.Find("txt_text").GetComponent<TextMeshProUGUI>();
      if (data.Count > 0) {
        index = defalutValueIndex;
      }
    }

    private void OnEnable() {
      transform.Find("btn_left").GetComponent<Button>().onClick.AddListener(OnLeftClicked);
      transform.Find("btn_right").GetComponent<Button>().onClick.AddListener(OnRightClicked);
    }

    private void OnDisable() {
      transform.Find("btn_left").GetComponent<Button>().onClick.RemoveListener(OnLeftClicked);
      transform.Find("btn_right").GetComponent<Button>().onClick.RemoveListener(OnRightClicked);
    }

    void OnLeftClicked() {
      if (index == 0) {
        index = data.Count - 1; //Index starts from 0
      } else {
        index--;
      }
      OnValueChanged?.Invoke(index);
    }

    void OnRightClicked() {
      if (index + 1 >= data.Count) {
        index = 0;
      } else {
        index++;
      }
      OnValueChanged?.Invoke(index);
    }
  }
}