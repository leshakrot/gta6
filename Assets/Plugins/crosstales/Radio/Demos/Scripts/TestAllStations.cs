#if CT_RADIO_ONRADIO
using UnityEngine;
using System.Collections;

namespace Crosstales.Radio.Demo
{
   /// <summary>Test all stations of a given RadioManager.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_test_all_stations.html")]
   public class TestAllStations : MonoBehaviour
   {
      #region Variables

      public BasePlayer Player;
      public Provider.BaseRadioProvider Provider;
      public OnRadio.Service.BaseService Service;

      public string ErrorFilePath;

      public Model.Enum.AudioCodec Codec = Model.Enum.AudioCodec.MP3_NLayer;

      public bool UseService = true;
      public bool UpdateInfo = true;
      public bool Silent = true;

      private bool stopped = true;
      private bool error;
      private int errorCount;

      private bool updateInfoComplete = true;

      private System.Collections.Generic.List<string> data = new System.Collections.Generic.List<string>();

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         Player.OnAudioStart += onAudioStart;
         Player.OnAudioEnd += onAudioEnd;
         Player.OnErrorInfo += onErrorInfo;

         Provider.OnProviderReady += onProviderReady;

         if (UseService)
            Service.OnQueryComplete += onQueryComplete;

         /*
         if (Player != null)
         {
            Player.isLegacyMode = true;
         }
         */

         Player.Volume = Silent ? 0f : 1f;

         Util.Constants.DEFAULT_CODEC_MP3_WINDOWS = Codec;
      }

      public void OnDestroy()
      {
         Player.OnAudioStart -= onAudioStart;
         Player.OnAudioEnd -= onAudioEnd;
         Player.OnErrorInfo -= onErrorInfo;
         Provider.OnProviderReady -= onProviderReady;

         if (UseService)
            Service.OnQueryComplete -= onQueryComplete;

#if (!UNITY_WSA && !UNITY_WEBGL && !UNITY_XBOXONE) || UNITY_EDITOR
         if (data.Count > 0)
         {
            try
            {
               System.IO.File.WriteAllLines(ErrorFilePath, data.ToArray());
            }
            catch (System.Exception ex)
            {
               Debug.LogError("Could not write log entries to file: " + ex, this);
            }
         }
#endif
      }

      #endregion


      #region Private methods

      private IEnumerator verify()
      {
         WaitForSeconds waitPlay = new WaitForSeconds(2f);
         WaitForSeconds waitCleanup = new WaitForSeconds(1f);

         if (Provider != null)
         {
            Debug.Log("+++ Verification of " + Provider.Stations.Count + " stations started +++", this);

            for (int ii = 0; ii < Provider.Stations.Count; ii++)
            {
               Player.Station = Provider.Stations[ii];

               Debug.Log("Verifying station " + (ii + 1) + ":  " + Player.Station, this);

               if (UseService)
               {
                  updateInfoComplete = false;
                  Service.StationService(Provider.Stations[ii]);
                  //Service.StationService(Provider.Stations[ii]);

                  do
                  {
                     yield return null;
                  } while (!updateInfoComplete);

                  updateInfoComplete = false;
                  Service.DARStationService(Provider.Stations[ii], false);
                  //Service.StationService(Provider.Stations[ii]);

                  do
                  {
                     yield return null;
                  } while (!updateInfoComplete);
               }

               int bitrate = Player.Station.Bitrate;

               Player.Play();

               while (stopped && !error)
                  yield return null;

               yield return waitPlay; //give it time to play

               Player.Stop();

               while (!stopped && !error)
                  yield return null;

               if (bitrate != Player.Station.Bitrate)
               {
                  string msg = "Warning: Bitrate has changed from " + bitrate + " to " + Player.Station.Bitrate + " for station: " + Player.Station;

                  Debug.LogWarning(msg, this);
                  data.Add(msg);
               }

               if (UpdateInfo)
                  Provider.Stations[ii] = Player.Station;

               yield return waitCleanup; //give it time to cleanup

               error = false;
            }

            Debug.Log("+++ Verification ended: " + errorCount + "/" + Provider.Stations.Count + " +++", this);

            if (errorCount > 0)
               Debug.LogError("Stations with errors found!", this);
         }
         else
         {
            Debug.LogWarning("'Manager' is null!", this);
         }
      }

      #endregion


      #region Callback methods

      private void onAudioStart(Model.RadioStation station)
      {
         stopped = false;
      }

      private void onAudioEnd(Model.RadioStation station)
      {
         stopped = true;
      }

      private void onErrorInfo(Model.RadioStation station, string info)
      {
         string msg = "Error: " + station;

         Debug.LogWarning(msg, this);
         data.Add(msg);

         stopped = true;
         error = true;
         errorCount++;
      }

      private void onProviderReady()
      {
         StartCoroutine(verify());
      }

      private void onQueryComplete(string id)
      {
         updateInfoComplete = true;
      }

      #endregion
   }
}
#endif
// © 2017-2021 crosstales LLC (https://www.crosstales.com)