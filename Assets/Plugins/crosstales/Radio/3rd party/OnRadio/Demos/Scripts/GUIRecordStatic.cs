using UnityEngine;

namespace Crosstales.Radio.OnRadio.Demo
{
   /// <summary>GUI for a record.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_demo_1_1_g_u_i_record_static.html")]
   public class GUIRecordStatic : BaseGUIStatic
   {
      #region Properties

      public override OnRadio.Model.RecordInfoExt Record
      {
         get => record;

         set
         {
            record = value;

            TitleText.text = Record.Artist + " - " + Record.Title;
            SubText.text = Record.Station.Name;
         }
      }

      #endregion


      #region MonoBehaviour methods

      protected override void Start()
      {
         base.Start();

         if (Record != null)
         {
            TitleText.text = Record.Artist + " - " + Record.Title;
            SubText.text = Record.Station.Name;
         }
      }

      #endregion


      #region Callback methods

      protected override void onRecordChange(Crosstales.Radio.Model.RadioStation station, Crosstales.Radio.Model.RecordInfo newrecord)
      {
         uidQuery = null;

         if (station.Equals(Record.Station) && !string.IsNullOrEmpty(newrecord.Artist) && !string.IsNullOrEmpty(newrecord.Title))
         {
            TitleText.text = newrecord.Artist + " - " + newrecord.Title;
            SubText.text = Record.Station.Name;
         }
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)