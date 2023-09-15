using UnityEngine;

namespace Solace {
  public class ForceFeedbackSettings : MonoBehaviour {
    /// <summary>
    /// Overall force feedback strength for this vehicle.
    /// </summary>
    [Range(0, 3)]
    public float overallCoeff = 1f;

    /// <summary>
    /// Friction strength for this vehicle.
    /// </summary>
    [Range(0, 3)]
    public float frictionCoeff = 1f;

    /// <summary>
    /// Low speed friction strength for this vehicle.
    /// </summary>
    [Range(0, 3)]
    public float lowSpeedFrictionCoeff = 1f;

    /// <summary>
    /// Self aligning torque strength for this vehicle.
    /// </summary>
    [Range(0, 3)]
    public float satCoeff = 1f;

    /// <summary>
    /// Centering force coefficient for this vehicle.
    /// </summary>
    [Range(0, 3)]
    public float centeringCoeff = 1f;
  }
}
