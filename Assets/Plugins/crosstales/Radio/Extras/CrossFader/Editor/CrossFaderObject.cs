#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Crosstales.Radio.EditorIntegration
{
   /// <summary>Editor component for the "Hierarchy"-menu.</summary>
   public static class CrossFaderGameObject
   {
      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/CrossFader", false, EditorHelper.GO_ID + 10)]
      private static void AddCrossFader()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath($"Assets{EditorConfig.ASSET_PATH}Extras/CrossFader/Resources/Prefabs/CrossFader.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }
   }
}
#endif
// © 2021 crosstales LLC (https://www.crosstales.com)