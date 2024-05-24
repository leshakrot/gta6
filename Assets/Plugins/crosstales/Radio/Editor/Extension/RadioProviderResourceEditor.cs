#if UNITY_EDITOR
using UnityEditor;
using Crosstales.Radio.EditorUtil;

namespace Crosstales.Radio.EditorExtension
{
   /// <summary>Custom editor for the 'RadioProviderResource'-class.</summary>
   [CustomEditor(typeof(Provider.RadioProviderResource))]
   public class RadioProviderResourceEditor : BaseRadioProviderEditor
   {
      #region Variables

      private Provider.RadioProviderResource _script;

      #endregion


      #region Editor methods

      public override void OnEnable()
      {
         base.OnEnable();
         _script = (Provider.RadioProviderResource)target;
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