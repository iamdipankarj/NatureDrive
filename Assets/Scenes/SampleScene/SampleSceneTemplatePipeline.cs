using UnityEngine.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine;

namespace Solace {
  public class SampleSceneTemplatePipeline : ISceneTemplatePipeline {
    public virtual bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset) {
      return true;
    }

    public virtual void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName) {
      Debug.Log($"Creating {sceneName} create with custom template...");
    }

    public virtual void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, Scene scene, bool isAdditive, string sceneName) {
      Debug.Log($"{sceneName} create with custom template.");
    }
  }
}
