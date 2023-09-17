#if UNITY_EDITOR
using UnityEditor;

namespace NSVehicle {
  [CustomEditor(typeof(DragObject))]
  [CanEditMultipleObjects]
  public class DragObjectEditor : NUIEditor {
    public override bool OnInspectorNUI() {
      if (!base.OnInspectorNUI()) {
        return false;
      }

      drawer.EndEditor(this);
      return true;
    }


    public override bool UseDefaultMargins() {
      return false;
    }
  }
}

#endif