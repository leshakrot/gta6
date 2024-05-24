namespace Crosstales.Radio.OnRadio.Model
{
   /// <summary>Extended RecordInfo.</summary>
   public class RecordInfoExt : Crosstales.Radio.Model.RecordInfo
   {
      //public string StationId;

      public RadioStationExt Station;

      public RecordInfoExt(string title, string artist, RadioStationExt station)
      {
         Title = title.Trim();
         Artist = artist.Trim();
         Station = station;
      }
/*
      public override string ToString()
      {
         //return base.ToString();

         System.Text.StringBuilder result = new System.Text.StringBuilder();

         //result.Append(base.ToString());
         //result.Append(Crosstales.Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Station='");
         result.Append(Station);
         result.Append(Crosstales.Radio.Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Crosstales.Radio.Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }
*/
   }
}