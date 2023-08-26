using System;
using System.Collections.Generic;

namespace Solace {
  [Serializable]
  public class PlayerStats {
    public string version;
    public PlayerTransform player = new();
    public GameLevel lastLevel = GameLevel.LEVEL_1;
    public List<string> cars = new();
    public Dictionary<GameLevel, List<int>> checkpoints = new();
    public Dictionary<GameLevel, LevelScore> highestScores = new();
  }
}
