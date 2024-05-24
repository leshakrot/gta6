#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;

namespace Crosstales.Radio.EditorIntegration
{
   /// <summary>Editor component for the "Tools"-menu.</summary>
   public static class RadioMenu
   {
      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/RadioPlayer", false, EditorHelper.MENU_ID + 20)]
      private static void AddRadioPlayer()
      {
         EditorHelper.InstantiatePrefab("RadioPlayer");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/RadioProviderResource", false, EditorHelper.MENU_ID + 40)]
      private static void AddRadioProviderResource()
      {
         EditorHelper.InstantiatePrefab("RadioProviderResource");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/RadioProviderShoutcast", false, EditorHelper.MENU_ID + 50)]
      private static void AddRadioProviderShoutcast()
      {
         EditorHelper.InstantiatePrefab("RadioProviderShoutcast");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/RadioProviderURL", false, EditorHelper.MENU_ID + 60)]
      private static void AddRadioProviderURL()
      {
         EditorHelper.InstantiatePrefab("RadioProviderURL");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/RadioProviderUser", false, EditorHelper.MENU_ID + 70)]
      private static void AddRadioProviderUser()
      {
         EditorHelper.InstantiatePrefab("RadioProviderUser");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/RadioSet", false, EditorHelper.MENU_ID + 90)]
      private static void AddRadioSet()
      {
         EditorHelper.InstantiatePrefab("RadioSet");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/RadioManager", false, EditorHelper.MENU_ID + 110)]
      private static void AddRadioManager()
      {
         EditorHelper.InstantiatePrefab("RadioManager");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/SimplePlayer", false, EditorHelper.MENU_ID + 130)]
      private static void AddSimplePlayer()
      {
         EditorHelper.InstantiatePrefab("SimplePlayer");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Manual", false, EditorHelper.MENU_ID + 600)]
      private static void ShowManual()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_MANUAL_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/API", false, EditorHelper.MENU_ID + 610)]
      private static void ShowAPI()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_API_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Forum", false, EditorHelper.MENU_ID + 620)]
      private static void ShowForum()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_FORUM_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Product", false, EditorHelper.MENU_ID + 630)]
      private static void ShowProduct()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_WEB_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Videos/Promo", false, EditorHelper.MENU_ID + 650)]
      private static void ShowVideoPromo()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_VIDEO_PROMO);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Videos/Tutorial", false, EditorHelper.MENU_ID + 660)]
      private static void ShowVideoTutorial()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_VIDEO_TUTORIAL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Videos/All Videos", false, EditorHelper.MENU_ID + 680)]
      private static void ShowAllVideos()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_SOCIAL_YOUTUBE);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/3rd Party Assets", false, EditorHelper.MENU_ID + 700)]
      private static void Show3rdPartyAV()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_3P_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/About/Unity AssetStore", false, EditorHelper.MENU_ID + 800)]
      private static void ShowUAS()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_CT_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/About/" + Util.Constants.ASSET_AUTHOR, false, EditorHelper.MENU_ID + 820)]
      private static void ShowCT()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_AUTHOR_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/About/Info", false, EditorHelper.MENU_ID + 840)]
      private static void ShowInfo()
      {
         EditorUtility.DisplayDialog(Util.Constants.ASSET_NAME + " - About",
            "Version: " + Util.Constants.ASSET_VERSION +
            System.Environment.NewLine +
            System.Environment.NewLine +
            "© 2015-2021 by " + Util.Constants.ASSET_AUTHOR +
            System.Environment.NewLine +
            System.Environment.NewLine +
            Util.Constants.ASSET_AUTHOR_URL +
            System.Environment.NewLine, "Ok");
      }
   }
}
#endif
// © 2015-2021 crosstales LLC (https://www.crosstales.com)