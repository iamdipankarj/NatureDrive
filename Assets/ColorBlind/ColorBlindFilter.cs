using UnityEngine;

namespace Solace {
  [ExecuteInEditMode]
  public class ColorBlindFilter : MonoBehaviour
  {
      public ColorBlindMode mode = ColorBlindMode.Normal;
      private ColorBlindMode previousMode = ColorBlindMode.Normal;

      public bool showDifference = false;

      private Material material;

      void Awake()
      {
          material = new Material(Shader.Find("Hidden/ChannelMixer"));
          material.SetColor("_R", ColorBlindHelper.RGB[0, 0]);
          material.SetColor("_G", ColorBlindHelper.RGB[0, 1]);
          material.SetColor("_B", ColorBlindHelper.RGB[0, 2]);
      }

      void OnRenderImage(RenderTexture source, RenderTexture destination)
      {
          // No effect
          if (mode == ColorBlindMode.Normal)
          {
              Graphics.Blit(source, destination);
              return;
          }

          // Change effect
          if (mode != previousMode)
          {
              material.SetColor("_R", ColorBlindHelper.RGB[(int)mode, 0]);
              material.SetColor("_G", ColorBlindHelper.RGB[(int)mode, 1]);
              material.SetColor("_B", ColorBlindHelper.RGB[(int)mode, 2]);
              previousMode = mode;
          }

          // Apply effect
          Graphics.Blit(source, destination, material, showDifference ? 1 : 0);
      }
  }
}