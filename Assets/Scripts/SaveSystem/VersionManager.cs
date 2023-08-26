using UnityEngine;

namespace Solace {
  public class VersionManager {
    private static int m_major = 0;
    private static int m_minor = 0;
    private static int m_patch = 0;

    public static int Major {
      get {
        return m_major;
      }
      set {
        string version = Application.version;
        string[] prefixes = version.Split(".");
        if (int.TryParse(prefixes[0], out int result)) {
          m_major = result;
        }
        else {
          Debug.LogWarning("Error while converting string to int in version major");
        }
      }
    }

    public static int Minor {
      get {
        return m_minor;
      }
      set {
        string version = Application.version;
        string[] prefixes = version.Split(".");
        if (int.TryParse(prefixes[1], out int result)) {
          m_minor = result;
        }
        else {
          Debug.LogWarning("Error while converting string to int in version minor");
        }
      }
    }

    public static int Patch {
      get {
        return m_patch;
      }
      set {
        string version = Application.version;
        string[] prefixes = version.Split(".");
        if (int.TryParse(prefixes[2], out int result)) {
          m_patch = result;
        }
        else {
          Debug.LogWarning("Error while converting string to int in version patch");
        }
      }
    }
  }
}
