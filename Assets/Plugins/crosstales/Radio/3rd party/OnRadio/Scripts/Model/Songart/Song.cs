using System.Xml.Serialization;

namespace Crosstales.Radio.OnRadio.Model.Songart
{
   /// <summary>Model of a song.</summary>
   [XmlRoot(ElementName = "song")]
   [System.SerializableAttribute]
   public class Song
   {
      [XmlElement(ElementName = "arturl")] public string Arturl { get; set; }
      [XmlElement(ElementName = "artist")] public string Artist { get; set; }
      [XmlElement(ElementName = "title")] public string Title { get; set; }
      [XmlElement(ElementName = "album")] public string Album { get; set; }
      [XmlElement(ElementName = "size")] public string Size { get; set; }

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_START);

         result.Append("Arturl='");
         result.Append(Arturl);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Artist='");
         result.Append(Artist);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Title='");
         result.Append(Title);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Album='");
         result.Append(Album);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Size='");
         result.Append(Size);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Radio.Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)