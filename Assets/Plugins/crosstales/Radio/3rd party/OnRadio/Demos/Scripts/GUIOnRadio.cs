using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Crosstales.Radio.OnRadio.Demo
{
   /// <summary>GUI for OnRadio.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_demo_1_1_g_u_i_on_radio.html")]
   public class GUIOnRadio : MonoBehaviour
   {
      #region Variables

      /// <summary>'RadioPlayer' from the scene.</summary>
      [Header("Settings")] [Tooltip("'RadioPlayer' from the scene.")] public RadioPlayer Player;

      /// <summary>'BaseService' from the scene.</summary>
      [Tooltip("'BaseService' from the scene.")] public Service.BaseService Service;

      /// <summary>'Provider' from the scene.</summary>
      [Tooltip("'Provider' from the scene.")] public Crosstales.Radio.Provider.RadioProviderUser Provider;

      /// <summary>Prefab for the radio list.</summary>
      [Tooltip("Prefab for the radio list.")] public GameObject ItemPrefab;

      /// <summary>Query the service on start (default: false).</summary>
      [Header("Behaviour Settings")] [Tooltip("Query the service on start (default: false).")] public bool QueryOnStart;


      [Header("UI Objects")] public GameObject Target;
      public Scrollbar Scroll;
      public int ColumnCount = 1;
      public Vector2 SpaceWidth = new Vector2(8, 8);
      public Vector2 SpaceHeight = new Vector2(8, 8);
      public Color32 EvenColor = new Color32(242, 236, 224, 128);
      public Color32 OddColor = new Color32(128, 128, 128, 128);
      public Text ErrorText;

      public Text RecordInfo;
      public Text StationInfo;

      public Image SongIcon;
      public Image StationIcon;

      public GameObject QueryPanel;

      private bool isBuilding;
      private int recordPlaytime;

      private string uidQuery;
      private string uidSongIcon;
      private string uidStationIcon;

      private System.Collections.Generic.List<ComplexObject> prefabs = new System.Collections.Generic.List<ComplexObject>(50);
      private Transform tf;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         Service.OnQueryComplete += onQueryComplete;
         Player.OnRecordChange += onRecordChange;
         Player.OnPlaybackStart += onPlaybackStart;
         Player.OnPlaybackEnd += onPlaybackEnd;
         Player.OnBufferingProgressUpdate += onBufferingProgressUpdate;
         Player.OnRecordPlayTimeUpdate += onRecordPlayTimeUpdate;
         Player.OnErrorInfo += onErrorInfo;

         tf = transform;

         QueryPanel.SetActive(false);

         onPlaybackEnd(null);

         ErrorText.text = string.Empty;

         if (QueryOnStart)
            Query();
      }

      private void Update()
      {
         if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Query();
      }

      private void OnDestroy()
      {
         Service.OnQueryComplete -= onQueryComplete;
         Player.OnRecordChange -= onRecordChange;
         Player.OnPlaybackStart -= onPlaybackStart;
         Player.OnPlaybackEnd -= onPlaybackEnd;
         Player.OnBufferingProgressUpdate -= onBufferingProgressUpdate;
         Player.OnRecordPlayTimeUpdate -= onRecordPlayTimeUpdate;
         Player.OnErrorInfo -= onErrorInfo;
      }

      #endregion


      #region Public methods

      public void AddToProvider()
      {
         if (!Provider.Stations.Contains(Player.Station))
         {
            Player.Station.Rating = 5f;
            Provider.Stations.Add(Player.Station);
         }
      }

      public void Query()
      {
         QueryPanel.SetActive(true);

         uidQuery = Service.Query();
      }

      public void OpenUrl()
      {
         if (!string.IsNullOrEmpty(Player.Station.Station))
            Radio.Util.Helper.OpenURL(Player.Station.Station);
      }

      public void OpenSpotifyUrl()
      {
         Application.OpenURL(Player.RecordInfo.SpotifyUrl);
      }

      public void OpenLyricsUrl()
      {
         Radio.Util.Helper.OpenURL(Player.RecordInfo.LyricsUrl);
      }

      public void Rebuild()
      {
         prefabs.Clear();
         StartCoroutine(buildRecordList());
      }

      #endregion


      #region Callback methods

      private void onQueryComplete(string id)
      {
         if (Radio.Util.Config.DEBUG)
            Debug.Log("onQueryComplete: " + id, this);

         if (id.Equals(uidQuery))
         {
            StartCoroutine(buildRecordList());

            QueryPanel.SetActive(false);
         }

         if (id.Equals(uidSongIcon) && Player.RecordInfo.Icon != null)
            //if (Player.RecordInfo.Icon != null)
            SongIcon.sprite = Player.RecordInfo.Icon;

         if (id.Equals(uidStationIcon) && Player.Station.Icon != null)
            //if (Player.Station.Icon != null)
            StationIcon.sprite = Player.Station.Icon;
      }

      private void onRecordPlayTimeUpdate(Crosstales.Radio.Model.RadioStation station, Crosstales.Radio.Model.RecordInfo record, float playtime)
      {
         if ((int)playtime != recordPlaytime)
         {
            recordPlaytime = (int)playtime;

            RecordInfo.text = getRecordInfo(playtime, record.Title, record.Artist);
         }
      }

      private void onRecordChange(Crosstales.Radio.Model.RadioStation station, Crosstales.Radio.Model.RecordInfo newrecord)
      {
         if (Radio.Util.Config.DEBUG)
            Debug.Log("onRecordChange", this);

         SongIcon.sprite = Service.DefaultSongIcon;
         //StationIcon.sprite = DefaultStationIcon;

         uidSongIcon = Service.SongArtService(newrecord, true);
         //Service.SongArtService(newrecord, true);

         if (Player.Station.GetType() == typeof(OnRadio.Model.RadioStationExt))
         {
            uidStationIcon = Service.DARStationService((OnRadio.Model.RadioStationExt)station, true);
            //Service.DARStationService((OnRadio.Model.RadioStationExt)station, true);
         }

         RecordInfo.text = getRecordInfo(0, newrecord.Title, newrecord.Artist);
         StationInfo.text = getStationInfo(station);
         ErrorText.text = string.Empty;
      }

      private void onPlaybackStart(Crosstales.Radio.Model.RadioStation station)
      {
         ErrorText.text = string.Empty;
         StationInfo.text = getStationInfo(station);
      }

      private void onPlaybackEnd(Crosstales.Radio.Model.RadioStation station)
      {
         if (Radio.Util.Config.DEBUG)
            Debug.Log("onPlaybackEnd", this);

         RecordInfo.text = "<b>" + Crosstales.Radio.Util.Constants.TEXT_STOPPED + "</b>";
         StationInfo.text = string.Empty;

         SongIcon.sprite = Service.DefaultSongIcon;
         StationIcon.sprite = Service.DefaultStationIcon;
      }

      private void onBufferingProgressUpdate(Crosstales.Radio.Model.RadioStation station, float progress)
      {
         RecordInfo.text = getRecordInfo(progress, Player.RecordInfo.Title, Player.RecordInfo.Artist, true);
      }

      private void onErrorInfo(Crosstales.Radio.Model.RadioStation station, string info)
      {
         ErrorText.text = info;
      }

      #endregion


      #region Private methods

      private string getStationInfo(Crosstales.Radio.Model.RadioStation station)
      {
         System.Text.StringBuilder sb = new System.Text.StringBuilder();
         sb.Append("<b>");
         sb.Append(station.Name);
         sb.AppendLine("</b>");
         if (!string.IsNullOrEmpty(station.Genres))
         {
            sb.AppendLine();
            sb.Append("Genres: ");
            sb.AppendLine(station.Genres);
            sb.AppendLine();
            sb.Append("Bitrate: ");
            sb.Append(station.Bitrate);
            sb.AppendLine("kbit/s");
         }

         return sb.ToString();
      }

      private string getRecordInfo(float value, string title, string artist, bool isBuffer = false)
      {
         System.Text.StringBuilder sb = new System.Text.StringBuilder();
         sb.Append("<b>");
         if (isBuffer)
         {
            sb.Append(Crosstales.Radio.Util.Constants.TEXT_BUFFER);
            sb.Append(value.ToString(Crosstales.Radio.Util.Constants.FORMAT_PERCENT));
         }
         else
         {
            sb.Append(Crosstales.Radio.Util.Helper.FormatSecondsToHourMinSec(value));
         }

         sb.AppendLine("</b>");
         sb.AppendLine();
         sb.Append("<b><i>");
         sb.Append(title);
         sb.AppendLine("</i></b>");
         sb.AppendLine();
         sb.AppendLine(artist);

         return sb.ToString();
      }

      private IEnumerator buildRecordList()
      {
         if (!isBuilding && Service != null && ItemPrefab != null && Scroll != null)
         {
            isBuilding = true;

            ErrorText.text = string.Empty;

            RectTransform rowRectTransform = ItemPrefab.GetComponent<RectTransform>();
            RectTransform containerRectTransform = Target.GetComponent<RectTransform>();
            Vector2 modifierVector = Vector2.zero;
            Transform ttf = Target.transform;

            //clear existing items
            for (int ii = ttf.childCount - 1; ii >= 0; ii--)
            {
               Transform child = ttf.GetChild(ii);
               child.SetParent(tf);

               if (ii % 20 == 0) //return to the main thread after n processed items (reduces jitter)
                  yield return null;

               //Destroy(child.gameObject);
            }

            yield return null;

            if (Service.Records.Count > 0)
            {
               //calculate the width and height of each child item.
               float width = containerRectTransform.rect.width / ColumnCount - (SpaceWidth.x + SpaceWidth.y) * ColumnCount;
               float height = rowRectTransform.rect.height - (SpaceHeight.x + SpaceHeight.y);

               int rowCount = Service.Records.Count / ColumnCount;

               if (rowCount > 0 && Service.Records.Count % rowCount > 0)
                  rowCount++;

               //adjust the height of the container so that it will just barely fit all its children
               float scrollHeight = height * rowCount;
               modifierVector.Set(containerRectTransform.offsetMin.x, -scrollHeight / 2f);
               containerRectTransform.offsetMin = modifierVector;
               modifierVector.Set(containerRectTransform.offsetMax.x, scrollHeight / 2f);
               containerRectTransform.offsetMax = modifierVector;

               int jj = 0;

               Rect rect = containerRectTransform.rect;
               float middleHeight = -rect.width / 2f;
               float middleWidth = rect.height / 2f;

               for (int ii = 0; ii < Service.Records.Count; ii++)
               {
                  //this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
                  if (ii % ColumnCount == 0)
                     jj++;


                  ComplexObject newItem;

                  if (ii >= prefabs.Count)
                  {
                     GameObject go = Instantiate(ItemPrefab, tf, true);
                     BaseGUIStatic script = go.GetComponent<BaseGUIStatic>();
                     script.Player = Player;
                     script.Service = Service;

                     newItem = new ComplexObject(script, go.transform, go.GetComponent<RectTransform>(), go.GetComponent<Image>());
                     prefabs.Add(newItem);
                  }
                  else
                  {
                     newItem = prefabs[ii];
                  }

                  newItem.Script.Record = newItem.Script.GetType() == typeof(GUIStationStatic) ? Service.RecordsByStationName()[ii] : Service.RecordsByArtist()[ii];

                  newItem.ObjectTransform.SetParent(ttf);
                  newItem.ObjectTransform.localScale = Vector3.one;

                  if (ii % 2 == 0)
                  {
                     newItem.ObjectImage.color = OddColor;
                     newItem.Script.StopColor = OddColor;
                  }
                  else
                  {
                     newItem.ObjectImage.color = EvenColor;
                     newItem.Script.StopColor = EvenColor;
                  }

                  RectTransform rectTransform = newItem.ObjectRectTransform;

                  //move and size the new item
                  modifierVector.Set(
                     middleHeight + (width + SpaceWidth.x) * (ii % ColumnCount) + SpaceWidth.x * ColumnCount,
                     middleWidth - height * jj);
                  Vector2 offsetMin = modifierVector;
                  rectTransform.offsetMin = offsetMin;
                  modifierVector.Set(offsetMin.x + width, offsetMin.y + height);
                  rectTransform.offsetMax = modifierVector;

                  if (ii % 20 == 0)
                  {
                     //return to the main thread after n processed items (reduces jitter)
                     Scroll.value = 1f;
                     yield return null;
                  }
               }
            }
            else
            {
               ErrorText.text = "No records found! Please try other query parameters.";
            }

            isBuilding = false;

            Scroll.value = 1f;
         }
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)