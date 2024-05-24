#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;

namespace Crosstales.Radio.EditorExtension
{
   /// <summary>Custom editor for the 'RadioProviderURL'-class.</summary>
   [CustomEditor(typeof(Provider.RadioProviderURL))]
   public class RadioProviderURLEditor : BaseRadioProviderEditor
   {
      #region Variables

      private Provider.RadioProviderURL _script;

      #endregion


      #region Editor methods

      public override void OnEnable()
      {
         base.OnEnable();
         _script = (Provider.RadioProviderURL)target;
      }

      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();

         EditorHelper.SeparatorUI();

         if (_script.isActiveAndEnabled)
         {
            if (_script.Entries?.Count > 0)
            {
               showData();
            }
            else
            {
               EditorGUILayout.HelpBox("Please add 'Entries'!", MessageType.Warning);
            }
         }
         else
         {
            EditorGUILayout.HelpBox("Script is disabled!", MessageType.Info);
         }
      }

      #endregion
   }
}
#endif
// © 2016-2021 crosstales LLC (https://www.crosstales.com)