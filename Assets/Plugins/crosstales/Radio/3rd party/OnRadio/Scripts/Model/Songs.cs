using System.Xml.Serialization;
using System.Collections.Generic;

namespace Crosstales.Radio.OnRadio.Model
{
   /// <summary>Model of a song holder.</summary>
   [XmlRoot(ElementName = "songs")]
   [System.SerializableAttribute]
   public class Songs
   {
      [XmlElement(ElementName = "song")] public List<Song> Song { get; set; }

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_START);

         result.Append("Song='");
         result.Append(Song.CTDump());
         result.Append(Radio.Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Radio.Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }
   }
}
// © 2019-2021 crosstales LLC (https://www.crosstales.com)