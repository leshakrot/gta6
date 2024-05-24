using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Crosstales.Radio.OnRadio.Service
{
   /// <summary>Topsongs service implementation.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_service_1_1_topsongs_service.html")]
   public class TopsongsService : BaseService
   {
      #region Variables

      /// <summary>Genre for the search. 'All' will lead to 'Hit Music'.</summary>
      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Genre")] [Header("Filter")] [Tooltip("Genre for the search. 'All' will lead to 'Hit Music'."), SerializeField]
      private OnRadio.Model.Genre genre;

      /// <summary>Include non-US (international) stations (default: true).</summary>
      [UnityEngine.Serialization.FormerlySerializedAsAttribute("International")] [Tooltip("Include non-US (international) stations (default: true)."), SerializeField]
      private bool international = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Limit")] [Tooltip("Limit the number of results (default: 20)"), Range(1, 50), SerializeField]
      private int limit = 20;

      private bool currentlyPlaying = true;

      private const string baseurl = "https://apidarfm.global.ssl.fastly.net/topsongs.php";

      #endregion


      #region Properties

      /// <summary>Genre for the search. 'All' will lead to 'Hit Music'.</summary>
      public OnRadio.Model.Genre Genre
      {
         get => genre;
         set => genre = value;
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
            TotalTopsongsRequests++;

            clearData();

            string apiCall = createUrl(genre == OnRadio.Model.Genre.All ? Util.Helper.getGenre(OnRadio.Model.Genre.Music) : Util.Helper.getGenre(genre));

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
                     Debug.LogWarning("No songs for genre '" + genre + "' found.", this);
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

      private string createUrl(string _genre)
      {
         StringBuilder sb = new StringBuilder();

         sb.Append(baseurl);
         sb.Append("?");

         sb.Append("q=");
         sb.Append(_genre);
         sb.Append("&mp3only=1");

         if (international)
            sb.Append("&intl=1");

         if (limit != 20)
         {
            sb.Append("&page_size=");
            sb.Append(limit);
         }

         if (!currentlyPlaying)
            sb.Append("&nonplaying=1");

         sb.Append("&partner_token=");
         sb.Append(Token.Trim());

         //return sb.ToString();
         return System.Uri.EscapeUriString(sb.ToString());
      }

      #endregion
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)