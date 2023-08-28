using UnityEngine;

namespace Solace {
  public class GroundMaterialManager : MonoBehaviour {
    public GroundMaterial[] groundMaterials = new GroundMaterial[0];


#if UNITY_EDITOR
    // Editor-only code for initializing the elements in the groundMaterials array from
    // a zero-sized array. Otherwise, they would be initialized to all-zero.

    bool m_firstDeserialization = true;
    int m_materialsLength = 0;

    void OnValidate() {
      if (m_firstDeserialization) {
        m_materialsLength = groundMaterials.Length;
        m_firstDeserialization = false;
      }
      else {
        if (groundMaterials.Length != m_materialsLength) {
          if (m_materialsLength == 0) {
            for (int i = 0; i < groundMaterials.Length; i++)
              groundMaterials[i] = new GroundMaterial();
          }

          m_materialsLength = groundMaterials.Length;
        }
      }
    }
#endif

    public GroundMaterial GetGroundMaterial(PhysicMaterial physicMaterial) {
      for (int i = 0, c = groundMaterials.Length; i < c; i++) {
        if (groundMaterials[i].physicMaterial == physicMaterial)
          return groundMaterials[i];
      }

      return null;
    }
  }
}
