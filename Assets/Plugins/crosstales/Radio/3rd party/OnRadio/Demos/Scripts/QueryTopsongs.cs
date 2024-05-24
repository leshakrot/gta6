using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.OnRadio.Demo
{
   /// <summary>Query for the Topsongs service.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_demo_1_1_query_topsongs.html")]
   public class QueryTopsongs : MonoBehaviour
   {
      /// <summary>'TopsongsService' from the scene.</summary>
      [Tooltip("'TopsongsService' from the scene.")] public OnRadio.Service.TopsongsService Service;

      public Dropdown Genres;

      public void Start()
      {
         System.Collections.Generic.List<Dropdown.OptionData> options = (from object arp in System.Enum.GetValues(typeof(OnRadio.Model.Genre)) select new Dropdown.OptionData(arp.ToString())).ToList();

         if (Genres != null)
         {
            Genres.ClearOptions();
            Genres.AddOptions(options);
         }
      }

      public void GenresDropdownChanged(int index)
      {
         Service.Genre = (OnRadio.Model.Genre)index;
      }
   }
}