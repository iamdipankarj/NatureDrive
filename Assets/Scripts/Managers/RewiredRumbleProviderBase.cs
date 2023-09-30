using UnityEngine;
using Rewired;
using Random = System.Random;

namespace Solace {
  public class RewiredRumbleProviderBase : MonoBehaviour {
    // Rewired
    protected Player player;
    private const int LOW_MOTOR_INDEX = 0;
    private const int HIGH_MOTOR_INDEX = 1;

    private const float DRIFT_RUMBLE_SCALE = 0.85f;
    private const float staticRumbleScale = 0.09f;
    private Random random;
    private bool isRumbleEnabled = true;

    public float GetRandomFloat(float minimum, float maximum) {
      return (float)(random.NextDouble() * (maximum - minimum) + minimum);
    }

    private (float low, float high) GetFrequency(float clampedSpeed) {
      return (clampedSpeed * DRIFT_RUMBLE_SCALE, clampedSpeed * DRIFT_RUMBLE_SCALE);
    }

    protected void SetStaticVibration(float clampedSpeed) {
      float leftRand = GetRandomFloat(0f, 0.3f) * GetRandomFloat(0f, 1f);
      float rightRand = GetRandomFloat(0f, 0.2f) * GetRandomFloat(0f, 1f);
      float low = leftRand * clampedSpeed * staticRumbleScale;
      float high = rightRand * clampedSpeed * staticRumbleScale;
      if (isRumbleEnabled) {
        player.SetVibration(LOW_MOTOR_INDEX, low, 0.1f);
        player.SetVibration(HIGH_MOTOR_INDEX, high, 0.1f);
      }
    }

    protected void SetDriftVibration(float clampedSpeed) {
      if (isRumbleEnabled) {
        player.SetVibration(LOW_MOTOR_INDEX, GetFrequency(clampedSpeed).low, 0.3f);
        player.SetVibration(HIGH_MOTOR_INDEX, GetFrequency(clampedSpeed).high, 0.3f);
      }
    }

    protected void SetStartupVibration() {
      if (isRumbleEnabled) {
        player.SetVibration(LOW_MOTOR_INDEX, 0.0f, 0.3f);
        player.SetVibration(HIGH_MOTOR_INDEX, 0.8f, 0.3f);
      }
    }

    protected void SetCollisionVibration(float strength, float duration = 0.3f) {
      if (isRumbleEnabled) {
        player.SetVibration(LOW_MOTOR_INDEX, strength, duration);
        player.SetVibration(HIGH_MOTOR_INDEX, strength, duration);
      }
    }

    public virtual void Awake() {
      random = new();
      player = ReInput.players.GetPlayer(0);
    }
  }
}
