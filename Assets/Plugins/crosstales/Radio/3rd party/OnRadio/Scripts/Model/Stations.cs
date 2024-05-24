using System.Xml.Serialization;

namespace Crosstales.Radio.OnRadio.Model
{
   /// <summary>Model of a station.</summary>
   [XmlRoot(ElementName = "stations")]
   [System.SerializableAttribute]
   public class Stations
   {
      [XmlElement(ElementName = "url")] public string Url { get; set; }
      [XmlElement(ElementName = "encoding")] public string Encoding { get; set; }
      [XmlElement(ElementName = "callsign")] public string Callsign { get; set; }
      [XmlElement(ElementName = "websiteurl")] public string Websiteurl { get; set; }

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_START);

         result.Append("Url='");
         result.Append(Url);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Encoding='");
         result.Append(Encoding);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Callsign='");
         result.Append(Callsign);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Websiteurl='");
         result.Append(Websiteurl);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Radio.Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)