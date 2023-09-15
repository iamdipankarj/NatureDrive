namespace Solace {
  public class SteeringWheelVehicleData {
    public float vehicleSpeed;

    public float leftWheelLoad = 4400;
    public float leftWheelFrictionPresetZ = 1f;
    public float leftWheelLateralSlip;
    public float leftWheelSpringLength;
    public float leftWheelSpringMaxLength;

    public float rightWheelLoad = 4400;
    public float rightWheelFrictionPresetZ = 1f;
    public float rightWheelLongitudinalSlip;
    public float rightWheelSpringLength;
    public float rightWheelSpringMaxLength;

    public bool useRawInput;
  }
}
