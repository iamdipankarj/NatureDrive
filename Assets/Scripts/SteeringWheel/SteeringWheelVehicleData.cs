namespace Solace {
  public class SteeringWheelVehicleData {
    public float vehicleSpeed;

    public float leftWheelLoad = 4400;
    public float leftWheelFrictionPresetZ = 0.7f;
    public float leftWheelLateralSlip = -0.02f;
    public float leftWheelSpringLength = 0.3f;
    public float leftWheelSpringMaxLength = 0.4f;

    public float rightWheelLoad = 4400;
    public float rightWheelFrictionPresetZ = 0.7f;
    public float rightWheelLongitudinalSlip = 2.4f;
    public float rightWheelSpringLength = 0.3f;
    public float rightWheelSpringMaxLength = 0.4f;

    public bool useRawInput;
  }
}
