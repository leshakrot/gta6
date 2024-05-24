using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Crosstales.Radio.OnRadio.Service
{
   [System.Serializable]
   public class QueryCompleteEvent : UnityEngine.Events.UnityEvent<string>
   {
   }

   /// <summary>Base-class of a service.</summary>
   public abstract class BaseService : MonoBehaviour
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Token")] [Header("Credentials"), Tooltip("Token to access OnRadio."), SerializeField]
      private string token;


      [UnityEngine.Serialization.FormerlySerializedAsAttribute("EnableStation")] [Header("Station"), Tooltip("Enable Station service (default: false)."), SerializeField]
      private bool enableStation;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("AwaitStationQuery")] [Tooltip("Wait until Station query is finished (default: false)."), SerializeField]
      private bool awaitStationQuery;


      [UnityEngine.Serialization.FormerlySerializedAsAttribute("EnableSongArt")] [Header("SongArt"), Tooltip("Enable SongArt service (default: false)."), SerializeField]
      private bool enableSongArt;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("AwaitSongArtQuery")] [Tooltip("Wait until SongArt query is finished (default: false)."), SerializeField]
      private bool awaitSongArtQuery;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("LoadRecordIcon")] [Tooltip("Load the record icon (default: false)."), SerializeField]
      private bool loadRecordIcon;

      private Model.ImageResolution Resolution = Model.ImageResolution.med; //TODO has resolution no effect?


      [UnityEngine.Serialization.FormerlySerializedAsAttribute("EnableDARStation")] [Header("DARStation"), Tooltip("Enable DARStation service (default: false)."), SerializeField]
      private bool enableDARStation;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("AwaitDARStationQuery")] [Tooltip("Wait until DARStation query is finished (default: false)."), SerializeField]
      private bool awaitDARStationQuery;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("LoadStationIcon")] [Tooltip("Load the station icon (default: false)."), SerializeField]
      private bool loadStationIcon;


      [UnityEngine.Serialization.FormerlySerializedAsAttribute("DefaultStationIcon")] [Header("Icons"), Tooltip("Default icon for the radio station."), SerializeField]
      private Sprite defaultStationIcon;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("DefaultSongIcon")] [Tooltip("Default icon for the song."), SerializeField]
      private Sprite defaultSongIcon;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("QueryOnStart")] [Header("Behaviour Settings"), Tooltip("Query the service on start (default: false)."), SerializeField]
      private bool queryOnStart;

      public readonly System.Collections.Generic.List<Model.RadioStationExt> Stations = new System.Collections.Generic.List<Model.RadioStationExt>();
      public readonly System.Collections.Generic.List<Model.RecordInfoExt> Records = new System.Collections.Generic.List<Model.RecordInfoExt>();

      public static int TotalPlaylistRequests;
      public static int TotalReco2Requests;
      public static int TotalTopsongsRequests;
      public static int TotalStationRequests;
      public static int TotalSongArtRequests;
      public static int TotalDARStationRequests;

      protected const string tokenNull = "'Token' is null - can not access OnRadio!";
      protected bool loggedTokenNull;

      private System.Collections.Generic.List<Model.RecordInfoExt> recordsByArtistDesc;
      private System.Collections.Generic.List<Model.RecordInfoExt> recordsByArtistAsc;
      private System.Collections.Generic.List<Model.RecordInfoExt> recordsByTitleDesc;
      private System.Collections.Generic.List<Model.RecordInfoExt> recordsByTitleAsc;
      private System.Collections.Generic.List<Model.RecordInfoExt> recordsByStationNameDesc;
      private System.Collections.Generic.List<Model.RecordInfoExt> recordsByStationNameAsc;
      private System.Collections.Generic.List<Model.RadioStationExt> stationsByNameDesc;
      private System.Collections.Generic.List<Model.RadioStationExt> stationsByNameAsc;

      private Model.Stations stationStations;
      private Model.Songart.Songs songArtSongs;
      private Model.DARStations.Stations DARStationsStations;

      private const string stationUrl = "https://apidarfm.global.ssl.fastly.net/uberstationurlxml.php";
      private const string songArtUrl = "https://apidarfm.global.ssl.fastly.net/songart.php";
      private const string DARStationUrl = "https://apidarfm.global.ssl.fastly.net/darstations.php";

      private static string lastToken = string.Empty;

      #endregion


      #region Properties

      /// <summary>Token to access OnRadio.</summary>
