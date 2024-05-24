using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.Demo
{
   /// <summary>GUI for a radio player.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_g_u_i_radio_static.html")]
   public class GUIRadioStatic : MonoBehaviour
   {
      #region Variables

      /// <summary>'RadioPlayer' from the scene.</summary>
      [Header("Settings")] [Tooltip("'RadioPlayer' from the scene.")] public RadioPlayer Player;

      /// <summary>The color for the Play-mode.</summary>
      [Tooltip("The color for the Play-mode.")] public Color32 PlayColor = new Color32(0, 255, 0, 64);

      /// <summary>How many times should the radio station restart after an error before giving up (default: 3).</summary>
      [Tooltip("How many times should the radio station restart after an error before giving up (default: 3).")]
      public int Retries = 3;

      [Header("UI Objects")] public Text Name;
      public Text Station;
      public Text Bitrate;
      public Text Genre;
      public InputField Rating;
      public Text SongTitle;

      public Text Elapsed;

      public GameObject PlayButton;
      public GameObject StopButton;
      public Image MainImage;


      [HideInInspector] public Color32 StopColor;

      //private Color32 color;
      private int invokeDelayCounter = 1;
      private bool isStopped = true;
      private int playtime;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         if (Player != null)
         {
            Player.OnPlaybackStart += onPlayBackStart;
            Player.OnPlaybackEnd += onPlaybackEnd;
            Player.OnAudioStart += onAudioStart;
            Player.OnAudioEnd += onAudioEnd;
            Player.OnAudioPlayTimeUpdate += onAudioPlayTime;
            Player.OnBufferingProgressUpdate += onBufferingProgress;
            Player.OnErrorInfo += onErrorInfo;

            // Fill fields from the Radio component
            Name.text = Player.Station.Name;
            Station.text = Util.Helper.CleanUrl(Player.Station.Station);
            Genre.text = Player.Station.Genres;
            Bitrate.text = Player.Station.Bitrate + " kbit/s";
            Rating.text = Player.Station.Rating.ToString(System.Globalization.CultureInfo.InvariantCulture);

            PlayButton.SetActive(!Player.isPlayback);
            StopButton.SetActive(Player.isPlayback);
            MainImage.color = Player.isPlayback ? PlayColor : StopColor;
         }
         else
         {
            Debug.LogError("'Player' is null!", this);
         }

         if (Elapsed != null)
            Elapsed.text = Util.Constants.TEXT_STOPPED;

         /*
         if (Elapsed != null)
             StopColor = MainImage.color;
             */
      }

      private void Update()
      {
         //if (SongTitle != null)
         if (Time.frameCount % 15 == 0)
         {
            SongTitle.text = Player.isPlayback ? Player.RecordInfo.StreamTitle : string.Empty;
         }
      }

      private void OnDestroy()
      {
         if (Player != null)
         {
            Player.OnPlaybackStart -= onPlayBackStart;
            Player.OnPlaybackEnd -= onPlaybackEnd;
            Player.OnAudioStart += onAudioStart;
            Player.OnAudioEnd += onAudioEnd;
            Player.OnAudioPlayTimeUpdate -= onAudioPlayTime;
            Player.OnBufferingProgressUpdate -= onBufferingProgress;
            Player.OnErrorInfo -= onErrorInfo;
         }
      }

      #endregion


      #region Public methods

      public void Play()
      {
         if (Player != null)
            Player.Play();
      }

      public void Stop()
      {
         if (Player != null)
            Player.Stop();
      }

      public void OpenUrl()
      {
         if (Player != null)
            Util.Helper.OpenURL(Player.Station.Station);
      }

      public void ChangeVolume(float volume)
      {
         if (Player != null && Player.Source != null)
            Player.Source.volume = volume;
      }

/*
      public void NameChanged(string name)
      {
         if (Player != null)
            Player.Station.Name = name;
      }

      public void StationChanged(string station)
      {
         if (Player != null)
            Player.Station.Station = station;
      }

      public void UrlChanged(string url)
      {
         if (Player != null)
            Player.Station.Url = url;
      }

      public void GenresChanged(string genres)
      {
         if (Player != null)
            Player.Station.Genres = genres;
      }

      public void BitrateChanged(string bitrateString)
      {
         if (Player != null)
         {
            int bitrate;
            if (int.TryParse(bitrateString, out bitrate))
            {
               Player.Station.Bitrate = Util.Helper.NearestBitrate(bitrate, Player.Station.Format);
            }

            if (Bitrate != null)
               Bitrate.text = Player.Station.Bitrate.ToString() + " kbit/s";
         }
      }
*/
      public void RatingChanged(string ratingString)
      {
         if (Player != null)
         {
            if (float.TryParse(ratingString, out float rating))
               Player.Station.Rating = Mathf.Clamp(rating, 0f, 5f);

            if (Rating != null)
               Rating.text = Player.Station.Rating.ToString(System.Globalization.CultureInfo.InvariantCulture);
         }
      }

      public void OpenSpotifyUrl()
      {
         if (Player != null)
            Application.OpenURL(Player.RecordInfo.SpotifyUrl);
      }

      #endregion


      #region Callback methods

      private void onPlayBackStart(Model.RadioStation station)
      {
         if (PlayButton != null)
            PlayButton.SetActive(false);

         if (StopButton != null)
            StopButton.SetActive(true);

         if (MainImage != null)
            MainImage.color = PlayColor;
      }

      private void onPlaybackEnd(Model.RadioStation station)
      {
         if (PlayButton != null)
            PlayButton.SetActive(true);

         if (StopButton != null)
            StopButton.SetActive(false);

         if (MainImage != null)
            MainImage.color = StopColor;

         if (Elapsed != null)
            Elapsed.text = Util.Constants.TEXT_STOPPED;
      }

      private void onAudioStart(Model.RadioStation station)
      {
         isStopped = false;
      }

      private void onAudioEnd(Model.RadioStation station)
      {
         isStopped = true;
      }

      private void onAudioPlayTime(Model.RadioStation station, float _playtime)
      {
         if ((int)_playtime != playtime)
         {
            if (Elapsed != null)
               Elapsed.text = Util.Helper.FormatSecondsToHourMinSec(_playtime);

            playtime = (int)_playtime;

            if (_playtime > 30f)
               invokeDelayCounter = 1;
         }
      }

      private void onBufferingProgress(Model.RadioStation station, float progress)
      {
         if (Elapsed != null)
            Elapsed.text = Util.Constants.TEXT_BUFFER + progress.ToString(Util.Constants.FORMAT_PERCENT);
      }

      private void onErrorInfo(Model.RadioStation station, string info)
      {
         Stop();
         onPlaybackEnd(station);

         if (!isStopped)
         {
            if (invokeDelayCounter < Retries)
            {
               Debug.LogWarning("Error occured -> Restarting station." + System.Environment.NewLine + info, this);

               Invoke(nameof(play), Util.Constants.INVOKE_DELAY * invokeDelayCounter);

               invokeDelayCounter++;
            }
            else
            {
               Debug.LogError("Restarting station failed more than " + Retries + " times - giving up!" + System.Environment.NewLine + info, this);
            }
         }
         else
         {
            Debug.LogError("Could not start the station '" + station.Name + "'! Please try another station. " + System.Environment.NewLine + info, this);
         }
      }

      private void play()
      {
         if (!isStopped)
            Play();
      }

      #endregion
   }
}
// © 2015-2021 crosstales LLC (https://www.crosstales.com)