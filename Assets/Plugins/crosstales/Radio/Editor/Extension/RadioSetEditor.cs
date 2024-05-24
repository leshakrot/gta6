using System.Linq;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Crosstales.Radio.EditorUtil;

namespace Crosstales.Radio.EditorExtension
{
   /// <summary>Custom editor for the 'RadioSet'-class.</summary>
   //[InitializeOnLoad]
   [CustomEditor(typeof(Set.RadioSet))]
   public class RadioSetEditor : Editor
   {
      #region Variables

      private Set.RadioSet script;

      private bool showStations;

      #endregion


      #region Editor methods

      public void OnEnable()
      {
         script = (Set.RadioSet)target;
      }

      public override bool RequiresConstantRepaint()
      {
         return true;
      }

      public override void OnInspectorGUI()
      {
         DrawDefaultInspector();

         EditorHelper.SeparatorUI();

         if (script.isActiveAndEnabled)
         {
            if (script.Providers != null && script.Providers.Length > 0)
            {
               GUILayout.Label("Data", EditorStyles.boldLabel);

               GUILayout.Label("Ready:\t" + (script.isReady ? "Yes" : "No"));

               GUILayout.Space(6);

               showStations = EditorGUILayout.Foldout(showStations, "Stations (" + script.CountStations() + "/" + script.Stations.Count + ")");
               if (showStations)
               {
                  EditorGUI.indentLevel++;

                  foreach (Model.RadioStation station in script.StationsByName())
                  {
                     EditorGUILayout.SelectableLabel(station.ToShortString(), GUILayout.Height(16), GUILayout.ExpandHeight(false));
                  }

                  EditorGUI.indentLevel--;
               }

               GUILayout.Space(8);

               if (Util.Helper.isEditorMode)
               {
                  if (GUILayout.Button(new GUIContent(" Load", EditorHelper.Icon_Refresh, "Loads all radio stations from the given set.")))
                  {
                     if (script.Providers != null)
                     {
                        foreach (Provider.BaseRadioProvider _rp in script.Providers.Where(_rp => _rp != null && _rp.isActiveAndEnabled))
                        {
                           _rp.Load();
                        }

                        script.Load();
                     }
                     else
                     {
                        Debug.LogWarning("'Providers' is null - please add at least one provider in the Inspector!");
                     }
                  }

                  GUI.enabled = script.Stations != null && script.Stations.Count > 0;

                  GUILayout.BeginHorizontal();
                  {
                     if (GUILayout.Button(new GUIContent(" Save TXT", EditorHelper.Icon_Save, "Saves all loaded radio stations as a text-file with streams.")))
                     {
                        string path = EditorUtility.SaveFilePanel("Save radio stations as text-file", string.Empty, "Radio.txt", "txt");
                        if (!string.IsNullOrEmpty(path))
                        {
                           script.Save(path, script.Filter);
                        }
                     }

                     if (GUILayout.Button(new GUIContent(" Save M3U", EditorHelper.Icon_Save, "Saves the list of all loaded radio stations as an M3U-file.")))
                     {
                        string path = EditorUtility.SaveFilePanel("Save radio stations as M3U-file", string.Empty, "Radio.m3u", "m3u");
                        if (!string.IsNullOrEmpty(path))
                        {
                           Util.Helper.SaveAsM3U(path, script.GetStations(false, script.Filter));
                        }
                     }

                     if (GUILayout.Button(new GUIContent(" Save PLS", EditorHelper.Icon_Save, "Saves the list of all loaded radio stations as a PLS-file.")))
                     {
                        string path = EditorUtility.SaveFilePanel("Save radio stations as PLS-file", string.Empty, "Radio.pls", "pls");
                        if (!string.IsNullOrEmpty(path))
                        {
                           Util.Helper.SaveAsPLS(path, script.GetStations(false, script.Filter));
                        }
                     }

                     if (GUILayout.Button(new GUIContent(" Save XSPF", EditorHelper.Icon_Save, "Saves the list of all loaded radio stations as a XSPF-file.")))
                     {
                        string path = EditorUtility.SaveFilePanel("Save radio stations as XSPF-file", string.Empty, "Radio.xspf", "xspf");
                        if (!string.IsNullOrEmpty(path))
                        {
                           Util.Helper.SaveAsXSPF(path, script.GetStations(false, script.Filter));
                        }
                     }
                  }

                  GUILayout.EndHorizontal();

                  GUI.enabled = true;
               }
            }
            else
            {
               EditorGUILayout.HelpBox("Please add 'Providers'!", MessageType.Warning);
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