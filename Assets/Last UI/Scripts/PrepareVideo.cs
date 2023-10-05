using UnityEngine;
using UnityEngine.Video;

namespace LastUI {
  public class PrepareVideo : MonoBehaviour {
    public VideoPlayer player;

    private void Start() {
      player = GetComponent<VideoPlayer>();
      player.Prepare();
    }
  }
}