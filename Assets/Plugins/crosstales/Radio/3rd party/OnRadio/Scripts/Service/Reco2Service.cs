using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Crosstales.Radio.OnRadio.Service
{
   /// <summary>Reco2 service implementation.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_service_1_1_reco2_service.html")]
   public class Reco2Service : BaseService
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Artist")] [Header("Filter"), Tooltip("Artist of the song."), SerializeField] private string artist;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("International")] [Tooltip("Include non-US (international) stations (default: true)."), SerializeField] private bool international = true;

      /// <summary>Limit the number of results (default: 25).</summary>
      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Limit")] [Tooltip("Limit the number of results (default: 25)"), SerializeField, Range(1, 50)] private int limit = 25;

      //[HideInInspector]
      //public Model.Songs Songs { get; private set; }

      private const string baseurl = "https://apidarfm.global.ssl.fastly.net/reco2.php";

      #endregion


      #region Properties

      /// <summary>Artist of the song</summary>
      public string Artist
      {
         get => artist;
         set => artist = value;
      }

      /// <summary>Include non-US (international) stations.</summary>
      public bool International
      {
         get => international;
         set => international = value;
      }

      /// <summary>Limit the number of results (range 1-50).</summary>
      public int Limit
      {
         get => limit;
         set => limit = Mathf.Clamp(value, 1, 50);
      }

      public Model.Songs Songs { get; protected set; }

      protected override QueryCompleteEvent onQueryCompleted => OnQueryCompleted;

      #endregion


      #region Events

      [Header("Events")] public QueryCompleteEvent OnQueryCompleted;

      #endregion


      #region Private methods

      protected override IEnumerator query(string id)
      {
         if (!string.IsNullOrEmpty(Token))
         {
            TotalReco2Requests++;

            clearData();

            string apiCall = createUrl();

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
                  Songs = Common.Util.XmlHelper.DeserializeFromString<Model.Songs>(www.downloadHandler.text, false);

                  if (Songs != null && Songs.Song.Count > 0)
                  {
                     foreach (Model.Song song in Songs.Song)
                     {
                        Model.RadioStationExt station = new Model.RadioStationExt(song.Callsign.Trim(), song.Station_id.Trim());
                        Model.RecordInfoExt record = new Model.RecordInfoExt(song.Songtitle.Trim(), song.Songartist.Trim(), station);

                        if (!Stations.Contains(station) && !Records.Contains(record))
                        {
                           if (EnableStation)
                           {
                              if (AwaitStationQuery)
                              {
                                 yield return queryStation(id, station, true);
                              }
                              else
                              {
                                 StartCoroutine(queryStation(id, station, true));
                              }
                           }

                           if (EnableSongArt)
                           {
                              if (AwaitSongArtQuery)
                              {
                                 yield return querySongArt(id, record, LoadRecordIcon, true);
                              }
                              else
                              {
                                 StartCoroutine(querySongArt(id, record, LoadRecordIcon, true));
                              }
                           }

                           if (EnableDARStation)
                           {
                              if (AwaitDARStationQuery)
                              {
                                 yield return queryDARStation(id, station, LoadStationIcon, true);
                              }
                              else
                              {
                                 StartCoroutine(queryDARStation(id, station, LoadStationIcon, true));
                              }
                           }

                           Stations.Add(station);
                           Records.Add(record);
                        }
                     }
                  }
                  else
                  {
                     Debug.LogWarning("No songs found.", this);
                  }
               }
               else
               {
                  Debug.LogWarning("Could not query the API: " + www.error, this);
               }
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

         onQueryComplete(id);
      }

      private string createUrl()
      {
         StringBuilder sb = new StringBuilder();
         sb.Append(baseurl);
         sb.Append("?");

         if (limit != 25)
         {
            sb.Append("howmany=");
            sb.Append(limit);
         }

         if (international)
            sb.Append("&intl=1");

         sb.Append("&mp3only=1");

         if (!string.IsNullOrEmpty(artist))
         {
            sb.Append("&artist=");
            sb.Append(artist.Trim());
         }

         sb.Append("&partner_token=");
         sb.Append(Token.Trim());

         return System.Uri.EscapeUriString(sb.ToString());
      }

      #endregion
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)