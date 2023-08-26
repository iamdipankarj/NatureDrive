using UnityEngine;

namespace Solace {
  public class Checkpoint : MonoBehaviour {
    private bool isTriggered = false;
    public int checkpointId;

    private void OnTriggerEnter(Collider other) {
      if (other.CompareTag(TagManager.PLAYER)) {
        if (!isTriggered) {
          isTriggered = true;
          SaveManager.instance.SaveCheckpoint(GameLevel.LEVEL_1, checkpointId);
        }
      }
    }

    private void OnDrawGizmos() {
      Gizmos.color = new Color(1, 0, 0, 0.4f);
      if (TryGetComponent<BoxCollider>(out var boxCollider)) {
        Gizmos.DrawCube(transform.position + boxCollider.center, new Vector3(boxCollider.size.x * transform.localScale.x, boxCollider.size.y, boxCollider.size.z));
      }
    }

    void Start() {
    }

    void Update() {
    
    }
  }
}
