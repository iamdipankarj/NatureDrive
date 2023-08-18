using UnityEngine;

namespace Solace {
  public class CarInputController : MonoBehaviour {
    [HideInInspector]
    public bool isAcceleratingForward;
    [HideInInspector]
    public bool isAcceleratingBackward;
    [HideInInspector]
    public bool isTurningLeft;
    [HideInInspector]
    public bool isTurningRight;
    [HideInInspector]
    public bool isPressingHandbrake;
    [HideInInspector]
    public bool isReleasingHandbrake;

    void Start() {
    
    }

    // Update is called once per frame
    void Update() {
      isAcceleratingForward = Input.GetKey(KeyCode.W);
      isAcceleratingBackward = Input.GetKey(KeyCode.S);
      isTurningLeft = Input.GetKey(KeyCode.A);
      isTurningRight = Input.GetKey(KeyCode.D);
      isPressingHandbrake = Input.GetKey(KeyCode.Space);
      isReleasingHandbrake = Input.GetKeyUp(KeyCode.Space);
    }
  }
}
