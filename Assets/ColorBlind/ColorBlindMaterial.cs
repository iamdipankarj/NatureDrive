using UnityEngine;

namespace Solace {
  [ExecuteInEditMode]
  public class ColorBlindMaterial : MonoBehaviour {
    public ColorBlindMode mode = ColorBlindMode.Deuteranomaly;
    private ColorBlindMode previousMode = ColorBlindMode.Normal;

    private Material material;
    private Material currentMaterial;

    void Awake() {
      material = new Material(Shader.Find("Hidden/ChannelMixer"));
      material.SetColor("_R", ColorBlindHelper.RGB[0, 0]);
      material.SetColor("_G", ColorBlindHelper.RGB[0, 1]);
      material.SetColor("_B", ColorBlindHelper.RGB[0, 2]);

    }

    void Start() {
      currentMaterial = GetComponent<MeshRenderer>().sharedMaterial;
      Debug.Log(currentMaterial);
    }

    void Update() {
      //Graphics.Blit(currentMaterial.mainTexture, material, 1);
      //currentMaterial.Set
      //currentMaterial.SetColor("_R", ColorBlindHelper.RGB[(int)mode, 0]);
      //currentMaterial.SetColor("_G", ColorBlindHelper.RGB[(int)mode, 1]);
      //currentMaterial.SetColor("_B", ColorBlindHelper.RGB[(int)mode, 2]);
    }
  }
}
