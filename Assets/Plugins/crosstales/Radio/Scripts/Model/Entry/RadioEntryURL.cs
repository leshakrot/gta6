using UnityEngine;

namespace Crosstales.Radio.Model.Entry
{
   /// <summary>Model for an URL entry.</summary>
   [System.Serializable]
   public class RadioEntryURL : BaseRadioEntry
   {
      #region Variables

      /// <summary>URL (add the protocol-type 'http://', 'file://' etc.) with the radios.</summary>
      [Header("Source Settings")] [Tooltip("URL (add the protocol-type 'http://', 'file://' etc.) with the radios.")]
      public string URL;

      /// <summary>Prefixes for URLs, like 'http://' (default: URLPrefix.None).</summary>
      [Tooltip("Prefixes for URLs, like 'http://' (default: URLPrefix.None).")] public Enum.URLPrefix Prefix = Enum.URLPrefix.None;

      /// <summary>Data format of the data with the radios (default: DataFormatURL.Stream).</summary>
      [Tooltip("Data format of the resource with the radios (default: DataFormatURL.Stream).")] public Enum.DataFormatURL DataFormat = Enum.DataFormatURL.Stream;

      /// <summary>Reads only the given number of radio stations (default: : 0 (= all)).</summary>
      [Tooltip("Reads only the given number of radio stations (default: : 0 (= all))")] public int ReadNumberOfStations;

      #endregion


      #region Properties

      /// <summary>Returns the final URL including an optional prefix.</summary>
      /// <returns>Final URL including an optional prefix.</returns>
      public string FinalURL
      {
         get
         {
            switch (Prefix)
            {
               case Enum.URLPrefix.Http:
                  return Util.Constants.PREFIX_HTTP + URL.Trim();
               case Enum.URLPrefix.Https:
                  return Util.Constants.PREFIX_HTTPS + URL.Trim();
               case Enum.URLPrefix.File:
                  return Util.Helper.ValidURLFromFilePath(URL);
               case Enum.URLPrefix.PersistentDataPath:
                  return Util.Helper.ValidURLFromFilePath(Application.persistentDataPath + '/' + URL.Trim());
               case Enum.URLPrefix.DataPath:
                  return Util.Helper.ValidURLFromFilePath(Application.dataPath + '/' + URL.Trim());
               case Enum.URLPrefix.TempPath:
                  return Util.Helper.ValidURLFromFilePath(Util.Constants.PREFIX_TEMP_PATH + URL.Trim());
               default:
                  return URL.Trim();
            }
         }
      }

      #endregion


      #region Constructors

      /// <summary>Constructor for a RadioEntryURL.</summary>
      /// <param name="entry">BaseRadioEntry as base.</param>
      /// <param name="url">Stream-URL of the station.</param>
      /// <param name="dataFormat">Data format of the data with the radios (default: DataFormatURL.Stream, optional).</param>
      /// <param name="readNumberOfStations">Reads only the given number of radio stations (default: : 0 (= all), optional).</param>
      public RadioEntryURL(BaseRadioEntry entry, string url, Enum.DataFormatURL dataFormat = Enum.DataFormatURL.Stream, int readNumberOfStations = 0) : base(entry.Name, entry.ForceName, entry.EnableSource, entry.Station, entry.Genres, entry.Rating, entry.Description, entry.Icon, entry.IconUrl, entry.City, entry.Country, entry.Language, entry.Format, entry.Bitrate, entry.ChunkSize, entry.BufferSize, entry.ExcludedCodec, entry.AllowOnlyHTTPS)
      {
         URL = url;
         DataFormat = dataFormat;
         ReadNumberOfStations = readNumberOfStations;
      }

      /// <summary>Constructor for a RadioEntryURL.</summary>
      /// <param name="entry">RadioStation as base.</param>
      /// <param name="url">Stream-URL of the station.</param>
      /// <param name="dataFormat">Data format of the data with the radios (default: DataFormatURL.Stream, optional).</param>
      /// <param name="readNumberOfStations">Reads only the given number of radio stations (default: : 0 (= all), optional).</param>
      public RadioEntryURL(RadioStation entry, string url, Enum.DataFormatURL dataFormat = Enum.DataFormatURL.Stream, int readNumberOfStations = 0) : base(entry.Name, true, true, entry.Station, entry.Genres, entry.Rating, entry.Description, entry.Icon, entry.IconUrl, entry.City, entry.Country, entry.Language, entry.Format, entry.Bitrate, entry.ChunkSize, entry.BufferSize, entry.ExcludedCodec, entry.AllowOnlyHTTPS)
      {
         URL = url;
         DataFormat = dataFormat;
         ReadNumberOfStations = readNumberOfStations;
      }

      #endregion


      #region Overridden methods

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder(base.ToString());

         result.Append(GetType().Name);
         result.Append(Util.Constants.TEXT_TOSTRING_START);

         result.Append("URL='");
         result.Append(URL);
         result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Prefix='");
         result.Append(Prefix);
         result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("DataFormat='");
         result.Append(DataFormat);
         result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("ReadNumberOfStations='");
         result.Append(ReadNumberOfStations);
         result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }

      #endregion
   }
}
// © 2016-2021 crosstales LLC (https://www.crosstales.com)