using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NWH.VehiclePhysics2.Effects {
  /// <summary>
  ///     Main class for handling visual effects such as skidmarks, lights and exhausts.
  /// </summary>
  [Serializable]
  public partial class EffectManager : ManagerVehicleComponent {
    public ExhaustFlash exhaustFlash = new();
    public ExhaustSmoke exhaustSmoke = new();
    [FormerlySerializedAs("lights")] public LightsMananger lightsManager = new();
    [FormerlySerializedAs("skidmarks")] public SkidmarkManager skidmarkManager = new();
    public SurfaceParticleManager surfaceParticleManager = new();

    protected override void FillComponentList() {
      _components = new List<VehicleComponent>
                          {
                              exhaustFlash,
                              exhaustSmoke,
                              lightsManager,
                              skidmarkManager,
                              surfaceParticleManager,
                          };
    }
  }
}


#if UNITY_EDITOR
namespace NWH.VehiclePhysics2.Effects {
  [CustomPropertyDrawer(typeof(EffectManager))]
  public partial class EffectManagerDrawer : ComponentNUIPropertyDrawer {
    public override bool OnNUI(Rect position, SerializedProperty property, GUIContent label) {
      if (!base.OnNUI(position, property, label)) {
        return false;
      }


      int effectsTab = drawer.HorizontalToolbar("effectsTab",
                                                new[]
                                                {
                                                          "Skidmarks", "Lights", "Surf. Part.", "Ex. Smoke",
                                                          "Ex. Flash",
                                                });

      switch (effectsTab) {
        case 0:
          drawer.Property("skidmarkManager");
          break;
        case 1:
          drawer.Property("lightsManager");
          break;
        case 2:
          drawer.Property("surfaceParticleManager");
          break;
        case 3:
          drawer.Property("exhaustSmoke");
          break;
        case 4:
          drawer.Property("exhaustFlash");
          break;
        default:
          drawer.Property("skidmarks");
          break;
      }


      drawer.EndProperty();
      return true;
    }
  }
}

#endif
