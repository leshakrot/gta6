#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Crosstales.Radio.EditorIntegration
{
   /// <summary>Editor component for the "Hierarchy"-menu.</summary>
   public static class StreamSaverGameObject
   {
      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/StreamSaver", false, EditorHelper.GO_ID + 9)]
      private static void AddStreamSaver()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath($"Assets{EditorConfig.ASSET_PATH}Extras/StreamSaver/Resources/Prefabs/StreamSaver.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }
   }
}
#endif
// © 2021 crosstales LLC (https://www.crosstales.com)