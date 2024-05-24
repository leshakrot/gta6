﻿#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Crosstales.Radio.EditorIntegration
{
   /// <summary>Editor component for the "Tools"-menu.</summary>
   public static class LoudspeakerMenu
   {
      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/Loudspeaker", false, EditorHelper.MENU_ID + 150)]
      private static void AddLoudspeaker()
      {
         PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath($"Assets{EditorConfig.ASSET_PATH}Extras/Loudspeaker/Resources/Prefabs/Loudspeaker.prefab", typeof(GameObject)));
         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      }
   }
}
#endif
// © 2021 crosstales LLC (https://www.crosstales.com)