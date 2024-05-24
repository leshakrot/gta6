#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Crosstales.Radio.OnRadio.EditorExtension
{
   /// <summary>Custom editor for the 'BaseService'-class.</summary>
   [CustomEditor(typeof(Service.BaseService))]
   public abstract class BaseServiceEditor : Editor
   {
      #region Variables

      private Service.BaseService _script;

      private bool showStations;
      private bool showRecords;

      #endregion


      #region Editor methods

      public void OnEnable()
      {
         _script = (Service.BaseService)target;
      }

      public override bool RequiresConstantRepaint()
      {
         return true;
      }

      public override void OnInspectorGUI()
      {
         EditorUtil.EditorHelper.BannerOC();

         if (GUILayout.Button(new GUIContent(" Learn more", EditorUtil.EditorHelper.Icon_Manual, "Learn more about OnRadio.")))
            Crosstales.Radio.Util.Helper.OpenURL(Util.Constants.ONRADIO_URL);

         DrawDefaultInspector();

         EditorUtil.EditorHelper.SeparatorUI();

         if (_script.isActiveAndEnabled)
         {
            if (_script.isValidToken)
            {
               GUILayout.Label("Data", EditorStyles.boldLabel);

               GUILayout.Space(6);

               showStations = EditorGUILayout.Foldout(showStations, "Stations (" + _script.Stations.Count + ")");
               if (showStations)
               {
                  EditorGUI.indentLevel++;

                  foreach (OnRadio.Model.RadioStationExt station in _script.Stations)
                  {
                     EditorGUILayout.SelectableLabel(station.ToShortString(), GUILayout.Height(16), GUILayout.ExpandHeight(false));
                  }

                  EditorGUI.indentLevel--;
               }

               showRecords = EditorGUILayout.Foldout(showRecords, "Records (" + _script.Records.Count + ")");
               if (showRecords)
               {
                  EditorGUI.indentLevel++;

                  foreach (OnRadio.Model.RecordInfoExt record in _script.Records)
                  {
                     EditorGUILayout.SelectableLabel(record.ToShortString(), GUILayout.Height(16), GUILayout.ExpandHeight(false));
                  }

                  EditorGUI.indentLevel--;
               }

               EditorUtil.EditorHelper.SeparatorUI();

               GUILayout.Label("Stats:", EditorStyles.boldLabel);

               GUILayout.Label("Playlist Requests:\t\t" + Service.BaseService.TotalPlaylistRequests);
               GUILayout.Label("Reco2 Requests:\t\t" + Service.BaseService.TotalReco2Requests);
               GUILayout.Label("Topsongs Requests:\t\t" + Service.BaseService.TotalTopsongsRequests);
               GUILayout.Label("Station Requests:\t\t" + Service.BaseService.TotalStationRequests);
               GUILayout.Label("SongArt Requests:\t\t" + Service.BaseService.TotalSongArtRequests);
               GUILayout.Label("DARStation Requests:\t\t" + Service.BaseService.TotalDARStationRequests);
               GUILayout.Space(6);
               GUILayout.Label("Total Requests:\t\t" + Service.BaseService.TotalRequests);
            }
            else
            {
               EditorGUILayout.HelpBox("Please add a valid 'Token' to access OnRadio!", MessageType.Warning);
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