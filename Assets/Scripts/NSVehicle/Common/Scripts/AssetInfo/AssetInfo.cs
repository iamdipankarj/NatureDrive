using UnityEngine;

namespace NSVehicle {
  [CreateAssetMenu(fileName = "AssetInfo", menuName = "NSVehicle/AssetInfo", order = 0)]
  public class AssetInfo : ScriptableObject {
    public string assetName = "Asset";
    public string version = "1.0";
    public string upgradeNotesURL = "";
    public string changelogURL = "";
    public string quickStartURL = "";
    public string documentationURL = "";
    public string forumURL = "";
    public string emailURL = "iamdipankarj@gmail.com";
    public string assetURL = "https://solacegame.com/";
  }
}