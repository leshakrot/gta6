#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Crosstales.Radio.EditorIntegration
{
   /// <summary>Editor component for the "Tools"-menu.</summary>
   public static class CrossFaderMenu
   {
      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/CrossFader", false, EditorHelper.MENU_ID + 170)]
      private static void AddCrossFader()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath($"Assets{EditorConfig.ASSET_PATH}Extras/CrossFader/Resources/Prefabs/CrossFader.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }
   }
}
#endif
// © 2021 crosstales LLC (https://www.crosstales.com)