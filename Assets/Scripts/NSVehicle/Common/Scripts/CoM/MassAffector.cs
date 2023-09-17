using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NSVehicle {
  public class MassAffector : MonoBehaviour, IMassAffector {
    public float mass = 100.0f;

    public float GetMass() {
      return mass;
    }

    public Transform GetTransform() {
      return transform;
    }

    public Vector3 GetWorldCenterOfMass() {
      return transform.position;
    }
  }
}

#if UNITY_EDITOR
namespace NSVehicle {
  [CustomEditor(typeof(MassAffector))]
  [CanEditMultipleObjects]
  public class MassAffectorEditor : NUIEditor {
    public override bool OnInspectorNUI() {
      if (!base.OnInspectorNUI()) {
        return false;
      }

      drawer.Field("mass", true, "kg");

      drawer.EndEditor(this);
      return true;
    }
  }
}
#endif