#if CT_DEVELOP
      public string Token
      {
         get => string.IsNullOrEmpty(token) ? APIKeys.OnRadio : token;
         set => token = value;
      }
#else
      public string Token
      {
         get => token;
         set => token = value;
      }
#endif
      /// <summary>Enable Station service.</summary>
      public bool EnableStation
      {
         get => enableStation;
         set => enableStation = value;
      }

      /// <summary>Wait until Station query is finished.</summary>
      public bool AwaitStationQuery
      {
         get => awaitStationQuery;
         set => awaitStationQuery = value;
      }

      /// <summary>Enable SongArt service.</summary>
      public bool EnableSongArt
      {
         get => enableSongArt;
         set => enableSongArt = value;
      }

      /// <summary>Wait until SongArt query is finished.</summary>
      public bool AwaitSongArtQuery
      {
         get => awaitSongArtQuery;
         set => awaitSongArtQuery = value;
      }

      /// <summary>Load the record icon.</summary>
      public bool LoadRecordIcon
      {
         get => loadRecordIcon;
         set => loadRecordIcon = value;
      }

      /// <summary>Enable DARStation service.</summary>
      public bool EnableDARStation
      {
         get => enableDARStation;
         set => enableDARStation = value;
      }

      /// <summary>Wait until DARStation query is finished.</summary>
      public bool AwaitDARStationQuery
      {
         get => awaitDARStationQuery;
         set => awaitDARStationQuery = value;
      }

      /// <summary>Load the station icon.</summary>
      public bool LoadStationIcon
      {
         get => loadStationIcon;
         set => loadStationIcon = value;
      }

      /// <summary>Default icon for the radio station.</summary>
      public Sprite DefaultStationIcon
      {
         get => defaultStationIcon;
         set => defaultStationIcon = value;
      }

      /// <summary>Default icon for the song.</summary>
      public Sprite DefaultSongIcon
      {
         get => defaultSongIcon;
         set => defaultSongIcon = value;
      }

      /// <summary>Query the service on start.</summary>
      public bool QueryOnStart
      {
         get => queryOnStart;
         set => queryOnStart = value;
      }

      /// <summary>Indicates if the token is valid.</summary>
      /// <returns>True if the token is valid.</returns>
      public bool isValidToken => !string.IsNullOrEmpty(Token) && Token.Length >= 10 && Token.CTisInteger();

      /// <summary>Total number of requests to OnRadio.</summary>
      /// <returns>Total number of requests to OnRadio.</returns>
      public static int TotalRequests => TotalPlaylistRequests + TotalReco2Requests + TotalTopsongsRequests + TotalStationRequests + TotalSongArtRequests + TotalDARStationRequests;

      protected abstract QueryCompleteEvent onQueryCompleted { get; }

      #endregion


      #region Events

      public delegate void QueryComplete(string id);

      /// <summary>An event triggered whenever the query is completed.</summary>
      public event QueryComplete OnQueryComplete;

      #endregion


      #region MonoBehaviour methods

#if !CT_DEVELOP
      protected virtual void OnEnable()
      {
         if (string.IsNullOrEmpty(token))
         {
            token = lastToken;
         }
         else
         {
            lastToken = token;
         }
      }
