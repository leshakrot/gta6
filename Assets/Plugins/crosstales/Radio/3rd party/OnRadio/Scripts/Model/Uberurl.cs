using System.Xml.Serialization;

namespace Crosstales.Radio.OnRadio.Model
{
   /// <summary>Model of a Uberurl.</summary>
   [XmlRoot(ElementName = "uberurl")]
   [System.SerializableAttribute]
   public class Uberurl
   {
      private string _url;
      private string _encoding;
      private string _callsign;
      private string _websiteurl;
      private string _stationId;

      [XmlElement(ElementName = "url")]
      public string Url
      {
         get => _url;
         set => _url = value.Trim();
      }

      [XmlElement(ElementName = "encoding")]
      public string Encoding
      {
         get => _encoding;
         set
         {
            _encoding = value.Trim();
            if (!_encoding.Contains("MP3"))
               UnityEngine.Debug.LogWarning("Not correct encoded: " + this);
         }
      }

      [XmlElement(ElementName = "callsign")]
      public string Callsign
      {
         get => _callsign;
         set => _callsign = value.Trim();
      }

      [XmlElement(ElementName = "websiteurl")]
      public string Websiteurl
      {
         get => _websiteurl;
         set => _websiteurl = value.Trim();
      }

      [XmlElement(ElementName = "station_id")]
      public string Station_id
      {
         get => _stationId;
         set => _stationId = value.Trim();
      }

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