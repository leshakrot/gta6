#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;

namespace Crosstales.Radio.EditorIntegration
{
   /// <summary>Editor component for the "Hierarchy"-menu.</summary>
   public static class RadioGameObject
   {
      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/RadioPlayer", false, EditorHelper.GO_ID)]
      private static void AddRadioPlayer()
      {
         EditorHelper.InstantiatePrefab("RadioPlayer");
      }

      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/RadioProviderResource", false, EditorHelper.GO_ID + 1)]
      private static void AddRadioProviderResource()
      {
         EditorHelper.InstantiatePrefab("RadioProviderResource");
      }

      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/RadioProviderShoutcast", false, EditorHelper.GO_ID + 2)]
      private static void AddRadioProviderShoutcast()
      {
         EditorHelper.InstantiatePrefab("RadioProviderShoutcast");
      }

      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/RadioProviderURL", false, EditorHelper.GO_ID + 3)]
      private static void AddRadioProviderURL()
      {
         EditorHelper.InstantiatePrefab("RadioProviderURL");
      }

      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/RadioProviderUser", false, EditorHelper.GO_ID + 4)]
      private static void AddRadioProviderUser()
      {
         EditorHelper.InstantiatePrefab("RadioProviderUser");
      }

      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/RadioSet", false, EditorHelper.GO_ID + 5)]
      private static void AddRadioSet()
      {
         EditorHelper.InstantiatePrefab("RadioSet");
      }

      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/RadioManager", false, EditorHelper.GO_ID + 6)]
      private static void AddRadioManager()
      {
         EditorHelper.InstantiatePrefab("RadioManager");
      }

      [MenuItem("GameObject/" + Util.Constants.ASSET_NAME + "/SimplePlayer", false, EditorHelper.GO_ID + 7)]
      private static void AddSimplePlayer()
      {
         EditorHelper.InstantiatePrefab("SimplePlayer");
      }
   }
}
#endif
// © 2017-2021 crosstales LLC (https://www.crosstales.com)