#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Crosstales.Radio.Demo
{
   /// <summary>Installs the 'UI'-package from Common amd OnRadio.</summary>
   [InitializeOnLoad]
   public static class ZInstaller
   {
      #region Constructor

      static ZInstaller()
      {
#if !CT_UI && !CT_DEVELOP
         string pathInstaller = $"{Application.dataPath}/Plugins/crosstales/Common/Extras/";

         try
         {
            string package = $"{pathInstaller}UI.unitypackage";

            if (System.IO.File.Exists(package))
            {
               AssetDatabase.ImportPackage(package, false);

               Crosstales.Common.EditorTask.BaseCompileDefines.AddSymbolsToAllTargets("CT_UI");
            }
            else
            {
               Debug.LogWarning($"Package not found: {package}");
            }
         }
         catch (System.Exception ex)
         {
            Debug.LogError($"Could not import the 'UI'-package: {ex}");
         }
#endif

         installOnRadio();
      }

      private static void installOnRadio()
      {
#if !CT_RADIO_ONRADIO && !CT_DEVELOP
         string pathInstaller = Application.dataPath + EditorUtil.EditorConfig.ASSET_PATH + "3rd party/";

         try
         {
            string package = pathInstaller + "OnRadio.unitypackage";

            if (System.IO.File.Exists(package))
            {
               AssetDatabase.ImportPackage(package, false);

               Crosstales.Common.EditorTask.BaseCompileDefines.AddSymbolsToAllTargets("CT_RADIO_ONRADIO");
            }
            else
            {
               Debug.LogWarning("Package not found: " + package);
            }
         }
         catch (System.Exception ex)
         {
            Debug.LogError("Could not import the 'OnRadio'-package: " + ex);
         }
#endif
      }

      #endregion
   }
}
#endif
// © 2020-2021 crosstales LLC (https://www.crosstales.com)