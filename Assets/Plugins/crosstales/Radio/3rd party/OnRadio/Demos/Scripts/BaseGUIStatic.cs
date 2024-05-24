using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.OnRadio.Demo
{
   /// <summary>Base-class for a static GUI entry.</summary>
   public abstract class BaseGUIStatic : MonoBehaviour
   {
      #region Variables

      /// <summary>'RadioPlayer' from the scene.</summary>
      [Header("Settings")] [Tooltip("'RadioPlayer' from the scene.")] public RadioPlayer Player;

      /// <summary>'BaseService' from the scene.</summary>
      [Tooltip("'BaseService' from the scene.")] public Service.BaseService Service;

      /// <summary>The color for the Play-mode.</summary>
      [Tooltip("The color for the Play-mode.")] public Color32 PlayColor = new Color32(0, 255, 0, 64);

      /// <summary>How many times should the radio station restart after an error before giving up (default: 3).</summary>
      [Tooltip("How many times should the radio station restart after an error before giving up (default: 3).")]
      public int Retries = 3;

      [Header("UI Objects")] public Text TitleText;
      public Text SubText;
      public GameObject PlayButton;
      public GameObject StopButton;
      public Image MainImage;

      [HideInInspector] public Color32 StopColor;

      //private Color32 color;
      private int invokeDelayCounter = 1;
      private bool isStopped = true;

      protected OnRadio.Model.RecordInfoExt record;
      protected string uidQuery;

      #endregion


      #region Properties

      /// <summary>'Record' for the player.</summary>
      public abstract OnRadio.Model.RecordInfoExt Record { get; set; }

      #endregion


      #region MonoBehaviour methods

      protected virtual void Start()
      {
         if (Player != null)
         {
            if (Record != null)
               TitleText.text = Record.Artist + " - " + Record.Title;

            PlayButton.SetActive(true);
            StopButton.SetActive(false);

            // Subscribe event listeners
            Player.OnPlaybackStart += onPlayBackStart;
            Player.OnPlaybackEnd += onPlaybackEnd;
            Player.OnAudioStart += onAudioStart;
            Player.OnAudioEnd += onAudioEnd;
            Player.OnErrorInfo += onErrorInfo;
            Player.OnRecordChange += onRecordChange;
         }
         else
         {
            Debug.LogError("'Player' is null!", this);
         }

         if (Service != null)
         {
            Service.OnQueryComplete += onQueryComplete;
         }
         else
         {
            Debug.LogError("'Service' is null!", this);
         }
      }

      protected virtual void OnDisable()
      {
         if (Player != null)
         {
            // Unsubscribe event listeners
            Player.OnPlaybackStart -= onPlayBackStart;
            Player.OnPlaybackEnd -= onPlaybackEnd;
            Player.OnAudioStart += onAudioStart;
            Player.OnAudioEnd += onAudioEnd;
            Player.OnErrorInfo -= onErrorInfo;
            Player.OnRecordChange -= onRecordChange;
         }

         if (Service != null)
            Service.OnQueryComplete -= onQueryComplete;
      }

      #endregion


      #region Public methods

      public void Play()
      {
         if (Player != null)
         {
            if (Player.isPlayback)
               Player.Stop();

            if (uidQuery == null)
            {
               uidQuery = Service.StationService(Record.Station);
               //Debug.Log("QUERY: " + uidQuery, this);
            }
            else
            {
               Invoke(nameof(play), 0.2f);
               //Debug.Log("PLAY", this);
            }
         }
      }

      public void Stop()
      {
         if (Player != null)
            Player.Stop();
      }

      public void OpenUrl()
      {
         if (Player != null)
            Radio.Util.Helper.OpenURL(Player.Station.Station);
      }

      #endregion


      #region Callback methods

      private void onQueryComplete(string id)
      {
         if (id.Equals(uidQuery))
         {
            if (Radio.Util.Config.DEBUG)
               Debug.Log("onQueryComplete: " + id, this);

            play();
         }
      }

      private void onPlayBackStart(Crosstales.Radio.Model.RadioStation station)
      {
         if (station.Equals(Record.Station))
         {
            if (Radio.Util.Config.DEBUG)
               Debug.Log("onPlayBackStart: " + station, this);

            if (PlayButton != null)
               PlayButton.SetActive(false);

            if (StopButton != null)
               StopButton.SetActive(true);

            if (MainImage != null)
               MainImage.color = PlayColor;
         }
      }

      private void onPlaybackEnd(Crosstales.Radio.Model.RadioStation station)
      {
         if (station.Equals(Record.Station))
         {
            if (Radio.Util.Config.DEBUG)
               Debug.Log("onPlaybackEnd: " + station, this);

            if (PlayButton != null)
               PlayButton.SetActive(true);

            if (StopButton != null)
               StopButton.SetActive(false);

            if (MainImage != null)
               MainImage.color = StopColor;
         }
      }

      private void onAudioStart(Crosstales.Radio.Model.RadioStation station)
      {
         if (station.Equals(Record.Station))
         {
            if (Radio.Util.Config.DEBUG)
               Debug.Log("onAudioStart: " + station, this);

            isStopped = false;
         }
      }

      private void onAudioEnd(Crosstales.Radio.Model.RadioStation station)
      {
         if (station.Equals(Record.Station))
         {
            if (Radio.Util.Config.DEBUG)
               Debug.Log("onAudioEnd: " + station, this);

            isStopped = true;
         }
      }

      private void onErrorInfo(Crosstales.Radio.Model.RadioStation station, string info)
      {
         if (station.Equals(Record.Station))
         {
            if (Radio.Util.Config.DEBUG)
               Debug.Log("onErrorInfo: " + info + " - " + station, this);

            Stop();
            onPlaybackEnd(station);

            if (!isStopped)
            {
               if (invokeDelayCounter < Retries)
               {
                  Debug.LogWarning("Error occured -> Restarting station." + System.Environment.NewLine + info);

                  if (!isStopped)
                     Invoke(nameof(play), Crosstales.Radio.Util.Constants.INVOKE_DELAY * invokeDelayCounter);

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
      }

      protected abstract void onRecordChange(Crosstales.Radio.Model.RadioStation station, Crosstales.Radio.Model.RecordInfo newrecord);

      private void play()
      {
         Player.Station = Record?.Station;
         Player.Play();
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)