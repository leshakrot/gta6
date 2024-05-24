using System.Xml.Serialization;

namespace Crosstales.Radio.OnRadio.Model
{
   /// <summary>Model of a playlist.</summary>
   [XmlRoot(ElementName = "playlist")]
   [System.SerializableAttribute]
   public class Playlist
   {
      private string _callsign;
      private string _stationId;
      private string _genre;
      private string _band;
      private string _artist;
      private string _title;
      private string _songstamp;
      private string _secondsRemaining;

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

      [XmlElement(ElementName = "genre")]
      public string Genre
      {
         get => _genre;
         set => _genre = value.Trim();
      }

      [XmlElement(ElementName = "band")]
      public string Band
      {
         get => _band;
         set => _band = value.Trim();
      }

      [XmlElement(ElementName = "artist")]
      public string Artist
      {
         get => _artist;
         set => _artist = value.Trim();
      }

      [XmlElement(ElementName = "title")]
      public string Title
      {
         get => _title;
         set => _title = value.Trim();
      }

      [XmlElement(ElementName = "songstamp")]
      public string Songstamp
      {
         get => _songstamp;
         set => _songstamp = value.Trim();
      }

      [XmlElement(ElementName = "seconds_remaining")]
      public string Seconds_remaining
      {
         get => _secondsRemaining;
         set => _secondsRemaining = value.Trim();
      }

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_START);

         result.Append("Callsign='");
         result.Append(Callsign);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Station_id='");
         result.Append(Station_id);
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
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Radio.Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)