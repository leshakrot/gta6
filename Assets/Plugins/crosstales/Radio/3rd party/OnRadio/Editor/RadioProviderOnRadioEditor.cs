#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Crosstales.Radio.OnRadio.EditorExtension
{
   /// <summary>Custom editor for the 'RadioProviderOnRadio'-class.</summary>
   [CustomEditor(typeof(Provider.RadioProviderOnRadio))]
   public class RadioProviderOnRadioEditor : Editor
   {
      #region Variables

      private Provider.RadioProviderOnRadio _script;

      private bool showStations;

      #endregion


      #region Editor methods

      public void OnEnable()
      {
         _script = (Provider.RadioProviderOnRadio)target;
      }

      public override bool RequiresConstantRepaint()
      {
         return true;
      }

      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();

         EditorUtil.EditorHelper.SeparatorUI();

         if (_script.isActiveAndEnabled)
         {
            if (_script.Services != null && _script.Services.Length > 0)
            {
               GUILayout.Label("Data", EditorStyles.boldLabel);

               GUILayout.Label("Ready:\t" + (_script.isReady ? "Yes" : "No"));

               GUILayout.Space(6);

               showStations = EditorGUILayout.Foldout(showStations, "Stations (" + _script.Stations.Count + ")");
               if (showStations)
               {
                  EditorGUI.indentLevel++;

                  foreach (Radio.Model.RadioStation station in _script.Stations)
                  {
                     EditorGUILayout.SelectableLabel(station.ToShortString(), GUILayout.Height(16), GUILayout.ExpandHeight(false));
                  }

                  EditorGUI.indentLevel--;
               }
            }
            else
            {
               EditorGUILayout.HelpBox("Please add a 'Service'!", MessageType.Warning);
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
// © 2020-2021 crosstales LLC (https://www.crosstales.com)