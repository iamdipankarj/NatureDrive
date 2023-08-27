using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Solace {
  public class SaveManager : MonoBehaviour {
    private const string relativePath = "/player-stats.json";
    public static SaveManager instance;
    private string version;
    private string savePath;

    private CancellationTokenSource tokenSource;

    public PlayerStats playerStats = new();
    private readonly JsonDataService dataService = new();
    private bool encryptionEnabled = false;
    private bool isSaving = false;

    public PlayerTransform GetPlayerTransform() {
      return playerStats.player;
    }

    public void SaveCheckpoint(GameLevel level, int checkpointId) {
#if UNITY_EDITOR
      Debug.Log($"<color=#00FF00>Saving Checkpoint: </color> {checkpointId}");
#endif
      // Save position
      playerStats.player.posX = Camera.main.transform.position.x;
      playerStats.player.posY = Camera.main.transform.position.y;
      playerStats.player.posZ = Camera.main.transform.position.z;

      playerStats.player.rotX = Camera.main.transform.rotation.x;
      playerStats.player.rotY = Camera.main.transform.rotation.y;
      playerStats.player.rotZ = Camera.main.transform.rotation.z;

      playerStats.lastCheckPoint = checkpointId;

      if (playerStats.checkpoints.ContainsKey(level)) {
        List<int> currentCheckpoints = playerStats.checkpoints[level];
        currentCheckpoints.Add(checkpointId);
      } else {
        playerStats.checkpoints.Add(level, new()
        {
          checkpointId
        });
      }
      SaveStats();
    }

    public async void SaveStats() {
      playerStats.version = version;
      isSaving = true;
      if (tokenSource.IsCancellationRequested) {
        return;
      }
      var result = await Task.Run(async () => {
        if (dataService.SaveData(savePath, playerStats, encryptionEnabled)) {
          isSaving = false;
        }
        while (isSaving) {
          await Task.Yield();
        }
        return true;
      }, tokenSource.Token);
    }

    private void OnDisable() {
      tokenSource?.Cancel();
    }

    private void Start() {
      savePath = Application.persistentDataPath + relativePath;
      tokenSource = new();
      version = Application.version;
      try {
        PlayerStats data = dataService.LoadData<PlayerStats>(savePath, encryptionEnabled);
        if (data != null) {
          playerStats = data;
        }
      } catch (Exception e) {
        Debug.LogError($"Error reading save data: {e.StackTrace}");
      }
    }

    private void Awake() {
      if (instance == null) {
        instance = this;
      }
      else {
        Destroy(gameObject);
      }
      DontDestroyOnLoad(gameObject);
    }
  }
}
