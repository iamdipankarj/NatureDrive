using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MSVehicle {
  [Serializable]
  public class GroundFrictionClass {
    [Range(0.1f, 2)]
    [Tooltip("This is the standard friction that the wheels will have when they do not find a terrain defined by a tag(torque friction).")]
    public float standardForwardFriction = 1.5f;
    [Range(0.1f, 2)]
    [Tooltip("This is the standard friction that the wheels will have when they do not find a terrain defined by a tag(slip friction).")]
    public float standardSideFriction = 0.8f;
    [Tooltip("Here you must configure the terrain and the friction that the wheels will have in each of them.")]
    public GroundFrictionSubClass[] grounds;
  }

  [Serializable]
  public class GroundFrictionSubClass {
    public string name;
    [Space(10)]
    [Header("Ground detection")]
    [Tooltip("When the wheel finds this tag, it will receive the references from that index.")]
    public string groundTag;
    [Tooltip("When the wheel finds this 'PhysicMaterial', it will receive the references from that index.")]
    public PhysicMaterial physicMaterial;
    [Tooltip("When the wheel is on a terrain and finds some texture present in this index, it will receive the references from that index.")]
    public List<int> terrainTextureIndices = new List<int>();
    //
    [Space(10)]
    [Header("Ground settings")]
    [Range(0.1f, 2)]
    [Tooltip("In this variable you must adjust the friction that the wheels will have when they receive torque.")]
    public float forwardFriction = 1.0f;
    [Range(0.1f, 2)]
    [Tooltip("In this variable you must adjust the friction that the wheels will have when they slide.")]
    public float sidewaysFriction = 1.0f;
    [Range(0.05f, 1.0f)]
    [Tooltip("In this variable you must adjust the torque that the vehicle will have on the ground defined by the tag of this index.")]
    public float torqueInThisGround = 1.0f;
  }
  [Serializable]
  public class WheelRotationClass {
    [Tooltip("If this variable is true, the wheel associated with this index will receive rotation defined by the flywheel.")]
    public bool wheelTurn = false;
    [Tooltip("If this variable is true, the wheel associated with this index will invert its rotation, ie the opposite of what the steering wheel of the vehicle is passing to it.")]
    public bool reverseTurn = false;
    [Range(0.01f, 1.0f)]
    [Tooltip("This variable defines how much rotation of the steering wheel of the vehicle this wheel will receive. If it is a low value, the wheel will turn little compared to the steering wheel of the vehicle.")]
    public float angleFactor = 1;
  }
  [Serializable]
  public class WheelClass {
    [Tooltip("In this variable you must associate the mesh of the wheel of this class.")]
    public Transform wheelMesh;
    [Tooltip("In this variable you must associate the collider of the wheel of this class.")]
    public WheelCollider wheelCollider;
    [Tooltip("If this variable is true, this wheel will receive engine torque.")]
    public bool wheelDrive = true;
    [Tooltip("If this variable is true, this wheel will receive handbrake force.")]
    public bool wheelHandBrake = true;
    [Range(0.1f, 1.0f)]
    [Tooltip("In this variable you can define how much torque this wheel will receive from the engine.")]
    public float torqueFactor = 1.0f;
    [Range(-2.0f, 2.0f)]
    [Tooltip("In this variable you can set the horizontal offset of the sliding mark of this wheel.")]
    public float skidMarkShift = 0.0f;
    public enum WheelPosition { Right, Left }
    [Tooltip("In this variable you must define the position in which the wheel is relative to the vehicle. It will only take effect if the wheel is an 'extra wheel'.")]
    public WheelPosition wheelPosition = WheelPosition.Right;
    //
    [Space(5)]
    [Header("Wheel rotation")]
    [Tooltip("If this variable is true, the wheel associated with this index will receive rotation defined by the flywheel.")]
    public bool wheelTurn = false;
    [Tooltip("If this variable is true, the wheel associated with this index will invert its rotation, ie the opposite of what the steering wheel of the vehicle is passing to it.")]
    public bool reverseTurn = false;
    [Range(0.01f, 1.0f)]
    [Tooltip("This variable defines how much rotation of the steering wheel of the vehicle this wheel will receive. If it is a low value, the wheel will turn little compared to the steering wheel of the vehicle.")]
    public float angleFactor = 1;
    //
    [Space(5)]
    [Header("Custom friction")]
    [Tooltip("If this variable is true, the friction of the wheels can be adjusted by the 'adjustCustomFriction' class just below.")]
    public bool useCustomFriction = false;
    [Tooltip("Here you will adjust the friction of the wheels in relation to torque and slippage.")]
    public WheelFrictionClass adjustCustomFriction;
    //
    [Space(5)]
    [Header("Custom brand width")]
    [Tooltip("If this variable is true, the vehicle will generate traces with the width defined by the variable below, 'customBrandWidth'. Otherwise, the vehicle will generate traces according to the variable 'standardBrandWidth', present in the class 'skidmarks'.")]
    public bool useCustomBrandWidth = false;
    [Range(0.1f, 6.0f)]
    [Tooltip("This variable defines the custom width of the vehicle's skid trace.")]
    public float customBrandWidth = 0.3f;

    //
    [HideInInspector] public Vector3 wheelWorldPosition;
    [HideInInspector] public Mesh rendSKDmarks;
    [HideInInspector] public bool generateSkidBool;
    [HideInInspector] public float wheelColliderRPM;
    [HideInInspector] public float forwardSkid;
    [HideInInspector] public float sidewaysSkid;
  }

  [Serializable]
  public class VehicleWheelsClass {
    [Range(5, 50000)]
    [Tooltip("In this variable you can define the mass that the wheels will have. The script will leave all wheels with the same mass.")]
    public int wheelMass = 150;
    [Range(0.0f, 5.0f)]
    [Tooltip("In this variable you can adjust the influence that the differential will have on the wheels.")]
    public float differentialInfluence = 1.0f;
    [Space(10)]
    [Tooltip("If this variable is true, the code will adjust the 'ForwardFriction' and 'SidewaysFriction' frictions of the 'wheelCollider' through the values set in the code. If this variable is false, no friction settings will be changed.")]
    public bool setFrictionByCode = true;
    [Tooltip("Here you will adjust the friction of the wheels in relation to torque and slippage.")]
    public WheelFrictionClass defaultFriction;
    [Space(10)]
    [Tooltip("The front right wheel collider must be associated with this variable")]
    public WheelClass rightFrontWheel;
    [Tooltip("The front left wheel collider must be associated with this variable")]
    public WheelClass leftFrontWheel;
    [Tooltip("The rear right wheel collider should be associated with this variable")]
    public WheelClass rightRearWheel;
    [Tooltip("The rear left wheel collider should be associated with this variable")]
    public WheelClass leftRearWheel;
    [Tooltip("Extra wheel colliders must be associated with this class.")]
    public WheelClass[] extraWheels;
  }
  [Serializable]

  public class WheelFrictionClass {
    [Tooltip("Here you can adjust the friction of the wheels in relation to torque and brake.")]
    public FrictionFWClass ForwardFriction;
    [Tooltip("Here you can adjust the friction of the wheels in relation to the side sliding of the vehicle.")]
    public FrictionSWClass SidewaysFriction;
  }
  [Serializable]
  public class FrictionFWClass { //general values:  {1, 5, 5, 1}   or   {2, 5, 5, 2}   or   {2, 3, 3, 2}   or   {1, 1, 1, 1}   or others
    [Tooltip("In this variable you can set the 'ExtremumSlip' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float ExtremumSlip = 1.0f;
    [Tooltip("In this variable you can set the 'ExtremumValue' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float ExtremumValue = 1.5f;
    [Tooltip("In this variable you can set the 'AsymptoteSlip' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float AsymptoteSlip = 1.5f;
    [Tooltip("In this variable you can set the 'AsymptoteValue' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float AsymptoteValue = 1.0f;
  }
  [Serializable]
  public class FrictionSWClass { //general values:  {0.2f, 1.0f, 0.5f, 0.75f}   or others
    [Tooltip("In this variable you can set the 'ExtremumSlip' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float ExtremumSlip = 0.2f;
    [Tooltip("In this variable you can set the 'ExtremumValue' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float ExtremumValue = 1.0f;
    [Tooltip("In this variable you can set the 'AsymptoteSlip' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float AsymptoteSlip = 0.5f;
    [Tooltip("In this variable you can set the 'AsymptoteValue' parameter of the wheel collider. This parameter will be passed to the collider automatically when the vehicle starts.")]
    public float AsymptoteValue = 0.75f;
  }

  [Serializable]
  public class SuspensionAdjustmentClass {
    [Header("Settings")]
    [Range(-1.5f, 1.5f)]
    [Tooltip("This parameter defines the point where the wheel forces will applied. This is expected to be in metres from the base of the wheel at rest position along the suspension travel direction. When forceAppPointDistance = 0 the forces will be applied at the wheel base at rest. A better vehicle would have forces applied slightly below the vehicle centre of mass.")]
    public float forceAppPointDistance = 0.0f;
    [Range(7500, 15000000)]
    [Tooltip("In this variable you can define the hardness of the vehicle suspension.")]
    public int suspensionHardness = 45000;
    [Range(200, 7500000)]
    [Tooltip("In this variable you can define how much the suspension will swing after receiving some force.")]
    public int suspensionSwing = 2500;

    [Header("Height")]
    [Range(0.1f, 5.0f)]
    [Tooltip("Maximum extension distance of wheel suspension, measured in local space. Suspension always extends downwards through the local Y-axis. This value will be applied to the suspension when the vehicle is enabled.")]
    public float vehicleStartHeight = 0.25f;
    [Range(0.1f, 5.0f)]
    [Tooltip("Here you can set a custom height for the suspension. It is possible to change the height of the vehicle through the inputs.")]
    public float[] vehicleCustomHeights;

    //
    [HideInInspector]
    public float constVehicleHeightStart = 0.0f;
    [HideInInspector]
    public int indexCustomSuspensionHeight = 0;
  }

  [Serializable]
  public class VehicleBrakesClass {
    [Tooltip("If this variable is true, the vehicle brake system becomes ABS type.")]
    public bool ABS = true;
    [Range(0.01f, 1.0f)]
    [Tooltip("This variable defines the ABS brake force of the vehicle.")]
    public float ABSForce = 0.125f;
    [Tooltip("If this variable is true, the vehicle's handbrake will lock the vehicle's rear wheels instead of gently braking the vehicle.")]
    public bool handBrakeLock = true;
    [Tooltip("If this variable is true, the vehicle will lock the wheels automatically when the wheel rotation is too low.")]
    public bool brakingWithLowRpm = true;
    [Tooltip("If this variable is true, the wheels of the vehicle will receive a brake force when the player is not inside the vehicle. The force will only be applied if the vehicle is stationary.")]
    public bool brakeOnExitingTheVehicle = true;
    [Range(1.0f, 2.0f)]
    [Tooltip("In this variable, you can define how early the engine brake will start to act in the vehicle. The higher the value of this variable, the higher the speed limit the system will tolerate until it starts to apply a force.")]
    public float speedFactorEngineBrake = 1.0f;
    [Range(0.0f, 3.0f)]
    [Tooltip("In this variable you can set the brake force of the vehicle motor.")]
    public float forceEngineBrake = 1.5f;
    [Range(0.05f, 10.0f)]
    [Tooltip("In this variable you can set the brake force of the vehicle.")]
    public float vehicleBrakingForce = 0.6f;
    [Space(14)]
    [Tooltip("If this variable is true, the vehicle will brake the wheels smoothly.")]
    public bool brakeSlowly = false;
    [Range(0.2f, 5.0f)]
    [Tooltip("This variable defines how smooth the wheels will be braked. This parameter is valid only if the variable 'brakeSlowly' is true.")]
    public float speedBrakeSlowly = 1.0f;
  }

  [Serializable]
  public class VolantSettingsClass {
    [Header("Settings")]
    [Range(0.2f, 5.0f)]
    [Tooltip("In this variable you can define how fast the steering wheel of the vehicle will rotate. This directly implies the speed that the wheels will rotate.")]
    public float steeringWheelSpeed = 1.75f;
    [Range(5, 70)]
    [Tooltip("In this variable you can set the maximum angle that the vehicle wheels can reach.")]
    public float maxAngle = 25.0f;
    [Tooltip("If this variable is true, the handwheel will remain rotated instead of automatically returning to the starting position.")]
    public bool keepRotated = false;

    public enum SelectRotation { RotationInY, RotationInZ };
    [Space(10)]
    [Header("Steering Wheel Object")]
    [Tooltip("In this variable you can define in which axis the handwheel will rotate.")]
    public SelectRotation rotationType = SelectRotation.RotationInZ;
    [Tooltip("In this variable you must associate the object that represents the steering wheel of the vehicle. The pivot of the object must already be correctly rotated to avoid problems.")]
    public GameObject steeringWheelObject;
    [Range(0.5f, 2.5f)]
    [Tooltip("In this variable you can set the number of turns that the steering wheel of the vehicle will rotate.")]
    public float numberOfTurns = 0.75f;
    [Tooltip("If this variable is true, the steering wheel rotation is reversed. This does not affect the wheel rotation of the vehicle.")]
    public bool invertRotation = false;

    [Space(10)]
    [Header("Assistant")]
    [Range(0.0f, 2.0f)]
    [Tooltip("In this variable it is possible to define with which influence the code will control the steering wheel of the vehicle automatically, assisting the player. This significantly improves vehicle control.")]
    public float steeringAssist = 0.5f;
    [Range(0.0f, 1.0f)]
    [Tooltip("In this variable it is possible to define with which influence the final angle of the wheels of the vehicle will be affected by the movement and the speed of movement.")]
    public float steeringLimit = 0.4f;
    [Tooltip("If this variable is true, the steering assistant will intervene smoothly. It is advisable to use this option only if the steering wheel speed is high.")]
    public bool useLerp = false;
  }

  [Serializable]
  public class VehicleAdjustmentClass {
    [Range(500, 2000000)]
    [Tooltip("In this variable you must define the mass that the vehicle will have. Common vehicles usually have a mass around 1500")]
    public int vehicleMass = 2000;
    [Tooltip("If this variable is true, the vehicle will start with the engine running. But this only applies if the player starts inside this vehicle.")]
    public bool startOn = true;
    [Tooltip("If this variable is true, the vehicle starts braking.")]
    public bool startBraking = false;
    [Tooltip("If this variable is true, the vehicle starts the engine automatically when the player accelerates.")]
    public bool turnOnWhenAccelerating = true;
    [Range(0.1f, 2.0f)]
    [Tooltip("The time it takes to start the engine. If a sound is associated with the variable (Sounds > EngineStartSound), this time will be automatically adjusted to the audio duration time.")]
    public float delayToStartTheEngine = 1.5f;
    [Tooltip("If this variable is true, the vehicle's engine will die in low torque situations, for example, moving slowly at too high a gear. (Works in manual gears only).")]
    public bool engineDie = true;

    [Space(5)]
    [Header("Vehicle RPM")]
    [Range(400, 2000)]
    [Tooltip("In this variable it is possible to define the minimum RPM that the vehicle's engine can reach.")]
    public int minVehicleRPM = 850;
    [Range(2500, 7500)]
    [Tooltip("In this variable it is possible to define the maximum RPM that the vehicle's engine can reach.")]
    public int maxVehicleRPM = 7000;
    [HideInInspector]
    public float vehicleRPMValue;
    [HideInInspector]
    public AnimationCurve rpmCurve;
  }

  [Serializable]
  public class TorqueAdjustmentClass {
    [Range(20, 420)]
    [Tooltip("This variable sets the maximum speed that the vehicle can achieve. It must be configured on the KMh unit")]
    public int maxVelocityKMh = 200;
    [Range(2, 12)]
    [Tooltip("This variable defines the number of gears that the vehicle will have.")]
    public int numberOfGears = 7;
    [Range(0.1f, 0.8f)]
    [Tooltip("This variable defines how long the vehicle takes to change gears.")]
    public float gearShiftTime = 0.4f;
    [Range(0.5f, 2.1f)]
    [Tooltip("This variable defines the speed range of each gear. The higher the range, the faster the vehicle goes, however, the torque is relatively lower.")]
    public float speedOfGear = 1.3f;
    [Range(0.0f, 0.2f)]
    [Tooltip("In this variable it is possible to configure the decrease of the torque that the vehicle will have when changing gears. The higher the gear, the lower the torque the vehicle can transmit to the wheels, and this variable controls this.")]
    public float decreaseTorqueByGear = 0.05f;
    [Range(0.2f, 15.0f)]
    [Tooltip("How much the engine RPM will affect the vehicle's torque. If the vehicle is in neutral or with the hand brake pulled, it is possible to raise the engine RPM, then make a quick acceleration with the vehicle. This high RPM will affect the initial acceleration torque.")]
    public float rpmAffectsTheTorque = 4.0f;

    [HideInInspector]
    public AnimationCurve[] gearsArray = new AnimationCurve[12]{
    new (new Keyframe(+0.0000f, 1.40f, 0, 0), new Keyframe(+30.00f, 0.75f, -0.04f, -0.04f), new Keyframe(55.00f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(-40.000f, 0.00f, 0, 0), new Keyframe(+0.000f, 0.15f, 0.01f, 0.01f), new Keyframe(40.00f, 1.0f, +0.01f, +0.01f), new Keyframe(50.00f, 1.0f, -0.01f, -0.01f), new Keyframe(75.00f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(-20.000f, 0.00f, 0, 0), new Keyframe(+20.00f, 0.15f, 0.01f, 0.01f), new Keyframe(60.00f, 1.0f, +0.01f, +0.01f), new Keyframe(70.00f, 1.0f, -0.01f, -0.01f), new Keyframe(95.00f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+00.000f, 0.00f, 0, 0), new Keyframe(+40.00f, 0.15f, 0.01f, 0.01f), new Keyframe(80.00f, 1.0f, +0.01f, +0.01f), new Keyframe(90.00f, 1.0f, -0.01f, -0.01f), new Keyframe(115.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+20.000f, 0.00f, 0, 0), new Keyframe(+60.00f, 0.15f, 0.01f, 0.01f), new Keyframe(100.0f, 1.0f, +0.01f, +0.01f), new Keyframe(110.0f, 1.0f, -0.01f, -0.01f), new Keyframe(135.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+40.000f, 0.00f, 0, 0), new Keyframe(+80.00f, 0.15f, 0.01f, 0.01f), new Keyframe(120.0f, 1.0f, +0.01f, +0.01f), new Keyframe(130.0f, 1.0f, -0.01f, -0.01f), new Keyframe(155.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+60.000f, 0.00f, 0, 0), new Keyframe(+100.0f, 0.15f, 0.01f, 0.01f), new Keyframe(140.0f, 1.0f, +0.01f, +0.01f), new Keyframe(150.0f, 1.0f, -0.01f, -0.01f), new Keyframe(175.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+80.000f, 0.00f, 0, 0), new Keyframe(+120.0f, 0.15f, 0.01f, 0.01f), new Keyframe(160.0f, 1.0f, +0.01f, +0.01f), new Keyframe(170.0f, 1.0f, -0.01f, -0.01f), new Keyframe(195.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+100.00f, 0.00f, 0, 0), new Keyframe(+140.0f, 0.15f, 0.01f, 0.01f), new Keyframe(180.0f, 1.0f, +0.01f, +0.01f), new Keyframe(190.0f, 1.0f, -0.01f, -0.01f), new Keyframe(215.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+120.00f, 0.00f, 0, 0), new Keyframe(+160.0f, 0.15f, 0.01f, 0.01f), new Keyframe(200.0f, 1.0f, +0.01f, +0.01f), new Keyframe(210.0f, 1.0f, -0.01f, -0.01f), new Keyframe(235.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+140.00f, 0.00f, 0, 0), new Keyframe(+180.0f, 0.15f, 0.01f, 0.01f), new Keyframe(220.0f, 1.0f, +0.01f, +0.01f), new Keyframe(230.0f, 1.0f, -0.01f, -0.01f), new Keyframe(255.0f, 0, 0, 0)),
    new AnimationCurve(new Keyframe(+160.00f, 0.00f, 0, 0), new Keyframe(+200.0f, 0.15f, 0.01f, 0.01f), new Keyframe(240.0f, 1.0f, +0.01f, +0.01f), new Keyframe(250.0f, 1.0f, -0.01f, -0.01f), new Keyframe(275.0f, 0, 0, 0)),
  };
    [HideInInspector]
    public int[] minVelocityGears = new int[12] { 0, 15, 35, 55, 75, 95, 115, 135, 155, 175, 195, 215 };
    [HideInInspector]
    public int[] idealVelocityGears = new int[12] { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240 };
    [HideInInspector]
    public int[] maxVelocityGears = new int[12] { 45, 65, 85, 105, 125, 145, 165, 185, 205, 225, 245, 265 };

    [Space(5)]
    [Header("---Vehicle Torque------------------------------------------------------------------------------------------------------------------------------------")]
    [Range(0.05f, 10.0f)]
    [Tooltip("This variable defines the torque that the motor of the vehicle will have. The common value for this variable is between 0.5 and 1.5 for most common vehicles.")]
    public float engineTorque = 1.0f;
    [Range(0.01f, 5.0f)]
    [Tooltip("This variable defines the speed at which the vehicle will receive the engine torque. The lower the value, the slower the engine will receive the torque, as if the pilot was stepping slowly on the accelerator.")]
    public float speedEngineTorque = 0.75f;
  }



  [Serializable]
  public class FuelAdjustmentClass {
    [Tooltip("If this variable is true, the vehicle will not count the fuel. It will always be maximum.")]
    public bool infinityFuel = true;
    [Range(10, 500)]
    [Tooltip("This variable defines the maximum fuel capacity of the vehicle, measured in liters.")]
    public int capacityInLiters = 50;
    [Range(0, 500)]
    [Tooltip("This variable defines the initial fuel that the vehicle will have in the tank, measured in liters.")]
    public int startingFuel = 50;
    [Range(0.01f, 5.0f)]
    [Tooltip("This variable defines the speed at which the vehicle will consume fuel.")]
    public float consumption = 0.2f;
  }



  [Serializable]
  public class VehicleParticlesClass {
    [Tooltip("If this variable is true, the vehicle emits particles associated with the classes below.")]
    public bool enableParticles = true;
    [Tooltip("When vehicle nitro is activated, the associated particles in this list will be activated as well.")]
    public ParticleSystem[] nitroParticles;
    [Tooltip("In this class the exhaust particles of the vehicle are configured.")]
    public ExhaustSmokeClass[] exhaustSmoke;
  }
  [Serializable]
  public class ExhaustSmokeClass {
    [Tooltip("In this variable, the particles referring to this class must be associated.")]
    public ParticleSystem smoke;
    [Range(5, 100)]
    [Tooltip("This is the critical speed that defines when the vehicle will stop emitting the particle. If the vehicle is below this speed, it will emit the particle. If the vehicle is above this speed, it will stop emitting the particle.")]
    public float criticalVelocity = 30;
  }
  [Serializable]
  public class DustParticleClass {
    [Header("Ground settings")]
    [Tooltip("In this variable, it is possible to define the color that the dust particle will have on this soil.")]
    public Color particleColor;
    [Range(0.1f, 200)]
    [Tooltip("This is the critical speed that defines when the vehicle will begin to emit the particle. If the vehicle is below that speed, it will stop emitting the particle. If the vehicle is above that speed, it will begin to emit the particle.")]
    public float criticalVelocity = 10;

    [Header("Ground detection")]
    [Tooltip("In this variable, you can configure the tag that will control the emission of this particle. It will only be issued when the vehicle is on a land with this tag.")]
    public string groundTag;
    [Tooltip("In this variable, you can configure the 'Physic Material' that will control the emission of this particle. It will only be issued when the vehicle is in a field with this 'PhysicMaterial'.")]
    public PhysicMaterial physicMaterial;
    [Tooltip("When the wheel is in a terrain and finds some texture present in this index, it will emit the particles of that index.")]
    public List<int> terrainTextureIndices = new List<int>();
    //
    [HideInInspector]
    public List<ParticleSystem> wheelDustList = new List<ParticleSystem>();

  }


  [Serializable]
  public class VehicleSkidMarksClass {
    [Tooltip("If this variable is true, the vehicle generates skid marks and traces.")]
    public bool enableSkidMarks = true;
    public enum SizeEnum { _600, _1200, _2400, _4800, _7200, _9600 };
    [Header("Settings")]
    [Tooltip("The maximum length that the 'skidMarks' track can achieve.")]
    public SizeEnum maxTrailLength = SizeEnum._2400;
    [Range(0.1f, 6.0f)]
    [Tooltip("This variable defines the width of the vehicle's skid trace.")]
    public float standardBrandWidth = 0.3f;
    [Range(1.0f, 10.0f)]
    [Tooltip("This variable sets the sensitivity of the vehicle to start generating traces of skidding. The more sensitive, the easier to generate the traces.")]
    public float sensibility = 2.8f;
    [Range(0.1f, 2.0f)]
    [Tooltip("This variable defines the sensitivity of the vehicle to generate skid marks when skating or sliding forward or backward.")]
    public float forwordSensibility = 1.0f;
    [Range(0.1f, 1.0f)]
    [Tooltip("This variable defines the default opacity of the skid marks.")]
    public float standardOpacity = 0.9f;
    [Range(0.001f, 0.2f)]
    [Tooltip("This variable defines the distance from the ground on which the marks are to be generated. It is advisable to leave this value at 0.02")]
    public float groundDistance = 0.04f;
    [Tooltip("This variable sets the default color of the skid marks.")]
    public Color standardColor = new Color(0.15f, 0.15f, 0.15f, 0);

    [Space(5)]
    [Header("Grounds")]
    [Tooltip("Here you can configure other terrains.")]
    public OtherGroundClass[] otherGround;

    [Header("Default Material")]
    [Range(0.0f, 1.0f)]
    [Tooltip("This variable defines the intensity of the 'normalMap' of skid trails.")]
    public float normalMapIntensity = 0.7f;
    [Range(0.0f, 1.0f)]
    [Tooltip("This variable defines the intensity of the 'smoothness' of skid trails.")]
    public float smoothness = 0.0f;
    [Range(0.0f, 1.0f)]
    [Tooltip("This variable defines the intensity of the 'metallic' of skid trails.")]
    public float metallic = 0.0f;

    [Header("Custom Material (optional)")]
    [Tooltip("An optional material, which can be used in place of the standard material of the asset.")]
    public Material customMaterial;
  }
  [Serializable]
  public class OtherGroundClass {
    public string name;
    [Space(10)]
    [Header("Ground detection")]
    [Tooltip("When the wheel finds this tag, it will receive the references from that index.")]
    public string groundTag;
    [Tooltip("When the wheel finds this 'PhysicMaterial', it will receive the references from that index.")]
    public PhysicMaterial physicMaterial;
    [Tooltip("When the wheel is on a terrain and finds some texture present in this index, it will receive the references from that index.")]
    public List<int> terrainTextureIndices = new List<int>();
    //
    [Space(10)]
    [Header("Skid marks settings")]
    [Tooltip("This variable defines whether the trace should be generated continuously when the wheel finds the tag configured in this index.")]
    public bool continuousMarking = false;
    [Tooltip("This variable defines the color of the skid marks for lands that have the tag defined in this index.")]
    public Color color = new Color(0.5f, 0.2f, 0.0f, 0);
    [Range(0.1f, 1.0f)]
    [Tooltip("This variable defines the opacity of the skid marks for lands that have the tag defined in this index.")]
    public float opacity = 0.3f;
  }


  [Serializable]
  public class VehiclePhysicsStabilizersClass {
    [Space(-5)]
    [Header("Very important!")]
    [Tooltip("In this variable an empty object affiliated to the vehicle should be associated with the center position of the vehicle, perhaps displaced slightly downward, with the intention of representing the center of mass of the vehicle. Correct adjustment of the 'center of mass' position makes a GIANT difference in the simulation of physics, so it is very important to position it correctly.")]
    public Transform centerOfMass;

    [Space(5)]
    [Header("Vehicle down force")]
    [Range(0.0f, 1.0f)]
    [Tooltip("This variable defines the amount of force that will be simulated in the vehicle while it is tilted, the steeper it is, the lower the force applied. Values too high make the vehicle too tight and prevent it from slipping.")]
    public float downForceAngleFactor = 0.2f;
    [Range(0.0f, 3.0f)]
    [Tooltip("This variable defines how much force will be simulated in the vehicle while on flat terrain. Values too high cause the suspension to reach the spring limit. If the vehicle is on sloped terrain, this force will be attenuated according to the 'downforceAngleFactor' variable.")]
    public float vehicleDownForce = 1.0f;

    [Space(5)]
    [Header("Helpers")]
    [Range(0.0f, 5.0f)]
    [Tooltip("This variable defines how much force will be added to the vehicle suspension to avoid rotations. This makes the vehicle more rigid and harder to knock over.")]
    public float antiRollForce = 3.5f;
    [Range(0.0f, 1.0f)]
    [Tooltip("This variable helps to stabilize the slip, because the higher its value, the more spin speed the vehicle will take in the turns, so it will rotate instead of sliding.")]
    public float stabilizeSlippage = 0.0f;
    [Range(0.0f, 5.0f)]
    [Tooltip("When the player is not turning the vehicle, his speed of rotation will tend to zero according to the speed defined in this variable. This helps straighten the vehicle when it comes out of curves.")]
    public float stabilizeAngularVelocity = 0.0f;

    [Space(5)]
    [Header("Aerodynamic")]
    [Range(0.0f, 0.2f)]
    [Tooltip("Here you can set the maximum drag that the 'Rigidbody' will receive. The higher the speed of the vehicle, the greater the drag it receives.")]
    public float rigidbodyMaxDrag = 0.025f;
    [Range(0.0f, 1.0f)]
    [Tooltip("Here it is possible to define the drag that the vehicle will suffer due to the air resistance. It is a useful force to brake the vehicle smoothly when it is only moving because of the stroke.")]
    public float airDrag = 0.3f;
    [Range(0.01f, 1.0f)]
    [Tooltip("In this variable you can define how much help the vehicle will receive to rotate in the air. This makes the vehicle stay as straight as possible while it is in the air. This variable also affects the control over the rotation of the vehicle while it flies.")]
    public float airRotation = 0.5f;

    [Space(5)]
    [Header("Tire slips")]
    [Tooltip("If this variable is true, the code will simulate 'tire slips'. The behavior of the vehicle will be controlled largely according to the value of the following variables.")]
    public bool stabilizeTireSlips = true;
    [Range(0.0f, 1.3f)]
    [Tooltip("How much the code will stabilize the vehicle's skidding. This variable affects the behavior described by all variables below.")]
    public float tireSlipsFactor = 1.0f;
    [Range(0.0f, 2.0f)]
    [Tooltip("This variable defines how much lateral force the vehicle will receive when the steering wheel is rotated. This helps the vehicle to rotate more realistically.")]
    public float helpToTurn = 0.1f;
    [Range(0.0f, 1.0f)]
    [Tooltip("This variable defines how fast the vehicle will straighten automatically. This occurs naturally in a vehicle when it exits a curve.")]
    public float helpToStraightenOut = 0.1f;
    [Range(0.5f, 1.5f)]
    [Tooltip("In this variable it is possible to define the influence that the surface forces will have on the vehicle.")]
    public float localSurfaceForce = 1.0f;

    [Space(5)]
    [Header("Gravity")]
    [Range(0.0f, 5.0f)]
    [Tooltip("The extra gravitational force that will be applied to the vehicle. The '0' value means that no extra gravitational force will be applied.")]
    public float extraGravity = 1.0f;
  }


  [Serializable]
  public class SpeedometerModel1 {
    [Tooltip("Here you must associate the prefab 'Canvas_Gauges' that you have affiliated with this vehicle, so that this vehicle happens to have gauges.")]
    public MSVSGauges canvas_Gauges;
    [Tooltip("If this variable is true, the size of the speedometers will be automatically set via the code, depending on the value of the variable below.")]
    public bool setSizeViaCode = true;
    [Range(0.5f, 1.5f)]
    [Tooltip("Here you can define the scale of the 'UI Gauges' of this vehicle.")]
    public float _UIScale = 0.9f;
    [Space(5)]
    [Tooltip("If this variable is true, the position of the speedometers will be automatically set via the code, depending on the value of the variable below.")]
    public bool setPositionViaCode = true;
    [Range(-0.35f, 0.35f)]
    [Tooltip("Here you can define the horizontal position of the 'UI Gauges' of this vehicle.")]
    public float _UIHorizontalOffset = 0.0f;
    [Space(5)]
    [Tooltip("If this variable is true, the background of the speed and rpm markers will be set automatically according to the maximum vehicle RPM and speed values.")]
    public bool setBackgroundViaCode = true;
  }
  [Serializable]
  public class SpeedometerModel2 {
    public enum RotType { RotationInY, RotationInZ }
    [Header("MainObject  (REQUIRED)")]
    [Tooltip("When the player enters the vehicle this object is activated automatically, and when the player exits the vehicle, the object is automatically deactivated. Then the speed and rpm pointers of the vehicle must be affiliated with this object.")]
    public GameObject masterObject;

    [Space(5)]
    [Header("SPEED")]
    [Tooltip("Here you can set whether the speed hand will rotate on the Y axis or the Z axis.")]
    public RotType rotationAxisSPEED = RotType.RotationInZ;
    [Tooltip("In this variable, you must associate the vehicle speed pointer.")]
    public GameObject speedometerSpeedPointer;
    [Range(0.1f, 10.0f)]
    [Tooltip("This variable will multiply the rotation of the speedometer pointer, allowing you to decide whether the pointer should rotate more or less.")]
    public float speedPointerRotationFactor = 0.75f;

    [Header("RPM")]
    [Tooltip("Here you can set whether the rpm hand will rotate on the Y axis or the Z axis.")]
    public RotType rotationAxisRPM = RotType.RotationInZ;
    [Tooltip("In this variable, you must associate the vehicle rpm pointer.")]
    public GameObject speedometerRPMPointer;
    [Range(0.1f, 10.0f)]
    [Tooltip("This variable will multiply the rotation of the speedometer pointer, allowing you to decide whether the pointer should rotate more or less.")]
    public float RPMPointerRotationFactor = 0.95f;

    [Header("FUEL")]
    [Tooltip("Here you can set whether the fuel hand will rotate on the Y axis or the Z axis.")]
    public RotType rotationAxisFUEL = RotType.RotationInZ;
    [Tooltip("In this variable, you must associate the vehicle fuel pointer.")]
    public GameObject speedometerFuelPointer;
    [Range(0.1f, 10.0f)]
    [Tooltip("This variable will multiply the rotation of the speedometer pointer, allowing you to decide whether the pointer should rotate more or less.")]
    public float FuelPointerRotationFactor = 1.0f;

    [Header("GEAR")]
    [Tooltip("Here the 'UI Text' that represents the current gear of the vehicle must be associated.")]
    public Text gearText;

    [Header("NITRO")]
    [Tooltip("Here you must associate an Image UI that represents the vehicle's nitro percentage.")]
    public Image nitroBar_filled;

  }

  [Serializable]
  public class SpeedometerAndOthersClass {
    public SpeedometerModel1 _speedometerModel1__ScreenSpace;
    public SpeedometerModel2 _speedometerModel2__WorldSpace;
  }

  [Serializable]
  public class VehicleSoundsClass {
    [Header("Engine Sound")]
    [Range(1.5f, 6.0f)]
    [Tooltip("This variable defines the speed of the engine sound.")]
    public float speedOfEngineSound = 3.5f;
    [Range(0.1f, 1.0f)]
    [Tooltip("This variable defines the volume of the engine sound.")]
    public float volumeOfTheEngineSound = 1.0f;
    [Tooltip("The audio referring to the sound of the engine must be associated here.")]
    public AudioClip engineSound;
    [Tooltip("Here you can associate a Transform to set the sound position of the vehicle engine.")]
    public Transform engineSoundPosition;
    [Tooltip("In this variable, the motor starting sound must be associated. This sound is optional. If no sound is associated here, the code itself will generate an engine starting sound based on the vehicle's own engine sound.")]
    public AudioClip engineStartSound;

    [Space(7)]
    [Header("Vehicle Sounds")]
    [Tooltip("The audio referring to the sound of the vehicle's flashers must be associated here.")]
    public AudioClip blinkingSound;
    [Tooltip("The audio referring to the horn of the vehicle must be associated with this variable.")]
    public AudioClip hornSound;
    [Tooltip("The audio relating to the siren of the reverse gear of the vehicle must be associated with this variable.")]
    public AudioClip reverseSirenSound;
    [Tooltip("The vehicle's handbrake audio must be associated with this variable.")]
    public AudioClip handBrakeSound;

    [Space(7)]
    [Header("Nitro")]
    [Tooltip("The nitro audio of this vehicle should be associated with this variable.")]
    public AudioClip nitroStartSound;

    [Space(7)]
    [Header("Collision Sounds")]
    [Tooltip("Collision sounds should be associated with this list.")]
    public AudioClip[] collisionSounds;
    [Range(0.1f, 1.0f)]
    [Tooltip("In this variable it is possible to set the volume of collision sounds of the vehicle.")]
    public float volumeCollisionSounds = 0.5f;

    [Space(7)]
    [Header("Wheel Impact Sound")]
    [Tooltip("The sound related to a collision in the wheel must be associated with this variable.")]
    public AudioClip wheelImpactSound;
    [Range(0.05f, 0.3f)]
    [Tooltip("In this variable it is possible to configure the sensitivity with which the collisions on the wheels are perceived by the script.")]
    public float sensibilityWheelImpact = 0.175f;
    [Range(0.01f, 0.7f)]
    [Tooltip("In this variable you can configure the sound volume associated with the variable 'wheelImpactSound'.")]
    public float volumeWheelImpact = 0.25f;

    [Space(7)]
    [Header("Wind Sound")]
    [Tooltip("The sound related to the wind noise should be associated with this variable.")]
    public AudioClip windSound;
    [Range(0.001f, 0.05f)]
    [Tooltip("The sensitivity to the script begins to emit the sound of the wind. The less sensitive, the faster the vehicle should go to emit the sound.")]
    public float sensibilityWindSound = 0.007f;

    [Space(7)]
    [Header("Air Brake Sound")]
    [Tooltip("The air brake sound should be associated with this variable.")]
    public AudioClip airBrakeSound;
    [Range(0.1f, 3.0f)]
    [Tooltip("This variable sets the volume of the sound associated with the variable 'volumeAirBrakeSound'.")]
    public float volumeAirBrakeSound = 0.75f;
  }

  [Serializable]
  public class GroundSoundsClass {
    public string name;
    //
    [Header("Sounds")]
    [Tooltip("The sound that the vehicle will emit when slipping or skidding on some soil defined in this index.")]
    public AudioClip skiddingSound;
    [Range(0.1f, 1.0f)]
    [Tooltip("The sound volume associated with the variable 'skiddingSound'")]
    public float volumeSkid = 0.8f;
    [Space(10)]
    [Tooltip("The sound that the wheels will emit when they find the ground defined in this index.")]
    public AudioClip groundSound;
    [Range(0.01f, 1.0f)]
    [Tooltip("The sound volume associated with the variable 'groundSound'")]
    public float volumeSound = 0.15f;
    //
    [Space(10)]
    [Header("Ground detection")]
    [Tooltip("When the wheel finds this tag, it will emit the sound set in this index.")]
    public string groundTag;
    [Tooltip("When the wheel finds this 'PhysicMaterial', it will emit the sound set in this index.")]
    public PhysicMaterial physicMaterial;
    [Tooltip("When the wheel is on a terrain and finds some texture present in this index, it will emit the sound configured in this index.")]
    public List<int> terrainTextureIndices = new List<int>();
  }
  [Serializable]
  public class VehicleGroundSoundsClass {
    [Tooltip("The default sound that will be emitted when the vehicle slides or skates.")]
    public AudioClip standardSkidSound;
    [Range(0.05f, 1.0f)]
    [Tooltip("The default volume of the skid sound.")]
    public float standardSkidVolume = 0.9f;
    [Header("Grounds")]
    [Tooltip("The sounds that the vehicle will emit on different terrains should be set here.")]
    public GroundSoundsClass[] groundSounds;
  }


  [Serializable]
  public class VehicleLightsClass {
    [Tooltip("If this variable is true, the vehicle may emit sounds.")]
    public bool enableLights = true;
    public MainLightClass mainLights;
    public BrakeLightClass brakeLights;
    public ReverseGearLightClass reverseGearLights;
    public FlashingLightClass flashingLights;
    public HeadlightsClass headlights;
    public ExtraLightsClass extraLights;
  }
  [Serializable]
  public class ExtraLightsClass {
    [Tooltip("Here you can select the 'RenderMode' of the lights associated with this class.")]
    public LightRenderMode renderMode = LightRenderMode.ForcePixel;
    [Tooltip("If this variable is true, the lights associated here will simulate shadows.")]
    public bool shadow = false;
    [Tooltip("The color of the list lights.")]
    public Color color = Color.white;
    [Range(0.1f, 5.0f)]
    [Tooltip("The intensity of the list lights.")]
    public float intensity = 3;
    [Range(0.1f, 15.0f)]
    [Tooltip("The speed at which the light will go from the 'on' state to the 'off' state. This only affects the light if it is a siren.")]
    public float speed = 4.0f;
    [Tooltip("The type of the light.")]
    public LightType lightType = LightType.Point;
    public enum TipoLuz { Continnous, Siren }
    [Tooltip("The effect that the light will have, being a continuous light or a siren.")]
    public TipoLuz lightEffect = TipoLuz.Continnous;
    [Space(5)]
    [Tooltip("In this list, you must associate all objects that contain the 'Light' component related to this class")]
    public Light[] lights;
    [Tooltip("In this variable, you must associate all meshes that represent the vehicle's unlit light, referring to this class.")]
    public GameObject meshesLightOn;
    [Tooltip("In this variable, you must associate all the meshes that represent the connected light of the vehicle, referring to this class.")]
    public GameObject meshesLightOff;
  }
  [Serializable]
  public class HeadlightsClass {
    [Tooltip("Here you can select the 'RenderMode' of the lights associated with this class.")]
    public LightRenderMode renderMode = LightRenderMode.ForcePixel;
    [Tooltip("If this variable is true, the lights associated here will simulate shadows.")]
    public bool shadow = false;
    [Tooltip("The color of the list lights.")]
    public Color color = new Color(0.5f, 1, 1);
    [Range(0.1f, 4.0f)]
    [Tooltip("The intensity of the list lights.")]
    public float intensity = 3;
    [Space(5)]
    [Tooltip("In this list, you must associate all objects that contain the 'Light' component related to this class")]
    public Light[] lights;
    [Tooltip("In this variable, you must associate all meshes that represent the vehicle's unlit light, referring to this class.")]
    public GameObject meshesLightOn;
    [Tooltip("In this variable, you must associate all the meshes that represent the connected light of the vehicle, referring to this class.")]
    public GameObject meshesLightOff;
  }
  [Serializable]
  public class MainLightClass {
    [Tooltip("Here you can select the 'RenderMode' of the lights associated with this class.")]
    public LightRenderMode renderMode = LightRenderMode.ForcePixel;
    [Tooltip("If this variable is true, the lights associated here will simulate shadows.")]
    public bool shadow = false;
    [Tooltip("The color of the list lights.")]
    public Color color = Color.white;
    [Range(0.1f, 5.0f)]
    [Tooltip("The intensity of the list lights.")]
    public float intensity = 3;
    [Space(5)]
    [Tooltip("In this list, you must associate all objects that contain the 'Light' component related to this class")]
    public Light[] lights;
    [Tooltip("In this variable, you must associate all meshes that represent the vehicle's low light by referring to the main lights.")]
    public GameObject meshesLightOn_low;
    [Tooltip("In this variable, you must associate all meshes that represent the vehicle's low light by referring to the main lights.")]
    public GameObject meshesLightOn_high;
    [Tooltip("In this variable, you must associate all the meshes that represent the connected light of the vehicle, referring to this class.")]
    public GameObject meshesLightOff;
  }
  [Serializable]
  public class BrakeLightClass {
    [Tooltip("Here you can select the 'RenderMode' of the lights associated with this class.")]
    public LightRenderMode renderMode = LightRenderMode.ForcePixel;
    [Tooltip("If this variable is true, the lights associated here will simulate shadows.")]
    public bool shadow = false;
    [Tooltip("The color of the list lights.")]
    public Color color = Color.red;
    [Range(0.1f, 4.0f)]
    [Tooltip("The intensity of the list lights.")]
    public float intensity = 3;
    [Space(5)]
    [Tooltip("In this list, you must associate all objects that contain the 'Light' component related to this class")]
    public Light[] lights;
    [Tooltip("In this variable, you must associate all meshes that represent the vehicle's unlit light, referring to this class.")]
    public GameObject meshesLightOn;
    [Tooltip("In this variable, you must associate all the meshes that represent the connected light of the vehicle, referring to this class.")]
    public GameObject meshesLightOff;
  }
  [Serializable]
  public class ReverseGearLightClass {
    [Tooltip("Here you can select the 'RenderMode' of the lights associated with this class.")]
    public LightRenderMode renderMode = LightRenderMode.ForcePixel;
    [Tooltip("If this variable is true, the lights associated here will simulate shadows.")]
    public bool shadow = false;
    [Tooltip("The color of the list lights.")]
    public Color color = Color.white;
    [Range(0.1f, 4.0f)]
    [Tooltip("The intensity of the list lights.")]
    public float intensity = 1.5f;
    [Space(5)]
    [Tooltip("In this list, you must associate all objects that contain the 'Light' component related to this class")]
    public Light[] lights;
    [Tooltip("In this variable, you must associate all meshes that represent the vehicle's unlit light, referring to this class.")]
    public GameObject meshesLightOn;
    [Tooltip("In this variable, you must associate all the meshes that represent the connected light of the vehicle, referring to this class.")]
    public GameObject meshesLightOff;
  }
  [Serializable]
  public class FlashingLightClass {
    [Tooltip("Here you can select the 'RenderMode' of the lights associated with this class.")]
    public LightRenderMode renderMode = LightRenderMode.ForcePixel;
    [Tooltip("If this variable is true, the lights associated here will simulate shadows.")]
    public bool shadow = false;
    [Tooltip("The color of the lights.")]
    public Color color = new Color(1, 0.5f, 0);
    [Range(0.1f, 4.0f)]
    [Tooltip("The intensity of the list lights.")]
    public float intensity = 2.0f;
    [Range(0.1f, 7.0f)]
    [Tooltip("The speed at which the light will go from the 'on' state to the 'off' state.")]
    public float speed = 3.8f;
    [Space(5)]
    [Tooltip("Right flashing light components")]
    public FlashingLightTypeClass rightFlashingLight;
    [Tooltip("Left flashing light components")]
    public FlashingLightTypeClass leftFlashingLight;
  }
  [Serializable]
  public class FlashingLightTypeClass {
    [Tooltip("In this list, you must associate all objects that contain the 'Light' component related to this class")]
    public Light[] light;
    [Tooltip("In this variable, you must associate all meshes that represent the vehicle's unlit light, referring to this class.")]
    public GameObject meshesLightOn;
    [Tooltip("In this variable, you must associate all the meshes that represent the connected light of the vehicle, referring to this class.")]
    public GameObject meshesLightOff;
  }



  [Serializable]
  public class VehicleSubstepsClass {
    [Range(0, 1000)]
    [Tooltip("The speed threshold of the sub-stepping algorithm.")]
    public int speedThreshold = 1000;
    [Range(1, 40)]
    [Tooltip("Amount of simulation sub-steps when vehicle's speed is below speedThreshold.")]
    public int stepsBelowThreshold = 30;
    [Range(1, 40)]
    [Tooltip("Amount of simulation sub-steps when vehicle's speed is above speedThreshold.")]
    public int stepsAboveThreshold = 30;
  }

  [Serializable]
  public class VehicleAdditionalSettingsClass {
    [Header("Nitro")]
    [Tooltip("Here you can set whether the vehicle will have the nitro feature or not.")]
    public bool useNitro = false;
    [Range(1.1f, 3.5f)]
    [Tooltip("Here you can define how many times the vehicle torque will be stronger when nitro is activated.")]
    public float additionalNitroTorque = 2.0f;
    [Range(3, 25)]
    [Tooltip("Here you can set how long the vehicle nitro will last.")]
    public float nitroTime = 5;
    [Range(0.05f, 2.0f)]
    [Tooltip("Here you can set how fast the vehicle nitro will automatically regenerate after use.")]
    public float rechargeSpeed = 0.25f;
    //
    [HideInInspector]
    public bool nitroIsTrueInput = false;
    [HideInInspector]
    public bool nitroIsTrueVar = false;
    [HideInInspector]
    public bool canUseNitro = true;
    [HideInInspector]
    public float timerNitro = 0;
  }
}
