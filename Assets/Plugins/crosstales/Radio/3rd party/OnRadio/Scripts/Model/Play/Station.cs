using System.Xml.Serialization;

namespace Crosstales.Radio.OnRadio.Model.Play
{
   /// <summary>Model of a station.</summary>
   [XmlRoot(ElementName = "station")]
   [System.SerializableAttribute]
   public class Station
   {
      [XmlElement(ElementName = "callsign")] public string Callsign { get; set; }
      [XmlElement(ElementName = "genre")] public string Genre { get; set; }
      [XmlElement(ElementName = "band")] public string Band { get; set; }
      [XmlElement(ElementName = "artist")] public string Artist { get; set; }
      [XmlElement(ElementName = "title")] public string Title { get; set; }
      [XmlElement(ElementName = "songstamp")] public string Songstamp { get; set; }
      [XmlElement(ElementName = "seconds_remaining")] public string Seconds_remaining { get; set; }
      [XmlElement(ElementName = "station_id")] public string Station_id { get; set; }

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_START);

         result.Append("Callsign='");
         result.Append(Callsign);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Genre='");
         result.Append(Genre);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Band='");
         result.Append(Band);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Artist='");
         result.Append(Artist);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Title='");
         result.Append(Title);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Songstamp='");
         result.Append(Songstamp);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Seconds_remaining='");
         result.Append(Seconds_remaining);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Station_id='");
         result.Append(Station_id);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Radio.Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)