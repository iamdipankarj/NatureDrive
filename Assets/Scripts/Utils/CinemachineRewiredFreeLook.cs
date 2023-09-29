using UnityEngine;
using Rewired;
using Cinemachine;
using System.Collections;

namespace Solace {
  [RequireComponent(typeof(CinemachineFreeLook))]
  public class CinemachineRewiredFreeLook : MonoBehaviour {
    private const int playerId = 0;

    private void Reset() {
      OnValidate();
    }

    private void OnValidate() {
      if (Application.isPlaying && ReInput.isReady)
        InitializeInput();
    }

    private IEnumerator Start() {
      yield return new WaitUntil(() => ReInput.isReady);
      InitializeInput();
    }

    private void InitializeInput() {
      if (ReInput.isReady) {
        Player input = ReInput.players.GetPlayer(playerId);
        CinemachineCore.GetInputAxis = input.GetAxis;
      }
    }
  }
}
