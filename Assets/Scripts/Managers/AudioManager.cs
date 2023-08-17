using UnityEngine;

namespace Solace {
  public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    private AudioClip buttonFocusClip;
    private AudioClip buttonClickClip;
    private AudioSource audioSource;

    private void Awake() {
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }

    void Start() {
      buttonFocusClip = Resources.Load<AudioClip>("Audio/focus");
      buttonClickClip = Resources.Load<AudioClip>("Audio/click");
      audioSource = GetComponent<AudioSource>();
    }

    public void PlayFocusClip() {
      audioSource.PlayOneShot(buttonFocusClip);
    }

    public void PlayClickClip() {
      audioSource.PlayOneShot(buttonClickClip);
    }
  }
}
