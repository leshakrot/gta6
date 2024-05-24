﻿namespace Crosstales.Radio.Util
{
   /// <summary>Configuration for the asset.</summary>
   public static class Config
   {
      #region Variables

      /// <summary>Enable or disable debug logging for the asset.</summary>
      public static bool DEBUG = Constants.DEFAULT_DEBUG || Constants.DEV_DEBUG;

      /// <summary>Default bitrate for a RadioPlayer.</summary>
      public static int DEFAULT_BITRATE = Constants.DEFAULT_DEFAULT_BITRATE;

      /// <summary>Default chunk-size for a RadioPlayer.</summary>
      public static int DEFAULT_CHUNKSIZE = Constants.DEFAULT_DEFAULT_CHUNKSIZE;

      /// <summary>Default buffer-size for a RadioPlayer.</summary>
      public static int DEFAULT_BUFFERSIZE = Constants.DEFAULT_DEFAULT_BUFFERSIZE;

      /// <summary>Default cachestream-size for a RadioPlayer.</summary>
      public static int DEFAULT_CACHESTREAMSIZE = Constants.DEFAULT_DEFAULT_CACHESTREAMSIZE;

      /// <summary>Maximal cachestream-size for a RadioPlayer.</summary>
      public static int MAX_CACHESTREAMSIZE = Constants.DEFAULT_MAX_CACHESTREAMSIZE; //TODO not exposed in the UI - remove or document?

      /// <summary>Is the configuration loaded?</summary>
      public static bool isLoaded;

      #endregion

#if UNITY_EDITOR

      #region Public static methods

      /// <summary>Resets all changeable variables to their default value.</summary>
      public static void Reset()
      {
         if (!Constants.DEV_DEBUG)
            DEBUG = Constants.DEFAULT_DEBUG;

         DEFAULT_BITRATE = Constants.DEFAULT_DEFAULT_BITRATE;
         DEFAULT_CHUNKSIZE = Constants.DEFAULT_DEFAULT_CHUNKSIZE;
         DEFAULT_BUFFERSIZE = Constants.DEFAULT_DEFAULT_BUFFERSIZE;
         DEFAULT_CACHESTREAMSIZE = Constants.DEFAULT_DEFAULT_CACHESTREAMSIZE;
         MAX_CACHESTREAMSIZE = Constants.DEFAULT_MAX_CACHESTREAMSIZE;
      }

      /// <summary>Loads all changeable variables.</summary>
      public static void Load()
      {
         if (!Constants.DEV_DEBUG)
         {
            if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DEBUG))
               DEBUG = Common.Util.CTPlayerPrefs.GetBool(Constants.KEY_DEBUG);
         }
         else
         {
            DEBUG = Constants.DEV_DEBUG;
         }

         if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DEFAULT_BITRATE))
            DEFAULT_BITRATE = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_DEFAULT_BITRATE);

         if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DEFAULT_CHUNKSIZE))
            DEFAULT_CHUNKSIZE = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_DEFAULT_CHUNKSIZE);

         if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DEFAULT_BUFFERSIZE))
            DEFAULT_BUFFERSIZE = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_DEFAULT_BUFFERSIZE);

         if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_DEFAULT_CACHESTREAMSIZE))
            DEFAULT_CACHESTREAMSIZE = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_DEFAULT_CACHESTREAMSIZE);

         if (Common.Util.CTPlayerPrefs.HasKey(Constants.KEY_MAX_CACHESTREAMSIZE))
            MAX_CACHESTREAMSIZE = Common.Util.CTPlayerPrefs.GetInt(Constants.KEY_MAX_CACHESTREAMSIZE);

         isLoaded = true;
      }

      /// <summary>Saves all changeable variables.</summary>
      public static void Save()
      {
         if (!Constants.DEV_DEBUG)
            Common.Util.CTPlayerPrefs.SetBool(Constants.KEY_DEBUG, DEBUG);

         Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_DEFAULT_BITRATE, DEFAULT_BITRATE);
         Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_DEFAULT_CHUNKSIZE, DEFAULT_CHUNKSIZE);
         Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_DEFAULT_BUFFERSIZE, DEFAULT_BUFFERSIZE);
         Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_DEFAULT_CACHESTREAMSIZE, DEFAULT_CACHESTREAMSIZE);
         Common.Util.CTPlayerPrefs.SetInt(Constants.KEY_MAX_CACHESTREAMSIZE, MAX_CACHESTREAMSIZE);

         Common.Util.CTPlayerPrefs.Save();
      }

      #endregion

#endif
   }
}
// © 2017-2021 crosstales LLC (https://www.crosstales.com)