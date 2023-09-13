using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

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
  public class VehicleDamageClass {
    [Tooltip("If this variable is true, the vehicle may receive damages.")]
    public bool enableDamage = true;
    [Range(50, 2000)]
    [Tooltip("This variable defines how much damage the vehicle is able to withstand.")]
    public float damageSupported = 350;
    [Tooltip("If this variable is true, the vehicle will lose torque as it receives damage.")]
    public bool affectTorque = false;
    [Tooltip("If this variable is true, the direction of the vehicle will be affected by the damage caused to the vehicle.")]
    public bool affectVolant = false;
    [Tooltip("In this class you can configure the deformations that the vehicle can suffer when it receives beats.")]
    public DeformMeshClass deformMesh;
  }
  [Serializable]
  public class DeformMeshClass {
    [Tooltip("The meshes of the vehicle must be associated with this variable.")]
    public MeshFilter[] meshes;
    [Tooltip("How much damage the vehicle will receive per hit.")]
    public float hitDamage = 0.2f;
    [Tooltip("The deformation area of each beat.")]
    public float areaOfDeformation = 0.5f;
    [Tooltip("if this variable is true, deformations will occur towards the vehicle's center of mass.")]
    public bool warpToTheCenter = true;
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

    [Space(5)]
    [Header("Player mesh")]
    [Tooltip("If there is an object to represent the driver inside the vehicle, this object must be associated with this variable to be activated or deactivated automatically when someone is controlling the vehicle.")]
    public GameObject internalPlayerMesh;
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
    new AnimationCurve(new Keyframe(+0.0000f, 1.40f, 0, 0), new Keyframe(+30.00f, 0.75f, -0.04f, -0.04f), new Keyframe(55.00f, 0, 0, 0)),
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
    [Tooltip("A particle of smoke must be associated with this variable. It will issue automatically when the vehicle receives too much damage.")]
    public ParticleSystem[] damageSmoke;
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
  public class VehicleAISettings {
    [Tooltip("If this variable is true, the vehicle may receive external commands, for example to an AI controller.")]
    public bool _enableTheUseOfAI = false;
    //
    [HideInInspector]
    public bool AIIsActive = true;
    [HideInInspector]
    public bool _AIisInControl;
    [HideInInspector]
    public float _AIVerticalInput;
    [HideInInspector]
    public float _AIHorizontalInput;
    [HideInInspector]
    public float vehicleMaxSteerAngle;
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




  [RequireComponent(typeof(Rigidbody))]
  [DisallowMultipleComponent]
  public class MSVehicleController : MonoBehaviour {
    public Transform SkidMarksContainer;

    [Tooltip("If this variable is checked, the vehicle will automatically manage the gearshift.")]
    public bool automaticGears = true;
    [Tooltip("The simulation rate of the vehicle wheels. This represents how many extra simulations will be made for the wheels beyond the simulation of physics. The more 'substeps', the greater the accuracy.")]
    public VehicleSubstepsClass substeps;
    [Tooltip("In this variable, empty objects must be associated with positions close to the vehicle doors.")]
    public GameObject[] doorPosition;

    [Tooltip("In this class you can configure the vehicle torque, number of gears and their respective torques.")]
    public TorqueAdjustmentClass _vehicleTorque;

    [Tooltip("In this class you can configure the vehicle suspension.")]
    public SuspensionAdjustmentClass _suspension;

    [Tooltip("Here it is possible to define how the physics of the vehicle will happen. In general, the behavior of the vehicle is defined here.")]
    public VehiclePhysicsStabilizersClass _vehiclePhysicStabilizers;

    [Tooltip("In this class you can set the vehicle's brakes, their strength, their initial state, and how they will work.")]
    public VehicleBrakesClass _brakes;

    [Tooltip("Here you can set the initial state of some vehicle variables, such as vehicle mass, brake and engine states, among other things.")]
    public VehicleAdjustmentClass _vehicleSettings;

    [Tooltip("In this class you can adjust all the settings related to the steering wheel of the vehicle and how much the wheels will turn.")]
    public VolantSettingsClass _steeringWheel;

    [Tooltip("In this class, you can adjust all preferences relative to the fuel consumption of the vehicle.")]
    public FuelAdjustmentClass _fuel;

    [Tooltip("In this class, you can adjust all preferences related to the damage received by the vehicle.")]
    public VehicleDamageClass _damage;

    [Tooltip("In this class, you can adjust all wheels of the vehicle separately, each with its own settings.")]
    public VehicleWheelsClass _wheels;

    [Tooltip("In this class, you can adjust all vehicle lights, and the preferences of each.")]
    public VehicleLightsClass _lights;

    [Tooltip("In this class, you can adjust all vehicle sounds, and the preferences of each.")]
    public VehicleSoundsClass _sounds;

    [Tooltip("In this class, you can adjust vehicle particles, and the preferences of each.")]
    public VehicleParticlesClass _particles;

    [Tooltip("In this class, you can adjust some indicators, such as the 'speedometer' and the RPM gauge.")]
    public SpeedometerAndOthersClass _speedometer;

    [Tooltip("Here you can configure various additional features of the vehicle, such as nitro, among others.")]
    public VehicleAdditionalSettingsClass _additionalFeatures;

    //

    public enum GroundDetectionMode { Tag, PhysicMaterial, TerrainTextureIndices, All };
    [Space(18)]
    [Tooltip("Here you can decide how the wheels of the vehicle will detect the ground, whether it is through Tag, PhysicMaterial, Index of texture or all at the same time. For performance reasons, the option 'ALL' is not advised because it is the heaviest.")]
    public GroundDetectionMode _groundDetection = GroundDetectionMode.All;

    [Tooltip("Here you can set the terrain on which the vehicle will walk. This variable is optional, because if you leave it empty, the code will fetch the active terrain automatically.")]
    public Terrain activeTerrain_optional;

    [Tooltip("In this class it is possible to define and configure all sounds that the vehicle will emit relative to the ground.")]
    public VehicleGroundSoundsClass _groundSounds;

    [Tooltip("In this class, you can adjust all the terrains on which the vehicle will walk, and set the friction of each of them.")]
    public GroundFrictionClass _groundFriction;

    [Tooltip("In this class, you can adjust the particles that the wheels of the vehicle will emit on each type of terrain.")]
    public DustParticleClass[] _groundParticles;

    [Tooltip("In this class, you can adjust all preferences in relation to vehicle skid marks, such as color, width, among other options.")]
    public VehicleSkidMarksClass _skidMarks;

    [Space(15)]
    [Tooltip("In this class, you can configure the use of external inputs for the vehicle.")]
    public VehicleAISettings _AISettings;



    [Space(5)]
    [Header("Resources")]
    [Tooltip("In this variable, the 'SkidMarks' shader must be associated. Otherwise, the vehicle will not generate skid marks.")]
    public Shader skidMarksShader;
    [Tooltip("In this variable must be associated a dust particle generated by the wheels. If no particle is associated with this variable, the vehicle will not emit particles in the ground.")]
    public ParticleSystem wheelDustParticle;
    [HideInInspector]
    public GameObject _mainSceneControl;


    [HideInInspector]
    public float verticalInput = 0;
    [HideInInspector]
    public float horizontalInput = 0;
    [HideInInspector]
    public float mouseXInput = 0;
    [HideInInspector]
    public float mouseYInput = 0;
    [HideInInspector]
    public float mouseScrollWheelInput = 0;


    bool _error = false;
    bool changinGearsAuto;
    bool brakingAuto;
    bool colliding;
    bool windLoop;
    bool boolTimeAirBrake;

    bool loopBlinkersOn;

    bool enableLightsOnStart;
    bool enableDamageOnStart;
    bool enableSkidMarksOnStart;
    bool enableParticlesOnStart;

    float mediumRPM;
    float volantDir_horizontalInput;
    float angleSteeringClamp;
    float finalAngleDegress;
    float angleVolantIntern;
    float volantStartRotation;
    float previousRotation;
    float volantDistortion = 0;
    float lastKnownTorque = 0;
    float speedLerpSound = 5;
    float vehicleScale;

    [HideInInspector]
    public float pitchAUDforRPM = 1;
    [HideInInspector]
    public float previousDelayStartEngine = 1;

    float rpmTorqueFactor;

    float fixedDeltaTime;

    float rpmFactorSpeedometerModel1 = 0;
    float speedFactorSpeedometerModel1 = 0;
    float rpmFactorSpeedometerModel2 = 0;
    float speedFactorSpeedometerModel2 = 0;
    Vector3 speedPointerStartRotation;
    Vector3 rpmPointerStartRotation;
    Vector3 fuelPointerStartRotation;

    float intensityFlashingL;
    float brakeLightIntensityParameter;
    float intensitySirenL;

    float lastRightFrontPositionY;
    float lastLeftFrontPositionY;
    float lastRightRearPositionY;
    float lastLeftRearPositionY;
    float sensImpactFR;
    float sensImpactFL;
    float sensImpactRR;
    float sensImpactRL;
    float currentEngineBrakeLerpValue;

    float[] lastPositionYExtraWheels;
    float[] sensImpactExtraWheels;
    float[] headlightsRange;
    int[] wheelEmitterSoundX;
    int[] wheelBlockSoundX;
    int[] wheelEmitterSoundXSkid;
    int[] wheelBlockSoundXSkid;
    bool forwardTempSKid;
    bool forwardHandBrakeSKid;

    Rigidbody ms_Rigidbody;

    [HideInInspector]
    public AudioSource engineSoundAUD;
    [HideInInspector]
    public AudioSource engineStartSoundAUD;
    AudioSource flashingSoundAUD;
    AudioSource hornSoundAUD;
    AudioSource beatsSoundAUD;
    AudioSource beatsOnWheelSoundAUD;
    AudioSource skiddingSoundAUD;
    AudioSource windSoundAUD;
    AudioSource airBrakeSoundAUD;
    AudioSource sirenSoundAUD;
    AudioSource handBrakeSoundAUD;
    AudioSource nitroSoundAUD;
    AudioSource[] groundSoundsAUD;
    AudioSource[] groundSoundsAUDSkid;

    bool handBrakeSoundWasPlayed;
    bool nitroSoundWasPlayed;

    WheelCollider[] wheelColliderList;
    Vector2 tireSL;
    Vector2 tireFO;
    float averageWheelRadius;

    WheelHit tempWheelHit;

    float inclinationFactorForcesDown;
    float downForceFixedUpdateRef;

    bool isBraking;
    float absLerpFactor;

    float reverseForceTimer;

    bool wheelFDIsGrounded;
    bool wheelFEIsGrounded;
    bool wheelTDIsGrounded;
    bool wheelTEIsGrounded;
    int groundedWheels;

    private readonly Dictionary<Mesh, int> currentIndexes = new Dictionary<Mesh, int>();
    Vector3[] lastPoint;
    int CacheSize = 2400;
    List<Vector3> vertices;
    List<Vector3> normals;
    List<Color> colors;
    List<Vector2> uv;
    List<int> tris;

    //terrain
    TerrainData terrainData;
    float[] terrainCompositionArray;
    float[,,] alphaMaps;
    float[] terrainCompositionMix;
    //
    int ms_sumImpactCount = 0;
    float ms_lastImpactTime = 0.0f;
    Vector3 ms_sumImpactPosition = Vector3.zero;
    Vector3 ms_sumImpactVelocity = Vector3.zero;
    Vector3[][] ms_originalMeshes;

    [HideInInspector]
    public float KMh;
    [HideInInspector]
    public int currentGear;
    [HideInInspector]
    public bool handBrakeTrue;
    [HideInInspector]
    public float vehicleLife;
    [HideInInspector]
    public float currentFuelLiters;
    [HideInInspector]
    public bool hornIsOn;
    [HideInInspector]
    public bool youCanCall;
    [HideInInspector]
    public bool theEngineIsRunning;
    [HideInInspector]
    public bool enableEngineSound;
    [HideInInspector]
    public bool changinGears;
    [HideInInspector]
    public bool lowLightOn;
    [HideInInspector]
    public bool highLightOn;
    [HideInInspector]
    public float brakeLightsIntensity;
    [HideInInspector]
    public bool headlightsOn;
    [HideInInspector]
    public bool disableBlinkers1;
    [HideInInspector]
    public bool disableBlinkers2;
    [HideInInspector]
    public bool alertOn;
    [HideInInspector]
    public bool rightBlinkersOn;
    [HideInInspector]
    public bool leftBlinkersOn;
    [HideInInspector]
    public bool extraLightsOn;

    //light hide variables
    [HideInInspector]
    public bool msvs_useMainLights;
    [HideInInspector]
    public bool msvs_useHeadLights;
    [HideInInspector]
    public bool msvs_useExtraLights;
    [HideInInspector]
    public bool msvs_useWarningOrFlashingLights;

    public enum ControlState { isNull, isAI, isPlayer };
    [HideInInspector]
    public ControlState _vehicleState = ControlState.isNull;

#if UNITY_EDITOR
    private void OnValidate() {
      //The 'OnValidate' function is called only in the editor.
      //suspension
      if (_suspension != null) {
        for (int x = 0; x < _suspension.vehicleCustomHeights.Length; x++) {
          if (_suspension.vehicleCustomHeights[x] < 0.1f) {
            _suspension.vehicleCustomHeights[x] = 0.25f;
          }
        }
      }

      //damage
      if (_damage != null) {
        if (_damage.deformMesh != null) {
          _damage.deformMesh.hitDamage = Mathf.Clamp(_damage.deformMesh.hitDamage, 0.02f, 100.0f);
          _damage.deformMesh.areaOfDeformation = Mathf.Clamp(_damage.deformMesh.areaOfDeformation, 0.1f, 5.0f);
        }
      }

      //extra wheels
      if (_wheels != null) {
        for (int x = 0; x < _wheels.extraWheels.Length; x++) {
          if (_wheels.extraWheels[x].angleFactor < 0.01f) {
            _wheels.extraWheels[x].angleFactor = 1;
          }
          if (_wheels.extraWheels[x].customBrandWidth < 0.1f) {
            _wheels.extraWheels[x].customBrandWidth = 0.3f;
          }
          if (_wheels.extraWheels[x].torqueFactor < 0.1f) {
            _wheels.extraWheels[x].torqueFactor = 1.0f;
          }

          //custom friction extra wheels
          if (_wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.ExtremumSlip <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.ExtremumSlip = 1.0f;
          }
          if (_wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.ExtremumValue <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.ExtremumValue = 1.5f;
          }
          if (_wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.AsymptoteSlip <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.AsymptoteSlip = 1.5f;
          }
          if (_wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.AsymptoteValue <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.AsymptoteValue = 1.0f;
          }
          //
          if (_wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.ExtremumSlip <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.ExtremumSlip = 0.2f;
          }
          if (_wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.ExtremumValue <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.ExtremumValue = 1.0f;
          }
          if (_wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.AsymptoteSlip <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.AsymptoteSlip = 0.5f;
          }
          if (_wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.AsymptoteValue <= 0) {
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.AsymptoteValue = 0.75f;
          }
        }
      }

      //particles
      if (_particles != null) {
        for (int x = 0; x < _particles.exhaustSmoke.Length; x++) {
          if (_particles.exhaustSmoke[x].criticalVelocity < 5) {
            _particles.exhaustSmoke[x].criticalVelocity = 40;
          }
        }
      }
      if (_groundParticles != null) {
        for (int x = 0; x < _groundParticles.Length; x++) {
          Color testColor = new Color(0, 0, 0, 0);
          if (_groundParticles[x].particleColor == testColor) {
            _groundParticles[x].particleColor = new Color(0.2f, 0.1f, 0.0f, 0.9f);
          }
          if (_groundParticles[x].criticalVelocity < 0.1f) {
            _groundParticles[x].criticalVelocity = 5;
          }
        }
      }

      //steering assist
      if (_wheels != null) {
        if (_steeringWheel != null) {
          if (_steeringWheel.steeringAssist > 0.1f) {
            if (_wheels.rightFrontWheel.wheelCollider && _wheels.rightRearWheel.wheelCollider && _wheels.leftFrontWheel.wheelCollider && _wheels.leftRearWheel.wheelCollider) {
              float frontWheelRadius = _wheels.rightFrontWheel.wheelCollider.radius + _wheels.leftFrontWheel.wheelCollider.radius;
              float rearWheelRadius = _wheels.rightRearWheel.wheelCollider.radius + _wheels.leftRearWheel.wheelCollider.radius;
              if (Mathf.Abs(frontWheelRadius - rearWheelRadius) > 0.18f) {
                _steeringWheel.steeringAssist = 0;
                Debug.LogWarning("The radius of the front wheels of the vehicle is very different from the radius of the rear wheels of the vehicle, and this can cause problems in the steering assist." +
                  "In these cases, the vehicle steering assist will be disabled automatically (SteeringWheel > Steering Assist = 0).");
              }
            }
          }
        }
      }

      //sounds
      if (_groundSounds != null) {
        for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
          if (_groundSounds.groundSounds[x].volumeSkid < 0.1f) {
            _groundSounds.groundSounds[x].volumeSkid = 0.7f;
          }
          if (_groundSounds.groundSounds[x].volumeSound < 0.01f) {
            _groundSounds.groundSounds[x].volumeSound = 0.2f;
          }
        }
      }

      //ground friction
      if (_groundFriction != null) {
        for (int x = 0; x < _groundFriction.grounds.Length; x++) {
          if (_groundFriction.grounds[x].forwardFriction < 0.1f) {
            _groundFriction.grounds[x].forwardFriction = 1.5f;
          }
          if (_groundFriction.grounds[x].sidewaysFriction < 0.1f) {
            _groundFriction.grounds[x].sidewaysFriction = 0.8f;
          }
          if (_groundFriction.grounds[x].torqueInThisGround < 0.05f) {
            _groundFriction.grounds[x].torqueInThisGround = 1.0f;
          }
        }
      }

      //skidmarks
      if (_skidMarks != null) {
        for (int x = 0; x < _skidMarks.otherGround.Length; x++) {
          Color testColor = new Color(0, 0, 0, 0);
          if (_skidMarks.otherGround[x].color == testColor) {
            _skidMarks.otherGround[x].color = new Color(0.2f, 0.1f, 0.0f, 0.9f);
          }
          if (_skidMarks.otherGround[x].opacity < 0.1f) {
            _skidMarks.otherGround[x].opacity = 0.7f;
          }
        }
      }

      //fuel
      if (_fuel != null) {
        _fuel.startingFuel = Mathf.Clamp(_fuel.startingFuel, 0, _fuel.capacityInLiters);
      }

      //torque
      if (_vehicleTorque != null) {
        _vehicleTorque.engineTorque = Mathf.Clamp(_vehicleTorque.engineTorque, 0.1f, 10.0f);
      }

      //others
      if (_additionalFeatures != null) {
        _additionalFeatures.additionalNitroTorque = Mathf.Clamp(_additionalFeatures.additionalNitroTorque, 1.1f, 3.5f);
        _additionalFeatures.nitroTime = Mathf.Clamp(_additionalFeatures.nitroTime, 3.0f, 25.0f);
        _additionalFeatures.rechargeSpeed = Mathf.Clamp(_additionalFeatures.rechargeSpeed, 0.05f, 2.0f);
      }

      // wheelMeshes on wheelCollider variable
      if (_wheels != null) {
        WheelCollider tempCollider = null;
        if (_wheels.rightFrontWheel.wheelMesh) { //rightFrontWheel
          tempCollider = _wheels.rightFrontWheel.wheelMesh.transform.GetComponentInChildren<WheelCollider>();
          if (tempCollider) {
            Debug.LogError("You have associated an object that contains the 'wheelCollider' component in the '" + transform.name + " > Wheels> RightFrontWheel> WheelMesh' variable, and this can cause problems. Associate only objects that have meshes in this variable.");
            tempCollider = null;
          }
        }
        if (_wheels.leftFrontWheel.wheelMesh) { //leftFrontWheel
          tempCollider = _wheels.leftFrontWheel.wheelMesh.transform.GetComponentInChildren<WheelCollider>();
          if (tempCollider) {
            Debug.LogError("You have associated an object that contains the 'wheelCollider' component in the '" + transform.name + " > Wheels> LeftFrontWheel> WheelMesh' variable, and this can cause problems. Associate only objects that have meshes in this variable.");
            tempCollider = null;
          }
        }
        if (_wheels.rightRearWheel.wheelMesh) { //rightRearWheel
          tempCollider = _wheels.rightRearWheel.wheelMesh.transform.GetComponentInChildren<WheelCollider>();
          if (tempCollider) {
            Debug.LogError("You have associated an object that contains the 'wheelCollider' component in the '" + transform.name + " > Wheels> RightRearWheel> WheelMesh' variable, and this can cause problems. Associate only objects that have meshes in this variable.");
            tempCollider = null;
          }
        }
        if (_wheels.leftRearWheel.wheelMesh) { //leftRearWheel
          tempCollider = _wheels.leftRearWheel.wheelMesh.transform.GetComponentInChildren<WheelCollider>();
          if (tempCollider) {
            Debug.LogError("You have associated an object that contains the 'wheelCollider' component in the '" + transform.name + " > Wheels> LeftRearWheel> WheelMesh' variable, and this can cause problems. Associate only objects that have meshes in this variable.");
            tempCollider = null;
          }
        }
        for (int x = 0; x < _wheels.extraWheels.Length; x++) { //extra wheels
          if (_wheels.extraWheels[x].wheelMesh) {
            tempCollider = _wheels.extraWheels[x].wheelMesh.transform.GetComponentInChildren<WheelCollider>();
            if (tempCollider) {
              Debug.LogError("You have associated an object that contains the 'wheelCollider' component in the '" + transform.name + " > Wheels> ExtraWheels[" + x + "] > WheelMesh' variable, and this can cause problems. Associate only objects that have meshes in this variable.");
              tempCollider = null;
            }
          }
        }
      }
    }
#endif

    void OnEnable() {
      _error = false;

      //null by default
      _vehicleState = ControlState.isNull;

      //check wheels
      if (!_wheels.rightFrontWheel.wheelCollider || !_wheels.leftFrontWheel.wheelCollider || !_wheels.rightRearWheel.wheelCollider || !_wheels.leftRearWheel.wheelCollider) {
        Debug.LogError("The vehicle " + transform.name + " must have at least the four main wheels associated with its variables, within class '_wheels'.");
        _error = true;
        this.transform.gameObject.SetActive(false);
        return;
      }

      hornIsOn = false;
      youCanCall = true;
      previousDelayStartEngine = _vehicleSettings.delayToStartTheEngine;

      //'Configure Vehicle Substeps' temporarily sets parameters, and is rebooting when a vehicle is deactivated, so it needs to be reconfigured when it is enabled.
      WheelCollider WheelColliders = GetComponentInChildren<WheelCollider>();
      if (WheelColliders) {
        WheelColliders.ConfigureVehicleSubsteps(substeps.speedThreshold, substeps.stepsBelowThreshold, substeps.stepsAboveThreshold);
      }

      //get Active Terrain
      if (!activeTerrain_optional) {
        activeTerrain_optional = Terrain.activeTerrain;
      }

      //meshes to deform
      if (!_error) {
        ms_originalMeshes = new Vector3[_damage.deformMesh.meshes.Length][];
        for (int i = 0; i < _damage.deformMesh.meshes.Length; i++) {
          if (_damage.deformMesh.meshes[i]) {
            Mesh mesh = _damage.deformMesh.meshes[i].mesh;
            ms_originalMeshes[i] = mesh.vertices;
            mesh.MarkDynamic();
          }
        }
      }

      //AI
      _AISettings._AIisInControl = false; //false by default
    }
    void OnDisable() {
      if (!_error) {
        for (int i = 0; i < _damage.deformMesh.meshes.Length; i++) {
          if (_damage.deformMesh.meshes[i]) {
            Mesh mesh = _damage.deformMesh.meshes[i].mesh;
            mesh.vertices = ms_originalMeshes[i];
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
          }
        }
      }
      if (_AISettings._enableTheUseOfAI) {
        if (_vehicleState == MSVehicleController.ControlState.isAI) {
          _AISettings._AIisInControl = false;
          ExitTheVehicle();
        }
      }
    }


    void Start() {
      if (!_error) {
        enableLightsOnStart = _lights.enableLights;
        enableDamageOnStart = _damage.enableDamage;
        enableSkidMarksOnStart = _skidMarks.enableSkidMarks;
        enableParticlesOnStart = _particles.enableParticles;

        SetValues();
        SetWheelCollidersOnStart();
        DisableParticles();

        // lights
        msvs_useExtraLights = msvs_useHeadLights = msvs_useMainLights = msvs_useWarningOrFlashingLights = false;
        DisableAllLightsOnStart();
        if (enableLightsOnStart) {
          SetLightValuesStart();
        }

        //skid marks
        if (skidMarksShader) {
          if (enableSkidMarksOnStart) {
            SetSkidMarksValues();
          }
        } else {
          enableSkidMarksOnStart = false;
        }
      }
    }
    void Update() {
      if (_vehicleState == ControlState.isAI) {
        automaticGears = true;
      }

      KMh = ms_Rigidbody.velocity.magnitude * 3.6f;

      DiscoverAverageRpm();

      //engine DIES
      if (_vehicleSettings.engineDie) {
        if (_vehicleState == ControlState.isPlayer && theEngineIsRunning) {
          if (!automaticGears && currentGear > 0) {
            if (wheelFDIsGrounded && wheelFEIsGrounded && wheelTDIsGrounded && wheelTEIsGrounded) {

              float tempPoint_EvaluateFactor = KMh / _vehicleTorque.speedOfGear;
              float existingTorque = Mathf.Abs(_vehicleTorque.gearsArray[currentGear - 1].Evaluate(tempPoint_EvaluateFactor));

              float minimumRPM = _vehicleSettings.minVehicleRPM * 1.2f;
              if (Mathf.Clamp01(verticalInput) > 0.5f && _vehicleSettings.vehicleRPMValue < minimumRPM) {
                float conditionRPM = (15.0f / averageWheelRadius);
                if (existingTorque < 0.03f || mediumRPM < -conditionRPM) { // to currentGear >= 1 
                  ms_Rigidbody.AddTorque(transform.right * ms_Rigidbody.mass * 0.5f, ForceMode.Impulse);
                  previousDelayStartEngine = _vehicleSettings.delayToStartTheEngine;
                  _vehicleSettings.delayToStartTheEngine = 2;
                  StartCoroutine("StartEngineCoroutine", false);
                }
              }
            }
          }
        }
      }

      //turn on and turn off
      if (_vehicleState == ControlState.isNull && theEngineIsRunning) {
        StartCoroutine("StartEngineCoroutine", false);
      }
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        float limit = 0.5f;
        if (_vehicleState == ControlState.isAI) {
          limit = 0.05f;
        }
        if (Mathf.Abs(verticalInput) > limit && !theEngineIsRunning && _vehicleSettings.turnOnWhenAccelerating && youCanCall) {
          if (currentFuelLiters > 0) {
            enableEngineSound = true;
            if (_sounds.engineSound) {
              engineSoundAUD.pitch = 0.5f;
              pitchAUDforRPM = 0.7f;
            }
            if (_sounds.engineStartSound) {
              previousDelayStartEngine = _vehicleSettings.delayToStartTheEngine;
              _vehicleSettings.delayToStartTheEngine = Mathf.Abs(_sounds.engineStartSound.length - _sounds.engineStartSound.length * 0.3f);
              engineSoundAUD.volume = 0;
              engineStartSoundAUD.PlayOneShot(engineStartSoundAUD.clip);
            }
            //
            StartCoroutine("StartEngineCoroutine", true);
          }
        }
      }

      //VEHICLE NITRO
      if (theEngineIsRunning && (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI)) {
        NitroFunction();
      } else {
        _additionalFeatures.nitroIsTrueVar = false;
        _additionalFeatures.canUseNitro = false;
      }

      VehicleSoundsFunction();

      if (enableLightsOnStart) { //if false, mobile buttons is off automatically
        LightsManager();
      }

      UpdateWheelMeshes();

      //particles
      ParticlesEmitter();

      //gears
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        if (automaticGears) {
          AutomaticGears();
        }
      }
      //
      VehicleRPMFunction();
      FuelManager();
      SpeedometerAndOthers();
      //
      if (_vehicleSettings.internalPlayerMesh) {
        if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
          if (!_vehicleSettings.internalPlayerMesh.activeInHierarchy) {
            _vehicleSettings.internalPlayerMesh.SetActive(true);
          }
        } else {
          if (_vehicleSettings.internalPlayerMesh.activeInHierarchy) {
            _vehicleSettings.internalPlayerMesh.SetActive(false);
          }
        }
      }
    }
    void FixedUpdate() {
      CheckGroundedWheelsOnFixedUpdate();

      //fixedDeltaTime
      fixedDeltaTime = Time.fixedDeltaTime;
      if (fixedDeltaTime < 0.0001f || fixedDeltaTime > 1) {
        fixedDeltaTime = 0.02f;
      }

      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        VehicleSteeringWheel();
        //set vehicle heigth (Move Towards)
        if (_suspension.vehicleCustomHeights.Length > 0) {
          SetGeneralSuspensionHeight();
        }
      }

      //motor brake
      StabilizeMotorBrakeForce();

      //forces factor
      inclinationFactorForcesDown = Mathf.Clamp(Mathf.Abs(Vector3.Dot(Vector3.up, transform.up)), _vehiclePhysicStabilizers.downForceAngleFactor, 1.0f);

      if (KMh > 0.1f) {
        //set friction on wheels
        if (_groundFriction.grounds.Length > 0) {
          SetWheelCollidersFrictionFixedUpdate(_wheels.rightFrontWheel.wheelCollider);
          SetWheelCollidersFrictionFixedUpdate(_wheels.leftFrontWheel.wheelCollider);
          SetWheelCollidersFrictionFixedUpdate(_wheels.rightRearWheel.wheelCollider);
          SetWheelCollidersFrictionFixedUpdate(_wheels.leftRearWheel.wheelCollider);
          for (int x = 0; x < _wheels.extraWheels.Length; x++) {
            SetWheelCollidersFrictionFixedUpdate(_wheels.extraWheels[x].wheelCollider);
          }
        }
        //stabilizers
        StabilizeVehicleRollForces();
        StabilizeVehicleRotation();
        StabilizeAngularRotation();
        StabilizeAirRotation();
        //drag
        if (_vehiclePhysicStabilizers.rigidbodyMaxDrag > 0.01f) {
          ms_Rigidbody.drag = Mathf.Clamp((KMh / _vehicleTorque.maxVelocityKMh) * 0.2f, 0.001f, _vehiclePhysicStabilizers.rigidbodyMaxDrag);
        }
      }

      //torque and brake
      ApplyTorque();
      Brakes();
      //

      //extra gravity
      if (_vehiclePhysicStabilizers.extraGravity > 0) {
        ms_Rigidbody.AddForce(Vector3.down * _vehiclePhysicStabilizers.extraGravity * _vehicleSettings.vehicleMass);
      }

      //down Force
      if (groundedWheels > 0) {
        float lerpNextValue = _vehiclePhysicStabilizers.vehicleDownForce * _vehicleSettings.vehicleMass * inclinationFactorForcesDown;
        downForceFixedUpdateRef = Mathf.Lerp(downForceFixedUpdateRef, lerpNextValue, fixedDeltaTime * 2.5f);
      } else {
        downForceFixedUpdateRef = Mathf.Lerp(downForceFixedUpdateRef, 0, fixedDeltaTime * 2.5f);
      }
      ms_Rigidbody.AddForce(-transform.up * downForceFixedUpdateRef);

      //tire slips
      if (_vehiclePhysicStabilizers.stabilizeTireSlips) {
        if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI || KMh > 0.1f) {
          SetWheelForces(_wheels.rightFrontWheel.wheelCollider);
          SetWheelForces(_wheels.leftFrontWheel.wheelCollider);
          SetWheelForces(_wheels.rightRearWheel.wheelCollider);
          SetWheelForces(_wheels.leftRearWheel.wheelCollider);
          for (int x = 0; x < _wheels.extraWheels.Length; x++) {
            SetWheelForces(_wheels.extraWheels[x].wheelCollider);
          }
        }
      }

      //damage_deform
      if (_damage.deformMesh.meshes.Length > 0 && enableDamageOnStart) {
        if (Time.time - ms_lastImpactTime >= 0.2f && ms_sumImpactCount > 0) {
          ms_sumImpactPosition *= (1.0f / ms_sumImpactCount);
          ms_sumImpactVelocity *= (1.0f / ms_sumImpactCount);
          Vector3 impactVelocityFixedUpdate = Vector3.zero;
          if (ms_sumImpactVelocity.sqrMagnitude > 1.5f) {
            impactVelocityFixedUpdate = transform.TransformDirection(ms_sumImpactVelocity) * 0.02f;
          }
          if (impactVelocityFixedUpdate.sqrMagnitude > 0.0f) {
            Vector3 contactPointFixedUpdate = transform.TransformPoint(ms_sumImpactPosition);
            for (int i = 0, c = _damage.deformMesh.meshes.Length; i < c; i++) {
              DeformMesh(_damage.deformMesh.meshes[i].mesh, ms_originalMeshes[i], _damage.deformMesh.meshes[i].transform, contactPointFixedUpdate, impactVelocityFixedUpdate);
            }
          }
          ms_sumImpactCount = 0;
          ms_sumImpactPosition = Vector3.zero;
          ms_sumImpactVelocity = Vector3.zero;
          ms_lastImpactTime = Time.time + 0.2f * UnityEngine.Random.Range(-0.4f, 0.4f);
        }
      }

      //Reverse force
      if (_vehiclePhysicStabilizers.airDrag > 0.05f && KMh > 0.1f) {
        if (verticalInput == 0) {
          if (reverseForceTimer < 3.1f) {
            reverseForceTimer += fixedDeltaTime;
          }
        } else {
          reverseForceTimer = 0.0f;
        }
        //
        if (KMh > 2 && reverseForceTimer > 2.0f) {
          Vector3 reverseForceVector = -transform.forward * _vehicleSettings.vehicleMass * _vehiclePhysicStabilizers.airDrag * Mathf.Clamp(mediumRPM, -1, 1);
          ms_Rigidbody.AddForce(reverseForceVector);
        }
      } else {
        reverseForceTimer = 0.0f;
      }
      //

      //brakes ABS
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        if (_brakes.ABS && isBraking) {
          if (wheelFDIsGrounded && wheelFEIsGrounded && wheelTDIsGrounded && wheelTEIsGrounded) {
            if (_brakes.brakeSlowly) {
              absLerpFactor = Mathf.Lerp(absLerpFactor, 1, fixedDeltaTime * _brakes.speedBrakeSlowly);
            } else {
              absLerpFactor = 1;
            }
            float absSpeedFactor = Mathf.Clamp(KMh, 70, 150);
            //
            float absBrakeInput = 0.0f;
            if (automaticGears) {
              if (currentGear > 0 && mediumRPM > 0) {
                absBrakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, -1.0f, 0.0f));
              } else if (currentGear <= 0 && mediumRPM < 0) {
                absBrakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, 0.0f, 1.0f)) * -1;
              }
              //
              Vector3 absTotalForce = (-transform.forward * absSpeedFactor * _vehicleSettings.vehicleMass * _brakes.ABSForce * absBrakeInput * absLerpFactor);
              ms_Rigidbody.AddForce(absTotalForce);
            } else {
              if (currentGear > 0 && mediumRPM > 0) {
                absBrakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, -1.0f, 0.0f));
              } else if (currentGear <= 0 && mediumRPM < 0) {
                absBrakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, -1.0f, 0.0f)) * -1.0f;
              } else if (currentGear >= 0 && mediumRPM < 0) {
                absBrakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, -1.0f, 0.0f)) * -1.0f;
              } else if (currentGear <= 0 && mediumRPM > 0) {
                absBrakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, -1.0f, 0.0f));
              }
              //
              Vector3 absTotalForce = (-transform.forward * absSpeedFactor * _vehicleSettings.vehicleMass * _brakes.ABSForce * absBrakeInput * absLerpFactor);
              ms_Rigidbody.AddForce(absTotalForce);
            }
          }
        } else {
          if (KMh > 2.5f) {
            absLerpFactor = 0.0f;
          } else {
            absLerpFactor = Mathf.Lerp(absLerpFactor, 1, fixedDeltaTime * 3.0f);
          }
        }
      }
    }
    void LateUpdate() {
      //skid marks
      if (enableSkidMarksOnStart) {
        if (KMh > 0.01f) {
          CheckGroundForSKidMarks();
        } else {
          _wheels.rightFrontWheel.generateSkidBool = false;
          _wheels.leftFrontWheel.generateSkidBool = false;
          _wheels.rightRearWheel.generateSkidBool = false;
          _wheels.leftRearWheel.generateSkidBool = false;
          for (int x = 0; x < _wheels.extraWheels.Length; x++) {
            _wheels.extraWheels[x].generateSkidBool = false;
          }
        }
      }
    }
    void OnCollisionStay() {
      colliding = true;
    }
    void OnCollisionExit() {
      colliding = false;
    }

    public void AIHornInput() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        hornIsOn = true;
      }
    }
    public void AISuspensionHeight() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        if (_suspension.vehicleCustomHeights.Length > 0) {
          if (_suspension.indexCustomSuspensionHeight < (_suspension.vehicleCustomHeights.Length - 1)) {
            _suspension.indexCustomSuspensionHeight++;
          } else if (_suspension.indexCustomSuspensionHeight >= (_suspension.vehicleCustomHeights.Length - 1)) {
            _suspension.indexCustomSuspensionHeight = 0;
          }
        }
      }
    }
    public void AIMainLights() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        if (!lowLightOn && !highLightOn) {
          lowLightOn = true;
          brakeLightsIntensity = 0.5f;
        } else if (lowLightOn && !highLightOn) {
          lowLightOn = false;
          highLightOn = true;
          brakeLightsIntensity = 0.5f;
        } else if (!lowLightOn && highLightOn) {
          lowLightOn = false;
          highLightOn = false;
          brakeLightsIntensity = 0.0f;
        }
      }
    }
    public void AIHeadLights() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        headlightsOn = !headlightsOn;
      }
    }
    public void AIFlashesRightAlert() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        if (!rightBlinkersOn && !alertOn) {
          rightBlinkersOn = true;
          leftBlinkersOn = false;
          disableBlinkers1 = true;
        } else if (rightBlinkersOn && !alertOn) {
          rightBlinkersOn = false;
          leftBlinkersOn = false;
          disableBlinkers1 = false;
        }
      }
    }
    public void AIFlashesLeftAlert() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        if (!leftBlinkersOn && !alertOn) {
          rightBlinkersOn = false;
          leftBlinkersOn = true;
          disableBlinkers1 = true;
        } else if (leftBlinkersOn && !alertOn) {
          rightBlinkersOn = false;
          leftBlinkersOn = false;
          disableBlinkers1 = false;
        }
      }
    }
    public void AIWarningLight() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        if (alertOn) {
          alertOn = false;
          rightBlinkersOn = leftBlinkersOn = false;
        } else {
          alertOn = true;
          rightBlinkersOn = leftBlinkersOn = true;
        }
      }
    }
    public void AIExtraLights() {
      if (_AISettings._AIisInControl && _AISettings._enableTheUseOfAI) {
        extraLightsOn = !extraLightsOn;
      }
    }



    public void DisableParticles() {
      //DAMAGE 
      if (_particles.damageSmoke.Length > 0) {
        for (int x = 0; x < _particles.damageSmoke.Length; x++) {
          if (_particles.damageSmoke[x]) {
            ParticleSystem.MainModule particleMainModule = _particles.damageSmoke[x].main;
            particleMainModule.playOnAwake = false;
            _particles.damageSmoke[x].Stop(true);
            //
            ParticleSystem.EmissionModule tempParticle = _particles.damageSmoke[x].emission;
            tempParticle.rateOverTime = 1;
            tempParticle.enabled = false;
          }
        }
      }
      //NITRO
      if (_particles.nitroParticles.Length > 0) {
        for (int x = 0; x < _particles.nitroParticles.Length; x++) {
          if (_particles.nitroParticles[x]) {
            ParticleSystem.MainModule particleNitroMainModule = _particles.nitroParticles[x].main;
            particleNitroMainModule.playOnAwake = false;
            _particles.nitroParticles[x].Stop(true);
            //
            ParticleSystem.EmissionModule tempParticleNitro = _particles.nitroParticles[x].emission;
            tempParticleNitro.rateOverTime = 10;
            tempParticleNitro.enabled = false;
          }
        }
      }
      //
      //
      //exhaustSmoke
      if (_particles.exhaustSmoke.Length > 0) {
        for (int x = 0; x < _particles.exhaustSmoke.Length; x++) {
          if (_particles.exhaustSmoke[x].smoke) {
            ParticleSystem.EmissionModule particleTemp = _particles.exhaustSmoke[x].smoke.emission;
            particleTemp.rateOverTime = 1;

            ParticleSystem.MainModule particleMainModule = _particles.exhaustSmoke[x].smoke.main;
            particleMainModule.startSpeed = 1.5f;
            particleMainModule.playOnAwake = true;
            _particles.exhaustSmoke[x].smoke.Play(true);
            //
            if (!enableParticlesOnStart) {
              particleTemp = _particles.exhaustSmoke[x].smoke.emission;
              particleTemp.enabled = false;
            }
          }
        }
      }
      //
      //dust particle
      if (wheelDustParticle) {
        if (_groundParticles.Length > 0) {
          for (int x = 0; x < _groundParticles.Length; x++) {
            for (int y = 0; y < wheelColliderList.Length; y++) {
              //generate particle and position
              ParticleSystem tempParticleInstance = Instantiate(wheelDustParticle) as ParticleSystem;
              _groundParticles[x].wheelDustList.Add(tempParticleInstance);
              _groundParticles[x].wheelDustList[y].transform.parent = wheelColliderList[y].transform;
              _groundParticles[x].wheelDustList[y].transform.localPosition = new Vector3(0, -wheelColliderList[y].radius, 0);

              //get particle
              ParticleSystem.EmissionModule particleTemp = _groundParticles[x].wheelDustList[y].emission;
              ParticleSystem.MainModule particleMainModule = _groundParticles[x].wheelDustList[y].main;

              //set emission rate 
              particleTemp.rateOverTime = 1;

              //set color
              particleMainModule.startColor = _groundParticles[x].particleColor;
              if (particleMainModule.startColor.color.a == 0) {
                Color newColor = new Color(_groundParticles[x].particleColor.r, _groundParticles[x].particleColor.g, _groundParticles[x].particleColor.b, 1.0f);
                particleMainModule.startColor = newColor;
              }

              //disable paraticle
              particleMainModule.playOnAwake = false;
              //_groundParticles [x].wheelDustList [y].Stop (true);
              if (!_groundParticles[x].wheelDustList[y].isPlaying) {
                _groundParticles[x].wheelDustList[y].Play(true);
              }
              particleTemp.enabled = false;
            }
          }
        }
      }
    }
    void SetValues() {
      vehicleScale = transform.lossyScale.y;
      _suspension.constVehicleHeightStart = _suspension.vehicleStartHeight;
      _suspension.indexCustomSuspensionHeight = 0;
      reverseForceTimer = 0.0f;

      //speedometer
      if (_speedometer._speedometerModel1__ScreenSpace.canvas_Gauges) {
        _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.enabledGauge = false;
        _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.enableDamageText = _damage.enableDamage;
        _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.enableNitroBar = _additionalFeatures.useNitro;
      }
      if (_speedometer._speedometerModel2__WorldSpace.masterObject) {
        if (_speedometer._speedometerModel2__WorldSpace.speedometerSpeedPointer) {
          speedPointerStartRotation = _speedometer._speedometerModel2__WorldSpace.speedometerSpeedPointer.transform.localEulerAngles;
        }
        if (_speedometer._speedometerModel2__WorldSpace.speedometerRPMPointer) {
          rpmPointerStartRotation = _speedometer._speedometerModel2__WorldSpace.speedometerRPMPointer.transform.localEulerAngles;
        }
        if (_speedometer._speedometerModel2__WorldSpace.speedometerFuelPointer) {
          fuelPointerStartRotation = _speedometer._speedometerModel2__WorldSpace.speedometerFuelPointer.transform.localEulerAngles;
        }
        if (_speedometer._speedometerModel2__WorldSpace.gearText) {
          _speedometer._speedometerModel2__WorldSpace.gearText.text = "D0";
        }
        if (_speedometer._speedometerModel2__WorldSpace.nitroBar_filled) {
          _speedometer._speedometerModel2__WorldSpace.nitroBar_filled.type = Image.Type.Filled;
          _speedometer._speedometerModel2__WorldSpace.nitroBar_filled.fillAmount = 1;
        }
      }

      //skidMarks
      switch (_skidMarks.maxTrailLength) {
        case VehicleSkidMarksClass.SizeEnum._600: CacheSize = 600; break;
        case VehicleSkidMarksClass.SizeEnum._1200: CacheSize = 1200; break;
        case VehicleSkidMarksClass.SizeEnum._2400: CacheSize = 2400; break;
        case VehicleSkidMarksClass.SizeEnum._4800: CacheSize = 4800; break;
        case VehicleSkidMarksClass.SizeEnum._7200: CacheSize = 7200; break;
        case VehicleSkidMarksClass.SizeEnum._9600: CacheSize = 9600; break;
      }
      vertices = new List<Vector3>(CacheSize);
      normals = new List<Vector3>(CacheSize);
      colors = new List<Color>(CacheSize);
      uv = new List<Vector2>(CacheSize);
      tris = new List<int>(CacheSize * 3);
      lastPoint = new Vector3[4 + _wheels.extraWheels.Length];

      //doors
      if (doorPosition.Length == 0) {
        doorPosition = new GameObject[1];
      }
      for (int x = 0; x < doorPosition.Length; x++) {
        if (!doorPosition[x]) {
          doorPosition[x] = new GameObject("doorPos");
          doorPosition[x].transform.position = transform.position;
        }
        doorPosition[x].transform.rotation = transform.rotation;
        doorPosition[x].transform.parent = transform;
      }

      //wheel list
      wheelColliderList = new WheelCollider[(4 + _wheels.extraWheels.Length)];
      wheelColliderList[0] = _wheels.rightFrontWheel.wheelCollider;
      wheelColliderList[1] = _wheels.leftFrontWheel.wheelCollider;
      wheelColliderList[2] = _wheels.rightRearWheel.wheelCollider;
      wheelColliderList[3] = _wheels.leftRearWheel.wheelCollider;
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        wheelColliderList[x + 4] = _wheels.extraWheels[x].wheelCollider;
      }

      //mediumRadius
      float sumR = 0;
      for (int x = 0; x < wheelColliderList.Length; x++) {
        sumR += wheelColliderList[x].radius;
      }
      averageWheelRadius = sumR / wheelColliderList.Length;
      averageWheelRadius = Mathf.Clamp(averageWheelRadius, 0.01f, 10.0f);

      //fuel
      _fuel.startingFuel = Mathf.Clamp(_fuel.startingFuel, 0, _fuel.capacityInLiters);
      currentFuelLiters = _fuel.startingFuel;

      //steering wheel
      volantDistortion = Random.Range(-0.1f, 0.1f);
      while (volantDistortion >= -0.025f && volantDistortion <= 0.025f) {
        volantDistortion = Random.Range(-0.1f, 0.1f);
      }

      boolTimeAirBrake = windLoop = loopBlinkersOn = hornIsOn = false;
      vehicleLife = _damage.damageSupported;
      handBrakeTrue = _vehicleSettings.startBraking;
      changinGears = false;

      //start engine or not
      enableEngineSound = false;
      //theEngineIsRunning = false;
      youCanCall = true;
      previousDelayStartEngine = _vehicleSettings.delayToStartTheEngine;

      //vehicle rigidbody
      ms_Rigidbody = GetComponent<Rigidbody>();
      ms_Rigidbody.useGravity = true;
      ms_Rigidbody.mass = _vehicleSettings.vehicleMass;
      ms_Rigidbody.drag = 0.0f;
      ms_Rigidbody.angularDrag = 0.05f;
      ms_Rigidbody.maxAngularVelocity = 14.0f;
      ms_Rigidbody.maxDepenetrationVelocity = 8.0f;
      ms_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
      ms_Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

      //configure vehicle substeps on start
      WheelCollider WheelColliders = GetComponentInChildren<WheelCollider>();
      WheelColliders.ConfigureVehicleSubsteps(substeps.speedThreshold, substeps.stepsBelowThreshold, substeps.stepsAboveThreshold);

      //center of mass
      if (_vehiclePhysicStabilizers.centerOfMass) {
        ms_Rigidbody.centerOfMass = transform.InverseTransformPoint(_vehiclePhysicStabilizers.centerOfMass.position);
      } else {
        ms_Rigidbody.centerOfMass = Vector3.zero;
      }

      //steeringAssist bugs
      Vector3 centerOfMassPosition = this.transform.TransformPoint(ms_Rigidbody.centerOfMass);
      float distRFrontWheel = Vector3.Distance(centerOfMassPosition, _wheels.rightFrontWheel.wheelCollider.transform.position);
      float distLFrontWheel = Vector3.Distance(centerOfMassPosition, _wheels.leftFrontWheel.wheelCollider.transform.position);
      float distRRearWheel = Vector3.Distance(centerOfMassPosition, _wheels.rightRearWheel.wheelCollider.transform.position);
      float distLRearWheel = Vector3.Distance(centerOfMassPosition, _wheels.leftRearWheel.wheelCollider.transform.position);
      float rightDistance = distRFrontWheel + distRRearWheel;
      float leftDistance = distLFrontWheel + distLRearWheel;
      if (Mathf.Abs(rightDistance - leftDistance) > 0.015f) {
        Debug.LogWarning("The vehicle '" + this.transform.name + "' has its wheels misaligned relative to the center of mass of the vehicle. This can cause problems in the stability of the vehicle. It is recommended to align the wheels of the vehicle to its center of mass, or, leave the variable 'SteeringWheel> SteeringAssist' to 0. Leaving the center of mass perfectly aligned to the vehicle is also important for a realistic simulation.");
        _steeringWheel.steeringAssist *= 0.5f;
      }

      //steering wheel
      if (_steeringWheel.steeringWheelObject) {
        switch (_steeringWheel.rotationType) {
          case VolantSettingsClass.SelectRotation.RotationInY:
            volantStartRotation = _steeringWheel.steeringWheelObject.transform.localEulerAngles.y;
            break;
          case VolantSettingsClass.SelectRotation.RotationInZ:
            volantStartRotation = _steeringWheel.steeringWheelObject.transform.localEulerAngles.z;
            break;
        }
      }

      //additional settings
      if (_additionalFeatures.useNitro) {
        _additionalFeatures.timerNitro = _additionalFeatures.nitroTime;
        _additionalFeatures.nitroIsTrueInput = false;
        _additionalFeatures.nitroIsTrueVar = false;
        _additionalFeatures.canUseNitro = true;
      }

      //rpm animation curve
      _vehicleSettings.rpmCurve = new AnimationCurve(new Keyframe(0.85f, _vehicleSettings.minVehicleRPM), new Keyframe(_sounds.speedOfEngineSound + 0.15f, _vehicleSettings.maxVehicleRPM));

      //sounds
      speedLerpSound = 5;
      wheelEmitterSoundX = new int[_groundSounds.groundSounds.Length];
      wheelBlockSoundX = new int[_groundSounds.groundSounds.Length];
      wheelEmitterSoundXSkid = new int[_groundSounds.groundSounds.Length];
      wheelBlockSoundXSkid = new int[_groundSounds.groundSounds.Length];
      //
      GameObject vehicleSoundsMainObject = new GameObject("VehicleSounds");
      vehicleSoundsMainObject.transform.position = transform.position;
      vehicleSoundsMainObject.transform.parent = transform;
      if (_sounds.engineSound) {
        engineSoundAUD = GenerateAudioSource("Sound of engine", 10, 0, _sounds.engineSound, true, true, true, vehicleSoundsMainObject, 4.5f, 500);
        if (_sounds.engineSoundPosition) {
          _sounds.engineSoundPosition.parent = vehicleSoundsMainObject.transform;
          engineSoundAUD.transform.position = _sounds.engineSoundPosition.position;
        }
      }
      if (_sounds.engineStartSound) {
        engineStartSoundAUD = GenerateAudioSource("Engine start sound", 10, 1, _sounds.engineStartSound, false, false, false, vehicleSoundsMainObject, 4.5f, 250);
      }
      if (_sounds.blinkingSound) {
        flashingSoundAUD = GenerateAudioSource("Blinking Sound", 10, 1, _sounds.blinkingSound, true, true, false, vehicleSoundsMainObject, 4.5f, 500);
        flashingSoundAUD.Stop();
        loopBlinkersOn = false;
      }
      if (_sounds.hornSound) {
        hornSoundAUD = GenerateAudioSource("Sound of horn", 10, 1, _sounds.hornSound, false, false, false, vehicleSoundsMainObject, 10, 500);
      }
      if (_sounds.reverseSirenSound) {
        sirenSoundAUD = GenerateAudioSource("Sound of siren", 10, 1, _sounds.reverseSirenSound, true, false, false, vehicleSoundsMainObject, 5, 100);
      }
      if (_sounds.handBrakeSound) {
        handBrakeSoundAUD = GenerateAudioSource("Sound of hand brake", 10, 0.6f, _sounds.handBrakeSound, false, false, false, vehicleSoundsMainObject, 4.5f, 100);
        handBrakeSoundWasPlayed = false;
      }
      if (_sounds.nitroStartSound) {
        nitroSoundAUD = GenerateAudioSource("Nitro sound", 10, 1, _sounds.nitroStartSound, true, false, false, vehicleSoundsMainObject, 4.5f, 50);
        nitroSoundWasPlayed = false;
      }
      if (_sounds.airBrakeSound) {
        airBrakeSoundAUD = GenerateAudioSource("Sound of air brake", 10, _sounds.volumeAirBrakeSound, _sounds.airBrakeSound, false, false, false, vehicleSoundsMainObject, 5, 200);
      }
      if (_sounds.wheelImpactSound) {
        beatsOnWheelSoundAUD = GenerateAudioSource("Sound of wheel beats", 10, _sounds.volumeWheelImpact, _sounds.wheelImpactSound, false, false, false, vehicleSoundsMainObject, 5, 100);
      }
      if (_sounds.windSound) {
        windSoundAUD = GenerateAudioSource("Sound of wind", 10, 0.0f, _sounds.windSound, true, false, false, vehicleSoundsMainObject, 10, 100);
      }
      if (_groundSounds.standardSkidSound) {
        skiddingSoundAUD = GenerateAudioSource("Sound of skid", 10, 1, _groundSounds.standardSkidSound, false, false, false, vehicleSoundsMainObject, 10, 500);
      }
      if (_sounds.collisionSounds.Length > 0) {
        if (_sounds.collisionSounds[0]) {
          beatsSoundAUD = GenerateAudioSource("Sound of beats", 10, _sounds.volumeCollisionSounds, _sounds.collisionSounds[UnityEngine.Random.Range(0, _sounds.collisionSounds.Length)], false, false, false, vehicleSoundsMainObject, 10, 500);
        }
      }
      groundSoundsAUD = new AudioSource[_groundSounds.groundSounds.Length];
      if (_groundSounds.groundSounds.Length > 0) {
        for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
          if (_groundSounds.groundSounds[x].groundSound) {
            groundSoundsAUD[x] = GenerateAudioSource("GroundSounds" + x, 10, _groundSounds.groundSounds[x].volumeSound, _groundSounds.groundSounds[x].groundSound, true, false, false, vehicleSoundsMainObject, 10, 500);
          }
        }
      }
      groundSoundsAUDSkid = new AudioSource[_groundSounds.groundSounds.Length];
      if (_groundSounds.groundSounds.Length > 0) {
        for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
          if (_groundSounds.groundSounds[x].skiddingSound) {
            groundSoundsAUDSkid[x] = GenerateAudioSource("GroundSoundsSkid" + x, 10, _groundSounds.groundSounds[x].volumeSkid, _groundSounds.groundSounds[x].skiddingSound, true, false, false, vehicleSoundsMainObject, 10, 500);
          }
        }
      }


      //wheel impact sound
      Vector3 posWheel;
      Quaternion rotWheel;
      //rightFrontWheel
      _wheels.rightFrontWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      lastRightFrontPositionY = transform.InverseTransformPoint(posWheel).y;
      //leftFrontWheel
      _wheels.leftFrontWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      lastLeftFrontPositionY = transform.InverseTransformPoint(posWheel).y;
      //rightRearWheel
      _wheels.rightRearWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      lastRightRearPositionY = transform.InverseTransformPoint(posWheel).y;
      //leftRearWheel
      _wheels.leftRearWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      lastLeftRearPositionY = transform.InverseTransformPoint(posWheel).y;

      sensImpactFR = (0.25f - _sounds.sensibilityWheelImpact) * (2.65f * _wheels.rightFrontWheel.wheelCollider.radius);
      sensImpactFL = (0.25f - _sounds.sensibilityWheelImpact) * (2.65f * _wheels.leftFrontWheel.wheelCollider.radius);
      sensImpactRR = (0.25f - _sounds.sensibilityWheelImpact) * (2.65f * _wheels.rightRearWheel.wheelCollider.radius);
      sensImpactRL = (0.25f - _sounds.sensibilityWheelImpact) * (2.65f * _wheels.leftRearWheel.wheelCollider.radius);

      //extra wheels
      sensImpactExtraWheels = new float[_wheels.extraWheels.Length];
      lastPositionYExtraWheels = new float[_wheels.extraWheels.Length];
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        sensImpactExtraWheels[x] = (0.25f - _sounds.sensibilityWheelImpact) * (2.65f * _wheels.extraWheels[x].wheelCollider.radius);
        _wheels.extraWheels[x].wheelCollider.GetWorldPose(out posWheel, out rotWheel);
        lastPositionYExtraWheels[x] = transform.InverseTransformPoint(posWheel).y;
      }
    }




    public static float ClampAngle(float angle, float min, float max) {
      if (angle < -360F) { angle += 360F; }
      if (angle > 360F) { angle -= 360F; }
      return Mathf.Clamp(angle, min, max);
    }


    float DeformMesh(Mesh mesh, Vector3[] originalMesh, Transform localTransform, Vector3 contactPoint, Vector3 contactVelocity) {
      Vector3[] verticesDeformMesh = mesh.vertices;
      Vector3 localContactPointDeformMesh = localTransform.InverseTransformPoint(contactPoint);
      //
      Vector3 localContactForceDeformMesh = Vector3.zero;
      if (_damage.deformMesh.warpToTheCenter) {
        Vector3 vel = (contactPoint - _vehiclePhysicStabilizers.centerOfMass.position).normalized;
        localContactForceDeformMesh = -localTransform.InverseTransformDirection(vel);
      } else {
        localContactForceDeformMesh = localTransform.InverseTransformDirection(contactVelocity);
      }
      //
      float totalDamageDeformMesh = 0.0f;
      float damagedVerticesDeformMesh = 0;
      float computedDamage = _damage.deformMesh.hitDamage * (1 + Mathf.Clamp((KMh * _damage.deformMesh.hitDamage * 0.018f), 0, 0.7f)); //velocity_x_Damage = 0.018f
      float computedImpactArea = _damage.deformMesh.areaOfDeformation * (1 + Mathf.Clamp((KMh * _damage.deformMesh.areaOfDeformation * 0.018f), 0, 0.7f)); //velocity_x_Damage = 0.018f
      for (int i = 0; i < verticesDeformMesh.Length; i++) {
        float distDeformMesh = (localContactPointDeformMesh - verticesDeformMesh[i]).sqrMagnitude;
        if (distDeformMesh < computedImpactArea) {
          Vector3 damageDeformMesh = computedDamage * ((localContactForceDeformMesh * ((computedImpactArea * 1.2f) - Mathf.Sqrt(distDeformMesh))) / (computedImpactArea));// + Random.onUnitSphere*_damage.deformMesh.breaksInTheMesh;
          verticesDeformMesh[i] += damageDeformMesh;
          Vector3 deformDeformMesh = verticesDeformMesh[i] - originalMesh[i];
          float maxDistortion = _damage.deformMesh.hitDamage * 2.0f; // *_damage.deformMesh.maxDistortion (removed) -- (1 = ideal value)
          if (deformDeformMesh.sqrMagnitude > (maxDistortion * maxDistortion)) {
            verticesDeformMesh[i] = originalMesh[i] + deformDeformMesh.normalized * maxDistortion;
          }
          totalDamageDeformMesh += damageDeformMesh.magnitude;
          damagedVerticesDeformMesh++;
        }
      }
      mesh.vertices = verticesDeformMesh;
      mesh.RecalculateNormals();
      mesh.RecalculateBounds();
      return damagedVerticesDeformMesh > 0 ? totalDamageDeformMesh / damagedVerticesDeformMesh : 0.0f;
    }



    void SpeedometerAndOthers() { //On Update
      if (_speedometer._speedometerModel1__ScreenSpace.canvas_Gauges) {
        if (_vehicleState == ControlState.isPlayer) {
          //set parameters
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehicleMaxSpeed = _vehicleTorque.maxVelocityKMh;
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehicleMaxRPM = _vehicleSettings.maxVehicleRPM;
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.enabledGauge = true;
          float speedFactorModel1 = (KMh / _vehicleTorque.maxVelocityKMh);
          float rpmFactorModel1 = (_vehicleSettings.vehicleRPMValue / _vehicleSettings.maxVehicleRPM);
          if (theEngineIsRunning) {
            speedFactorSpeedometerModel1 = Mathf.Lerp(speedFactorSpeedometerModel1, speedFactorModel1, Time.deltaTime * 5);
            rpmFactorSpeedometerModel1 = Mathf.Lerp(rpmFactorSpeedometerModel1, rpmFactorModel1, Time.deltaTime * 5);
          } else {
            speedFactorSpeedometerModel1 = Mathf.Lerp(speedFactorSpeedometerModel1, 0, Time.deltaTime * 5);
            rpmFactorSpeedometerModel1 = Mathf.Lerp(rpmFactorSpeedometerModel1, 0, Time.deltaTime * 5);
          }
          //kmh
          if (groundedWheels > 0) {
            _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehicleKmh = (int)KMh;
            _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehiclePercentkmh = speedFactorSpeedometerModel1;
          }
          //rpm
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehicleCurrentRpm = (int)_vehicleSettings.vehicleRPMValue;
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehiclePercentrpm = rpmFactorSpeedometerModel1;
          //gear
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehicleCurrentGear = currentGear;
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.autoGears = automaticGears;
          //damage
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehiclePercentDamage = 1 - ((Mathf.Clamp(vehicleLife, 0.00001f, 2000)) / _damage.damageSupported);
          //nitro
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehiclePercentNitro = _additionalFeatures.timerNitro / _additionalFeatures.nitroTime;
          //fuel
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.vehiclePercentFuel = 1 - ((Mathf.Clamp(currentFuelLiters, 0.00001f, 500)) / _fuel.capacityInLiters);
          //handBrake
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.handBrakeTrue = handBrakeTrue;

          //position and size
          if (_speedometer._speedometerModel1__ScreenSpace.setPositionViaCode) {
            _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.uiOffset = _speedometer._speedometerModel1__ScreenSpace._UIHorizontalOffset;
          }
          if (_speedometer._speedometerModel1__ScreenSpace.setSizeViaCode) {
            _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.uiScaler = _speedometer._speedometerModel1__ScreenSpace._UIScale;
          }
        } else {
          _speedometer._speedometerModel1__ScreenSpace.canvas_Gauges.enabledGauge = false;
        }
      }
      if (_speedometer._speedometerModel2__WorldSpace.masterObject) {
        if (_vehicleState == ControlState.isPlayer) {
          if (!_speedometer._speedometerModel2__WorldSpace.masterObject.activeInHierarchy) {
            _speedometer._speedometerModel2__WorldSpace.masterObject.SetActive(true);
          }

          // Get Values
          float speedFactorModel2 = (KMh / _vehicleTorque.maxVelocityKMh);
          float rpmFactorModel2 = (_vehicleSettings.vehicleRPMValue / _vehicleSettings.maxVehicleRPM);
          if (theEngineIsRunning) {
            speedFactorSpeedometerModel2 = Mathf.MoveTowards(speedFactorSpeedometerModel2, speedFactorModel2, Time.deltaTime);
            rpmFactorSpeedometerModel2 = Mathf.MoveTowards(rpmFactorSpeedometerModel2, rpmFactorModel2, Time.deltaTime);
          } else {
            speedFactorSpeedometerModel2 = Mathf.Lerp(speedFactorSpeedometerModel2, 0, Time.deltaTime * 5);
            rpmFactorSpeedometerModel2 = Mathf.Lerp(rpmFactorSpeedometerModel2, 0, Time.deltaTime * 5);
          }

          //Speed Pointer
          if (_speedometer._speedometerModel2__WorldSpace.speedometerSpeedPointer) {
            float sumSpeedAngle = Mathf.Clamp01(speedFactorSpeedometerModel2) * 270.0f * _speedometer._speedometerModel2__WorldSpace.speedPointerRotationFactor;
            if (_speedometer._speedometerModel2__WorldSpace.rotationAxisSPEED == SpeedometerModel2.RotType.RotationInY) {
              _speedometer._speedometerModel2__WorldSpace.speedometerSpeedPointer.transform.localEulerAngles = new Vector3(speedPointerStartRotation.x, speedPointerStartRotation.y - sumSpeedAngle, speedPointerStartRotation.z);
            } else if (_speedometer._speedometerModel2__WorldSpace.rotationAxisSPEED == SpeedometerModel2.RotType.RotationInZ) {
              _speedometer._speedometerModel2__WorldSpace.speedometerSpeedPointer.transform.localEulerAngles = new Vector3(speedPointerStartRotation.x, speedPointerStartRotation.y, speedPointerStartRotation.z - sumSpeedAngle);
            }
          }

          //RPM Pointer
          if (_speedometer._speedometerModel2__WorldSpace.speedometerRPMPointer) {
            float sumRPMAngle = Mathf.Clamp01(rpmFactorSpeedometerModel2) * 270.0f * _speedometer._speedometerModel2__WorldSpace.RPMPointerRotationFactor;
            if (_speedometer._speedometerModel2__WorldSpace.rotationAxisRPM == SpeedometerModel2.RotType.RotationInY) {
              _speedometer._speedometerModel2__WorldSpace.speedometerRPMPointer.transform.localEulerAngles = new Vector3(rpmPointerStartRotation.x, rpmPointerStartRotation.y - sumRPMAngle, rpmPointerStartRotation.z);
            } else if (_speedometer._speedometerModel2__WorldSpace.rotationAxisRPM == SpeedometerModel2.RotType.RotationInZ) {
              _speedometer._speedometerModel2__WorldSpace.speedometerRPMPointer.transform.localEulerAngles = new Vector3(rpmPointerStartRotation.x, rpmPointerStartRotation.y, rpmPointerStartRotation.z - sumRPMAngle);
            }
          }

          //FUEL Pointer
          if (_speedometer._speedometerModel2__WorldSpace.speedometerFuelPointer) {
            float percentageToDecrement = 1 - ((Mathf.Clamp(currentFuelLiters, 0.00001f, 500)) / _fuel.capacityInLiters);
            float sumFuelAngle = Mathf.Clamp01(percentageToDecrement) * 135.0f * _speedometer._speedometerModel2__WorldSpace.FuelPointerRotationFactor;
            if (_speedometer._speedometerModel2__WorldSpace.rotationAxisFUEL == SpeedometerModel2.RotType.RotationInY) {
              _speedometer._speedometerModel2__WorldSpace.speedometerFuelPointer.transform.localEulerAngles = new Vector3(fuelPointerStartRotation.x, fuelPointerStartRotation.y + sumFuelAngle, fuelPointerStartRotation.z);
            } else if (_speedometer._speedometerModel2__WorldSpace.rotationAxisFUEL == SpeedometerModel2.RotType.RotationInZ) {
              _speedometer._speedometerModel2__WorldSpace.speedometerFuelPointer.transform.localEulerAngles = new Vector3(fuelPointerStartRotation.x, fuelPointerStartRotation.y, fuelPointerStartRotation.z + sumFuelAngle);
            }
          }

          //GEAR Text
          if (_speedometer._speedometerModel2__WorldSpace.gearText) {
            if (currentGear >= 0) {
              _speedometer._speedometerModel2__WorldSpace.gearText.text = "D" + currentGear;
            } else {
              _speedometer._speedometerModel2__WorldSpace.gearText.text = "R";
            }
          }

          //NITRO BAR FILLED
          if (_additionalFeatures.useNitro) {
            if (_speedometer._speedometerModel2__WorldSpace.nitroBar_filled) {
              _speedometer._speedometerModel2__WorldSpace.nitroBar_filled.enabled = true;
              _speedometer._speedometerModel2__WorldSpace.nitroBar_filled.fillAmount = _additionalFeatures.timerNitro / _additionalFeatures.nitroTime;
            }
          } else {
            if (_speedometer._speedometerModel2__WorldSpace.nitroBar_filled) {
              _speedometer._speedometerModel2__WorldSpace.nitroBar_filled.enabled = false;
              _speedometer._speedometerModel2__WorldSpace.nitroBar_filled.fillAmount = 0;
            }
          }

          //
        } else {
          if (_speedometer._speedometerModel2__WorldSpace.masterObject.activeInHierarchy) {
            _speedometer._speedometerModel2__WorldSpace.masterObject.SetActive(false);
          }
        }
      }
    }
    void NitroFunction() {
      if (_additionalFeatures.useNitro) {
        if (_additionalFeatures.nitroIsTrueInput) {
          if (_additionalFeatures.canUseNitro) {
            if (_additionalFeatures.timerNitro > 0) {
              _additionalFeatures.nitroIsTrueVar = true;
              _additionalFeatures.timerNitro -= Time.deltaTime;
              if (_additionalFeatures.timerNitro < 0) {
                _additionalFeatures.timerNitro = 0.001f; //avoid division by 0
                _additionalFeatures.nitroIsTrueVar = false;
                _additionalFeatures.canUseNitro = false;
              }
            } else {
              _additionalFeatures.canUseNitro = false;
              _additionalFeatures.nitroIsTrueInput = false;
            }
          } else {
            _additionalFeatures.nitroIsTrueInput = false;
            _additionalFeatures.nitroIsTrueVar = false;
            _additionalFeatures.timerNitro += (Time.deltaTime * _additionalFeatures.rechargeSpeed);
            _additionalFeatures.timerNitro = Mathf.Clamp(_additionalFeatures.timerNitro, 0.001f, _additionalFeatures.nitroTime);
            if (_additionalFeatures.timerNitro > 1) {
              _additionalFeatures.canUseNitro = true;
            }
          }
        } else {
          _additionalFeatures.nitroIsTrueVar = false;
          _additionalFeatures.timerNitro += (Time.deltaTime * _additionalFeatures.rechargeSpeed);
          _additionalFeatures.timerNitro = Mathf.Clamp(_additionalFeatures.timerNitro, 0.001f, _additionalFeatures.nitroTime);
          if (_additionalFeatures.timerNitro > 1) {
            _additionalFeatures.canUseNitro = true;
          }
        }
      }
    }
    void VehicleRPMFunction() { //On Update
      if (theEngineIsRunning && (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI)) {
        float maxSpeedCurrentGear = 0;
        if (currentGear == -1 || currentGear == 0) {
          maxSpeedCurrentGear = _vehicleTorque.maxVelocityGears[0] * _vehicleTorque.speedOfGear;
        } else {
          maxSpeedCurrentGear = _vehicleTorque.maxVelocityGears[currentGear - 1] * _vehicleTorque.speedOfGear;
        }

        float velxCurrentRPM = Mathf.Clamp(KMh, 0.1f, maxSpeedCurrentGear);
        float nextAUDRPMValue = (velxCurrentRPM / maxSpeedCurrentGear) * _sounds.speedOfEngineSound;

        // pitch RPM
        if (handBrakeTrue || currentGear == 0) {
          if (automaticGears) {
            nextAUDRPMValue = 0.85f + (Mathf.Abs(verticalInput) * _sounds.speedOfEngineSound * 0.8f);
          } else {
            nextAUDRPMValue = 0.85f + (Mathf.Clamp01(verticalInput) * _sounds.speedOfEngineSound * 0.8f);
          }
        }

        nextAUDRPMValue = Mathf.Clamp(nextAUDRPMValue, 0.85f, _sounds.speedOfEngineSound);
        pitchAUDforRPM = Mathf.MoveTowards(pitchAUDforRPM, nextAUDRPMValue, Time.deltaTime * 3.0f);

        if (pitchAUDforRPM > (_sounds.speedOfEngineSound * 0.95f) && Mathf.Abs(verticalInput) > 0.25f) {
          pitchAUDforRPM = _sounds.speedOfEngineSound * 0.85f;
          lastKnownTorque = lastKnownTorque * 0.5f;
        }

        //set RPM value
        _vehicleSettings.vehicleRPMValue = _vehicleSettings.rpmCurve.Evaluate(pitchAUDforRPM);
        //

        // RPM by TORQUE -- aditional torque (finalTorque = torque + rpmTorqueFactor*torque)
        if (_vehicleTorque.rpmAffectsTheTorque > 0.25f) {
          if (Mathf.Abs(verticalInput) > 0.9f) {
            if (handBrakeTrue || currentGear == 0) {
              rpmTorqueFactor = Mathf.Lerp(rpmTorqueFactor, _vehicleTorque.rpmAffectsTheTorque, Time.deltaTime * 2.5f);
            } else {
              rpmTorqueFactor = Mathf.Lerp(rpmTorqueFactor, 0.0f, Time.deltaTime * 1.5f);
            }
          } else {
            rpmTorqueFactor = Mathf.Lerp(rpmTorqueFactor, 0.0f, Time.deltaTime * 1.5f);
          }
        } else {
          rpmTorqueFactor = 0.0f;
        }
        //
      } else {
        if (enableEngineSound) {
          pitchAUDforRPM = 0.7f;
        } else {
          pitchAUDforRPM = 0;
          _vehicleSettings.vehicleRPMValue = Mathf.Lerp(_vehicleSettings.vehicleRPMValue, 0.0f, Time.deltaTime * 10);
        }
        rpmTorqueFactor = 0;
      }
    }
    void DiscoverAverageRpm() {
      float sumRPM = 0;
      _wheels.rightFrontWheel.wheelColliderRPM = _wheels.rightFrontWheel.wheelCollider.rpm;
      if (wheelFDIsGrounded) {
        sumRPM += _wheels.rightFrontWheel.wheelColliderRPM;
      }
      //
      _wheels.leftFrontWheel.wheelColliderRPM = _wheels.leftFrontWheel.wheelCollider.rpm;
      if (wheelFEIsGrounded) {
        sumRPM += _wheels.leftFrontWheel.wheelColliderRPM;
      }
      //
      _wheels.rightRearWheel.wheelColliderRPM = _wheels.rightRearWheel.wheelCollider.rpm;
      if (wheelTDIsGrounded) {
        sumRPM += _wheels.rightRearWheel.wheelColliderRPM;
      }
      //
      _wheels.leftRearWheel.wheelColliderRPM = _wheels.leftRearWheel.wheelCollider.rpm;
      if (wheelTEIsGrounded) {
        sumRPM += _wheels.leftRearWheel.wheelColliderRPM;
      }
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        _wheels.extraWheels[x].wheelColliderRPM = _wheels.extraWheels[x].wheelCollider.rpm;
        if (_wheels.extraWheels[x].wheelCollider.isGrounded) {
          sumRPM += _wheels.extraWheels[x].wheelColliderRPM;
        }
      }
      mediumRPM = sumRPM / groundedWheels;
      if (float.IsNaN(mediumRPM)) {
        mediumRPM = 0;
      }
      if (Mathf.Abs(mediumRPM) < 0.01f) {
        mediumRPM = 0.0f;
      }
    }
    void CheckGroundedWheelsOnFixedUpdate() {
      //get main wheels - is grounded
      wheelFDIsGrounded = _wheels.rightFrontWheel.wheelCollider.isGrounded;
      wheelFEIsGrounded = _wheels.leftFrontWheel.wheelCollider.isGrounded;
      wheelTDIsGrounded = _wheels.rightRearWheel.wheelCollider.isGrounded;
      wheelTEIsGrounded = _wheels.leftRearWheel.wheelCollider.isGrounded;
      //
      groundedWheels = 0;
      if (wheelFDIsGrounded) {
        groundedWheels++;
      }
      if (wheelFEIsGrounded) {
        groundedWheels++;
      }
      if (wheelTDIsGrounded) {
        groundedWheels++;
      }
      if (wheelTEIsGrounded) {
        groundedWheels++;
      }
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        if (_wheels.extraWheels[x].wheelCollider.isGrounded) {
          groundedWheels++;
        }
      }
    }



    public void EnterInVehicle(bool _isPlayer) {
      if (_vehicleState == ControlState.isNull || (_vehicleState == ControlState.isAI && _isPlayer)) {
        if (_isPlayer) {
          _vehicleState = ControlState.isPlayer;
        } else {
          _vehicleState = ControlState.isAI;
          automaticGears = true;
          handBrakeTrue = false;
          handBrakeSoundWasPlayed = true;
        }
      }
    }
    public void ExitTheVehicle() {
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        _vehicleState = ControlState.isNull;
        if (automaticGears) {
          handBrakeTrue = true;
          handBrakeSoundWasPlayed = true;
        }
      }
    }

    public void SetWheelCollidersFrictionFixedUpdate(WheelCollider collider) {
      if (collider.isGrounded) {
        bool changeFriction = false;
        bool changeWheelsFriction = false;
        float nextFrictionFW = 99;
        float nextFrictionSW = 99;
        collider.GetGroundHit(out tempWheelHit);
        for (int x = 0; x < _groundFriction.grounds.Length; x++) {
          if (!changeWheelsFriction) {
            switch (_groundDetection) {
              case GroundDetectionMode.Tag:
                if (!string.IsNullOrEmpty(_groundFriction.grounds[x].groundTag)) {
                  if (tempWheelHit.collider.gameObject.CompareTag(_groundFriction.grounds[x].groundTag)) {
                    nextFrictionFW = _groundFriction.grounds[x].forwardFriction;
                    nextFrictionSW = _groundFriction.grounds[x].sidewaysFriction;
                    changeWheelsFriction = true;
                    break;
                  }
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.PhysicMaterial:
                if (_groundFriction.grounds[x].physicMaterial) {
                  if (tempWheelHit.collider.sharedMaterial == _groundFriction.grounds[x].physicMaterial) {
                    nextFrictionFW = _groundFriction.grounds[x].forwardFriction;
                    nextFrictionSW = _groundFriction.grounds[x].sidewaysFriction;
                    changeWheelsFriction = true;
                    break;
                  }
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.TerrainTextureIndices:
                if (activeTerrain_optional) {
                  if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                    int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                    if (dominantTerrainIndex != -1) {
                      if (_groundFriction.grounds[x].terrainTextureIndices.Count > 0 && _groundFriction.grounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                        nextFrictionFW = _groundFriction.grounds[x].forwardFriction;
                        nextFrictionSW = _groundFriction.grounds[x].sidewaysFriction;
                        changeWheelsFriction = true;
                        break;
                      }
                    }
                  }
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.All:
                //tag
                if (!string.IsNullOrEmpty(_groundFriction.grounds[x].groundTag)) {
                  if (tempWheelHit.collider.gameObject.CompareTag(_groundFriction.grounds[x].groundTag)) {
                    nextFrictionFW = _groundFriction.grounds[x].forwardFriction;
                    nextFrictionSW = _groundFriction.grounds[x].sidewaysFriction;
                    changeWheelsFriction = true;
                    break;
                  }
                }
                //physicMaterial
                if (_groundFriction.grounds[x].physicMaterial) {
                  if (tempWheelHit.collider.sharedMaterial == _groundFriction.grounds[x].physicMaterial) {
                    nextFrictionFW = _groundFriction.grounds[x].forwardFriction;
                    nextFrictionSW = _groundFriction.grounds[x].sidewaysFriction;
                    changeWheelsFriction = true;
                    break;
                  }
                }
                //terrainTextureIndice
                if (activeTerrain_optional) {
                  if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                    int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                    if (dominantTerrainIndex != -1) {
                      if (_groundFriction.grounds[x].terrainTextureIndices.Count > 0 && _groundFriction.grounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                        nextFrictionFW = _groundFriction.grounds[x].forwardFriction;
                        nextFrictionSW = _groundFriction.grounds[x].sidewaysFriction;
                        changeWheelsFriction = true;
                        break;
                      }
                    }
                  }
                }
                break;
                //==============================================================================================================================
            }
          }
        }
        if (!changeWheelsFriction) {
          nextFrictionFW = _groundFriction.standardForwardFriction;
          nextFrictionSW = _groundFriction.standardSideFriction;
        }
        if ((collider.forwardFriction.stiffness != nextFrictionFW) || (collider.sidewaysFriction.stiffness != nextFrictionSW)) {
          changeFriction = true;
        }

        if (changeFriction) {
          WheelFrictionCurve wheelFrictionCurveFW = new WheelFrictionCurve();
          WheelFrictionCurve wheelFrictionCurveSW = new WheelFrictionCurve();
          wheelFrictionCurveFW.stiffness = nextFrictionFW;
          wheelFrictionCurveSW.stiffness = nextFrictionSW;
          //friction Fw
          wheelFrictionCurveFW.extremumSlip = collider.forwardFriction.extremumSlip;
          wheelFrictionCurveFW.extremumValue = collider.forwardFriction.extremumValue;
          wheelFrictionCurveFW.asymptoteSlip = collider.forwardFriction.asymptoteSlip;
          wheelFrictionCurveFW.asymptoteValue = collider.forwardFriction.asymptoteValue;
          collider.forwardFriction = wheelFrictionCurveFW;
          //friction Sw
          wheelFrictionCurveSW.extremumSlip = collider.sidewaysFriction.extremumSlip;
          wheelFrictionCurveSW.extremumValue = collider.sidewaysFriction.extremumValue;
          wheelFrictionCurveSW.asymptoteSlip = collider.sidewaysFriction.asymptoteSlip;
          wheelFrictionCurveSW.asymptoteValue = collider.sidewaysFriction.asymptoteValue;
          collider.sidewaysFriction = wheelFrictionCurveSW;
        }
      }
    }



    void SetWheelForces(WheelCollider wheelCollider) {
      wheelCollider.GetGroundHit(out tempWheelHit);
      if (wheelCollider.isGrounded) {
        TireSlips(wheelCollider, tempWheelHit);

        //lateral forces
        float distanceXForceTemp = ms_Rigidbody.centerOfMass.y - transform.InverseTransformPoint(wheelCollider.transform.position).y + wheelCollider.radius + (1.0f - wheelCollider.suspensionSpring.targetPosition) * wheelCollider.suspensionDistance;
        Vector3 lateralForcePointTemp = tempWheelHit.point + wheelCollider.transform.up * _vehiclePhysicStabilizers.helpToStraightenOut * distanceXForceTemp;
        Vector3 lateralForceTemp = tempWheelHit.sidewaysDir * (tireFO.x);
        if (Mathf.Abs(volantDir_horizontalInput) > 0.1f && wheelCollider.steerAngle != 0.0f && Mathf.Sign(wheelCollider.steerAngle) != Mathf.Sign(tireSL.x)) {
          lateralForcePointTemp += tempWheelHit.forwardDir * _vehiclePhysicStabilizers.helpToTurn;
        }
        ms_Rigidbody.AddForceAtPosition(lateralForceTemp, lateralForcePointTemp);

        //forword forces - only take effect if the vehicle exceeds the maximum speed
        Vector3 forwardForceTemp = tempWheelHit.forwardDir * (tireFO.y) * 3;
        ms_Rigidbody.AddForceAtPosition(forwardForceTemp, tempWheelHit.point + transform.up); //if (kmh > maxSpeed) = 1, else, = 0
      }
    }

    public Vector2 WheelLocalVelocity(WheelHit wheelHit) {
      Vector2 tempLocalVelocityVector2 = new Vector2(0, 0);
      Vector3 tempWheelVelocityVector3 = ms_Rigidbody.GetPointVelocity(wheelHit.point);
      Vector3 velocityLocalWheelTemp = tempWheelVelocityVector3 - Vector3.Project(tempWheelVelocityVector3, wheelHit.normal);
      tempLocalVelocityVector2.y = (float)Math.Round(Vector3.Dot(wheelHit.forwardDir, velocityLocalWheelTemp), 3);
      tempLocalVelocityVector2.x = (float)Math.Round(Vector3.Dot(wheelHit.sidewaysDir, velocityLocalWheelTemp), 3);
      return tempLocalVelocityVector2;
    }
    public Vector2 LocalSurfaceForce(WheelHit wheelHit) {
      //get Ribidbody velocity point
      Vector3 rbPointVel = ms_Rigidbody.GetPointVelocity(wheelHit.point);
      Vector3 wheelSpeedLocalSurface = new Vector3((float)Math.Round(rbPointVel.x, 3), (float)Math.Round(rbPointVel.y, 3), (float)Math.Round(rbPointVel.z, 3));
      //
      //(if < 1.0), return 1    //   (if > 10.0f), return 0         /default = 1.0, 0.25
      float forceFactorTempLocalSurface = Mathf.InverseLerp(10.0f, 1.0f, (wheelSpeedLocalSurface - Vector3.Project(wheelSpeedLocalSurface, wheelHit.normal)).sqrMagnitude);
      //
      Vector2 surfaceLocalForce = Vector2.zero;
      if (forceFactorTempLocalSurface > 0.0f) {
        Vector3 surfaceLocalForceTemp = Vector3.up * 1000000.0f;
        float normalTemp = Vector3.Dot(Vector3.up, wheelHit.normal);
        if (normalTemp > 0.000001f) {
          Vector3 downForceUPTemp = Vector3.up * wheelHit.force / normalTemp;
          surfaceLocalForceTemp = downForceUPTemp - Vector3.Project(downForceUPTemp, wheelHit.normal);
        }
        surfaceLocalForce.y = (float)Math.Round(Vector3.Dot(wheelHit.forwardDir, surfaceLocalForceTemp), 3);
        surfaceLocalForce.x = (float)Math.Round(Vector3.Dot(wheelHit.sidewaysDir, surfaceLocalForceTemp), 3);
        surfaceLocalForce *= forceFactorTempLocalSurface;
      }
      return surfaceLocalForce;
    }

    public void TireSlips(WheelCollider wheelCollider, WheelHit wheelHit) {
      Vector2 tireSlips = Vector2.zero;
      Vector2 tireForces = Vector2.zero;
      float minSlipY = 0;
      Vector2 localVelocityWheelTireSlips = WheelLocalVelocity(wheelHit);
      Vector2 localSurfaceForceDTireSlips = LocalSurfaceForce(wheelHit); //if (wheel relative velocity < 0.25), this force is 1, else, this force is 0

      float reverseForce = 0;
      if (KMh > _vehicleTorque.maxVelocityKMh) {
        reverseForce = -(ms_Rigidbody.velocity.magnitude * _vehicleSettings.vehicleMass * 0.003f);
      }

      tireSlips.x = localVelocityWheelTireSlips.x;
      tireSlips.y = (tempWheelHit.sidewaysSlip * (Mathf.Abs(verticalInput) + Mathf.Abs(volantDir_horizontalInput)));
      float downForceTireSlips = _vehiclePhysicStabilizers.localSurfaceForce * _vehicleSettings.vehicleMass * (wheelCollider.suspensionSpring.spring * 0.000025f);
      if (wheelCollider.brakeTorque > 10) {
        float wheelMaxBrakeSlip = Mathf.Max(Mathf.Abs(localVelocityWheelTireSlips.y * 0.2f), 0.3f);
        minSlipY = Mathf.Clamp(Mathf.Abs(reverseForce * tireSlips.x) / downForceTireSlips, 0.0f, wheelMaxBrakeSlip);
      } else {
        minSlipY = Mathf.Min(Mathf.Abs(reverseForce * tireSlips.x) / downForceTireSlips, Mathf.Clamp((verticalInput * 2.5f), -2.5f, 1.0f));
        if (reverseForce != 0.0f && minSlipY < 0.1f) {
          minSlipY = 0.1f;
        }
      }

      if (Mathf.Abs(tireSlips.y) < minSlipY) {
        tireSlips.y = minSlipY * Mathf.Sign(tireSlips.y);
      }
      Vector2 rawTireForceTireSlips = -(downForceTireSlips * tireSlips.normalized);
      rawTireForceTireSlips.x = Mathf.Abs(rawTireForceTireSlips.x);
      rawTireForceTireSlips.y = Mathf.Abs(rawTireForceTireSlips.y);

      float estimatedSprungMass = Mathf.Clamp(wheelHit.force / Mathf.Abs(Physics.gravity.y), 0.0f, wheelCollider.sprungMass) * 0.5f;
      Vector2 localRigForceTireSlips = (-estimatedSprungMass * localVelocityWheelTireSlips * (1 / fixedDeltaTime)) + localSurfaceForceDTireSlips * _vehiclePhysicStabilizers.localSurfaceForce;
      tireForces.x = Mathf.Clamp(localRigForceTireSlips.x, -rawTireForceTireSlips.x * _vehiclePhysicStabilizers.localSurfaceForce, +rawTireForceTireSlips.x * _vehiclePhysicStabilizers.localSurfaceForce);
      tireForces.y = Mathf.Clamp(reverseForce, -rawTireForceTireSlips.y, +rawTireForceTireSlips.y);//only active if kmh > maxSpeed

      tireSL = tireSlips * _vehiclePhysicStabilizers.tireSlipsFactor;
      tireFO = tireForces * _vehiclePhysicStabilizers.tireSlipsFactor;
    }



    void UpdateWheelMeshes() {
      Vector3 posWheel;
      Quaternion rotWheel;

      //rightFrontWheel
      _wheels.rightFrontWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      _wheels.rightFrontWheel.wheelWorldPosition = posWheel;
      if (_wheels.rightFrontWheel.wheelMesh) {
        _wheels.rightFrontWheel.wheelMesh.position = posWheel;
        _wheels.rightFrontWheel.wheelMesh.rotation = rotWheel;
      }
      //leftFrontWheel
      _wheels.leftFrontWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      _wheels.leftFrontWheel.wheelWorldPosition = posWheel;
      if (_wheels.leftFrontWheel.wheelMesh) {
        _wheels.leftFrontWheel.wheelMesh.position = posWheel;
        _wheels.leftFrontWheel.wheelMesh.rotation = rotWheel;
      }
      //rightRearWheel
      _wheels.rightRearWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      _wheels.rightRearWheel.wheelWorldPosition = posWheel;
      if (_wheels.rightRearWheel.wheelMesh) {
        _wheels.rightRearWheel.wheelMesh.position = posWheel;
        _wheels.rightRearWheel.wheelMesh.rotation = rotWheel;
      }
      //leftRearWheel
      _wheels.leftRearWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
      _wheels.leftRearWheel.wheelWorldPosition = posWheel;
      if (_wheels.leftRearWheel.wheelMesh) {
        _wheels.leftRearWheel.wheelMesh.position = posWheel;
        _wheels.leftRearWheel.wheelMesh.rotation = rotWheel;
      }

      if (_wheels.extraWheels.Length > 0) {
        for (int i = 0; i < _wheels.extraWheels.Length; i++) {
          if (_wheels.extraWheels[i].wheelCollider) {
            _wheels.extraWheels[i].wheelCollider.GetWorldPose(out posWheel, out rotWheel);
            _wheels.extraWheels[i].wheelWorldPosition = posWheel;
            if (_wheels.extraWheels[i].wheelMesh) {
              _wheels.extraWheels[i].wheelMesh.position = posWheel;
              _wheels.extraWheels[i].wheelMesh.rotation = rotWheel;
            }
          }
        }
      }
    }



    void StabilizeAngularRotation() {
      if (_vehiclePhysicStabilizers.stabilizeAngularVelocity > 0.1f) {  // Avoid unnecessary processing for very low forces
        if (Mathf.Abs(volantDir_horizontalInput) < 0.9f) {
          ms_Rigidbody.angularVelocity = Vector3.Lerp(ms_Rigidbody.angularVelocity, new Vector3(ms_Rigidbody.angularVelocity.x, 0, ms_Rigidbody.angularVelocity.z), fixedDeltaTime * _vehiclePhysicStabilizers.stabilizeAngularVelocity);
        }
      }
    }

    void StabilizeAirRotation() {
      if (_vehicleState == ControlState.isPlayer) {
        if (_vehiclePhysicStabilizers.airRotation > 0.02f) {  // Avoid unnecessary processing for very low forces
          if (!colliding) {
            if (groundedWheels == 0) {
              Vector3 axisFromRotate = Vector3.Cross(transform.up, Vector3.up);
              Vector3 torqueForceAirRotation = axisFromRotate.normalized * axisFromRotate.magnitude * _vehiclePhysicStabilizers.airRotation * 5.0f;
              torqueForceAirRotation -= ms_Rigidbody.angularVelocity;
              ms_Rigidbody.AddTorque(torqueForceAirRotation * _vehicleSettings.vehicleMass * 0.02f, ForceMode.Impulse);
              if (Mathf.Abs(volantDir_horizontalInput) > 0.1f) {
                ms_Rigidbody.AddTorque(transform.forward * -volantDir_horizontalInput * _vehicleSettings.vehicleMass * _vehiclePhysicStabilizers.airRotation * 1.5f);
              }
              if (Mathf.Abs(verticalInput) > 0.1f) {
                ms_Rigidbody.AddTorque(transform.right * verticalInput * _vehicleSettings.vehicleMass * _vehiclePhysicStabilizers.airRotation * 1.1f);
              }
            }
          }
        }
      }
    }

    void StabilizeMotorBrakeForce() {
      if (theEngineIsRunning) {
        int currentGearApplyForce = 0;
        if (currentGear > 0) {
          currentGearApplyForce = (currentGear - 1);
        }
        //
        int forwardForce = 1;
        if (KMh > (_vehicleTorque.maxVelocityGears[currentGearApplyForce] * _vehicleTorque.speedOfGear * _brakes.speedFactorEngineBrake)) {
          if (wheelFDIsGrounded && wheelFEIsGrounded && wheelTDIsGrounded && wheelTEIsGrounded) {
            if (currentGear > 0 && mediumRPM > 0) {
              forwardForce = 1;
            } else if (currentGear <= 0 && mediumRPM < 0) {
              forwardForce = -1;
            } else {
              forwardForce = 0;
            }
            float finalForce = _brakes.forceEngineBrake * _vehicleSettings.vehicleMass * forwardForce * 5;
            if (Mathf.Abs(KMh) > 1.2f) {
              currentEngineBrakeLerpValue = Mathf.Lerp(currentEngineBrakeLerpValue, finalForce, fixedDeltaTime);
              ms_Rigidbody.AddForce(-transform.forward * currentEngineBrakeLerpValue);
            } else {
              currentEngineBrakeLerpValue = Mathf.Lerp(currentEngineBrakeLerpValue, 0.0f, fixedDeltaTime);
            }
          }
        } else {
          currentEngineBrakeLerpValue = Mathf.Lerp(currentEngineBrakeLerpValue, 0.0f, fixedDeltaTime);
        }
      } else {
        currentEngineBrakeLerpValue = 0;
      }
    }

    void StabilizeVehicleRotation() {
      if (_vehiclePhysicStabilizers.stabilizeSlippage > 0.1f) {
        _wheels.rightFrontWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (tempWheelHit.normal == Vector3.zero) {
          return;
        }
        _wheels.leftFrontWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (tempWheelHit.normal == Vector3.zero) {
          return;
        }
        _wheels.rightRearWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (tempWheelHit.normal == Vector3.zero) {
          return;
        }
        _wheels.leftRearWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (tempWheelHit.normal == Vector3.zero) {
          return;
        }
        for (int x = 0; x < _wheels.extraWheels.Length; x++) {
          _wheels.extraWheels[x].wheelCollider.GetGroundHit(out tempWheelHit);
          if (tempWheelHit.normal == Vector3.zero) {
            return;
          }
        }

        if (Mathf.Abs(previousRotation - transform.eulerAngles.y) < 10f) {
          var tempQuaternion = (transform.eulerAngles.y - previousRotation) * _vehiclePhysicStabilizers.stabilizeSlippage;
          Quaternion tempRotStabilizers = Quaternion.AngleAxis(tempQuaternion, Vector3.up);
          ms_Rigidbody.velocity = tempRotStabilizers * ms_Rigidbody.velocity;
        }

        previousRotation = transform.eulerAngles.y;
      }
    }

    void StabilizeVehicleRollForces() {
      if (_vehiclePhysicStabilizers.antiRollForce > 0.05f) { // Avoid unnecessary processing for very low forces
        float leftFrontForce = 1.0f;
        float rightFrontForce = 1.0f;
        float leftRearForce = 1.0f;
        float rightRearForce = 1.0f;

        // [-_wheels.leftRearWheel.wheelCollider.transform.InverseTransformPoint (tempWheelHit.point).y] >> Is the distance between the center of the wheel and the hit.point

        //Rear wheels 
        bool isGround1 = _wheels.leftRearWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (isGround1) {
          leftRearForce = (-_wheels.leftRearWheel.wheelCollider.transform.InverseTransformPoint(tempWheelHit.point).y - _wheels.leftRearWheel.wheelCollider.radius) / _wheels.leftRearWheel.wheelCollider.suspensionDistance;
        }
        bool isGround2 = _wheels.rightRearWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (isGround2) {
          rightRearForce = (-_wheels.rightRearWheel.wheelCollider.transform.InverseTransformPoint(tempWheelHit.point).y - _wheels.rightRearWheel.wheelCollider.radius) / _wheels.rightRearWheel.wheelCollider.suspensionDistance;
        }
        //front wheels
        bool isGround3 = _wheels.leftFrontWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (isGround3) {
          leftFrontForce = (-_wheels.leftFrontWheel.wheelCollider.transform.InverseTransformPoint(tempWheelHit.point).y - _wheels.leftFrontWheel.wheelCollider.radius) / _wheels.leftFrontWheel.wheelCollider.suspensionDistance;
        }
        bool isGround4 = _wheels.rightFrontWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        if (isGround4) {
          rightFrontForce = (-_wheels.rightFrontWheel.wheelCollider.transform.InverseTransformPoint(tempWheelHit.point).y - _wheels.rightFrontWheel.wheelCollider.radius) / _wheels.rightFrontWheel.wheelCollider.suspensionDistance;
        }

        //apply forces 
        float roolForce1 = (leftRearForce - rightRearForce) * _vehiclePhysicStabilizers.antiRollForce * _vehicleSettings.vehicleMass * inclinationFactorForcesDown;
        float roolForce2 = (leftFrontForce - rightFrontForce) * _vehiclePhysicStabilizers.antiRollForce * _vehicleSettings.vehicleMass * inclinationFactorForcesDown;
        //rear wheels
        if (isGround1) {
          ms_Rigidbody.AddForceAtPosition(_wheels.leftRearWheel.wheelCollider.transform.up * -roolForce1, _wheels.leftRearWheel.wheelCollider.transform.position);
        }
        if (isGround2) {
          ms_Rigidbody.AddForceAtPosition(_wheels.rightRearWheel.wheelCollider.transform.up * roolForce1, _wheels.rightRearWheel.wheelCollider.transform.position);
        }
        //front wheels
        if (isGround3) {
          ms_Rigidbody.AddForceAtPosition(_wheels.leftFrontWheel.wheelCollider.transform.up * -roolForce2, _wheels.leftFrontWheel.wheelCollider.transform.position);
        }
        if (isGround4) {
          ms_Rigidbody.AddForceAtPosition(_wheels.rightFrontWheel.wheelCollider.transform.up * roolForce2, _wheels.rightFrontWheel.wheelCollider.transform.position);
        }
      }
    }



    void FuelManager() {
      if (theEngineIsRunning) {
        if (!_fuel.infinityFuel) {
          currentFuelLiters -= (((Mathf.Clamp(pitchAUDforRPM, 0.85f, _sounds.speedOfEngineSound) / _sounds.speedOfEngineSound) * Time.deltaTime) / 10.0f) * _fuel.consumption;
        } else {
          currentFuelLiters = _fuel.capacityInLiters;
        }

        if (currentFuelLiters <= 0) {
          currentFuelLiters = 0;
          StartCoroutine("StartEngineCoroutine", false);
        }
      }
    }



    void ParticlesEmitter() {
      if (enableParticlesOnStart) {

        //damage particle
        if (enableDamageOnStart) {
          if (_particles.damageSmoke.Length > 0) {
            for (int x = 0; x < _particles.damageSmoke.Length; x++) {
              ParticleSystem.EmissionModule tempParticle = _particles.damageSmoke[x].emission;
              if (vehicleLife / _damage.damageSupported < 0.5f) {
                if (!_particles.damageSmoke[x].isPlaying) {
                  _particles.damageSmoke[x].Play(true);
                }
                tempParticle.enabled = true;
                if (vehicleLife / _damage.damageSupported > 0.3f) {
                  tempParticle.rateOverTime = 1;
                } else {
                  tempParticle.rateOverTime = 50;
                }
              } else {
                tempParticle.enabled = false;
              }
            }
          }
        }

        //nitro
        if (_additionalFeatures.useNitro) {
          if (_particles.nitroParticles.Length > 0) {
            for (int x = 0; x < _particles.nitroParticles.Length; x++) {
              ParticleSystem.EmissionModule tempParticleN = _particles.nitroParticles[x].emission;
              if (_additionalFeatures.nitroIsTrueVar) {
                if (!_particles.nitroParticles[x].isPlaying) {
                  _particles.nitroParticles[x].Play(true);
                }
                tempParticleN.enabled = true;
              } else {
                tempParticleN.enabled = false;
              }
            }
          }
        }

        //exhaustSmoke
        if (_particles.exhaustSmoke.Length > 0) {
          if (theEngineIsRunning) {
            for (int x = 0; x < _particles.exhaustSmoke.Length; x++) {
              if (_particles.exhaustSmoke[x].smoke) {
                ParticleSystem.EmissionModule particleTemp = _particles.exhaustSmoke[x].smoke.emission;
                ParticleSystem.MainModule mainModule = _particles.exhaustSmoke[x].smoke.main;
                if (KMh < _particles.exhaustSmoke[x].criticalVelocity) {
                  particleTemp.enabled = true;
                  if (Mathf.Abs(verticalInput) > 0.05f) {
                    mainModule.startSpeed = 2.5f + 0.1f * KMh;
                  } else {
                    mainModule.startSpeed = 1.5f;
                  }
                } else {
                  particleTemp.enabled = false;
                }
              }
            }
          } else {
            for (int x = 0; x < _particles.exhaustSmoke.Length; x++) {
              if (_particles.exhaustSmoke[x].smoke) {
                ParticleSystem.EmissionModule particleTemp = _particles.exhaustSmoke[x].smoke.emission;
                particleTemp.enabled = false;
              }
            }
          }
        }
      }
      //dust generated by wheels
      if (wheelDustParticle) {
        if (_groundParticles.Length > 0) {
          for (int x = 0; x < _groundParticles.Length; x++) { //ground for
            for (int y = 0; y < _groundParticles[x].wheelDustList.Count; y++) {//particles for

              //get groundHit
              bool itsColliding = wheelColliderList[y].GetGroundHit(out tempWheelHit);
              //get particle
              ParticleSystem.EmissionModule particleTemp = _groundParticles[x].wheelDustList[y].emission;
              //set particle position
              if (wheelColliderList[y].isGrounded) {
                _groundParticles[x].wheelDustList[y].transform.position = tempWheelHit.point;
              }

              if (itsColliding && KMh > _groundParticles[x].criticalVelocity) {
                switch (_groundDetection) {
                  case GroundDetectionMode.Tag:
                    if (!string.IsNullOrEmpty(_groundParticles[x].groundTag)) {
                      if (tempWheelHit.collider.gameObject.CompareTag(_groundParticles[x].groundTag)) {
                        particleTemp.enabled = true;
                      } else {
                        particleTemp.enabled = false;
                      }
                    } else {
                      particleTemp.enabled = false;
                    }
                    break;
                  //==============================================================================================================================
                  case GroundDetectionMode.PhysicMaterial:
                    if (_groundParticles[x].physicMaterial) {
                      if (tempWheelHit.collider.sharedMaterial == _groundParticles[x].physicMaterial) {
                        particleTemp.enabled = true;
                      } else {
                        particleTemp.enabled = false;
                      }
                    } else {
                      particleTemp.enabled = false;
                    }
                    break;
                  //==============================================================================================================================
                  case GroundDetectionMode.TerrainTextureIndices:
                    if (activeTerrain_optional) {
                      if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                        int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                        if (dominantTerrainIndex != -1) {
                          if (_groundParticles[x].terrainTextureIndices.Count > 0 && _groundParticles[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                            particleTemp.enabled = true;
                          } else {
                            particleTemp.enabled = false;
                          }
                        } else {
                          particleTemp.enabled = false;
                        }
                      } else {
                        particleTemp.enabled = false;
                      }
                    } else {
                      particleTemp.enabled = false;
                    }
                    break;
                  //==============================================================================================================================
                  case GroundDetectionMode.All:
                    //tag
                    if (!string.IsNullOrEmpty(_groundParticles[x].groundTag)) {
                      if (tempWheelHit.collider.gameObject.CompareTag(_groundParticles[x].groundTag)) {
                        particleTemp.enabled = true;
                        break;
                      } else {
                        particleTemp.enabled = false;
                      }
                    } else {
                      particleTemp.enabled = false;
                    }
                    //physicMaterial
                    if (_groundParticles[x].physicMaterial) {
                      if (tempWheelHit.collider.sharedMaterial == _groundParticles[x].physicMaterial) {
                        particleTemp.enabled = true;
                        break;
                      } else {
                        particleTemp.enabled = false;
                      }
                    } else {
                      particleTemp.enabled = false;
                    }
                    //terrainTextureIndice
                    if (activeTerrain_optional) {
                      if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                        int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                        if (dominantTerrainIndex != -1) {
                          if (_groundParticles[x].terrainTextureIndices.Count > 0 && _groundParticles[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                            particleTemp.enabled = true;
                            break;
                          } else {
                            particleTemp.enabled = false;
                          }
                        } else {
                          particleTemp.enabled = false;
                        }
                      } else {
                        particleTemp.enabled = false;
                      }
                    } else {
                      particleTemp.enabled = false;
                    }
                    break;
                    //==============================================================================================================================
                }
              } else {
                particleTemp.enabled = false;
              }
            }
          }
        }
      }
    }



    public AudioSource GenerateAudioSource(string name, float minDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool playAwake, GameObject gameObjectParent, float minDist3D, float maxDist3D) {
      GameObject audioSource = new GameObject(name);
      audioSource.transform.position = transform.position;
      audioSource.transform.parent = gameObjectParent.transform;
      AudioSource temp = audioSource.AddComponent<AudioSource>() as AudioSource;
      temp.minDistance = minDistance;
      temp.volume = volume;
      temp.clip = audioClip;
      temp.loop = loop;
      temp.playOnAwake = playAwake;
      temp.spatialBlend = 1.0f;
      temp.dopplerLevel = 0.0f;
      temp.rolloffMode = AudioRolloffMode.Logarithmic;
      temp.minDistance = minDist3D;
      temp.maxDistance = maxDist3D;
      if (playNow) {
        temp.Play();
      }
      return temp;
    }

    void GroundSoundsEmitter(WheelCollider[] wheelColliders) {
      for (int j = 0; j < _groundSounds.groundSounds.Length; j++) {
        wheelEmitterSoundX[j] = 0;
        wheelBlockSoundX[j] = 0;
      }
      for (int i = 0; i < wheelColliders.Length; i++) {
        wheelColliders[i].GetGroundHit(out tempWheelHit);
        for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
          if (_groundSounds.groundSounds[x].groundSound) {
            if (wheelColliders[i].isGrounded) {
              switch (_groundDetection) {
                case GroundDetectionMode.Tag:
                  if (!string.IsNullOrEmpty(_groundSounds.groundSounds[x].groundTag)) {
                    if (tempWheelHit.collider.gameObject.CompareTag(_groundSounds.groundSounds[x].groundTag)) {
                      wheelEmitterSoundX[x]++;
                      if (!groundSoundsAUD[x].isPlaying && groundSoundsAUD[x]) {
                        groundSoundsAUD[x].PlayOneShot(groundSoundsAUD[x].clip);
                      }
                    } else {
                      wheelBlockSoundX[x]++;
                    }
                  } else {
                    wheelBlockSoundX[x]++;
                  }
                  break;
                //==============================================================================================================================
                case GroundDetectionMode.PhysicMaterial:
                  if (_groundSounds.groundSounds[x].physicMaterial) {
                    if (tempWheelHit.collider.sharedMaterial == _groundSounds.groundSounds[x].physicMaterial) {
                      wheelEmitterSoundX[x]++;
                      if (!groundSoundsAUD[x].isPlaying && groundSoundsAUD[x]) {
                        groundSoundsAUD[x].PlayOneShot(groundSoundsAUD[x].clip);
                      }
                    } else {
                      wheelBlockSoundX[x]++;
                    }
                  } else {
                    wheelBlockSoundX[x]++;
                  }
                  break;
                //==============================================================================================================================
                case GroundDetectionMode.TerrainTextureIndices:
                  if (activeTerrain_optional) {
                    if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                      int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                      if (dominantTerrainIndex != -1) {
                        if (_groundSounds.groundSounds[x].terrainTextureIndices.Count > 0 && _groundSounds.groundSounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                          wheelEmitterSoundX[x]++;
                          if (!groundSoundsAUD[x].isPlaying && groundSoundsAUD[x]) {
                            groundSoundsAUD[x].PlayOneShot(groundSoundsAUD[x].clip);
                          }
                        } else {
                          wheelBlockSoundX[x]++;
                        }
                      } else {
                        wheelBlockSoundX[x]++;
                      }
                    } else {
                      wheelBlockSoundX[x]++;
                    }
                  } else {
                    wheelBlockSoundX[x]++;
                  }
                  break;
                //==============================================================================================================================
                case GroundDetectionMode.All:
                  bool emitterSoundXBool = false;
                  //tag
                  if (!string.IsNullOrEmpty(_groundSounds.groundSounds[x].groundTag)) {
                    if (tempWheelHit.collider.gameObject.CompareTag(_groundSounds.groundSounds[x].groundTag)) {
                      emitterSoundXBool = true;
                      if (!groundSoundsAUD[x].isPlaying && groundSoundsAUD[x]) {
                        groundSoundsAUD[x].PlayOneShot(groundSoundsAUD[x].clip);
                      }
                    }
                  }
                  //physicMaterial
                  if (!emitterSoundXBool) {
                    if (_groundSounds.groundSounds[x].physicMaterial) {
                      if (tempWheelHit.collider.sharedMaterial == _groundSounds.groundSounds[x].physicMaterial) {
                        emitterSoundXBool = true;
                        if (!groundSoundsAUD[x].isPlaying && groundSoundsAUD[x]) {
                          groundSoundsAUD[x].PlayOneShot(groundSoundsAUD[x].clip);
                        }
                      }
                    }
                  }
                  //terrainTextureIndice
                  if (!emitterSoundXBool) {
                    if (activeTerrain_optional) {
                      if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                        int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                        if (dominantTerrainIndex != -1) {
                          if (_groundSounds.groundSounds[x].terrainTextureIndices.Count > 0 && _groundSounds.groundSounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                            emitterSoundXBool = true;
                            if (!groundSoundsAUD[x].isPlaying && groundSoundsAUD[x]) {
                              groundSoundsAUD[x].PlayOneShot(groundSoundsAUD[x].clip);
                            }
                          }
                        }
                      }
                    }
                  }
                  //check
                  if (emitterSoundXBool) {
                    wheelEmitterSoundX[x]++;
                  } else {
                    wheelBlockSoundX[x]++;
                  }
                  break;
                  //==============================================================================================================================
              }
            } else {
              wheelBlockSoundX[x]++;
            }
          }
        }
      }
      for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
        if (wheelBlockSoundX[x] > 0 && wheelEmitterSoundX[x] == 0 && groundSoundsAUD[x]) {
          groundSoundsAUD[x].Stop();
        }
        if (ms_Rigidbody.velocity.magnitude < 2.0f && groundSoundsAUD[x]) {
          groundSoundsAUD[x].Stop();
        }
      }
    }

    void CheckSKidForward(float wheelRPM, float sens1, float forwardSKid, float sens3) {
      if (KMh > sens1) {
        if (wheelRPM < (0.3333333 * _skidMarks.sensibility)) {
          forwardTempSKid = true;
        }
      }
      if (KMh < sens3) {
        if (forwardSKid > (0.8333333 * _skidMarks.sensibility)) {
          forwardTempSKid = true;
          forwardHandBrakeSKid = true;
        }
      }
      float RPMCondition = (1.75f / averageWheelRadius);// = 5.0f
      if (Mathf.Abs(wheelRPM) < RPMCondition && KMh > 5) {
        forwardTempSKid = true;
        forwardHandBrakeSKid = true;
      }
    }
    void SkiddingSounds() {
      forwardTempSKid = false;
      forwardHandBrakeSKid = false;
      float sidewaysSlipMaxSkid = 0;
      float forwardSlipMaxSkid = 0;
      float sensibility75kmh = (75.0f / _skidMarks.sensibility);
      float sensibilityLowSpeed = 20.0f * (Mathf.Clamp(_skidMarks.sensibility, 1, 3));
      //
      if (wheelFDIsGrounded) {
        _wheels.rightFrontWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        _wheels.rightFrontWheel.sidewaysSkid = Mathf.Abs(tempWheelHit.sidewaysSlip);
        if (_wheels.rightFrontWheel.sidewaysSkid > sidewaysSlipMaxSkid) {
          sidewaysSlipMaxSkid = _wheels.rightFrontWheel.sidewaysSkid;
        }
        _wheels.rightFrontWheel.forwardSkid = Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility);
        if (_wheels.rightFrontWheel.forwardSkid > forwardSlipMaxSkid) {
          forwardSlipMaxSkid = _wheels.rightFrontWheel.forwardSkid;
        }
        CheckSKidForward(_wheels.rightFrontWheel.wheelColliderRPM, sensibility75kmh, _wheels.rightFrontWheel.forwardSkid, sensibilityLowSpeed);
      }
      //
      if (wheelFEIsGrounded) {
        _wheels.leftFrontWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        _wheels.leftFrontWheel.sidewaysSkid = Mathf.Abs(tempWheelHit.sidewaysSlip);
        if (_wheels.leftFrontWheel.sidewaysSkid > sidewaysSlipMaxSkid) {
          sidewaysSlipMaxSkid = _wheels.leftFrontWheel.sidewaysSkid;
        }
        _wheels.leftFrontWheel.forwardSkid = Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility);
        if (_wheels.leftFrontWheel.forwardSkid > forwardSlipMaxSkid) {
          forwardSlipMaxSkid = _wheels.leftFrontWheel.forwardSkid;
        }
        CheckSKidForward(_wheels.leftFrontWheel.wheelColliderRPM, sensibility75kmh, _wheels.leftFrontWheel.forwardSkid, sensibilityLowSpeed);
      }
      //
      if (wheelTDIsGrounded) {
        _wheels.rightRearWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        _wheels.rightRearWheel.sidewaysSkid = Mathf.Abs(tempWheelHit.sidewaysSlip);
        if (_wheels.rightRearWheel.sidewaysSkid > sidewaysSlipMaxSkid) {
          sidewaysSlipMaxSkid = _wheels.rightRearWheel.sidewaysSkid;
        }
        _wheels.rightRearWheel.forwardSkid = Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility);
        if (_wheels.rightRearWheel.forwardSkid > forwardSlipMaxSkid) {
          forwardSlipMaxSkid = _wheels.rightRearWheel.forwardSkid;
        }
        CheckSKidForward(_wheels.rightRearWheel.wheelColliderRPM, sensibility75kmh, _wheels.rightRearWheel.forwardSkid, sensibilityLowSpeed);
      }
      //
      if (wheelTEIsGrounded) {
        _wheels.leftRearWheel.wheelCollider.GetGroundHit(out tempWheelHit);
        _wheels.leftRearWheel.sidewaysSkid = Mathf.Abs(tempWheelHit.sidewaysSlip);
        if (_wheels.leftRearWheel.sidewaysSkid > sidewaysSlipMaxSkid) {
          sidewaysSlipMaxSkid = _wheels.leftRearWheel.sidewaysSkid;
        }
        _wheels.leftRearWheel.forwardSkid = Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility);
        if (_wheels.leftRearWheel.forwardSkid > forwardSlipMaxSkid) {
          forwardSlipMaxSkid = _wheels.leftRearWheel.forwardSkid;
        }
        CheckSKidForward(_wheels.leftRearWheel.wheelColliderRPM, sensibility75kmh, _wheels.leftRearWheel.forwardSkid, sensibilityLowSpeed);
      }
      //
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        if (_wheels.extraWheels[x].wheelCollider.isGrounded) {
          _wheels.extraWheels[x].wheelCollider.GetGroundHit(out tempWheelHit);
          _wheels.extraWheels[x].sidewaysSkid = Mathf.Abs(tempWheelHit.sidewaysSlip);
          if (_wheels.extraWheels[x].sidewaysSkid > sidewaysSlipMaxSkid) {
            sidewaysSlipMaxSkid = _wheels.extraWheels[x].sidewaysSkid;
          }
          _wheels.extraWheels[x].forwardSkid = Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility);
          if (_wheels.extraWheels[x].forwardSkid > forwardSlipMaxSkid) {
            forwardSlipMaxSkid = _wheels.extraWheels[x].forwardSkid;
          }
          CheckSKidForward(_wheels.extraWheels[x].wheelColliderRPM, sensibility75kmh, _wheels.extraWheels[x].forwardSkid, sensibilityLowSpeed);
        }
      }
      //
      bool skiddingIsTrue = false;
      float maxSlipTemp = 0;
      if (sidewaysSlipMaxSkid > forwardSlipMaxSkid) {
        maxSlipTemp = sidewaysSlipMaxSkid;
      } else {
        maxSlipTemp = forwardSlipMaxSkid;
      }
      //
      if (forwardTempSKid || (maxSlipTemp > (1 / _skidMarks.sensibility))) {
        skiddingIsTrue = true;
      }
      if (forwardHandBrakeSKid) {
        maxSlipTemp = 0.8f;
      }

      //CHECK TAGS
      bool tempOtherGround = GroundSoundsEmitterSkid(wheelColliderList, maxSlipTemp, skiddingIsTrue);
      if (skiddingIsTrue) {
        if (!tempOtherGround) {//quer dizer que esta derrapando mas nao encontrou nenhuma (Tag, PhysicMaterial ou Terrain)
          skiddingSoundAUD.volume = Mathf.Lerp(skiddingSoundAUD.volume, (maxSlipTemp * _groundSounds.standardSkidVolume), Time.deltaTime * 7.0f);
          if (forwardTempSKid) {
            if (skiddingSoundAUD.volume < (0.3f * _groundSounds.standardSkidVolume)) {
              skiddingSoundAUD.volume = Mathf.Lerp(skiddingSoundAUD.volume, (0.3f * _groundSounds.standardSkidVolume), Time.deltaTime * 7.0f);
            }
          }
          if (!skiddingSoundAUD.isPlaying) {
            skiddingSoundAUD.Play();
          }
        } else {
          skiddingSoundAUD.volume = Mathf.Lerp(skiddingSoundAUD.volume, 0, Time.deltaTime * 7.0f);
          if (skiddingSoundAUD.volume < 0.3f) {
            skiddingSoundAUD.Stop();
          }
        }
      } else {
        skiddingSoundAUD.volume = Mathf.Lerp(skiddingSoundAUD.volume, 0, Time.deltaTime * 7.0f);
        if (skiddingSoundAUD.volume < 0.3f) {
          skiddingSoundAUD.Stop();
        }
      }
      //

      //This prevents the vehicle from emitting skidding sounds if no wheel is touching the ground.
      if (groundedWheels == 0) {
        //default skid sound
        skiddingSoundAUD.volume = 0;
        skiddingSoundAUD.Stop();
        //other grounds skid sounds
        for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
          groundSoundsAUDSkid[x].volume = 0;
          groundSoundsAUDSkid[x].Stop();
        }
      }
    }
    bool GroundSoundsEmitterSkid(WheelCollider[] wheelColliders, float slipForVolume, bool skidIsTrue) {
      for (int j = 0; j < _groundSounds.groundSounds.Length; j++) {
        wheelEmitterSoundXSkid[j] = 0;
        wheelBlockSoundXSkid[j] = 0;
      }
      bool otherGround = false;
      int wheelsInOtherGround = 0;
      int maxWheels = 0;
      for (int i = 0; i < wheelColliders.Length; i++) {//WHEELS FOR
        if (wheelColliders[i].isGrounded) {
          maxWheels++;
          wheelColliders[i].GetGroundHit(out tempWheelHit);
          float maxTempSKid = Mathf.Abs(tempWheelHit.sidewaysSlip);
          if (Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility) > maxTempSKid) {
            maxTempSKid = Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility);
          }

          bool detectGroundOnThisFor = false;
          for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {//SOUNDS FOR
            switch (_groundDetection) {
              case GroundDetectionMode.Tag:
                if (!string.IsNullOrEmpty(_groundSounds.groundSounds[x].groundTag)) {
                  if (tempWheelHit.collider.gameObject.CompareTag(_groundSounds.groundSounds[x].groundTag)) {
                    wheelsInOtherGround++;
                    if ((maxTempSKid > (1 / _skidMarks.sensibility)) || skidIsTrue) {
                      groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, (slipForVolume * _groundSounds.groundSounds[x].volumeSkid), Time.deltaTime * 5.0f);
                      if (_groundSounds.groundSounds[x].skiddingSound) {
                        wheelEmitterSoundXSkid[x]++;
                        if (!groundSoundsAUDSkid[x].isPlaying && groundSoundsAUDSkid[x]) {
                          groundSoundsAUDSkid[x].PlayOneShot(groundSoundsAUDSkid[x].clip);
                        }
                      }
                    } else {
                      wheelBlockSoundXSkid[x]++;
                    }
                  } else {
                    wheelBlockSoundXSkid[x]++;
                  }
                } else {
                  wheelBlockSoundXSkid[x]++;
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.PhysicMaterial:
                if (_groundSounds.groundSounds[x].physicMaterial) {
                  if (tempWheelHit.collider.sharedMaterial == _groundSounds.groundSounds[x].physicMaterial) {
                    wheelsInOtherGround++;
                    if ((maxTempSKid > (1 / _skidMarks.sensibility)) || skidIsTrue) {
                      groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, (slipForVolume * _groundSounds.groundSounds[x].volumeSkid), Time.deltaTime * 5.0f);
                      if (_groundSounds.groundSounds[x].skiddingSound) {
                        wheelEmitterSoundXSkid[x]++;
                        if (!groundSoundsAUDSkid[x].isPlaying && groundSoundsAUDSkid[x]) {
                          groundSoundsAUDSkid[x].PlayOneShot(groundSoundsAUDSkid[x].clip);
                        }
                      }
                    } else {
                      wheelBlockSoundXSkid[x]++;
                    }
                  } else {
                    wheelBlockSoundXSkid[x]++;
                  }
                } else {
                  wheelBlockSoundXSkid[x]++;
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.TerrainTextureIndices:
                if (activeTerrain_optional) {
                  if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                    int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                    if (dominantTerrainIndex != -1) {
                      if (_groundSounds.groundSounds[x].terrainTextureIndices.Count > 0 && _groundSounds.groundSounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                        wheelsInOtherGround++;
                        if ((maxTempSKid > (1 / _skidMarks.sensibility)) || skidIsTrue) {
                          groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, (slipForVolume * _groundSounds.groundSounds[x].volumeSkid), Time.deltaTime * 5.0f);
                          if (_groundSounds.groundSounds[x].skiddingSound) {
                            wheelEmitterSoundXSkid[x]++;
                            if (!groundSoundsAUDSkid[x].isPlaying && groundSoundsAUDSkid[x]) {
                              groundSoundsAUDSkid[x].PlayOneShot(groundSoundsAUDSkid[x].clip);
                            }
                          }
                        } else {
                          wheelBlockSoundXSkid[x]++;
                        }
                      } else {
                        wheelBlockSoundXSkid[x]++;
                      }
                    } else {
                      wheelBlockSoundXSkid[x]++;
                    }
                  } else {
                    wheelBlockSoundXSkid[x]++;
                  }
                } else {
                  wheelBlockSoundXSkid[x]++;
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.All:
                bool _emitterSoundXBool = false;
                bool _wheelsInOtherGround = false;
                if (!detectGroundOnThisFor) {
                  //tag
                  if (!string.IsNullOrEmpty(_groundSounds.groundSounds[x].groundTag)) {
                    if (tempWheelHit.collider.gameObject.CompareTag(_groundSounds.groundSounds[x].groundTag)) {
                      detectGroundOnThisFor = true;
                      _wheelsInOtherGround = true;
                      if ((maxTempSKid > (1 / _skidMarks.sensibility)) || skidIsTrue) {
                        groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, (slipForVolume * _groundSounds.groundSounds[x].volumeSkid), Time.deltaTime * 5.0f);
                        if (_groundSounds.groundSounds[x].skiddingSound) {
                          _emitterSoundXBool = true;
                          if (!groundSoundsAUDSkid[x].isPlaying && groundSoundsAUDSkid[x]) {
                            groundSoundsAUDSkid[x].PlayOneShot(groundSoundsAUDSkid[x].clip);
                          }
                        }
                      }
                    }
                  }
                  //physicMaterial
                  if (!_wheelsInOtherGround) {
                    if (_groundSounds.groundSounds[x].physicMaterial) {
                      if (tempWheelHit.collider.sharedMaterial == _groundSounds.groundSounds[x].physicMaterial) {
                        detectGroundOnThisFor = true;
                        _wheelsInOtherGround = true;
                        if ((maxTempSKid > (1 / _skidMarks.sensibility)) || skidIsTrue) {
                          groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, (slipForVolume * _groundSounds.groundSounds[x].volumeSkid), Time.deltaTime * 5.0f);
                          if (_groundSounds.groundSounds[x].skiddingSound) {
                            _emitterSoundXBool = true;
                            if (!groundSoundsAUDSkid[x].isPlaying && groundSoundsAUDSkid[x]) {
                              groundSoundsAUDSkid[x].PlayOneShot(groundSoundsAUDSkid[x].clip);
                            }
                          }
                        }
                      }
                    }
                  }
                  //terrainTextureIndice
                  if (!_wheelsInOtherGround) {
                    if (activeTerrain_optional) {
                      if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                        int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                        if (dominantTerrainIndex != -1) {
                          if (_groundSounds.groundSounds[x].terrainTextureIndices.Count > 0 && _groundSounds.groundSounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                            detectGroundOnThisFor = true;
                            _wheelsInOtherGround = true;
                            if ((maxTempSKid > (1 / _skidMarks.sensibility)) || skidIsTrue) {
                              groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, (slipForVolume * _groundSounds.groundSounds[x].volumeSkid), Time.deltaTime * 5.0f);
                              if (_groundSounds.groundSounds[x].skiddingSound) {
                                _emitterSoundXBool = true;
                                if (!groundSoundsAUDSkid[x].isPlaying && groundSoundsAUDSkid[x]) {
                                  groundSoundsAUDSkid[x].PlayOneShot(groundSoundsAUDSkid[x].clip);
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }

                //check
                if (_wheelsInOtherGround) {
                  wheelsInOtherGround++;
                }

                if (_emitterSoundXBool) {
                  wheelEmitterSoundXSkid[x]++;
                } else {
                  wheelBlockSoundXSkid[x]++;
                }
                //

                break;
                //==============================================================================================================================
            }
          }
        }
      }
      for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
        if (wheelBlockSoundXSkid[x] > 0 && wheelEmitterSoundXSkid[x] == 0 && groundSoundsAUDSkid[x]) {
          groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, 0, Time.deltaTime * 5.0f);
          if (groundSoundsAUDSkid[x].volume < 0.6f) {
            groundSoundsAUDSkid[x].Stop();
          }
        }
      }
      if (wheelsInOtherGround >= maxWheels) {
        otherGround = true;
      }
      return otherGround;
    }

    void VehicleSoundsFunction() {
      //airBrakeSound
      if (_sounds.airBrakeSound) {
        if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
          float brakeInput = 0.0f;
          if (automaticGears) {
            if (currentGear > 0) {
              brakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, -1.0f, 0.0f)) * 1.5f;
            } else {
              brakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, 0.0f, 1.0f)) * 1.5f;
            }
          } else {
            if ((currentGear < 0 && mediumRPM > 0) || (currentGear > 0 && mediumRPM < 0)) {
              brakeInput = Mathf.Abs(verticalInput);
            } else {
              brakeInput = Mathf.Abs(Mathf.Clamp(verticalInput, -1.0f, 0.0f));
            }
          }
          if (brakeInput > 0.8f) {
            boolTimeAirBrake = true;
          }
          if (brakeInput < 0.4f && boolTimeAirBrake) {
            if (!airBrakeSoundAUD.isPlaying) {
              airBrakeSoundAUD.PlayOneShot(airBrakeSoundAUD.clip);
            }
            boolTimeAirBrake = false;
          }
        }
      }

      //groundSounds
      if (_groundSounds.groundSounds.Length > 0) {
        if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI || KMh > 0.1f) {
          GroundSoundsEmitter(wheelColliderList);
        } else {
          for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
            if (groundSoundsAUD[x].isPlaying) {
              if (groundSoundsAUD[x]) {
                groundSoundsAUD[x].Stop();
              }
            }
          }
        }
      }

      //skiddingSound
      if (_groundSounds.standardSkidSound) {
        if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI || KMh > 0.1f) {
          SkiddingSounds();
        } else {
          skiddingSoundAUD.volume = Mathf.Lerp(skiddingSoundAUD.volume, 0, Time.deltaTime * 7.0f);
          if (skiddingSoundAUD.volume < 0.3f) {
            skiddingSoundAUD.Stop();
          }
          //
          for (int x = 0; x < _groundSounds.groundSounds.Length; x++) {
            if (groundSoundsAUDSkid[x]) {
              groundSoundsAUDSkid[x].volume = Mathf.Lerp(groundSoundsAUDSkid[x].volume, 0, Time.deltaTime * 5.0f);
              if (groundSoundsAUDSkid[x].volume < 0.6f) {
                groundSoundsAUDSkid[x].Stop();
              }
            }
          }
        }
      }

      //engineSound
      if (_sounds.engineSound) {
        if (theEngineIsRunning) {
          engineSoundAUD.volume = Mathf.Lerp(engineSoundAUD.volume, Mathf.Clamp(Mathf.Abs(verticalInput), 0.35f, 1.0f) * _sounds.volumeOfTheEngineSound, Time.deltaTime * 5.0f);

          pitchAUDforRPM = Mathf.Clamp(pitchAUDforRPM, 0.85f, _sounds.speedOfEngineSound);
          if (handBrakeTrue || currentGear == 0) {
            engineSoundAUD.pitch = Mathf.Lerp(engineSoundAUD.pitch, pitchAUDforRPM, Time.deltaTime * 5.0f);
          } else {
            //speed of the sound
            float minSpeedGear = 0;
            if (currentGear == -1 || currentGear == 0) {
              minSpeedGear = _vehicleTorque.minVelocityGears[0] * _vehicleTorque.speedOfGear;
            } else {
              minSpeedGear = _vehicleTorque.minVelocityGears[currentGear - 1] * _vehicleTorque.speedOfGear;
            }
            if (KMh < minSpeedGear) {
              speedLerpSound = 1.0f;
            } else {
              if (speedLerpSound < 4.9f) {
                speedLerpSound = Mathf.Lerp(speedLerpSound, 5.0f, Time.deltaTime * 2.0f);
              }
            }
            //set sound pitch
            engineSoundAUD.pitch = Mathf.Lerp(engineSoundAUD.pitch, pitchAUDforRPM, Time.deltaTime * speedLerpSound);
          }
        } else {
          if (enableEngineSound) {
            if (_sounds.engineStartSound) {
              engineSoundAUD.volume = 0;
            } else {
              engineSoundAUD.volume = _sounds.volumeOfTheEngineSound;
            }
            engineSoundAUD.pitch = Mathf.Lerp(engineSoundAUD.pitch, 0.7f, Time.deltaTime);
            pitchAUDforRPM = 0.7f;
          } else {
            engineSoundAUD.volume = Mathf.Lerp(engineSoundAUD.volume, 0f, Time.deltaTime);
            engineSoundAUD.pitch = Mathf.Lerp(engineSoundAUD.pitch, 0f, Time.deltaTime);
          }
        }
      }

      //other sounds
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        //hornSound
        if (_sounds.hornSound) { // hornIsOn
          if (hornIsOn && !hornSoundAUD.isPlaying) {
            hornSoundAUD.PlayOneShot(hornSoundAUD.clip);
            StartCoroutine("TimeHornSound");
          }
        }

        //nitroSound
        if (_sounds.nitroStartSound) {
          if (_additionalFeatures.nitroIsTrueVar && !nitroSoundWasPlayed) {
            nitroSoundWasPlayed = true;
            nitroSoundAUD.PlayOneShot(nitroSoundAUD.clip);
          }
          if (!_additionalFeatures.nitroIsTrueVar && nitroSoundWasPlayed) {
            nitroSoundWasPlayed = false;
          }
        }

        //handBrakeSound
        if (_sounds.handBrakeSound) {
          if (!handBrakeSoundAUD.isPlaying && handBrakeTrue && !handBrakeSoundWasPlayed) {
            handBrakeSoundWasPlayed = true;
            handBrakeSoundAUD.PlayOneShot(handBrakeSoundAUD.clip);
          }
          if (!handBrakeTrue && handBrakeSoundWasPlayed) {
            handBrakeSoundWasPlayed = false;
          }
        }

        //reverseSirenSound
        if (_sounds.reverseSirenSound) {
          if (currentGear == -1) {
            if (!sirenSoundAUD.isPlaying) {
              sirenSoundAUD.PlayOneShot(sirenSoundAUD.clip);
            }
          } else {
            sirenSoundAUD.Stop();
          }
        }
      }

      //windSound
      if (_sounds.windSound) {
        windSoundAUD.volume = Mathf.Clamp(ms_Rigidbody.velocity.magnitude * _sounds.sensibilityWindSound - 0.3f, 0.0f, 3.0f);
        if (ms_Rigidbody.velocity.magnitude > 15.0f) {
          if (!windSoundAUD.isPlaying && !windLoop) {
            windLoop = true;
            windSoundAUD.Play();
          }
        } else {
          windLoop = false;
          windSoundAUD.Stop();
        }
      }

      //wheelImpactSound
      if (_sounds.wheelImpactSound) {
        Vector3 posWheel;
        Quaternion rotWheel;
        float localPositionYAxis;

        // rightFrontWheel
        _wheels.rightFrontWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
        localPositionYAxis = transform.InverseTransformPoint(posWheel).y;
        if (Mathf.Abs(lastRightFrontPositionY - localPositionYAxis) > sensImpactFR) {
          beatsOnWheelSoundAUD.PlayOneShot(beatsOnWheelSoundAUD.clip);
        }
        lastRightFrontPositionY = localPositionYAxis;

        // leftFrontWheel
        _wheels.leftFrontWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
        localPositionYAxis = transform.InverseTransformPoint(posWheel).y;
        if (Mathf.Abs(lastLeftFrontPositionY - localPositionYAxis) > sensImpactFL) {
          beatsOnWheelSoundAUD.PlayOneShot(beatsOnWheelSoundAUD.clip);
        }
        lastLeftFrontPositionY = localPositionYAxis;

        // rightRearWheel
        _wheels.rightRearWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
        localPositionYAxis = transform.InverseTransformPoint(posWheel).y;
        if (Mathf.Abs(lastRightRearPositionY - localPositionYAxis) > sensImpactRR) {
          beatsOnWheelSoundAUD.PlayOneShot(beatsOnWheelSoundAUD.clip);
        }
        lastRightRearPositionY = localPositionYAxis;

        // leftRearWheel
        _wheels.leftRearWheel.wheelCollider.GetWorldPose(out posWheel, out rotWheel);
        localPositionYAxis = transform.InverseTransformPoint(posWheel).y;
        if (Mathf.Abs(lastLeftRearPositionY - localPositionYAxis) > sensImpactRL) {
          beatsOnWheelSoundAUD.PlayOneShot(beatsOnWheelSoundAUD.clip);
        }
        lastLeftRearPositionY = localPositionYAxis;

        // extra wheels
        for (int x = 0; x < _wheels.extraWheels.Length; x++) {
          _wheels.extraWheels[x].wheelCollider.GetWorldPose(out posWheel, out rotWheel);
          localPositionYAxis = transform.InverseTransformPoint(posWheel).y;
          if (Mathf.Abs(lastPositionYExtraWheels[x] - localPositionYAxis) > sensImpactExtraWheels[x]) {
            beatsOnWheelSoundAUD.PlayOneShot(beatsOnWheelSoundAUD.clip);
          }
          lastPositionYExtraWheels[x] = localPositionYAxis;
        }
      }

      //blinkingSound
      if (_sounds.blinkingSound) {
        if (rightBlinkersOn || leftBlinkersOn) {
          if (!flashingSoundAUD.isPlaying && !loopBlinkersOn) {
            loopBlinkersOn = true;
            flashingSoundAUD.Play();
          }
        } else {
          flashingSoundAUD.Stop();
          loopBlinkersOn = false;
        }
      }
    }
    IEnumerator TimeHornSound() {
      yield return new WaitForSeconds(hornSoundAUD.clip.length);
      hornIsOn = false;
    }



    public IEnumerator StartEngineCoroutine(bool startEngine) {
      if (startEngine) {
        youCanCall = false;
        yield return new WaitForSeconds(_vehicleSettings.delayToStartTheEngine);
        //
        youCanCall = true;
        _vehicleSettings.delayToStartTheEngine = previousDelayStartEngine;
        if (engineSoundAUD) {
          engineSoundAUD.volume = _sounds.volumeOfTheEngineSound;
        }
        if (currentFuelLiters > 0) {
          theEngineIsRunning = true;
        }
      } else {
        enableEngineSound = false;
        theEngineIsRunning = false;
        //
        youCanCall = false;
        yield return new WaitForSeconds(_vehicleSettings.delayToStartTheEngine * 0.8f);
        youCanCall = true;
        _vehicleSettings.delayToStartTheEngine = previousDelayStartEngine;
      }
    }



    public void ResetVehicleDamage() {
      for (int i = 0; i < _damage.deformMesh.meshes.Length; i++) {
        if (_damage.deformMesh.meshes[i]) {
          Mesh mesh = _damage.deformMesh.meshes[i].mesh;
          mesh.vertices = ms_originalMeshes[i];
          mesh.RecalculateNormals();
          mesh.RecalculateBounds();
        }
      }
      vehicleLife = _damage.damageSupported;
    }
    void OnCollisionEnter(Collision collision) {
      if (collision.contacts.Length > 0) {
        if (collision.relativeVelocity.magnitude > 1) {
          if (collision.contacts[0].thisCollider.gameObject.transform != transform.parent) {
            //vehicle life
            if (enableDamageOnStart) {
              vehicleLife -= collision.relativeVelocity.magnitude;
              if (vehicleLife < 0.01f) {
                vehicleLife = 0.01f;
              }
            }
            //impact sound
            if (_sounds.collisionSounds.Length > 0) {
              beatsSoundAUD.clip = _sounds.collisionSounds[UnityEngine.Random.Range(0, _sounds.collisionSounds.Length)];
              beatsSoundAUD.PlayOneShot(beatsSoundAUD.clip);
            }
            //deform_damage
            if (_damage.deformMesh.meshes.Length > 0 && enableDamageOnStart) {
              int impactCount = 0;
              Vector3 impactPosition = Vector3.zero;
              Vector3 impactVelocity = Vector3.zero;
              foreach (ContactPoint contact in collision.contacts) {
                float draagRatio = Vector3.Dot(ms_Rigidbody.GetPointVelocity(contact.point), contact.normal);
                if (draagRatio < -0.6f || collision.relativeVelocity.sqrMagnitude > 3.0f) {
                  impactCount++;
                  impactPosition += contact.point;
                  impactVelocity += collision.relativeVelocity;
                }
              }
              if (impactCount > 0) {
                float invCount = 1.0f / impactCount;
                impactPosition *= invCount;
                impactVelocity *= invCount;
                ms_sumImpactCount++;
                ms_sumImpactPosition += transform.InverseTransformPoint(impactPosition);
                ms_sumImpactVelocity += transform.InverseTransformDirection(impactVelocity);
              }
            }
          }
        }
      }
    }



    public IEnumerator ChangeGears(int gear) {
      changinGears = true;
      if (gear == 1) {
        lastKnownTorque = lastKnownTorque * 1.2f;
      } else {
        lastKnownTorque = lastKnownTorque * 0.8f;
      }
      yield return new WaitForSeconds(_vehicleTorque.gearShiftTime);
      changinGears = false;
      currentGear = gear;
    }
    void AutomaticGears() {
      if (currentGear == 0) {
        if (mediumRPM >= 0 && mediumRPM < 5) {
          currentGear = 1;
        }
        if (mediumRPM > -5 && mediumRPM < 0) {
          currentGear = -1;
        }
      }
      if (Mathf.Abs(verticalInput) < 0.1f) {
        if (mediumRPM < -0.3f) {
          currentGear = -1;
        }
        if (mediumRPM >= 0 && currentGear < 2) {
          currentGear = 1;
        }
      }
      if (_vehicleState == ControlState.isPlayer) {
        if ((Mathf.Abs(Mathf.Clamp(verticalInput, -1f, 0f))) > 0.8f) {
          if ((KMh < 5 && mediumRPM < 1) || mediumRPM < -2) {
            currentGear = -1;
          }
        }
        if ((Mathf.Abs(Mathf.Clamp(verticalInput, 0f, 1f))) > 0.8f) {
          if ((KMh < 5) || (mediumRPM > 2 && currentGear < 2)) {
            currentGear = 1;
          }
        }
      } else {
        if ((Mathf.Abs(Mathf.Clamp(verticalInput, -1f, 0f))) > 0.05f) {
          if ((KMh < 5) || mediumRPM < -2) {
            currentGear = -1;
          }
        }
        if ((Mathf.Abs(Mathf.Clamp(verticalInput, 0f, 1f))) > 0.05f) {
          if ((KMh < 5) || (mediumRPM > 2 && currentGear < 2)) {
            currentGear = 1;
          }
        }
      }


      // 
      if (currentGear >= 1) {
        if (KMh > (_vehicleTorque.idealVelocityGears[currentGear - 1] * _vehicleTorque.speedOfGear + 17.5f * _vehicleTorque.speedOfGear)) {
          if (currentGear < _vehicleTorque.numberOfGears && !changinGearsAuto) {                     //here, have !changinGearsAuto condition
            StartCoroutine(TimeAutoGears(currentGear + 1, _vehicleTorque.gearShiftTime, 1.5f));
          }
        } else if (KMh < (_vehicleTorque.idealVelocityGears[currentGear - 1] * _vehicleTorque.speedOfGear - 20 * _vehicleTorque.speedOfGear)) {
          if (currentGear > 1 && !changinGearsAuto) {
            StartCoroutine(TimeAutoGears(currentGear - 1, (_vehicleTorque.gearShiftTime * 0.5f), 0.0f));
          }
        }
        //
        if (verticalInput > 0.1f && KMh > (_vehicleTorque.idealVelocityGears[currentGear - 1] * _vehicleTorque.speedOfGear + 17.5f * _vehicleTorque.speedOfGear)) {
          if (currentGear < _vehicleTorque.numberOfGears) {                                          //here, not have !changinGearsAuto condition
            StartCoroutine(TimeAutoGears(currentGear + 1, _vehicleTorque.gearShiftTime, 0.0f));
          }
        }
      }
    }
    IEnumerator TimeAutoGears(int gear, float changeGearTime, float waitingTime) {
      changinGearsAuto = true;
      if (gear == 0) {
        lastKnownTorque = lastKnownTorque * 1.2f;
      } else {
        lastKnownTorque = lastKnownTorque * 0.8f;
      }
      yield return new WaitForSeconds(changeGearTime);
      currentGear = gear;
      yield return new WaitForSeconds(waitingTime);
      changinGearsAuto = false;
    }



    float SteerAngleOptimized(float clampHorizontalInput) {
      float v3Dot_speed = Vector3.Dot(ms_Rigidbody.velocity, transform.forward);
      float dot_factor = Mathf.InverseLerp(0.1f, 3.0f, v3Dot_speed);
      float speedAngle = Mathf.Sign(Vector3.Dot(ms_Rigidbody.velocity, transform.right)) * Vector3.Angle(ms_Rigidbody.velocity, transform.forward);
      float assistAngle = 0.0f;
      if (_steeringWheel.steeringAssist > 0.1f) {
        assistAngle = speedAngle * dot_factor * Mathf.InverseLerp(2.0f, 3.0f, Mathf.Abs(speedAngle)) * _steeringWheel.steeringAssist;
      }
      float inputSteering = _steeringWheel.maxAngle * clampHorizontalInput;
      if (_steeringWheel.steeringLimit > 0.1f) {
        float forwardSpeed = v3Dot_speed * dot_factor * _steeringWheel.steeringLimit;
        float maxEspAngle = Mathf.Rad2Deg * Mathf.Asin(Mathf.Clamp((3.0f / forwardSpeed), 0, 1));
        float steerAngleLimit = Mathf.Min(_steeringWheel.maxAngle, Mathf.Max(maxEspAngle, Mathf.Abs(speedAngle)));
        inputSteering = Mathf.Clamp(inputSteering, -steerAngleLimit, steerAngleLimit);
      }
      float finalAngle = Mathf.Clamp(inputSteering + assistAngle, -_steeringWheel.maxAngle, _steeringWheel.maxAngle);
      return finalAngle;
    }
    void VehicleSteeringWheel() {
      if (_vehicleState == ControlState.isPlayer) {
        //get steering wheel inputs
        if (_steeringWheel.keepRotated) {
          volantDir_horizontalInput += horizontalInput * fixedDeltaTime * _steeringWheel.steeringWheelSpeed;
          volantDir_horizontalInput = Mathf.Clamp(volantDir_horizontalInput, -1.0f, 1.0f);
          if (Mathf.Abs(volantDir_horizontalInput) < 0.15f) {
            volantDir_horizontalInput = Mathf.Lerp(volantDir_horizontalInput, 0, fixedDeltaTime * 0.5f);
          }
        } else {
          volantDir_horizontalInput = horizontalInput;
          if (_damage.affectVolant) {
            if (vehicleLife / _damage.damageSupported < 0.7f) {
              volantDir_horizontalInput = (volantDistortion / (vehicleLife / _damage.damageSupported)) + horizontalInput;
            }
          }
        }

        angleSteeringClamp = Mathf.MoveTowards(angleSteeringClamp, volantDir_horizontalInput, _steeringWheel.steeringWheelSpeed * fixedDeltaTime);
        float newFinalAngleDegress = SteerAngleOptimized(angleSteeringClamp);
        if (_steeringWheel.useLerp) {
          finalAngleDegress = Mathf.Lerp(finalAngleDegress, newFinalAngleDegress, fixedDeltaTime * 5.0f); //(0.02f * 5) = 0.1f per frame
        } else {
          finalAngleDegress = newFinalAngleDegress;
        }
      } else if (_vehicleState == ControlState.isAI) {
        //get steering wheel inputs
        volantDir_horizontalInput = horizontalInput;
        angleSteeringClamp = Mathf.MoveTowards(angleSteeringClamp, volantDir_horizontalInput, _steeringWheel.steeringWheelSpeed * fixedDeltaTime);
        finalAngleDegress = SteerAngleOptimized(angleSteeringClamp);
      }


      //APPLY ANGLE IN WHEELS--------------------------------------------------------------------------------------------------------------
      if (_wheels.rightFrontWheel.wheelTurn) {
        float angleMultiplFactor = _wheels.rightFrontWheel.angleFactor;
        if (_wheels.rightFrontWheel.reverseTurn) {
          angleMultiplFactor = angleMultiplFactor * -1;
        }
        _wheels.rightFrontWheel.wheelCollider.steerAngle = finalAngleDegress * angleMultiplFactor;
      }
      if (_wheels.leftFrontWheel.wheelTurn) {
        float angleMultiplFactor = _wheels.leftFrontWheel.angleFactor;
        if (_wheels.leftFrontWheel.reverseTurn) {
          angleMultiplFactor = angleMultiplFactor * -1;
        }
        _wheels.leftFrontWheel.wheelCollider.steerAngle = finalAngleDegress * angleMultiplFactor;
      }
      if (_wheels.rightRearWheel.wheelTurn) {
        float angleMultiplFactor = _wheels.rightRearWheel.angleFactor;
        if (_wheels.rightRearWheel.reverseTurn) {
          angleMultiplFactor = angleMultiplFactor * -1;
        }
        _wheels.rightRearWheel.wheelCollider.steerAngle = finalAngleDegress * angleMultiplFactor;
      }
      if (_wheels.leftRearWheel.wheelTurn) {
        float angleMultiplFactor = _wheels.leftRearWheel.angleFactor;
        if (_wheels.leftRearWheel.reverseTurn) {
          angleMultiplFactor = angleMultiplFactor * -1;
        }
        _wheels.leftRearWheel.wheelCollider.steerAngle = finalAngleDegress * angleMultiplFactor;
      }
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        if (_wheels.extraWheels[x].wheelTurn) {
          float angleMultiplFactor = _wheels.extraWheels[x].angleFactor;
          if (_wheels.extraWheels[x].reverseTurn) {
            angleMultiplFactor = angleMultiplFactor * -1;
          }
          _wheels.extraWheels[x].wheelCollider.steerAngle = finalAngleDegress * angleMultiplFactor;
        }
      }
      //

      if (_steeringWheel.steeringWheelObject) {
        float returnAngleToAssistInput = finalAngleDegress / _steeringWheel.maxAngle;
        angleVolantIntern = Mathf.MoveTowards(angleVolantIntern, returnAngleToAssistInput, _steeringWheel.steeringWheelSpeed * fixedDeltaTime);
        int reverseRotationVolant = 1;
        if (_steeringWheel.invertRotation) {
          reverseRotationVolant = -1;
        }
        //
        switch (_steeringWheel.rotationType) {
          case VolantSettingsClass.SelectRotation.RotationInY:
            _steeringWheel.steeringWheelObject.transform.localEulerAngles = new Vector3(_steeringWheel.steeringWheelObject.transform.localEulerAngles.x, volantStartRotation + (angleVolantIntern * reverseRotationVolant * (360.0f * _steeringWheel.numberOfTurns)), _steeringWheel.steeringWheelObject.transform.localEulerAngles.z);
            break;
          case VolantSettingsClass.SelectRotation.RotationInZ:
            _steeringWheel.steeringWheelObject.transform.localEulerAngles = new Vector3(_steeringWheel.steeringWheelObject.transform.localEulerAngles.x, _steeringWheel.steeringWheelObject.transform.localEulerAngles.y, volantStartRotation + (angleVolantIntern * reverseRotationVolant * (360.0f * _steeringWheel.numberOfTurns)));
            break;
        }
      }
      if (disableBlinkers1 && angleSteeringClamp > 0.6f && rightBlinkersOn) {
        disableBlinkers2 = true;
      }
      if (disableBlinkers1 && angleSteeringClamp < -0.6f && leftBlinkersOn) {
        disableBlinkers2 = true;
      }
      if ((angleSteeringClamp > -0.1f && angleSteeringClamp < 0.1f) && (rightBlinkersOn || leftBlinkersOn) && disableBlinkers2) {
        rightBlinkersOn = leftBlinkersOn = false;
        disableBlinkers2 = false;
        disableBlinkers1 = false;
      }
    }



    public float VehicleTorque(WheelCollider wheelCollider, float torqueInfluence) {
      float torqueToLerp = 0;
      float rpmTempTorque = Mathf.Abs(wheelCollider.rpm);
      if ((Mathf.Abs(verticalInput) < 0.05f) || KMh > _vehicleTorque.maxVelocityKMh) {
        return 0;
      }
      if ((rpmTempTorque * wheelCollider.radius) > 99999) {
        return 0;
      }
      if (KMh < 0.5f) {
        if (rpmTempTorque > (25.0f / wheelCollider.radius)) {
          return 0;
        }
      }
      if (!theEngineIsRunning || handBrakeTrue || isBraking) {
        return 0;
      }
      if (changinGears) {
        return 0;
      }

      //

      float vehicleTorque_x_Mass = (_vehicleSettings.vehicleMass * _vehicleTorque.engineTorque);
      float tempPoint_Evaluate = KMh / _vehicleTorque.speedOfGear;

      if (currentGear < 0) {
        if (automaticGears) {
          float clampInputTorque = Mathf.Clamp(verticalInput, -1, 0);
          torqueToLerp = 0.8f * vehicleTorque_x_Mass * clampInputTorque * (_vehicleTorque.gearsArray[0].Evaluate(tempPoint_Evaluate)); // reverse gear automatic
        } else {
          float clampInputTorque = Mathf.Clamp(verticalInput, 0, 1);
          torqueToLerp = 0.8f * vehicleTorque_x_Mass * -clampInputTorque * (_vehicleTorque.gearsArray[0].Evaluate(tempPoint_Evaluate)); // reverse gear manual
        }
      } else if (currentGear == 0) {
        return 0; // neutral gear
      } else {
        torqueToLerp = vehicleTorque_x_Mass * (Mathf.Clamp(verticalInput, 0, 1)) * _vehicleTorque.gearsArray[currentGear - 1].Evaluate(tempPoint_Evaluate); // other gears
      }



      //torque lerp
      if ((lastKnownTorque < 0 && torqueToLerp > 0) || (lastKnownTorque > 0 && torqueToLerp < 0)) {
        lastKnownTorque = 0.0f; //evita delay do torque ao trocar entre ré e primeira.
      }
      lastKnownTorque = Mathf.Lerp(lastKnownTorque, torqueToLerp, fixedDeltaTime * _vehicleTorque.speedEngineTorque);

      //ground torque factor
      float groundTorqueFactor = 1.0f;
      wheelCollider.GetGroundHit(out tempWheelHit);
      if (wheelCollider.isGrounded) {
        bool changeTorqueFactor = false;
        for (int x = 0; x < _groundFriction.grounds.Length; x++) {
          if (!changeTorqueFactor) {
            switch (_groundDetection) {
              case GroundDetectionMode.Tag:
                if (!string.IsNullOrEmpty(_groundFriction.grounds[x].groundTag)) {
                  if (tempWheelHit.collider.gameObject.CompareTag(_groundFriction.grounds[x].groundTag)) {
                    groundTorqueFactor = _groundFriction.grounds[x].torqueInThisGround;
                    changeTorqueFactor = true;
                    break;
                  }
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.PhysicMaterial:
                if (_groundFriction.grounds[x].physicMaterial) {
                  if (tempWheelHit.collider.sharedMaterial == _groundFriction.grounds[x].physicMaterial) {
                    groundTorqueFactor = _groundFriction.grounds[x].torqueInThisGround;
                    changeTorqueFactor = true;
                    break;
                  }
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.TerrainTextureIndices:
                if (activeTerrain_optional) {
                  if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                    int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                    if (dominantTerrainIndex != -1) {
                      if (_groundFriction.grounds[x].terrainTextureIndices.Count > 0 && _groundFriction.grounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                        groundTorqueFactor = _groundFriction.grounds[x].torqueInThisGround;
                        changeTorqueFactor = true;
                        break;
                      }
                    }
                  }
                }
                break;
              //==============================================================================================================================
              case GroundDetectionMode.All:
                //tag
                if (!string.IsNullOrEmpty(_groundFriction.grounds[x].groundTag)) {
                  if (tempWheelHit.collider.gameObject.CompareTag(_groundFriction.grounds[x].groundTag)) {
                    groundTorqueFactor = _groundFriction.grounds[x].torqueInThisGround;
                    changeTorqueFactor = true;
                    break;
                  }
                }
                //physicMaterial
                if (_groundFriction.grounds[x].physicMaterial) {
                  if (tempWheelHit.collider.sharedMaterial == _groundFriction.grounds[x].physicMaterial) {
                    groundTorqueFactor = _groundFriction.grounds[x].torqueInThisGround;
                    changeTorqueFactor = true;
                    break;
                  }
                }
                //terrainTextureIndice
                if (activeTerrain_optional) {
                  if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                    int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                    if (dominantTerrainIndex != -1) {
                      if (_groundFriction.grounds[x].terrainTextureIndices.Count > 0 && _groundFriction.grounds[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                        groundTorqueFactor = _groundFriction.grounds[x].torqueInThisGround;
                        changeTorqueFactor = true;
                        break;
                      }
                    }
                  }
                }
                break;
                //==============================================================================================================================
            }
          }
        }
      }

      //damage adjustment
      float damageFactorTorque = 1;
      if (_damage.affectTorque) {
        damageFactorTorque = (vehicleLife / _damage.damageSupported);
      }

      // compute torque
      float nitroFactor = 1;
      if (_additionalFeatures.useNitro && _additionalFeatures.nitroIsTrueVar) {
        nitroFactor = _additionalFeatures.additionalNitroTorque;
      }
      float adjustTorqueByGear = Mathf.Clamp((1 + _vehicleTorque.decreaseTorqueByGear) - (Mathf.Abs(currentGear) * _vehicleTorque.decreaseTorqueByGear), 0.3f, 1.0f);
      float computedTorque = lastKnownTorque * groundTorqueFactor * vehicleScale * adjustTorqueByGear * damageFactorTorque * torqueInfluence * nitroFactor;
      if (_vehicleState == ControlState.isPlayer) {
        float finalTorque = computedTorque + (computedTorque * rpmTorqueFactor);
        return finalTorque;
      } else {
        return computedTorque;
      }
    }

    void ApplyTorque() {
      //motor torque
      if ((theEngineIsRunning && _vehicleState == ControlState.isPlayer) || (theEngineIsRunning && _vehicleState == ControlState.isAI)) {
        float leftAngularDifferential = 1 + Mathf.Abs((0.2f * Mathf.Abs(Mathf.Clamp(volantDir_horizontalInput, 0, 1))) * (finalAngleDegress / 60)) * _wheels.differentialInfluence;
        float rightAngularDifferential = 1 + Mathf.Abs((0.2f * Mathf.Abs(Mathf.Clamp(volantDir_horizontalInput, -1, 0))) * (finalAngleDegress / 60)) * _wheels.differentialInfluence;
        //
        if (_wheels.rightFrontWheel.wheelDrive) {
          _wheels.rightFrontWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.rightFrontWheel.wheelCollider, _wheels.rightFrontWheel.torqueFactor) * rightAngularDifferential;
        }
        if (_wheels.leftFrontWheel.wheelDrive) {
          _wheels.leftFrontWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.leftFrontWheel.wheelCollider, _wheels.leftFrontWheel.torqueFactor) * leftAngularDifferential;
        }
        if (_wheels.rightRearWheel.wheelDrive) {
          _wheels.rightRearWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.rightRearWheel.wheelCollider, _wheels.rightRearWheel.torqueFactor) * rightAngularDifferential;
        }
        if (_wheels.leftRearWheel.wheelDrive) {
          _wheels.leftRearWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.leftRearWheel.wheelCollider, _wheels.leftRearWheel.torqueFactor) * leftAngularDifferential;
        }
        for (int x = 0; x < _wheels.extraWheels.Length; x++) {
          if (_wheels.extraWheels[x].wheelDrive) {
            switch (_wheels.extraWheels[x].wheelPosition) {
              case WheelClass.WheelPosition.Right:
                _wheels.extraWheels[x].wheelCollider.motorTorque = VehicleTorque(_wheels.extraWheels[x].wheelCollider, _wheels.extraWheels[x].torqueFactor) * rightAngularDifferential;
                break;
              case WheelClass.WheelPosition.Left:
                _wheels.extraWheels[x].wheelCollider.motorTorque = VehicleTorque(_wheels.extraWheels[x].wheelCollider, _wheels.extraWheels[x].torqueFactor) * leftAngularDifferential;
                break;
            }
          }
        }
      } else {
        if (_wheels.rightFrontWheel.wheelDrive) {
          _wheels.rightFrontWheel.wheelCollider.motorTorque = 0;
        }
        if (_wheels.leftFrontWheel.wheelDrive) {
          _wheels.leftFrontWheel.wheelCollider.motorTorque = 0;
        }
        if (_wheels.rightRearWheel.wheelDrive) {
          _wheels.rightRearWheel.wheelCollider.motorTorque = 0;
        }
        if (_wheels.leftRearWheel.wheelDrive) {
          _wheels.leftRearWheel.wheelCollider.motorTorque = 0;
        }
        for (int x = 0; x < _wheels.extraWheels.Length; x++) {
          if (_wheels.extraWheels[x].wheelDrive) {
            _wheels.extraWheels[x].wheelCollider.motorTorque = 0;
          }
        }
      }
    }



    void Brakes() {
      float brakeVerticalInput = 0.0f;
      float currentBrakeValue = 0.0f;
      float handBrake_Input = 0.0f;

      //handBrake
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        if (handBrakeTrue) {
          handBrake_Input = 1;
        }
      }
      if (automaticGears) {
        if (handBrakeTrue) {
          handBrake_Input = 1;
        }
      }
      if (_brakes.handBrakeLock) {
        handBrake_Input *= 1000;
      }


      // pedal brake
      if (_vehicleState == ControlState.isPlayer) {
        brakeVerticalInput = verticalInput;
        if (Mathf.Abs(brakeVerticalInput) < 0.05f) {
          brakeVerticalInput = 0;
        }
        if (automaticGears) {
          if (currentGear > 0) {
            currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, -1.0f, 0.0f));
          } else if (currentGear < 0) {
            currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, 0.0f, 1.0f));
          } else if (currentGear == 0) {
            if (mediumRPM > 0) {
              currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, -1.0f, 0.0f));
            } else {
              currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, 0.0f, 1.0f));
            }
          }
        } else {
          currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, -1.0f, 0.0f));
        }
      } else if (_vehicleState == ControlState.isAI) { //only automatic gears
        brakeVerticalInput = verticalInput;
        if (currentGear > 0) {
          currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, -1.0f, 0.0f));
        } else if (currentGear < 0) {
          currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, 0.0f, 1.0f));
        } else if (currentGear == 0) {
          if (mediumRPM > 0) {
            currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, -1.0f, 0.0f));
          } else {
            currentBrakeValue = Mathf.Abs(Mathf.Clamp(brakeVerticalInput, 0.0f, 1.0f));
          }
        }
      }


      //compute total brake
      float totalFootBrake = currentBrakeValue * _brakes.vehicleBrakingForce * _vehicleSettings.vehicleMass;
      float totalHandBrake = handBrake_Input * _brakes.vehicleBrakingForce * _vehicleSettings.vehicleMass * 2.0f;

      //auto brake
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        float autoBrakeInputValue = 0.5f;
        if (_vehicleState == ControlState.isAI) {
          autoBrakeInputValue = 0.05f;
        }
        if (_brakes.brakingWithLowRpm && wheelFDIsGrounded && wheelFEIsGrounded && wheelTDIsGrounded && wheelTEIsGrounded) {
          if (Mathf.Abs(mediumRPM) < 15.0f && Mathf.Abs(brakeVerticalInput) < autoBrakeInputValue && !handBrakeTrue && (totalFootBrake + totalHandBrake) < 100.0f) {
            brakingAuto = true;
            totalFootBrake = 5.0f * _brakes.vehicleBrakingForce * _vehicleSettings.vehicleMass;
          } else {
            brakingAuto = false;
          }
        } else {
          brakingAuto = false;
        }
      } else {
        brakingAuto = false;
      }

      isBraking = (totalFootBrake > 1) ? true : false;

      //cancel brakeForces for ABS
      if (_brakes.ABS && !brakingAuto) {
        if (isBraking && Mathf.Abs(KMh) > 1.2f) {
          totalFootBrake = 0;
        }
      }

      //apply computed brake forces
      ApplyBrakeInWheels(_wheels.rightFrontWheel.wheelCollider, _wheels.rightFrontWheel.wheelHandBrake, totalHandBrake, totalFootBrake);
      ApplyBrakeInWheels(_wheels.leftFrontWheel.wheelCollider, _wheels.leftFrontWheel.wheelHandBrake, totalHandBrake, totalFootBrake);
      ApplyBrakeInWheels(_wheels.rightRearWheel.wheelCollider, _wheels.rightRearWheel.wheelHandBrake, totalHandBrake, totalFootBrake);
      ApplyBrakeInWheels(_wheels.leftRearWheel.wheelCollider, _wheels.leftRearWheel.wheelHandBrake, totalHandBrake, totalFootBrake);
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        ApplyBrakeInWheels(_wheels.extraWheels[x].wheelCollider, _wheels.extraWheels[x].wheelHandBrake, totalHandBrake, totalFootBrake);
      }
    }

    void ApplyBrakeInWheels(WheelCollider wheelCollider, bool handBrake, float sumHandBrake, float sumFootBrake) {
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        if (_brakes.brakeSlowly) {
          if (sumFootBrake == 0) {
            if (handBrake) {
              wheelCollider.brakeTorque = sumHandBrake;
            } else {
              wheelCollider.brakeTorque = 0.0f;
            }
          } else {
            if (handBrake) {
              wheelCollider.brakeTorque = Mathf.Lerp(wheelCollider.brakeTorque, sumFootBrake + sumHandBrake, fixedDeltaTime * _brakes.speedBrakeSlowly);
            } else {
              wheelCollider.brakeTorque = Mathf.Lerp(wheelCollider.brakeTorque, sumFootBrake, fixedDeltaTime * _brakes.speedBrakeSlowly);
            }
          }
        } else {
          if (handBrake) {
            wheelCollider.brakeTorque = sumFootBrake + sumHandBrake;
          } else {
            wheelCollider.brakeTorque = sumFootBrake;
          }
        }
      } else {
        if ((wheelCollider.brakeTorque < sumHandBrake) && handBrake) {
          if (_brakes.brakeSlowly) {
            wheelCollider.brakeTorque = Mathf.Lerp(wheelCollider.brakeTorque, sumHandBrake, fixedDeltaTime * _brakes.speedBrakeSlowly);
          } else {
            wheelCollider.brakeTorque = sumHandBrake;
          }
        }
      }
      // 

      //avoid RPM, brake or invalid torques, Avoid Torque-free Rotation
      if (wheelCollider.brakeTorque < 900000000) {
        if (!wheelCollider.isGrounded) {
          if (Mathf.Abs(wheelCollider.rpm) > 0.5f && Mathf.Abs(verticalInput) < 0.05f && wheelCollider.motorTorque < 5.0f) {
            wheelCollider.brakeTorque += _vehicleSettings.vehicleMass * _brakes.vehicleBrakingForce * fixedDeltaTime * 100;
          }
        }
        if (wheelCollider.isGrounded) {
          if (_vehicleState == ControlState.isNull && _brakes.brakeOnExitingTheVehicle && KMh < 0.5f) {
            wheelCollider.brakeTorque += _vehicleSettings.vehicleMass * _brakes.vehicleBrakingForce * fixedDeltaTime * 100;
          }
        }
        if (KMh < 0.5f && Mathf.Abs(verticalInput) < 0.05f) {
          if (wheelCollider.rpm > (25 / wheelCollider.radius)) {
            wheelCollider.brakeTorque += _brakes.vehicleBrakingForce * _vehicleSettings.vehicleMass * Mathf.Abs(wheelCollider.rpm) * fixedDeltaTime;
          }
        }
      }
    }



    public void SetWheelCollidersOnStart() {
      //forces
      _wheels.rightFrontWheel.wheelCollider.wheelDampingRate = 0.75f;
      _wheels.rightFrontWheel.wheelCollider.forceAppPointDistance = _suspension.forceAppPointDistance;
      _wheels.leftFrontWheel.wheelCollider.wheelDampingRate = 0.75f;
      _wheels.leftFrontWheel.wheelCollider.forceAppPointDistance = _suspension.forceAppPointDistance;
      _wheels.rightRearWheel.wheelCollider.wheelDampingRate = 0.75f;
      _wheels.rightRearWheel.wheelCollider.forceAppPointDistance = _suspension.forceAppPointDistance;
      _wheels.leftRearWheel.wheelCollider.wheelDampingRate = 0.75f;
      _wheels.leftRearWheel.wheelCollider.forceAppPointDistance = _suspension.forceAppPointDistance;

      //suspension spring
      JointSpring suspensionSpringg = new JointSpring();
      suspensionSpringg.spring = _suspension.suspensionHardness;
      suspensionSpringg.damper = _suspension.suspensionSwing;
      suspensionSpringg.targetPosition = 0.5f;
      _wheels.rightFrontWheel.wheelCollider.suspensionSpring = suspensionSpringg;
      _wheels.leftFrontWheel.wheelCollider.suspensionSpring = suspensionSpringg;
      _wheels.rightRearWheel.wheelCollider.suspensionSpring = suspensionSpringg;
      _wheels.leftRearWheel.wheelCollider.suspensionSpring = suspensionSpringg;

      //extra wheels FOR
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        //suspension and rate
        _wheels.extraWheels[x].wheelCollider.wheelDampingRate = 0.75f;
        _wheels.extraWheels[x].wheelCollider.forceAppPointDistance = _suspension.forceAppPointDistance;
        //suspension spring
        _wheels.extraWheels[x].wheelCollider.suspensionSpring = suspensionSpringg;
        //mass and height
        _wheels.extraWheels[x].wheelCollider.mass = _wheels.wheelMass;
        _wheels.extraWheels[x].wheelCollider.suspensionDistance = _suspension.constVehicleHeightStart;
      }

      //mass
      _wheels.rightFrontWheel.wheelCollider.mass = _wheels.wheelMass;
      _wheels.leftFrontWheel.wheelCollider.mass = _wheels.wheelMass;
      _wheels.rightRearWheel.wheelCollider.mass = _wheels.wheelMass;
      _wheels.leftRearWheel.wheelCollider.mass = _wheels.wheelMass;

      //height
      _wheels.rightFrontWheel.wheelCollider.suspensionDistance = _suspension.constVehicleHeightStart;
      _wheels.leftFrontWheel.wheelCollider.suspensionDistance = _suspension.constVehicleHeightStart;
      _wheels.rightRearWheel.wheelCollider.suspensionDistance = _suspension.constVehicleHeightStart;
      _wheels.leftRearWheel.wheelCollider.suspensionDistance = _suspension.constVehicleHeightStart;
      //

      //Set friction in wheels
      if (_wheels.setFrictionByCode) {
        SetWheelColliderFrictionOnStart(_wheels.rightFrontWheel.wheelCollider, _wheels.rightFrontWheel.useCustomFriction,
          _wheels.rightFrontWheel.adjustCustomFriction.ForwardFriction.ExtremumSlip,
          _wheels.rightFrontWheel.adjustCustomFriction.ForwardFriction.ExtremumValue,
          _wheels.rightFrontWheel.adjustCustomFriction.ForwardFriction.AsymptoteSlip,
          _wheels.rightFrontWheel.adjustCustomFriction.ForwardFriction.AsymptoteValue,
          _wheels.rightFrontWheel.adjustCustomFriction.SidewaysFriction.ExtremumSlip,
          _wheels.rightFrontWheel.adjustCustomFriction.SidewaysFriction.ExtremumValue,
          _wheels.rightFrontWheel.adjustCustomFriction.SidewaysFriction.AsymptoteSlip,
          _wheels.rightFrontWheel.adjustCustomFriction.SidewaysFriction.AsymptoteValue);

        SetWheelColliderFrictionOnStart(_wheels.rightRearWheel.wheelCollider, _wheels.rightRearWheel.useCustomFriction,
          _wheels.rightRearWheel.adjustCustomFriction.ForwardFriction.ExtremumSlip,
          _wheels.rightRearWheel.adjustCustomFriction.ForwardFriction.ExtremumValue,
          _wheels.rightRearWheel.adjustCustomFriction.ForwardFriction.AsymptoteSlip,
          _wheels.rightRearWheel.adjustCustomFriction.ForwardFriction.AsymptoteValue,
          _wheels.rightRearWheel.adjustCustomFriction.SidewaysFriction.ExtremumSlip,
          _wheels.rightRearWheel.adjustCustomFriction.SidewaysFriction.ExtremumValue,
          _wheels.rightRearWheel.adjustCustomFriction.SidewaysFriction.AsymptoteSlip,
          _wheels.rightRearWheel.adjustCustomFriction.SidewaysFriction.AsymptoteValue);

        SetWheelColliderFrictionOnStart(_wheels.leftFrontWheel.wheelCollider, _wheels.leftFrontWheel.useCustomFriction,
          _wheels.leftFrontWheel.adjustCustomFriction.ForwardFriction.ExtremumSlip,
          _wheels.leftFrontWheel.adjustCustomFriction.ForwardFriction.ExtremumValue,
          _wheels.leftFrontWheel.adjustCustomFriction.ForwardFriction.AsymptoteSlip,
          _wheels.leftFrontWheel.adjustCustomFriction.ForwardFriction.AsymptoteValue,
          _wheels.leftFrontWheel.adjustCustomFriction.SidewaysFriction.ExtremumSlip,
          _wheels.leftFrontWheel.adjustCustomFriction.SidewaysFriction.ExtremumValue,
          _wheels.leftFrontWheel.adjustCustomFriction.SidewaysFriction.AsymptoteSlip,
          _wheels.leftFrontWheel.adjustCustomFriction.SidewaysFriction.AsymptoteValue);

        SetWheelColliderFrictionOnStart(_wheels.leftRearWheel.wheelCollider, _wheels.leftRearWheel.useCustomFriction,
          _wheels.leftRearWheel.adjustCustomFriction.ForwardFriction.ExtremumSlip,
          _wheels.leftRearWheel.adjustCustomFriction.ForwardFriction.ExtremumValue,
          _wheels.leftRearWheel.adjustCustomFriction.ForwardFriction.AsymptoteSlip,
          _wheels.leftRearWheel.adjustCustomFriction.ForwardFriction.AsymptoteValue,
          _wheels.leftRearWheel.adjustCustomFriction.SidewaysFriction.ExtremumSlip,
          _wheels.leftRearWheel.adjustCustomFriction.SidewaysFriction.ExtremumValue,
          _wheels.leftRearWheel.adjustCustomFriction.SidewaysFriction.AsymptoteSlip,
          _wheels.leftRearWheel.adjustCustomFriction.SidewaysFriction.AsymptoteValue);

        for (int x = 0; x < _wheels.extraWheels.Length; x++) {
          SetWheelColliderFrictionOnStart(_wheels.extraWheels[x].wheelCollider, _wheels.extraWheels[x].useCustomFriction,
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.ExtremumSlip,
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.ExtremumValue,
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.AsymptoteSlip,
            _wheels.extraWheels[x].adjustCustomFriction.ForwardFriction.AsymptoteValue,
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.ExtremumSlip,
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.ExtremumValue,
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.AsymptoteSlip,
            _wheels.extraWheels[x].adjustCustomFriction.SidewaysFriction.AsymptoteValue);
        }
      }
    }
    public void SetGeneralSuspensionHeight() {
      float height = _suspension.vehicleCustomHeights[_suspension.indexCustomSuspensionHeight];
      if (_wheels.rightFrontWheel.wheelCollider.suspensionDistance != height) {
        _wheels.rightFrontWheel.wheelCollider.suspensionDistance = Mathf.MoveTowards(_wheels.rightFrontWheel.wheelCollider.suspensionDistance, height, fixedDeltaTime);
      }
      if (_wheels.leftFrontWheel.wheelCollider.suspensionDistance != height) {
        _wheels.leftFrontWheel.wheelCollider.suspensionDistance = Mathf.MoveTowards(_wheels.leftFrontWheel.wheelCollider.suspensionDistance, height, fixedDeltaTime);
      }
      if (_wheels.rightRearWheel.wheelCollider.suspensionDistance != height) {
        _wheels.rightRearWheel.wheelCollider.suspensionDistance = Mathf.MoveTowards(_wheels.rightRearWheel.wheelCollider.suspensionDistance, height, fixedDeltaTime);
      }
      if (_wheels.leftRearWheel.wheelCollider.suspensionDistance != height) {
        _wheels.leftRearWheel.wheelCollider.suspensionDistance = Mathf.MoveTowards(_wheels.leftRearWheel.wheelCollider.suspensionDistance, height, fixedDeltaTime);
      }
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        if (_wheels.extraWheels[x].wheelCollider.suspensionDistance != height) {
          _wheels.extraWheels[x].wheelCollider.suspensionDistance = Mathf.MoveTowards(_wheels.extraWheels[x].wheelCollider.suspensionDistance, height, fixedDeltaTime);
        }
      }
    }

    public void SetWheelColliderFrictionOnStart(WheelCollider collider, bool useCustomFriction, float extSlipFW, float extValFW, float asySlipFW, float asyValueFW, float extSlipSL, float extValSL, float asySlipSL, float asyValueSL) {
      //friction Fw
      WheelFrictionCurve wheelFrictionCurveFW = new WheelFrictionCurve();
      if (useCustomFriction) {
        wheelFrictionCurveFW.extremumSlip = extSlipFW;
        wheelFrictionCurveFW.extremumValue = extValFW;
        wheelFrictionCurveFW.asymptoteSlip = asySlipFW;
        wheelFrictionCurveFW.asymptoteValue = asyValueFW;
        wheelFrictionCurveFW.stiffness = _groundFriction.standardForwardFriction;
      } else {
        wheelFrictionCurveFW.extremumSlip = _wheels.defaultFriction.ForwardFriction.ExtremumSlip;
        wheelFrictionCurveFW.extremumValue = _wheels.defaultFriction.ForwardFriction.ExtremumValue;
        wheelFrictionCurveFW.asymptoteSlip = _wheels.defaultFriction.ForwardFriction.AsymptoteSlip;
        wheelFrictionCurveFW.asymptoteValue = _wheels.defaultFriction.ForwardFriction.AsymptoteValue;
        wheelFrictionCurveFW.stiffness = _groundFriction.standardForwardFriction;
      }
      collider.forwardFriction = wheelFrictionCurveFW;

      //friction Sw
      WheelFrictionCurve wheelFrictionCurveSW = new WheelFrictionCurve();
      if (useCustomFriction) {
        wheelFrictionCurveSW.extremumSlip = extSlipSL;
        wheelFrictionCurveSW.extremumValue = extValSL;
        wheelFrictionCurveSW.asymptoteSlip = asySlipSL;
        wheelFrictionCurveSW.asymptoteValue = asyValueSL;
        wheelFrictionCurveSW.stiffness = _groundFriction.standardSideFriction;
      } else {
        wheelFrictionCurveSW.extremumSlip = _wheels.defaultFriction.SidewaysFriction.ExtremumSlip;
        wheelFrictionCurveSW.extremumValue = _wheels.defaultFriction.SidewaysFriction.ExtremumValue;
        wheelFrictionCurveSW.asymptoteSlip = _wheels.defaultFriction.SidewaysFriction.AsymptoteSlip;
        wheelFrictionCurveSW.asymptoteValue = _wheels.defaultFriction.SidewaysFriction.AsymptoteValue;
        wheelFrictionCurveSW.stiffness = _groundFriction.standardSideFriction;
      }
      collider.sidewaysFriction = wheelFrictionCurveSW;
    }



    void LightsManager() {
      float lightVerticalInput = 0.0f;

      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        //MAIN LIGHTS
        for (int x = 0; x < _lights.mainLights.lights.Length; x++) {
          if (_lights.mainLights.lights[x]) {
            if (lowLightOn && !highLightOn) {
              _lights.mainLights.lights[x].enabled = true;
              _lights.mainLights.lights[x].intensity = _lights.mainLights.intensity * 0.5f;
              _lights.mainLights.lights[x].range = headlightsRange[x] * 0.5f;
            } else if (!lowLightOn && highLightOn) {
              _lights.mainLights.lights[x].enabled = true;
              _lights.mainLights.lights[x].intensity = _lights.mainLights.intensity;
              _lights.mainLights.lights[x].range = headlightsRange[x];
            } else if (!lowLightOn && !highLightOn) {
              _lights.mainLights.lights[x].enabled = false;
            }
          }
        }
        //
        if (lowLightOn && !highLightOn) {
          if (_lights.mainLights.meshesLightOn_low) {
            _lights.mainLights.meshesLightOn_low.SetActive(true);
          }
          if (_lights.mainLights.meshesLightOn_high) {
            _lights.mainLights.meshesLightOn_high.SetActive(false);
          }
          if (_lights.mainLights.meshesLightOff) {
            _lights.mainLights.meshesLightOff.SetActive(false);
          }
        } else if (!lowLightOn && highLightOn) {
          if (_lights.mainLights.meshesLightOn_low) {
            _lights.mainLights.meshesLightOn_low.SetActive(false);
          }
          if (_lights.mainLights.meshesLightOn_high) {
            _lights.mainLights.meshesLightOn_high.SetActive(true);
          }
          if (_lights.mainLights.meshesLightOff) {
            _lights.mainLights.meshesLightOff.SetActive(false);
          }
        } else if (!lowLightOn && !highLightOn) {
          if (_lights.mainLights.meshesLightOn_low) {
            _lights.mainLights.meshesLightOn_low.SetActive(false);
          }
          if (_lights.mainLights.meshesLightOn_high) {
            _lights.mainLights.meshesLightOn_high.SetActive(false);
          }
          if (_lights.mainLights.meshesLightOff) {
            _lights.mainLights.meshesLightOff.SetActive(true);
          }
        }

        //headlights
        for (int x = 0; x < _lights.headlights.lights.Length; x++) {
          if (_lights.headlights.lights[x]) {
            if (headlightsOn) {
              _lights.headlights.lights[x].enabled = true;
            } else {
              _lights.headlights.lights[x].enabled = false;
            }
          }
        }
        if (_lights.headlights.meshesLightOn) {
          _lights.headlights.meshesLightOn.SetActive(headlightsOn);
        }
        if (_lights.headlights.meshesLightOff) {
          _lights.headlights.meshesLightOff.SetActive(!headlightsOn);
        }

        //get input signal (on inside the car)
        lightVerticalInput = verticalInput;
        if (!isBraking) {
          lightVerticalInput = 0;
        }
        if (brakingAuto) {
          lightVerticalInput = -1.0f;
        }
      }

      //
      // 
      // SET MATERIAL IN LIGHTS
      // 
      //
      //reverseGearLights ==========================================================================
      bool insideInVehicle = false;
      if (_vehicleState == ControlState.isPlayer || _vehicleState == ControlState.isAI) {
        insideInVehicle = true;
      }
      for (int x = 0; x < _lights.reverseGearLights.lights.Length; x++) {
        if (_lights.reverseGearLights.lights[x]) {
          if (currentGear == -1 && insideInVehicle) {
            _lights.reverseGearLights.lights[x].enabled = true;
          } else {
            _lights.reverseGearLights.lights[x].enabled = false;
          }
        }
      }
      if (_lights.reverseGearLights.meshesLightOff) {
        if (currentGear == -1 && insideInVehicle) {//light on
          _lights.reverseGearLights.meshesLightOff.SetActive(false);
        } else {//light off
          _lights.reverseGearLights.meshesLightOff.SetActive(true);
        }
      }
      if (_lights.reverseGearLights.meshesLightOn) {
        if (currentGear == -1 && insideInVehicle) {//light on
          _lights.reverseGearLights.meshesLightOn.SetActive(true);
        } else {//light off
          _lights.reverseGearLights.meshesLightOn.SetActive(false);
        }
      }
      //


      //brakeLights ================================================================================================
      if (!automaticGears) {
        brakeLightIntensityParameter = brakeLightsIntensity + Mathf.Abs(Mathf.Clamp(lightVerticalInput, -1.0f, 0.0f));
      } else {
        if (currentGear > 0) {
          brakeLightIntensityParameter = brakeLightsIntensity + Mathf.Abs(Mathf.Clamp(lightVerticalInput, -1.0f, 0.0f));
        } else if (currentGear <= 0) {
          brakeLightIntensityParameter = brakeLightsIntensity + Mathf.Abs(Mathf.Clamp(lightVerticalInput, 0.0f, 1.0f));
        }
      }
      for (int x = 0; x < _lights.brakeLights.lights.Length; x++) {
        if (_lights.brakeLights.lights[x]) {
          _lights.brakeLights.lights[x].intensity = brakeLightIntensityParameter;
        }
      }
      if (_lights.brakeLights.meshesLightOff) {
        if (brakeLightIntensityParameter > 0.0f) {
          _lights.brakeLights.meshesLightOff.SetActive(false);
        } else {
          _lights.brakeLights.meshesLightOff.SetActive(true);
        }
      }
      if (_lights.brakeLights.meshesLightOn) {
        if (brakeLightIntensityParameter > 0.0f) {
          _lights.brakeLights.meshesLightOn.SetActive(true);
        } else {
          _lights.brakeLights.meshesLightOn.SetActive(false);
        }
      }
      //

      //rightFlashingLight ==============================================================================================
      intensityFlashingL = Mathf.PingPong(Time.time * _lights.flashingLights.speed, _lights.flashingLights.intensity);
      for (int x = 0; x < _lights.flashingLights.rightFlashingLight.light.Length; x++) {
        if (_lights.flashingLights.rightFlashingLight.light[x]) {
          if (rightBlinkersOn) {
            _lights.flashingLights.rightFlashingLight.light[x].enabled = true;
            _lights.flashingLights.rightFlashingLight.light[x].intensity = intensityFlashingL;
          } else {
            _lights.flashingLights.rightFlashingLight.light[x].enabled = false;
          }
        }
      }
      if (_lights.flashingLights.rightFlashingLight.meshesLightOff) {
        if (rightBlinkersOn && intensityFlashingL >= 0.4f) {
          _lights.flashingLights.rightFlashingLight.meshesLightOff.SetActive(false);
        } else {
          _lights.flashingLights.rightFlashingLight.meshesLightOff.SetActive(true);
        }
      }
      if (_lights.flashingLights.rightFlashingLight.meshesLightOn) {
        if (rightBlinkersOn && intensityFlashingL >= 0.4f) {
          _lights.flashingLights.rightFlashingLight.meshesLightOn.SetActive(true);
        } else {
          _lights.flashingLights.rightFlashingLight.meshesLightOn.SetActive(false);
        }
      }
      //leftFlashingLight ==============================================================================================
      for (int x = 0; x < _lights.flashingLights.leftFlashingLight.light.Length; x++) {
        if (_lights.flashingLights.leftFlashingLight.light[x]) {
          if (leftBlinkersOn) {
            _lights.flashingLights.leftFlashingLight.light[x].enabled = true;
            _lights.flashingLights.leftFlashingLight.light[x].intensity = intensityFlashingL;
          } else {
            _lights.flashingLights.leftFlashingLight.light[x].enabled = false;
          }
        }
      }
      if (_lights.flashingLights.leftFlashingLight.meshesLightOff) {
        if (leftBlinkersOn && intensityFlashingL >= 0.4f) {
          _lights.flashingLights.leftFlashingLight.meshesLightOff.SetActive(false);
        } else {
          _lights.flashingLights.leftFlashingLight.meshesLightOff.SetActive(true);
        }
      }
      if (_lights.flashingLights.leftFlashingLight.meshesLightOn) {
        if (leftBlinkersOn && intensityFlashingL >= 0.4f) {
          _lights.flashingLights.leftFlashingLight.meshesLightOn.SetActive(true);
        } else {
          _lights.flashingLights.leftFlashingLight.meshesLightOn.SetActive(false);
        }
      }
      //


      //extraLights =============================================================================================
      intensitySirenL = Mathf.PingPong(Time.time * _lights.extraLights.speed, _lights.extraLights.intensity);
      if (_lights.extraLights.lightEffect == ExtraLightsClass.TipoLuz.Continnous) {
        intensitySirenL = 1;
      }
      for (int x = 0; x < _lights.extraLights.lights.Length; x++) {
        if (_lights.extraLights.lights[x]) {
          if (extraLightsOn) {
            _lights.extraLights.lights[x].enabled = true;
            _lights.extraLights.lights[x].intensity = intensitySirenL;
          } else {
            _lights.extraLights.lights[x].enabled = false;
          }
        }
      }
      if (_lights.extraLights.meshesLightOff) {
        if (extraLightsOn) {
          _lights.extraLights.meshesLightOff.SetActive(false);
        } else {
          _lights.extraLights.meshesLightOff.SetActive(true);
        }
      }
      if (_lights.extraLights.meshesLightOn) {
        if (extraLightsOn) {
          _lights.extraLights.meshesLightOn.SetActive(true);
        } else {
          _lights.extraLights.meshesLightOn.SetActive(false);
        }
      }
    }



    void DisableAllLightsOnStart() {
      disableBlinkers2 = disableBlinkers1 = alertOn = headlightsOn = highLightOn = lowLightOn = rightBlinkersOn = leftBlinkersOn = extraLightsOn = false;
      //brake lights===================================================================================
      for (int x = 0; x < _lights.brakeLights.lights.Length; x++) {//brake
        if (_lights.brakeLights.lights[x]) {
          _lights.brakeLights.lights[x].enabled = false;
        }
      }
      if (_lights.brakeLights.meshesLightOn) {
        _lights.brakeLights.meshesLightOn.SetActive(false);
      }
      if (_lights.brakeLights.meshesLightOff) {
        _lights.brakeLights.meshesLightOff.SetActive(true);
      }
      //
      //reverse lights===================================================================================
      for (int x = 0; x < _lights.reverseGearLights.lights.Length; x++) {//reverse
        if (_lights.reverseGearLights.lights[x]) {
          _lights.reverseGearLights.lights[x].enabled = false;
        }
      }
      if (_lights.reverseGearLights.meshesLightOn) {
        _lights.reverseGearLights.meshesLightOn.SetActive(false);
      }
      if (_lights.reverseGearLights.meshesLightOff) {
        _lights.reverseGearLights.meshesLightOff.SetActive(true);
      }
      //
      //main lights===================================================================================
      for (int x = 0; x < _lights.mainLights.lights.Length; x++) {//main
        if (_lights.mainLights.lights[x]) {
          _lights.mainLights.lights[x].enabled = false;
        }
      }
      if (_lights.mainLights.meshesLightOn_low) {
        _lights.mainLights.meshesLightOn_low.SetActive(false);
      }
      if (_lights.mainLights.meshesLightOn_high) {
        _lights.mainLights.meshesLightOn_high.SetActive(false);
      }
      if (_lights.mainLights.meshesLightOff) {
        _lights.mainLights.meshesLightOff.SetActive(true);
      }
      //
      //extra lights===================================================================================
      for (int x = 0; x < _lights.extraLights.lights.Length; x++) {
        if (_lights.extraLights.lights[x]) {
          _lights.extraLights.lights[x].enabled = false;
        }
      }
      if (_lights.extraLights.meshesLightOn) {
        _lights.extraLights.meshesLightOn.SetActive(false);
      }
      if (_lights.extraLights.meshesLightOff) {
        _lights.extraLights.meshesLightOff.SetActive(true);
      }
      //
      //head lights===================================================================================
      for (int x = 0; x < _lights.headlights.lights.Length; x++) {
        if (_lights.headlights.lights[x]) {
          _lights.headlights.lights[x].enabled = false;
        }
      }
      if (_lights.headlights.meshesLightOn) {
        _lights.headlights.meshesLightOn.SetActive(false);
      }
      if (_lights.headlights.meshesLightOff) {
        _lights.headlights.meshesLightOff.SetActive(true);
      }
      //
      //rightFlashingLight===================================================================================
      for (int x = 0; x < _lights.flashingLights.rightFlashingLight.light.Length; x++) {
        if (_lights.flashingLights.rightFlashingLight.light[x]) {
          _lights.flashingLights.rightFlashingLight.light[x].enabled = false;
        }
      }
      if (_lights.flashingLights.rightFlashingLight.meshesLightOn) {
        _lights.flashingLights.rightFlashingLight.meshesLightOn.SetActive(false);
      }
      if (_lights.flashingLights.rightFlashingLight.meshesLightOff) {
        _lights.flashingLights.rightFlashingLight.meshesLightOff.SetActive(true);
      }
      //
      //leftFlashingLight===================================================================================
      for (int x = 0; x < _lights.flashingLights.leftFlashingLight.light.Length; x++) {
        if (_lights.flashingLights.leftFlashingLight.light[x]) {
          _lights.flashingLights.leftFlashingLight.light[x].enabled = false;
        }
      }
      if (_lights.flashingLights.leftFlashingLight.meshesLightOn) {
        _lights.flashingLights.leftFlashingLight.meshesLightOn.SetActive(false);
      }
      if (_lights.flashingLights.leftFlashingLight.meshesLightOff) {
        _lights.flashingLights.leftFlashingLight.meshesLightOff.SetActive(true);
      }
    }

    void SetLightsValues(Light light, LightType type, bool startLightOn, Color lightColor, float intensity, bool shadows, LightRenderMode RendType) {
      light.type = type;
      light.enabled = startLightOn;
      light.renderMode = RendType;
      light.color = lightColor;
      light.intensity = intensity;
      light.shadowStrength = 0.0f;
      if (shadows) {
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 1;
      } else {
        light.shadows = LightShadows.None;
      }
      if (light.transform.gameObject.activeSelf == false) {
        light.transform.gameObject.SetActive(true);
      }
    }

    void SetLightValuesStart() {
      //enable or disable UI buttons (all these variables are 'false' by default)
      //mainLights
      if ((_lights.mainLights.lights.Length > 0) || (_lights.mainLights.meshesLightOff && _lights.mainLights.meshesLightOn_low && _lights.mainLights.meshesLightOn_high)) {
        msvs_useMainLights = true;
      }
      //headLights
      if ((_lights.headlights.lights.Length > 0) || (_lights.headlights.meshesLightOff && _lights.headlights.meshesLightOn)) {
        msvs_useHeadLights = true;
      }
      //extraLights
      if ((_lights.extraLights.lights.Length > 0) && (_lights.extraLights.meshesLightOn && _lights.extraLights.meshesLightOff)) {
        msvs_useExtraLights = true;
      }
      //flashing or warning lights
      if ((_lights.flashingLights.leftFlashingLight.light.Length > 0 && _lights.flashingLights.rightFlashingLight.light.Length > 0)) {
        msvs_useWarningOrFlashingLights = true;
      }
      if (_lights.flashingLights.leftFlashingLight.meshesLightOff && _lights.flashingLights.leftFlashingLight.meshesLightOn && _lights.flashingLights.rightFlashingLight.meshesLightOff && _lights.flashingLights.rightFlashingLight.meshesLightOn) {
        msvs_useWarningOrFlashingLights = true;
      }

      //
      //get light ranger to low light and high light (luz baixa e luz alta)
      headlightsRange = new float[_lights.mainLights.lights.Length];
      for (int x = 0; x < _lights.mainLights.lights.Length; x++) {
        if (_lights.mainLights.lights[x]) {
          headlightsRange[x] = _lights.mainLights.lights[x].range;
        } else {
          headlightsRange[x] = 40;
        }
      }
      brakeLightsIntensity = 0;
      disableBlinkers2 = disableBlinkers1 = alertOn = headlightsOn = highLightOn = lowLightOn = rightBlinkersOn = leftBlinkersOn = extraLightsOn = false;
      for (int x = 0; x < _lights.brakeLights.lights.Length; x++) {//brake
        if (_lights.brakeLights.lights[x]) {
          SetLightsValues(_lights.brakeLights.lights[x], LightType.Point, true, _lights.brakeLights.color, _lights.brakeLights.intensity, _lights.brakeLights.shadow, _lights.brakeLights.renderMode);
        }
      }
      for (int x = 0; x < _lights.reverseGearLights.lights.Length; x++) {//reverse
        if (_lights.reverseGearLights.lights[x]) {
          SetLightsValues(_lights.reverseGearLights.lights[x], LightType.Point, false, _lights.reverseGearLights.color, _lights.reverseGearLights.intensity, _lights.reverseGearLights.shadow, _lights.reverseGearLights.renderMode);
        }
      }
      for (int x = 0; x < _lights.mainLights.lights.Length; x++) {//main
        if (_lights.mainLights.lights[x]) {
          SetLightsValues(_lights.mainLights.lights[x], LightType.Spot, false, _lights.mainLights.color, _lights.mainLights.intensity, _lights.mainLights.shadow, _lights.mainLights.renderMode);
          _lights.mainLights.lights[x].transform.rotation = transform.rotation;
        }
      }
      for (int x = 0; x < _lights.flashingLights.rightFlashingLight.light.Length; x++) {//right
        if (_lights.flashingLights.rightFlashingLight.light[x]) {
          SetLightsValues(_lights.flashingLights.rightFlashingLight.light[x], LightType.Point, false, _lights.flashingLights.color, _lights.flashingLights.intensity, _lights.flashingLights.shadow, _lights.flashingLights.renderMode);
        }
      }
      for (int x = 0; x < _lights.flashingLights.rightFlashingLight.light.Length; x++) {//left
        if (_lights.flashingLights.leftFlashingLight.light[x]) {
          SetLightsValues(_lights.flashingLights.leftFlashingLight.light[x], LightType.Point, false, _lights.flashingLights.color, _lights.flashingLights.intensity, _lights.flashingLights.shadow, _lights.flashingLights.renderMode);
        }
      }
      for (int x = 0; x < _lights.extraLights.lights.Length; x++) {
        if (_lights.extraLights.lights[x]) {
          SetLightsValues(_lights.extraLights.lights[x], _lights.extraLights.lightType, false, _lights.extraLights.color, _lights.extraLights.intensity, _lights.extraLights.shadow, _lights.extraLights.renderMode);
        }
      }
      for (int x = 0; x < _lights.headlights.lights.Length; x++) {
        if (_lights.headlights.lights[x]) {
          SetLightsValues(_lights.headlights.lights[x], LightType.Spot, false, _lights.headlights.color, _lights.headlights.intensity, _lights.headlights.shadow, _lights.headlights.renderMode);
          _lights.headlights.lights[x].transform.rotation = transform.rotation;
        }
      }
    }



    int GetDominantTerrainTextureInWorldPosition(Vector3 worldPosition) {
      terrainCompositionArray = TerrainComposition(worldPosition, activeTerrain_optional);
      if (terrainCompositionArray != null) {
        int dominantIndex = 0;
        float maximumCompositionMix = 0;
        for (int x = 0; x < terrainCompositionArray.Length; ++x) {
          if (terrainCompositionArray[x] > maximumCompositionMix) {
            dominantIndex = x;
            maximumCompositionMix = terrainCompositionArray[x];
          }
        }
        return dominantIndex;
      } else {
        return -1;
      }
    }
    float[] TerrainComposition(Vector3 worldPosition, Terrain myTerrain) {
      try {
        if (!myTerrain) {
          myTerrain = Terrain.activeTerrain;
        }
        terrainData = myTerrain.terrainData;
        int terrainPosX = (int)(((worldPosition.x - myTerrain.transform.position.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int terrainPosZ = (int)(((worldPosition.z - myTerrain.transform.position.z) / terrainData.size.z) * terrainData.alphamapHeight);
        if (terrainPosX >= terrainData.alphamapWidth || terrainPosZ >= terrainData.alphamapHeight) { // if world position > terrain size
          return null;
        }
        //
        alphaMaps = terrainData.GetAlphamaps(terrainPosX, terrainPosZ, 1, 1);
        terrainCompositionMix = new float[alphaMaps.GetUpperBound(2) + 1];
        for (int x = 0; x < terrainCompositionMix.Length; ++x) {
          terrainCompositionMix[x] = alphaMaps[0, 0, x];
        }
        return terrainCompositionMix;
      } catch {
        return null;
      }
    }



    void CheckGroundForSKidMarks() {
      if (_wheels.rightFrontWheel.wheelCollider) {
        if (wheelFDIsGrounded) {
          _wheels.rightFrontWheel.generateSkidBool = GenerateSkidMarks(_wheels.rightFrontWheel.wheelCollider, _wheels.rightFrontWheel.wheelWorldPosition,
            _wheels.rightFrontWheel.rendSKDmarks, _wheels.rightFrontWheel.generateSkidBool, _wheels.rightFrontWheel.skidMarkShift, 0
            , _wheels.rightFrontWheel.useCustomBrandWidth, _wheels.rightFrontWheel.customBrandWidth);
        } else {
          _wheels.rightFrontWheel.generateSkidBool = false;
        }
      }
      //
      if (_wheels.leftFrontWheel.wheelCollider) {
        if (wheelFEIsGrounded) {
          _wheels.leftFrontWheel.generateSkidBool = GenerateSkidMarks(_wheels.leftFrontWheel.wheelCollider, _wheels.leftFrontWheel.wheelWorldPosition,
            _wheels.leftFrontWheel.rendSKDmarks, _wheels.leftFrontWheel.generateSkidBool, _wheels.leftFrontWheel.skidMarkShift, 1
            , _wheels.leftFrontWheel.useCustomBrandWidth, _wheels.leftFrontWheel.customBrandWidth);
        } else {
          _wheels.leftFrontWheel.generateSkidBool = false;
        }
      }
      //
      if (_wheels.rightRearWheel.wheelCollider) {
        if (wheelTDIsGrounded) {
          _wheels.rightRearWheel.generateSkidBool = GenerateSkidMarks(_wheels.rightRearWheel.wheelCollider, _wheels.rightRearWheel.wheelWorldPosition,
            _wheels.rightRearWheel.rendSKDmarks, _wheels.rightRearWheel.generateSkidBool, _wheels.rightRearWheel.skidMarkShift, 2
            , _wheels.rightRearWheel.useCustomBrandWidth, _wheels.rightRearWheel.customBrandWidth);
        } else {
          _wheels.rightRearWheel.generateSkidBool = false;
        }
      }
      //
      if (_wheels.leftRearWheel.wheelCollider) {
        if (wheelTEIsGrounded) {
          _wheels.leftRearWheel.generateSkidBool = GenerateSkidMarks(_wheels.leftRearWheel.wheelCollider, _wheels.leftRearWheel.wheelWorldPosition,
            _wheels.leftRearWheel.rendSKDmarks, _wheels.leftRearWheel.generateSkidBool, _wheels.leftRearWheel.skidMarkShift, 3
            , _wheels.leftRearWheel.useCustomBrandWidth, _wheels.leftRearWheel.customBrandWidth);
        } else {
          _wheels.leftRearWheel.generateSkidBool = false;
        }
      }
      //
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        if (_wheels.extraWheels[x].wheelCollider) {
          if (_wheels.extraWheels[x].wheelCollider.isGrounded) {
            _wheels.extraWheels[x].generateSkidBool = GenerateSkidMarks(_wheels.extraWheels[x].wheelCollider, _wheels.extraWheels[x].wheelWorldPosition,
              _wheels.extraWheels[x].rendSKDmarks, _wheels.extraWheels[x].generateSkidBool, _wheels.extraWheels[x].skidMarkShift, (x + 4)
              , _wheels.extraWheels[x].useCustomBrandWidth, _wheels.extraWheels[x].customBrandWidth);
          } else {
            _wheels.extraWheels[x].generateSkidBool = false;
          }
        }
      }
    }

    private int GetCurrentVerticeIndexForMesh(Mesh mesh) {
      int result;
      currentIndexes.TryGetValue(mesh, out result);
      result += 2;
      result %= mesh.vertexCount;
      result += mesh.vertexCount;
      currentIndexes[mesh] = result;
      return result;
    }

    private static T GetRepeatedArrayValue<T>(List<T> array, int index) {
      return array[GetRepeatedArrayIndex(array, index)];
    }

    private static int GetRepeatedArrayIndex<T>(List<T> array, int index) {
      return (index + array.Count) % array.Count;
    }

    private bool GenerateSkidMarks(WheelCollider wheelCollider, Vector3 wheelPos, Mesh wheelSkidMesh, bool generateBool, float lateralDisplacement, int indexLastMark, bool customWidthBool, float customWidthFloat) {
      if (!wheelCollider.GetGroundHit(out tempWheelHit) || !wheelCollider.isGrounded) {
        return false;
      }
      tempWheelHit.point = wheelPos - wheelCollider.transform.up * wheelCollider.radius * vehicleScale;
      float tempAlphaSkidMarks = Mathf.Abs(tempWheelHit.sidewaysSlip);
      if (Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility) > tempAlphaSkidMarks) {
        tempAlphaSkidMarks = Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility);
      }
      //
      float widthSkidMarks = _skidMarks.standardBrandWidth;
      if (customWidthBool) {
        widthSkidMarks = customWidthFloat;
      }
      //
      Vector3 skidTemp = tempWheelHit.sidewaysDir * (widthSkidMarks * vehicleScale) / 2f * Vector3.Dot(wheelCollider.attachedRigidbody.velocity.normalized, tempWheelHit.forwardDir);
      skidTemp -= tempWheelHit.forwardDir * (widthSkidMarks * vehicleScale) * 0.1f * Vector3.Dot(wheelCollider.attachedRigidbody.velocity.normalized, tempWheelHit.sidewaysDir);
      if (KMh > (75.0f / _skidMarks.sensibility) && Mathf.Abs(wheelCollider.rpm) < (3.0f / _skidMarks.sensibility)) {
        if (wheelCollider.isGrounded) {
          tempAlphaSkidMarks = 10;
        }
      }
      if (KMh < 20.0f * (Mathf.Clamp(_skidMarks.sensibility, 1, 3))) {
        if (Mathf.Abs(tempWheelHit.forwardSlip * _skidMarks.forwordSensibility) > (1.2f / _skidMarks.sensibility)) {
          if (wheelCollider.isGrounded) {
            tempAlphaSkidMarks = 10;
          }
        }
      }
      float RPMCondition = (1.75f / averageWheelRadius);// = 5.0f
      if (Mathf.Abs(wheelCollider.rpm) < RPMCondition && KMh > 5) {
        if (wheelCollider.isGrounded) {
          tempAlphaSkidMarks = 10;
        }
      }

      bool breakForAlphaSkidMarks = false;
      for (int x = 0; x < _skidMarks.otherGround.Length; x++) {
        if (!breakForAlphaSkidMarks) {
          switch (_groundDetection) {
            case GroundDetectionMode.Tag:
              if (!string.IsNullOrEmpty(_skidMarks.otherGround[x].groundTag)) {
                if (tempWheelHit.collider.gameObject.CompareTag(_skidMarks.otherGround[x].groundTag)) {
                  if (_skidMarks.otherGround[x].continuousMarking) {
                    tempAlphaSkidMarks = 10;
                  }
                  breakForAlphaSkidMarks = true;
                }
              }
              break;
            //==============================================================================================================================
            case GroundDetectionMode.PhysicMaterial:
              if (_skidMarks.otherGround[x].physicMaterial) {
                if (tempWheelHit.collider.sharedMaterial == _skidMarks.otherGround[x].physicMaterial) {
                  if (_skidMarks.otherGround[x].continuousMarking) {
                    tempAlphaSkidMarks = 10;
                  }
                  breakForAlphaSkidMarks = true;
                }
              }
              break;
            //==============================================================================================================================
            case GroundDetectionMode.TerrainTextureIndices:
              if (activeTerrain_optional) {
                if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                  int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                  if (dominantTerrainIndex != -1) {
                    if (_skidMarks.otherGround[x].terrainTextureIndices.Count > 0 && _skidMarks.otherGround[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                      if (_skidMarks.otherGround[x].continuousMarking) {
                        tempAlphaSkidMarks = 10;
                      }
                      breakForAlphaSkidMarks = true;
                    }
                  }
                }
              }
              break;
            //==============================================================================================================================
            case GroundDetectionMode.All:
              //tag
              if (!string.IsNullOrEmpty(_skidMarks.otherGround[x].groundTag)) {
                if (tempWheelHit.collider.gameObject.CompareTag(_skidMarks.otherGround[x].groundTag)) {
                  if (_skidMarks.otherGround[x].continuousMarking) {
                    tempAlphaSkidMarks = 10;
                  }
                  breakForAlphaSkidMarks = true;
                  break;
                }
              }
              //physicMaterial
              if (_skidMarks.otherGround[x].physicMaterial) {
                if (tempWheelHit.collider.sharedMaterial == _skidMarks.otherGround[x].physicMaterial) {
                  if (_skidMarks.otherGround[x].continuousMarking) {
                    tempAlphaSkidMarks = 10;
                  }
                  breakForAlphaSkidMarks = true;
                  break;
                }
              }
              //terrainTextureIndice
              if (activeTerrain_optional) {
                if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                  int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                  if (dominantTerrainIndex != -1) {
                    if (_skidMarks.otherGround[x].terrainTextureIndices.Count > 0 && _skidMarks.otherGround[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                      if (_skidMarks.otherGround[x].continuousMarking) {
                        tempAlphaSkidMarks = 10;
                      }
                      breakForAlphaSkidMarks = true;
                    }
                  }
                }
              }
              break;
              //==============================================================================================================================
          }
        }
      }

      if (tempAlphaSkidMarks < (1 / _skidMarks.sensibility)) {
        return false;
      }
      float distance = (lastPoint[indexLastMark] - tempWheelHit.point - skidTemp).sqrMagnitude;
      float alphaAplic = Mathf.Clamp(tempAlphaSkidMarks, 0.0f, 1.0f);

      if (generateBool) {
        if (distance < 0.1f) {
          return true;
        }
      }

      wheelSkidMesh.GetVertices(vertices);
      wheelSkidMesh.GetNormals(normals);
      wheelSkidMesh.GetTriangles(tris, 0);
      wheelSkidMesh.GetColors(colors);
      wheelSkidMesh.GetUVs(0, uv);

      int verLenght = GetCurrentVerticeIndexForMesh(wheelSkidMesh);
      int triLength = verLenght * 3;

      vertices[GetRepeatedArrayIndex(vertices, verLenght - 1)] = tempWheelHit.point + tempWheelHit.normal * _skidMarks.groundDistance - skidTemp + tempWheelHit.sidewaysDir * lateralDisplacement;
      vertices[GetRepeatedArrayIndex(vertices, verLenght - 2)] = tempWheelHit.point + tempWheelHit.normal * _skidMarks.groundDistance + skidTemp + tempWheelHit.sidewaysDir * lateralDisplacement;
      normals[GetRepeatedArrayIndex(normals, verLenght - 1)] = normals[GetRepeatedArrayIndex(normals, verLenght - 2)] = tempWheelHit.normal;

      Color skidMarkColor = _skidMarks.standardColor;
      skidMarkColor.a = Mathf.Clamp(alphaAplic * _skidMarks.standardOpacity, 0.01f, 1.0f);

      bool changeColor = false;
      for (int x = 0; x < _skidMarks.otherGround.Length; x++) {
        if (!changeColor) {
          switch (_groundDetection) {
            case GroundDetectionMode.Tag:
              if (!string.IsNullOrEmpty(_skidMarks.otherGround[x].groundTag)) {
                if (tempWheelHit.collider.gameObject.CompareTag(_skidMarks.otherGround[x].groundTag)) {
                  skidMarkColor = _skidMarks.otherGround[x].color;
                  skidMarkColor.a = Mathf.Clamp(alphaAplic * _skidMarks.otherGround[x].opacity, 0.01f, 1.0f);
                  changeColor = true;
                  break;
                }
              }
              break;
            //==============================================================================================================================
            case GroundDetectionMode.PhysicMaterial:
              if (_skidMarks.otherGround[x].physicMaterial) {
                if (tempWheelHit.collider.sharedMaterial == _skidMarks.otherGround[x].physicMaterial) {
                  skidMarkColor = _skidMarks.otherGround[x].color;
                  skidMarkColor.a = Mathf.Clamp(alphaAplic * _skidMarks.otherGround[x].opacity, 0.01f, 1.0f);
                  changeColor = true;
                  break;
                }
              }
              break;
            //==============================================================================================================================
            case GroundDetectionMode.TerrainTextureIndices:
              if (activeTerrain_optional) {
                if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                  int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                  if (dominantTerrainIndex != -1) {
                    if (_skidMarks.otherGround[x].terrainTextureIndices.Count > 0 && _skidMarks.otherGround[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                      skidMarkColor = _skidMarks.otherGround[x].color;
                      skidMarkColor.a = Mathf.Clamp(alphaAplic * _skidMarks.otherGround[x].opacity, 0.01f, 1.0f);
                      changeColor = true;
                      break;
                    }
                  }
                }
              }
              break;
            //==============================================================================================================================
            case GroundDetectionMode.All:
              //tag
              if (!string.IsNullOrEmpty(_skidMarks.otherGround[x].groundTag)) {
                if (tempWheelHit.collider.gameObject.CompareTag(_skidMarks.otherGround[x].groundTag)) {
                  skidMarkColor = _skidMarks.otherGround[x].color;
                  skidMarkColor.a = Mathf.Clamp(alphaAplic * _skidMarks.otherGround[x].opacity, 0.01f, 1.0f);
                  changeColor = true;
                  break;
                }
              }
              //physicMaterial
              if (_skidMarks.otherGround[x].physicMaterial) {
                if (tempWheelHit.collider.sharedMaterial == _skidMarks.otherGround[x].physicMaterial) {
                  skidMarkColor = _skidMarks.otherGround[x].color;
                  skidMarkColor.a = Mathf.Clamp(alphaAplic * _skidMarks.otherGround[x].opacity, 0.01f, 1.0f);
                  changeColor = true;
                  break;
                }
              }
              //terrainTextureIndice
              if (activeTerrain_optional) {
                if (tempWheelHit.collider.gameObject == activeTerrain_optional.gameObject) {
                  int dominantTerrainIndex = GetDominantTerrainTextureInWorldPosition(tempWheelHit.point);
                  if (dominantTerrainIndex != -1) {
                    if (_skidMarks.otherGround[x].terrainTextureIndices.Count > 0 && _skidMarks.otherGround[x].terrainTextureIndices.Contains(dominantTerrainIndex)) {
                      skidMarkColor = _skidMarks.otherGround[x].color;
                      skidMarkColor.a = Mathf.Clamp(alphaAplic * _skidMarks.otherGround[x].opacity, 0.01f, 1.0f);
                      changeColor = true;
                      break;
                    }
                  }
                }
              }
              break;
              //==============================================================================================================================
          }
        }
      }

      colors[GetRepeatedArrayIndex(colors, verLenght - 1)] = colors[GetRepeatedArrayIndex(colors, verLenght - 2)] = skidMarkColor;

      tris[GetRepeatedArrayIndex(tris, triLength + 0)] = tris[GetRepeatedArrayIndex(tris, triLength + 3)] =
        tris[GetRepeatedArrayIndex(tris, triLength + 1)] = tris[GetRepeatedArrayIndex(tris, triLength + 4)] =
          tris[GetRepeatedArrayIndex(tris, triLength + 2)] = tris[GetRepeatedArrayIndex(tris, triLength + 5)] =
            tris[GetRepeatedArrayIndex(tris, triLength + 6)] = tris[GetRepeatedArrayIndex(tris, triLength + 9)] =
              tris[GetRepeatedArrayIndex(tris, triLength + 7)] = tris[GetRepeatedArrayIndex(tris, triLength + 10)] =
                tris[GetRepeatedArrayIndex(tris, triLength + 8)] = tris[GetRepeatedArrayIndex(tris, triLength + 11)];

      if (generateBool) {
        tris[GetRepeatedArrayIndex(tris, triLength - 1)] = GetRepeatedArrayIndex(vertices, verLenght - 2);
        tris[GetRepeatedArrayIndex(tris, triLength - 2)] = GetRepeatedArrayIndex(vertices, verLenght - 1);
        tris[GetRepeatedArrayIndex(tris, triLength - 3)] = GetRepeatedArrayIndex(vertices, verLenght - 3);
        tris[GetRepeatedArrayIndex(tris, triLength - 4)] = GetRepeatedArrayIndex(vertices, verLenght - 3);
        tris[GetRepeatedArrayIndex(tris, triLength - 5)] = GetRepeatedArrayIndex(vertices, verLenght - 4);
        tris[GetRepeatedArrayIndex(tris, triLength - 6)] = GetRepeatedArrayIndex(vertices, verLenght - 2);

        uv[GetRepeatedArrayIndex(uv, verLenght - 1)] =
          uv[GetRepeatedArrayIndex(uv, verLenght - 3)] + Vector2.right * distance * 0.01f;
        uv[GetRepeatedArrayIndex(uv, verLenght - 2)] =
          uv[GetRepeatedArrayIndex(uv, verLenght - 4)] + Vector2.right * distance * 0.01f;

      } else {
        uv[GetRepeatedArrayIndex(uv, verLenght - 1)] = Vector2.zero;
        uv[GetRepeatedArrayIndex(uv, verLenght - 2)] = Vector2.up;
      }
      lastPoint[indexLastMark] = vertices[GetRepeatedArrayIndex(vertices, verLenght - 1)];
      wheelSkidMesh.SetVertices(vertices);
      wheelSkidMesh.SetNormals(normals);
      wheelSkidMesh.SetTriangles(tris, 0);
      wheelSkidMesh.SetColors(colors);
      wheelSkidMesh.SetUVs(0, uv);
      wheelSkidMesh.RecalculateBounds();
      return true;
    }

    Mesh GerarRendRef(Material skdMaterial, string wheelName) {
      GameObject rendRef = new("SkidMesh " + wheelName + " " + transform.name);
      rendRef.AddComponent<MeshFilter>();
      rendRef.AddComponent<MeshRenderer>();
      Mesh mesh = rendRef.GetComponent<MeshFilter>().mesh = new Mesh();
      mesh.vertices = new Vector3[CacheSize];
      mesh.normals = new Vector3[CacheSize];
      mesh.uv = new Vector2[CacheSize];
      mesh.colors = new Color[CacheSize];
      mesh.triangles = new int[CacheSize * 3];
      mesh.MarkDynamic();
      rendRef.GetComponent<MeshRenderer>().material = skdMaterial;
      rendRef.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      rendRef.transform.parent = SkidMarksContainer;
      return mesh;
    }

    void SetSkidMarksValues() {
      Material skidmarkMaterial;
      if (_skidMarks.customMaterial) {
        skidmarkMaterial = _skidMarks.customMaterial;
      } else {
        skidmarkMaterial = new Material(skidMarksShader);
        skidmarkMaterial.mainTexture = GenerateTextureAndNormalMap(true);
        skidmarkMaterial.SetTexture("_NormalMap", GenerateTextureAndNormalMap(false));
        skidmarkMaterial.SetFloat("_NormFactor", _skidMarks.normalMapIntensity);
        skidmarkMaterial.SetFloat("_Glossiness", _skidMarks.smoothness);
        skidmarkMaterial.SetFloat("_Metallic", _skidMarks.metallic);
      }
      Color skidColor = _skidMarks.standardColor;
      skidColor.a = _skidMarks.standardOpacity;
      skidmarkMaterial.color = skidColor;
      //
      _wheels.rightFrontWheel.rendSKDmarks = GerarRendRef(skidmarkMaterial, _wheels.rightFrontWheel.wheelCollider.gameObject.transform.name);
      _wheels.leftFrontWheel.rendSKDmarks = GerarRendRef(skidmarkMaterial, _wheels.leftFrontWheel.wheelCollider.gameObject.transform.name);
      _wheels.rightRearWheel.rendSKDmarks = GerarRendRef(skidmarkMaterial, _wheels.rightRearWheel.wheelCollider.gameObject.transform.name);
      _wheels.leftRearWheel.rendSKDmarks = GerarRendRef(skidmarkMaterial, _wheels.leftRearWheel.wheelCollider.gameObject.transform.name);
      for (int x = 0; x < _wheels.extraWheels.Length; x++) {
        _wheels.extraWheels[x].rendSKDmarks = GerarRendRef(skidmarkMaterial, _wheels.extraWheels[x].wheelCollider.gameObject.transform.name);
      }
    }

    public Texture GenerateTextureAndNormalMap(bool isTexture) {
      Texture2D texture = new Texture2D(32, 32, TextureFormat.ARGB32, false);
      Color transparentColor1 = new Color(0.0f, 0.0f, 0.0f, 0.5f);
      Color transparentColor2 = new Color(0.0f, 0.0f, 0.0f, 1.0f);
      if (isTexture) {
        transparentColor1 = new Color(1.0f, 1.0f, 1.0f, 0.15f);
        transparentColor2 = new Color(1.0f, 1.0f, 1.0f, 0.6f);
      }
      for (int x = 0; x < 32; x++) {
        for (int y = 0; y < 32; y++) {
          texture.SetPixel(x, y, Color.white);
        }
      }
      for (int y = 0; y < 32; y++) {
        for (int x = 0; x < 32; x++) {
          if (y == 0 || y == 1 || y == 30 || y == 31) {
            texture.SetPixel(x, y, transparentColor1);
          }
          if (y == 6 || y == 7 || y == 15 || y == 16 || y == 24 || y == 25) {
            texture.SetPixel(x, y, transparentColor2);
          }
        }
      }
      texture.Apply();
      return texture;
    }

  }
}