#endif
      protected virtual void Start()
      {
         if (queryOnStart)
            Query();
      }

      #endregion


      #region Public methods

      /// <summary>Query the service.</summary>
      /// <returns>UID of the query.</returns>
      public string Query()
      {
         string id = System.Guid.NewGuid().ToString();
         StartCoroutine(query(id));

         return id;
      }

      /// <summary>Query the Station service.</summary>
      /// <param name="station">Radio station to query</param>
      /// <returns>UID of the query.</returns>
      public string StationService(Radio.Model.RadioStation station)
      {
         string id = System.Guid.NewGuid().ToString();
         StartCoroutine(queryStation(id, station, false));

         return id;
      }

      /// <summary>Query the SongArt service.</summary>
      /// <param name="record">Record info to query</param>
      /// <param name="loadIcon">load the icon for the record</param>
      /// <returns>UID of the query.</returns>
      public string SongArtService(Radio.Model.RecordInfo record, bool loadIcon)
      {
         string id = System.Guid.NewGuid().ToString();
         StartCoroutine(querySongArt(id, record, loadIcon, false));

         return id;
      }

      /// <summary>Query the DARStation service.</summary>
      /// <param name="station">Radio station to query</param>
      /// <param name="loadIcon">load the icon for the station</param>
      /// <returns>UID of the query.</returns>
      public string DARStationService(Radio.Model.RadioStation station, bool loadIcon)
      {
         string id = System.Guid.NewGuid().ToString();
         StartCoroutine(queryDARStation(id, station, loadIcon, false));

         return id;
      }

      /// <summary>Returns all records of this service ordered by artist.</summary>
      /// <param name="desc">Descending order (default: false, optional)</param>
      /// <returns>All records of this set ordered by artist.</returns>
      public System.Collections.Generic.List<Model.RecordInfoExt> RecordsByArtist(bool desc = false)
      {
         if (desc)
            return recordsByArtistDesc ?? (recordsByArtistDesc = Records.OrderByDescending(entry => entry.Artist).ThenByDescending(entry => entry.Title).ToList());

         return recordsByArtistAsc ?? (recordsByArtistAsc = Records.OrderBy(entry => entry.Artist).ThenBy(entry => entry.Title).ToList());
      }

      /// <summary>Returns all records of this service ordered by title.</summary>
      /// <param name="desc">Descending order (default: false, optional)</param>
      /// <returns>All records of this set ordered by title.</returns>
      public System.Collections.Generic.List<Model.RecordInfoExt> RecordsByTitle(bool desc = false)
      {
         if (desc)
            return recordsByTitleDesc ?? (recordsByTitleDesc = Records.OrderByDescending(entry => entry.Title).ThenByDescending(entry => entry.Artist).ToList());

         return recordsByTitleAsc ?? (recordsByTitleAsc = Records.OrderBy(entry => entry.Title).ThenBy(entry => entry.Artist).ToList());
      }

      /// <summary>Returns all records of this service ordered by station name.</summary>
      /// <param name="desc">Descending order (default: false, optional)</param>
      /// <returns>All records of this set ordered by station name.</returns>
      public System.Collections.Generic.List<Model.RecordInfoExt> RecordsByStationName(bool desc = false)
      {
         if (desc)
            return recordsByStationNameDesc ?? (recordsByStationNameDesc = Records.OrderByDescending(entry => entry.Station.Name).ToList());

         return recordsByStationNameAsc ?? (recordsByStationNameAsc = Records.OrderBy(entry => entry.Station.Name).ToList());
      }

      /// <summary>Returns all stations of this service ordered by name.</summary>
      /// <param name="desc">Descending order (default: false, optional)</param>
      /// <returns>All stations of this set ordered by name.</returns>
      public System.Collections.Generic.List<Model.RadioStationExt> StationsByName(bool desc = false)
      {
         if (desc)
            return stationsByNameDesc ?? (stationsByNameDesc = Stations.OrderByDescending(entry => entry.Name).ToList());

         return stationsByNameAsc ?? (stationsByNameAsc = Stations.OrderBy(entry => entry.Name).ToList());
      }

      #endregion


      #region Protected methods

      protected abstract IEnumerator query(string id);

      protected void clearData()
      {
         Stations.Clear();
         Records.Clear();
         recordsByArtistDesc = null;
         recordsByArtistAsc = null;
         recordsByTitleDesc = null;
         recordsByTitleAsc = null;
         recordsByStationNameDesc = null;
         recordsByStationNameAsc = null;
         stationsByNameDesc = null;
         stationsByNameAsc = null;
      }

      #endregion


      #region Station

      protected IEnumerator queryStation(string id, Radio.Model.RadioStation station, bool isInternal)
      {
         if (!string.IsNullOrEmpty(Token))
         {
            if (station != null)
            {
               TotalStationRequests++;

               string apiCall = createStationUrl(station);

               using (UnityWebRequest www = UnityWebRequest.Get(apiCall))
               {
                  www.timeout = Radio.Util.Constants.MAX_WEB_LOAD_WAIT_TIME;

                  www.downloadHandler = new DownloadHandlerBuffer();
                  yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                  if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
                  if (!www.isHttpError && !www.isNetworkError)
#endif
                  {
                     stationStations = Common.Util.XmlHelper.DeserializeFromString<Model.Stations>(www.downloadHandler.text, false);

                     if (stationStations?.Callsign != null && stationStations.Url != null && stationStations.Websiteurl != null)
                     {
                        station.Name = stationStations.Callsign.Trim();
                        station.Url = stationStations.Url.Trim();
                        station.Station = stationStations.Websiteurl.Trim();
                     }
                     else
                     {
                        Debug.LogWarning("Station could not find any data: " + station, this);
                     }
                  }
                  else
                  {
                     Debug.LogWarning("Could not call the API: " + www.error, this);
                  }
               }
            }
            else
            {
               Debug.LogWarning("'station' is null!", this);
            }
         }
         else
         {
            if (!loggedTokenNull)
            {
               loggedTokenNull = true;
               Debug.LogWarning(tokenNull, this);
            }
         }

         if (!isInternal)
            onQueryComplete(id);
      }

      private string createStationUrl(Radio.Model.RadioStation station)
      {
         System.Text.StringBuilder sb = new System.Text.StringBuilder();
         sb.Append(stationUrl);
         sb.Append("?");

         if (station.GetType() == typeof(Model.RadioStationExt))
         {
            sb.Append("station_id=");
            sb.Append(((Model.RadioStationExt)station).StationId.Trim());
         }
         else
         {
            sb.Append("callsign=");
            sb.Append(station.Name.StartsWith("1.fm", System.StringComparison.OrdinalIgnoreCase) ? station.Name.Trim().Substring(7) : station.Name.Trim());
         }

         sb.Append("&partner_token=");
         sb.Append(Token.Trim());

         return System.Uri.EscapeUriString(sb.ToString());
      }

      #endregion


      #region SongArt

      protected IEnumerator querySongArt(string id, Radio.Model.RecordInfo record, bool loadIcon, bool isInternal)
      {
         if (!string.IsNullOrEmpty(Token))
         {
            if (record != null)
            {
               if (record.Icon == null)
               {
                  TotalSongArtRequests++;
                  string apiCall = createSongArtUrl(record);

                  using (UnityWebRequest www = UnityWebRequest.Get(apiCall))
                  {
                     www.timeout = Radio.Util.Constants.MAX_WEB_LOAD_WAIT_TIME;

                     www.downloadHandler = new DownloadHandlerBuffer();
                     yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                     if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
                     if (!www.isHttpError && !www.isNetworkError)
#endif
                     {
                        songArtSongs = Common.Util.XmlHelper.DeserializeFromString<Model.Songart.Songs>(www.downloadHandler.text, false);

                        if (songArtSongs?.Song != null)
                        {
                           string url = songArtSongs.Song.Arturl.Trim();
                           //Debug.Log("ART: " + url);

                           if (Radio.Util.Helper.isValidURL(url) && !url.CTContains("onradio-500.png"))
                           {
                              //Debug.Log("LOAD ART: " + url);
                              record.IconUrl = url;

                              if (loadIcon)
                                 yield return Radio.Tool.LoadIcon.Load(record);
                           }
                           else
                           {
                              if (loadIcon)
                                 record.Icon = defaultSongIcon;
                           }
                        }
                        else
                        {
                           Debug.LogWarning("SongArt could not find any data: " + record, this);

                           if (loadIcon)
                              record.Icon = defaultSongIcon;
                        }
                     }
                     else
                     {
                        Debug.LogWarning("Could not call the API: " + www.error, this);

                        if (loadIcon)
                           record.Icon = defaultSongIcon;
                     }
                  }
               }
            }
            else
            {
               Debug.LogWarning("'record' is null!", this);
            }
         }
         else
         {
            if (!loggedTokenNull)
            {
               loggedTokenNull = true;
               Debug.LogWarning(tokenNull, this);
            }
         }

         if (!isInternal)
            onQueryComplete(id);
      }

      private string createSongArtUrl(Radio.Model.RecordInfo record)
      {
         System.Text.StringBuilder sb = new System.Text.StringBuilder();
         sb.Append(songArtUrl);
         sb.Append("?");

         if (!string.IsNullOrEmpty(record.Artist))
         {
            sb.Append("artist=");
            sb.Append(record.Artist.Trim());
         }

         if (!string.IsNullOrEmpty(record.Title))
         {
            sb.Append("&title=");
            sb.Append(record.Title.Trim());
         }

         if (Resolution != Model.ImageResolution.med)
         {
            sb.Append("&res=");
            sb.Append(Resolution.ToString());
         }

         sb.Append("&partner_token=");
         sb.Append(Token.Trim());

         return System.Uri.EscapeUriString(sb.ToString());
      }

      #endregion


      #region DARStation

      protected IEnumerator queryDARStation(string id, Radio.Model.RadioStation station, bool loadIcon, bool isInternal)
      {
         if (!string.IsNullOrEmpty(Token))
         {
            if (station != null)
            {
               TotalDARStationRequests++;

               string apiCall = createDARStationUrl(station);

               using (UnityWebRequest www = UnityWebRequest.Get(apiCall))
               {
                  www.timeout = Radio.Util.Constants.MAX_WEB_LOAD_WAIT_TIME;

                  www.downloadHandler = new DownloadHandlerBuffer();
                  yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                  if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
                  if (!www.isHttpError && !www.isNetworkError)
#endif
                  {
                     DARStationsStations = Common.Util.XmlHelper.DeserializeFromString<Model.DARStations.Stations>(www.downloadHandler.text, false);

                     if (DARStationsStations?.Station != null)
                     {
                        station.Name = DARStationsStations.Station.Callsign.Trim();
                        station.Description = Radio.Util.Helper.ClearLineEndings(DARStationsStations.Station.Description.Trim());
                        station.Genres = Radio.Util.Helper.ClearLineEndings(DARStationsStations.Station.Genre.Trim());
                        station.Station = DARStationsStations.Station.Websiteurl.Trim();
                        station.City = DARStationsStations.Station.City.Trim();
                        station.Country = DARStationsStations.Station.Country.Trim();
                        station.Language = DARStationsStations.Station.Language.Trim();

                        string url = DARStationsStations.Station.Imageurl.Trim();

                        //Debug.Log("ART: " + url);

                        if (Radio.Util.Helper.isValidURL(url))
                        {
                           station.IconUrl = url;

                           if (loadIcon && station.Icon == null)
                              yield return Radio.Tool.LoadIcon.Load(station);
                        }
                        else
                        {
                           if (loadIcon)
                              station.Icon = defaultStationIcon;
                        }
                     }
                     else
                     {
                        Debug.LogWarning("DARStation could not find any data: " + station, this);

                        if (loadIcon)
                           station.Icon = defaultStationIcon;
                     }
                  }
                  else
                  {
                     Debug.LogWarning("Could not call the API: " + www.error, this);

                     if (loadIcon)
                        station.Icon = defaultStationIcon;
                  }
               }
            }
            else
            {
               Debug.LogWarning("'station' is null!", this);
            }
         }
         else
         {
            if (!loggedTokenNull)
            {
               loggedTokenNull = true;
               Debug.LogWarning(tokenNull, this);
            }
         }

         if (!isInternal)
            onQueryComplete(id);
      }

      private string createDARStationUrl(Radio.Model.RadioStation station)
      {
         System.Text.StringBuilder sb = new System.Text.StringBuilder();
         sb.Append(DARStationUrl);
         sb.Append("?");

         if (station.GetType() == typeof(Model.RadioStationExt))
         {
            sb.Append("station_id=");
            sb.Append(((Model.RadioStationExt)station).StationId.Trim());
         }
         else
         {
            sb.Append("callsign=");
            sb.Append(station.Name.StartsWith("1.fm", System.StringComparison.OrdinalIgnoreCase) ? station.Name.Trim().Substring(7) : station.Name.Trim());
         }

         sb.Append("&partner_token=");
         sb.Append(Token.Trim());

         return System.Uri.EscapeUriString(sb.ToString());
      }

      #endregion


      #region Event-trigger methods

      protected virtual void onQueryComplete(string id)
      {
         if (Radio.Util.Config.DEBUG)
            Debug.Log("onQueryComplete: " + id, this);

         if (!Radio.Util.Helper.isEditorMode)
            onQueryCompleted?.Invoke(id);

         OnQueryComplete?.Invoke(id);
      }

      #endregion
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)