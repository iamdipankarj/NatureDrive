using System;
using System.Collections.Generic;
using UnityEngine;

namespace Solace {
  public class SaveManager : MonoBehaviour {
    private const string SAVE_PATH = "/player-stats.json";
    public static SaveManager instance;
    private string version;

    public PlayerStats playerStats = new();
    private readonly JsonDataService dataService = new();
    private bool encryptionEnabled = false;
    private bool isSaving = false;

    public void SaveCheckpoint(GameLevel level, int checkpointId) {
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

    public void SaveStats() {
      playerStats.version = version;
      isSaving = true;
      if (dataService.SaveData(SAVE_PATH, playerStats, encryptionEnabled)) {
        isSaving = false;
      }
    }

    private void Start() {
      version = Application.version;
      try {
        PlayerStats data = dataService.LoadData<PlayerStats>(SAVE_PATH, encryptionEnabled);
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
