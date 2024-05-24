using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Crosstales.Radio.Demo
{
   /// <summary>GUI for multiple radio players.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_g_u_i_radioplayer.html")]
   public class GUIRadioplayer : MonoBehaviour
   {
      #region Variables

      /// <summary>'RadioManager' from the scene.</summary>
      [Header("Settings")] [Tooltip("'RadioManager' from the scene.")] public RadioManager Manager;

      /// <summary>Prefab for the radio list.</summary>
      [Tooltip("Prefab for the radio list.")] public GameObject ItemPrefab;

/*
      /// <summary>Number of cached prefab items.</summary>
      [Tooltip("Number of cached prefab items.")] public int CachedItems = 600;
*/
      [Header("UI Objects")] public GameObject Target;
      public GameObject BuildingPanel;
      public Scrollbar Scroll;
      public int ColumnCount = 1;
      public Vector2 SpaceWidth = new Vector2(8, 8);
      public Vector2 SpaceHeight = new Vector2(8, 8);
      public Color32 EvenColor = new Color32(242, 236, 224, 128);
      public Color32 OddColor = new Color32(128, 128, 128, 128);
      public Text StationCounter;
      public Text LimitText;

      private bool orderByNameDesc;
      private bool orderByNameStandard = true;
      private bool orderByStation;
      private bool orderByStationDesc;
      private bool orderByStationStandard = true;
      private bool orderByUrl;
      private bool orderByUrlDesc;
      private bool orderByUrlStandard = true;
      private bool orderByFormat;
      private bool orderByFormatDesc;
      private bool orderByFormatStandard;
      private bool orderByBitrate;
      private bool orderByBitrateDesc;
      private bool orderByBitrateStandard;
      private bool orderByGenre;
      private bool orderByGenreDesc;
      private bool orderByGenreStandard = true;
      private bool orderByRating;
      private bool orderByRatingDesc;
      private bool orderByRatingStandard;

      private bool isBuilding;

      private System.Collections.Generic.Dictionary<RadioPlayer, ComplexObject> prefabs;

      private Transform tf;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         if (Manager != null)
            Manager.OnProviderReady += onProviderReady;

         tf = transform;

         if (StationCounter != null)
            StationCounter.text = "<i>Loading...</i>";
      }

      private void Update()
      {
         if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Query();

         if (Time.frameCount % 10 == 0)
            BuildingPanel.SetActive(isBuilding);
      }

      private void OnDestroy()
      {
         if (Manager != null)
            Manager.OnProviderReady -= onProviderReady;
      }

      #endregion


      #region Public methods

      public void Query()
      {
         orderByNameStandard = false;
         OrderByName();
      }

      public void LimitChanged(float value)
      {
         if (Manager != null)
            Manager.Filter.Limit = (int)value;

         if (LimitText != null)
            LimitText.text = value.ToString("N0");
      }

      public void FilterName(string filter)
      {
         if (Manager != null)
            Manager.Filter.Names = filter;
      }

      public void FilterStation(string filter)
      {
         if (Manager != null)
            Manager.Filter.Stations = filter;
      }

      public void FilterUrl(string filter)
      {
         if (Manager != null)
            Manager.Filter.Urls = filter;
      }

      public void FilterBitrateMin(string bitrate)
      {
         if (Manager != null)
         {
            if (int.TryParse(bitrate, out int _bitrate))
            {
               Manager.Filter.BitrateMin = _bitrate;
            }
            else
            {
               Manager.Filter.BitrateMin = 32;
            }
         }
      }

      public void FilterBitrateMax(string bitrate)
      {
         if (Manager != null)
         {
            if (int.TryParse(bitrate, out int _bitrate))
            {
               Manager.Filter.BitrateMax = _bitrate;
            }
            else
            {
               Manager.Filter.BitrateMax = 500;
            }
         }
      }

      public void FilterGenre(string filter)
      {
         if (Manager != null)
            Manager.Filter.Genres = filter;
      }

      public void FilterRatingMin(string rating)
      {
         if (Manager != null)
         {
            if (float.TryParse(rating, out float _rating))
            {
               Manager.Filter.RatingMin = _rating;
            }
            else
            {
               Manager.Filter.RatingMin = 0f;
            }
         }
      }

      public void FilterRatingMax(string rating)
      {
         if (Manager != null)
         {
            if (float.TryParse(rating, out float _rating))
            {
               Manager.Filter.RatingMax = _rating;
            }
            else
            {
               Manager.Filter.RatingMax = 5f;
            }
         }
      }

      public void FilterFormat(string filter)
      {
         if (Manager != null)
            Manager.Filter.Format = filter;
      }

      public void OrderByName()
      {
         if (!isBuilding)
         {
            resetOrderBy();

            if (orderByNameStandard)
            {
               orderByNameDesc = true;

               orderByNameStandard = false;
            }
            else
            {
               orderByNameDesc = false;
               orderByNameStandard = true;
            }

            StartCoroutine(buildRadioPlayerList());
         }
      }

      public void OrderByStation()
      {
         if (!isBuilding)
         {
            resetOrderBy();

            if (orderByStationStandard)
            {
               orderByStation = true;
               orderByStationDesc = false;
               orderByStationStandard = false;
            }
            else
            {
               orderByStation = false;
               orderByStationDesc = true;
               orderByStationStandard = true;
            }

            StartCoroutine(buildRadioPlayerList());
         }
      }

      public void OrderByUrl()
      {
         if (!isBuilding)
         {
            resetOrderBy();

            if (orderByUrlStandard)
            {
               orderByUrl = true;
               orderByUrlDesc = false;
               orderByUrlStandard = false;
            }
            else
            {
               orderByUrl = false;
               orderByUrlDesc = true;
               orderByUrlStandard = true;
            }

            StartCoroutine(buildRadioPlayerList());
         }
      }

      public void OrderByFormat()
      {
         if (!isBuilding)
         {
            resetOrderBy();

            if (orderByFormatStandard)
            {
               orderByFormat = true;
               orderByFormatDesc = false;
               orderByFormatStandard = false;
            }
            else
            {
               orderByFormat = false;
               orderByFormatDesc = true;
               orderByFormatStandard = true;
            }

            StartCoroutine(buildRadioPlayerList());
         }
      }

      public void OrderByBitrate()
      {
         if (!isBuilding)
         {
            resetOrderBy();

            if (orderByBitrateStandard)
            {
               orderByBitrate = true;
               orderByBitrateDesc = false;
               orderByBitrateStandard = false;
            }
            else
            {
               orderByBitrate = false;
               orderByBitrateDesc = true;
               orderByBitrateStandard = true;
            }

            StartCoroutine(buildRadioPlayerList());
         }
      }

      public void OrderByGenre()
      {
         if (!isBuilding)
         {
            resetOrderBy();

            if (orderByGenreStandard)
            {
               orderByGenre = true;
               orderByGenreDesc = false;
               orderByGenreStandard = false;
            }
            else
            {
               orderByGenre = false;
               orderByGenreDesc = true;
               orderByGenreStandard = true;
            }

            StartCoroutine(buildRadioPlayerList());
         }
      }

      public void OrderByRating()
      {
         if (!isBuilding)
         {
            resetOrderBy();

            if (orderByRatingStandard)
            {
               orderByRating = true;
               orderByRatingDesc = false;
               orderByRatingStandard = false;
            }
            else
            {
               orderByRating = false;
               orderByRatingDesc = true;
               orderByRatingStandard = true;
            }

            StartCoroutine(buildRadioPlayerList());
         }
      }

      #endregion


      #region Callback methods

      private void onProviderReady()
      {
         if (prefabs == null)
         {
            int count = Manager.PlayersByName().Count;
            prefabs = new System.Collections.Generic.Dictionary<RadioPlayer, ComplexObject>(count);

            for (int ii = 0; ii < count; ii++)
            {
               GameObject go = Instantiate(ItemPrefab, tf, true);

               GUIRadioStatic script = go.GetComponent<GUIRadioStatic>();
               script.Player = Manager.PlayersByName()[ii];

               prefabs.Add(Manager.PlayersByName()[ii], new ComplexObject(script, go.transform, go.GetComponent<RectTransform>(), go.GetComponent<Image>()));
            }

            //StopAllCoroutines();
            StartCoroutine(buildRadioPlayerList());
         }
      }

      #endregion


      #region Private methods

      private IEnumerator buildRadioPlayerList()
      {
         if (!isBuilding && Manager != null && ItemPrefab != null && Scroll != null)
         {
            isBuilding = true;

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

            System.Collections.Generic.List<RadioPlayer> items;

            if (orderByNameDesc)
            {
               items = Manager.PlayersByName(true);
            }
            else if (orderByUrl)
            {
               items = Manager.PlayersByURL();
            }
            else if (orderByUrlDesc)
            {
               items = Manager.PlayersByURL(true);
            }
            else if (orderByFormat)
            {
               items = Manager.PlayersByFormat();
            }
            else if (orderByFormatDesc)
            {
               items = Manager.PlayersByFormat(true);
            }
            else if (orderByStation)
            {
               items = Manager.PlayersByStation();
            }
            else if (orderByStationDesc)
            {
               items = Manager.PlayersByStation(true);
            }
            else if (orderByBitrate)
            {
               items = Manager.PlayersByBitrate();
            }
            else if (orderByBitrateDesc)
            {
               items = Manager.PlayersByBitrate(true);
            }
            else if (orderByGenre)
            {
               items = Manager.PlayersByGenres();
            }
            else if (orderByGenreDesc)
            {
               items = Manager.PlayersByGenres(true);
            }
            else if (orderByRating)
            {
               items = Manager.PlayersByRating();
            }
            else if (orderByRatingDesc)
            {
               items = Manager.PlayersByRating(true);
            }
            else
            {
               items = Manager.PlayersByName();
            }

            StationCounter.text = "Stations: " + items.Count + " / " + Manager.Players.Count;

            //calculate the width and height of each child item.
            float width = containerRectTransform.rect.width / ColumnCount - (SpaceWidth.x + SpaceWidth.y) * ColumnCount;
            float height = rowRectTransform.rect.height - (SpaceHeight.x + SpaceHeight.y);

            int rowCount = items.Count / ColumnCount;

            if (rowCount > 0 && items.Count % rowCount > 0)
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

            for (int ii = 0; ii < items.Count; ii++)
            {
               //this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
               if (ii % ColumnCount == 0)
                  jj++;

               if (!prefabs.TryGetValue(items[ii], out ComplexObject newItem))
               {
                  RadioPlayer player;

                  if (orderByNameDesc)
                  {
                     player = Manager.PlayersByName(true)[ii];
                  }
                  else if (orderByUrl)
                  {
                     player = Manager.PlayersByURL()[ii];
                  }
                  else if (orderByUrlDesc)
                  {
                     player = Manager.PlayersByURL(true)[ii];
                  }
                  else if (orderByFormat)
                  {
                     player = Manager.PlayersByFormat()[ii];
                  }
                  else if (orderByFormatDesc)
                  {
                     player = Manager.PlayersByFormat(true)[ii];
                  }
                  else if (orderByStation)
                  {
                     player = Manager.PlayersByStation()[ii];
                  }
                  else if (orderByStationDesc)
                  {
                     player = Manager.PlayersByStation(true)[ii];
                  }
                  else if (orderByBitrate)
                  {
                     player = Manager.PlayersByBitrate()[ii];
                  }
                  else if (orderByBitrateDesc)
                  {
                     player = Manager.PlayersByBitrate(true)[ii];
                  }
                  else if (orderByGenre)
                  {
                     player = Manager.PlayersByGenres()[ii];
                  }
                  else if (orderByGenreDesc)
                  {
                     player = Manager.PlayersByGenres(true)[ii];
                  }
                  else if (orderByRating)
                  {
                     player = Manager.PlayersByRating()[ii];
                  }
                  else if (orderByRatingDesc)
                  {
                     player = Manager.PlayersByRating(true)[ii];
                  }
                  else
                  {
                     player = Manager.PlayersByName()[ii];
                  }

                  GameObject go = Instantiate(ItemPrefab, tf, true);
                  GUIRadioStatic script = go.GetComponent<GUIRadioStatic>();
                  script.Player = player;

                  newItem = new ComplexObject(script, go.transform, go.GetComponent<RectTransform>(), go.GetComponent<Image>());
                  prefabs.Add(player, new ComplexObject(script, go.transform, go.GetComponent<RectTransform>(), go.GetComponent<Image>()));
               }

               newItem.ObjectTransform.SetParent(ttf);
               newItem.ObjectTransform.localScale = Vector3.one;

               if (ii % 2 == 0)
               {
                  if (!items[ii].isPlayback) //ignore during playback
                     newItem.ObjectImage.color = OddColor;

                  newItem.Script.StopColor = OddColor;
               }
               else
               {
                  if (!items[ii].isPlayback) //ignore during playback
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

            isBuilding = false;

            Scroll.value = 1f;
         }
      }

      private void resetOrderBy()
      {
         orderByStation = false;
         orderByUrl = false;
         orderByFormat = false;
         orderByBitrate = false;
         orderByGenre = false;
         orderByRating = false;

         orderByNameDesc = false;
         orderByStationDesc = false;
         orderByUrlDesc = false;
         orderByFormatDesc = false;
         orderByBitrateDesc = false;
         orderByGenreDesc = false;
         orderByRatingDesc = false;
      }

      #endregion
   }
}
// © 2015-2021 crosstales LLC (https://www.crosstales.com)