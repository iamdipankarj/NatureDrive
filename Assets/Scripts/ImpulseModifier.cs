using Cinemachine;
using UnityEngine;

namespace Solace {
  [RequireComponent(typeof(CinemachineImpulseSource))]
  public class ImpulseModifier : MonoBehaviour {
    private CinemachineImpulseSource source;

    void Start() {
      source = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision collision) {
      if (collision.gameObject.TryGetComponent<Rigidbody>(out var result)) {
        GenerateImpulse(result.velocity);
      }
    }

    public void GenerateImpulse(Vector3 velocity) {
      source.GenerateImpulseWithVelocity(velocity);
    }

    void Update() {
    
    }
  }
}
