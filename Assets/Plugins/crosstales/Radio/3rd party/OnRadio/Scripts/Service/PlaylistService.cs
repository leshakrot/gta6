using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Crosstales.Radio.OnRadio.Service
{
   /// <summary>Playlist service implementation.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_service_1_1_playlist_service.html")]
   public class PlaylistService : BaseService
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Artist")] [Header("Filter"), Tooltip("Artist of the song."), SerializeField]
      private string artist = "Pink Floyd";

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Title")] [Tooltip("Title of the song."), SerializeField]
      private string title;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Callsign")] [Tooltip("Callsign of the radio station."), SerializeField]
      private string callsign;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Genre")] [Tooltip("Genre of the radio station."), SerializeField]
      private Model.Genre genre = Model.Genre.All;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("City")] [Tooltip("City of the radio station."), SerializeField]
      private string city;

      //private string state; //2-letter abbreviation

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Country")] [Tooltip("Country of the radio station (ISO 3166-1, e.g. 'ch')."), SerializeField]
      private string country; //ISO 3166-1 (Alpha 2)

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Language")] [Tooltip("Language of the radio station (like 'german')."), SerializeField]
      private string language; //like german

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("International")] [Tooltip("Include non-US (international) stations (default: true)."), SerializeField]
      private bool international = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Limit")] [Tooltip("Limit the number of results (default: 20)"), SerializeField, Range(1, 50)]
      private int limit = 20;

      private const string baseurl = "https://apidarfm.global.ssl.fastly.net/playlist.php";

      #endregion


      #region Properties

      /// <summary>Artist of the song</summary>
      public string Artist
      {
         get => artist;
         set => artist = value;
      }

      /// <summary>Title of the song.</summary>
      public string Title
      {
         get => title;
         set => title = value;
      }

      /// <summary>Callsign of the radio station.</summary>
      public string Callsign
      {
         get => callsign;
         set => callsign = value;
      }

      /// <summary>Genre of the radio station.</summary>
      public OnRadio.Model.Genre Genre
      {
         get => genre;
         set => genre = value;
      }

      /// <summary>City of the radio station.</summary>
      public string City
      {
         get => city;
         set => city = value;
      }

      /// <summary>Country of the radio station (ISO 3166-1, e.g. 'ch').</summary>
      public string Country
      {
         get => country;
         set => country = value;
      }

      /// <summary>Language of the radio station (like 'german').</summary>
      public string Language
      {
         get => language;
         set => language = value;
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

      public Model.Play.Playlist Songs { get; private set; }


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
            TotalPlaylistRequests++;

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
                  Songs = Common.Util.XmlHelper.DeserializeFromString<Model.Play.Playlist>(www.downloadHandler.text, false);

                  if (Songs != null && Songs.Station.Count > 0)
                  {
                     foreach (Model.Play.Station song in Songs.Station)
                     {
                        Model.RadioStationExt station = new Model.RadioStationExt(song.Callsign.Trim(), song.Station_id.Trim());
                        Model.RecordInfoExt record = new Model.RecordInfoExt(song.Title.Trim(), song.Artist.Trim(), station);

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

         sb.Append("q=");

         if (!string.IsNullOrEmpty(artist))
         {
            sb.Append(" @artist ");
            sb.Append(artist.Trim());
         }

         if (!string.IsNullOrEmpty(title))
         {
            sb.Append(" @title ");
            sb.Append(title.Trim());
         }

         if (!string.IsNullOrEmpty(callsign))
         {
            sb.Append(" @callsign ");
            sb.Append(callsign.Trim());
         }

         if (genre != Model.Genre.All)
         {
            sb.Append(" @genre ");
            sb.Append(Util.Helper.getGenre(genre));
         }

         if (!string.IsNullOrEmpty(city))
         {
            sb.Append(" @city ");
            sb.Append(city.Trim());
         }
/*
         if (!string.IsNullOrEmpty(state))
         {
            sb.Append(" @state ");
            sb.Append(state.Trim());
         }
*/
         if (!string.IsNullOrEmpty(country))
         {
            sb.Append(" @country ");
            sb.Append(country.Trim());
         }

         if (!string.IsNullOrEmpty(language))
         {
            sb.Append(" @language ");
            sb.Append(language.Trim());
         }

         if (international)
            sb.Append("&intl=1");

         if (limit != 20)
         {
            sb.Append("&pagesize=");
            sb.Append(limit);
         }

         sb.Append("+mp3&partner_token=");
         sb.Append(Token.Trim());

         return System.Uri.EscapeUriString(sb.ToString());
      }

      #endregion
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)