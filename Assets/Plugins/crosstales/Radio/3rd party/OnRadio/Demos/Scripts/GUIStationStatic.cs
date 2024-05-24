using UnityEngine;

namespace Crosstales.Radio.OnRadio.Demo
{
   /// <summary>GUI for a station.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_demo_1_1_g_u_i_station_static.html")]
   public class GUIStationStatic : BaseGUIStatic
   {
      #region Properties

      public override OnRadio.Model.RecordInfoExt Record
      {
         get => record;

         set
         {
            record = value;

            TitleText.text = Record.Station.Name;
            SubText.text = Record.Artist + " - " + Record.Title;
         }
      }

      #endregion


      #region MonoBehaviour methods

      protected override void Start()
      {
         base.Start();

         if (Record != null)
         {
            TitleText.text = Record.Station.Name;
            SubText.text = Record.Artist + " - " + Record.Title;
         }
      }

      #endregion


      #region Callback methods

      protected override void onRecordChange(Crosstales.Radio.Model.RadioStation station, Crosstales.Radio.Model.RecordInfo newrecord)
      {
         if (station.Equals(Record.Station))
         {
            if (Radio.Util.Config.DEBUG)
               Debug.Log("onRecordChange: " + station + " - " + newrecord, this);

            uidQuery = null;

            TitleText.text = Record.Station.Name;

            if (!string.IsNullOrEmpty(newrecord.Artist) && !string.IsNullOrEmpty(newrecord.Title))
               SubText.text = Record.Artist + " - " + Record.Title;
         }
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)