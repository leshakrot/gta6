using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.Demo
{
   /// <summary>GUI for a very simple radio player.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_g_u_i_play_radio.html")]
   public class GUIPlayRadio : MonoBehaviour
   {
      #region Variables

      /// <summary>'SimplePlayer' from the scene.</summary>
      [Header("Settings")] [Tooltip("'SimplePlayer' from the scene.")] public SimplePlayer Player;

      /// <summary>The color for the Play-mode.</summary>
      [Tooltip("The color for the Play-mode.")] public Color32 PlayColor = new Color32(0, 255, 0, 64);

/*
        /// <summary>How many times should the radio station restart after an error before giving up (default: 3).</summary>
        [Tooltip("How many times should the radio station restart after an error before giving up (default: 3).")]
        public int Retries = 3;
*/
      [Header("UI Objects")] public GameObject PlayButton;
      public GameObject StopButton;
      public Image MainImage;
      public Text Station;
      public Text ElapsedTime;
      public Text ErrorText;
      public Text ElapsedRecordTime;
      public Text RecordTitle;
      public Text RecordArtist;
      public Text DownloadSizeStation;
      public Text ElapsedStationTime;
      public Text NextRecordTitle;
      public Text NextRecordArtist;
      public Text NextRecordDelay;

      private Color32 color;
      private int playtime;
      private int recordPlaytime;

      private Model.RecordInfo currentRecord;
      private Model.RecordInfo nextRecord;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         if (Player != null && Player.Player != null)
         {
            Player.OnPlaybackStart += onPlaybackStart;
            Player.OnPlaybackEnd += onPlaybackEnd;
            Player.OnAudioPlayTimeUpdate += onAudioPlayTimeUpdate;
            Player.OnBufferingProgressUpdate += onBufferingProgressUpdate;
            Player.OnErrorInfo += onErrorInfo;
            Player.OnRecordChange += onRecordChange;
            Player.OnRecordPlayTimeUpdate += onRecordPlayTimeUpdate;
            Player.OnNextRecordChange += onNextRecordChange;
            Player.OnNextRecordDelayUpdate += onNextRecordDelayUpdate;

            if (ErrorText != null)
               ErrorText.text = string.Empty;
         }
         else
         {
            const string msg = "'Player' is null!";

            if (ErrorText != null)
               ErrorText.text = msg;

            Debug.LogError(msg, this);
         }

         if (ElapsedTime != null)
            ElapsedTime.text = Util.Constants.TEXT_STOPPED;

         if (Station != null)
            Station.text = Util.Constants.TEXT_QUESTIONMARKS;

         if (MainImage != null)
            color = MainImage.color;

         if (Player != null)
         {
            PlayButton.SetActive(!Player.isPlayback);
            StopButton.SetActive(Player.isPlayback);

            if (Player.isPlayback && MainImage != null)
               MainImage.color = PlayColor;
         }

         onPlaybackEnd(null); //initialize GUI-components
      }

      private void Update()
      {
         //if (Time.frameCount % 30 == 0 && Player != null && Player.Station != null && Player.isPlayback)
         if (Time.frameCount % 30 == 0 && Player.isPlayback)
         {
            //if (Station != null)
            Station.text = Player.Station.Name;

            if (nextRecord?.Equals(currentRecord) == true)
            {
               if (NextRecordTitle != null)
                  NextRecordTitle.text = string.Empty;

               if (NextRecordArtist != null)
                  NextRecordArtist.text = string.Empty;
            }
         }
      }

      private void OnDestroy()
      {
         if (Player != null)
         {
            // Unsubscribe event listeners
            Player.OnPlaybackStart -= onPlaybackStart;
            Player.OnPlaybackEnd -= onPlaybackEnd;
            Player.OnAudioPlayTimeUpdate -= onAudioPlayTimeUpdate;
            Player.OnBufferingProgressUpdate -= onBufferingProgressUpdate;
            Player.OnErrorInfo -= onErrorInfo;
            Player.OnRecordChange -= onRecordChange;
            Player.OnRecordPlayTimeUpdate -= onRecordPlayTimeUpdate;
            Player.OnNextRecordChange -= onNextRecordChange;
            Player.OnNextRecordDelayUpdate -= onNextRecordDelayUpdate;
         }
      }

      #endregion


      #region Public methods

      public void OpenUrl()
      {
         if (Player != null && Player.Player != null)
            Util.Helper.OpenURL(Player.Player.Station.Station);
      }

      public void OpenSpotifyUrl()
      {
         if (Player != null && Player.Player != null)
            Application.OpenURL(Player.Player.RecordInfo.SpotifyUrl);
      }

      #endregion


      #region Callback methods

      private void onPlaybackStart(Model.RadioStation station)
      {
         if (ErrorText != null)
            ErrorText.text = string.Empty;

         if (PlayButton != null)
            PlayButton.SetActive(false);

         if (StopButton != null)
            StopButton.SetActive(true);

         if (MainImage != null)
            MainImage.color = PlayColor;

         if (Player.Station != null && Station != null)
            Station.text = Player.Station.Name;
      }

      private void onPlaybackEnd(Model.RadioStation station)
      {
         if (ElapsedTime != null)
            ElapsedTime.text = Util.Constants.TEXT_STOPPED;

         if (ElapsedRecordTime != null)
            ElapsedRecordTime.text = Util.Helper.FormatSecondsToHourMinSec(0f);

         if (ElapsedStationTime != null)
            ElapsedStationTime.text = Util.Helper.FormatSecondsToHourMinSec(0f);

         if (DownloadSizeStation != null)
            DownloadSizeStation.text = Util.Helper.FormatBytesToHRF(0);

         if (RecordTitle != null)
            RecordTitle.text = string.Empty;

         if (RecordArtist != null)
            RecordArtist.text = string.Empty;

         if (NextRecordTitle != null)
            NextRecordTitle.text = string.Empty;

         if (NextRecordArtist != null)
            NextRecordArtist.text = string.Empty;

         if (NextRecordDelay != null)
            NextRecordDelay.text = string.Empty;

         if (PlayButton != null)
            PlayButton.SetActive(true);

         if (StopButton != null)
            StopButton.SetActive(false);

         if (MainImage != null)
            MainImage.color = color;

         if (ElapsedTime != null)
            ElapsedTime.text = Util.Constants.TEXT_STOPPED;

         if (Station != null)
            Station.text = Util.Constants.TEXT_QUESTIONMARKS;
      }

      private void onAudioPlayTimeUpdate(Model.RadioStation station, float _playtime)
      {
         if ((int)_playtime != playtime)
         {
            if (ElapsedTime != null)
               ElapsedTime.text = Util.Helper.FormatSecondsToHourMinSec(_playtime);

            if (DownloadSizeStation != null)
               DownloadSizeStation.text = Util.Helper.FormatBytesToHRF(station.TotalDataSize);

            if (ElapsedStationTime != null)
               ElapsedStationTime.text = Util.Helper.FormatSecondsToHourMinSec(station.TotalPlayTime);

            playtime = (int)_playtime;
         }
      }

      private void onBufferingProgressUpdate(Model.RadioStation station, float progress)
      {
         if (ElapsedTime != null)
            ElapsedTime.text = Util.Constants.TEXT_BUFFER + progress.ToString(Util.Constants.FORMAT_PERCENT);
      }

      private void onErrorInfo(Model.RadioStation station, string info)
      {
         if (ErrorText != null)
            ErrorText.text = info;
      }

      private void onRecordChange(Model.RadioStation station, Model.RecordInfo record)
      {
         currentRecord = record;

         if (RecordTitle != null)
            RecordTitle.text = record.Title;

         if (RecordArtist != null)
            RecordArtist.text = record.Artist;

         if (NextRecordDelay != null)
            NextRecordDelay.text = string.Empty;
      }

      private void onRecordPlayTimeUpdate(Model.RadioStation station, Model.RecordInfo record, float _playtime)
      {
         if ((int)_playtime != recordPlaytime)
         {
            recordPlaytime = (int)_playtime;

            if (ElapsedRecordTime != null)
               ElapsedRecordTime.text = Util.Helper.FormatSecondsToHourMinSec(_playtime);
         }
      }

      private void onNextRecordChange(Model.RadioStation station, Model.RecordInfo record, float delay)
      {
         nextRecord = record;

         if (NextRecordTitle != null)
            NextRecordTitle.text = record.Title;

         if (NextRecordArtist != null)
            NextRecordArtist.text = record.Artist;
      }

      private void onNextRecordDelayUpdate(Model.RadioStation station, Model.RecordInfo record, float delay)
      {
         if (NextRecordDelay != null)
            NextRecordDelay.text = delay.ToString("#0.0");
      }

      #endregion
   }
}
// © 2015-2021 crosstales LLC (https://www.crosstales.com)