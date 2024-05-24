using System.Xml.Serialization;

namespace Crosstales.Radio.OnRadio.Model
{
   /// <summary>Model of a song.</summary>
   [XmlRoot(ElementName = "song")]
   [System.SerializableAttribute]
   public class Song
   {
      private string _songartist;
      private string _songtitle;
      private string _currentlyPlaying;
      private string _callsign;
      private string _stationId;
      private string _band;

      [XmlElement(ElementName = "songartist")]
      public string Songartist
      {
         get => _songartist;
         set => _songartist = value.Trim();
      }

      [XmlElement(ElementName = "songtitle")]
      public string Songtitle
      {
         get => _songtitle;
         set => _songtitle = value.Trim();
      }

      [XmlElement(ElementName = "currently_playing")]
      public string Currently_playing
      {
         get => _currentlyPlaying;
         set => _currentlyPlaying = value.Trim();
      }

      [XmlElement(ElementName = "callsign")]
      public string Callsign
      {
         get => _callsign;
         set => _callsign = value.Trim();
      }

      [XmlElement(ElementName = "station_id")]
      public string Station_id
      {
         get => _stationId;
         set => _stationId = value.Trim();
      }

      [XmlElement(ElementName = "band")]
      public string Band
      {
         get => _band;
         set => _band = value.Trim();
      }

      [XmlElement(ElementName = "playlist")] public Playlist Playlist { get; set; }

      [XmlElement(ElementName = "uberurl")] public Uberurl Uberurl { get; set; }

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_START);

         result.Append("Songartist='");
         result.Append(Songartist);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Songtitle='");
         result.Append(Songtitle);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Currently_playing='");
         result.Append(Currently_playing);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Callsign='");
         result.Append(Callsign);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Station_id='");
         result.Append(Station_id);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Band='");
         result.Append(Band);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Playlist='");
         result.Append(Playlist);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Uberurl='");
         result.Append(Uberurl);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Radio.Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)