using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.OnRadio.Demo
{
   /// <summary>Query for the Playlist service.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_demo_1_1_query_playlist.html")]
   public class QueryPlaylist : MonoBehaviour
   {
      /// <summary>'PlaylistService' from the scene.</summary>
      [Tooltip("'PlaylistService' from the scene.")] public OnRadio.Service.PlaylistService Service;

      public Dropdown Genres;

      public void Start()
      {
         if (Genres != null)
         {
            System.Collections.Generic.List<Dropdown.OptionData> options = (from object arp in System.Enum.GetValues(typeof(OnRadio.Model.Genre)) select new Dropdown.OptionData(arp.ToString())).ToList();

            if (Genres != null)
            {
               Genres.ClearOptions();
               Genres.AddOptions(options);
            }
         }
      }

      public void GenresDropdownChanged(int index)
      {
         Service.Genre = (OnRadio.Model.Genre)index;
      }

      public void SetArtist(string artist)
      {
         Service.Artist = artist;
      }

      public void SetTitle(string title)
      {
         Service.Title = title;
      }

      public void SetCallsign(string call)
      {
         Service.Callsign = call;
      }

      public void SetCity(string city)
      {
         Service.City = city;
      }

      public void SetCountry(string country)
      {
         Service.Country = country;
      }

      public void SetLanguage(string lang)
      {
         Service.Language = lang;
      }
   }
}