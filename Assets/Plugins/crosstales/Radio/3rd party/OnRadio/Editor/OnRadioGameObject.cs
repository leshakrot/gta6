#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Crosstales.Radio.OnRadio.EditorExtension
{
   /// <summary>Editor component for for adding the prefabs from 'OnRadio' in the "Hierarchy"-menu.</summary>
   public static class OnRadioGameObject
   {
      [MenuItem("GameObject/" + Crosstales.Radio.Util.Constants.ASSET_NAME + "/3rd party/OnRadio/PlaylistService", false, EditorUtil.EditorHelper.GO_ID + 20)]
      private static void AddPlaylistService()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets" + EditorUtil.EditorConfig.ASSET_PATH + "3rd party/OnRadio/Prefabs/PlaylistService.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }

      [MenuItem("GameObject/" + Crosstales.Radio.Util.Constants.ASSET_NAME + "/3rd party/OnRadio/Reco2Service", false, EditorUtil.EditorHelper.GO_ID + 21)]
      private static void AddReco2Service()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets" + EditorUtil.EditorConfig.ASSET_PATH + "3rd party/OnRadio/Prefabs/Reco2Service.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }

      [MenuItem("GameObject/" + Crosstales.Radio.Util.Constants.ASSET_NAME + "/3rd party/OnRadio/TopsongsService", false, EditorUtil.EditorHelper.GO_ID + 22)]
      private static void AddTopsongsService()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets" + EditorUtil.EditorConfig.ASSET_PATH + "3rd party/OnRadio/Prefabs/TopsongsService.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }

      [MenuItem("GameObject/" + Crosstales.Radio.Util.Constants.ASSET_NAME + "/3rd party/OnRadio/RadioProviderOnRadio", false, EditorUtil.EditorHelper.GO_ID + 23)]
      private static void AddRadioProviderOnRadio()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets" + EditorUtil.EditorConfig.ASSET_PATH + "3rd party/OnRadio/Prefabs/RadioProviderOnRadio.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }
   }
}
#endif
// © 2020-2021 crosstales LLC (https://www.crosstales.com)