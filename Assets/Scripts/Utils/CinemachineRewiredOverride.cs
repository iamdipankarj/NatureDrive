using Cinemachine;
using Rewired;
using UnityEngine;

public class CinemachineRewiredOverride {
  [RuntimeInitializeOnLoadMethod]
  private static void InitializeSystemPlayer() {
    CinemachineCore.GetInputAxis = ReInput.players.GetPlayer(0).GetAxis;
  }
}
