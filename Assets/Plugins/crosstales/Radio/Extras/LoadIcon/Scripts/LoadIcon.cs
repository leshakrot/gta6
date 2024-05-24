using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Crosstales.Radio.Tool
{
   /// <summary>Loads an icon for a radio station or a record.</summary>
   public static class LoadIcon
   {
      #region Public methods

      /// <summary>Loads an icon for a station.</summary>
      /// <param name="station">Station for the icon</param>
      public static IEnumerator Load(Model.RadioStation station)
      {
         if (station != null)
         {
            yield return load(station.IconUrl, station, null);
         }
         else
         {
            Debug.LogWarning("'station' is null!'");
         }
      }

      /// <summary>Loads an icon for a record.</summary>
      /// <param name="record">Record for the icon</param>
      public static IEnumerator Load(Model.RecordInfo record)
      {
         if (record != null)
         {
            yield return load(record.IconUrl, null, record);
         }
         else
         {
            Debug.LogWarning("'record' is null!'");
         }
      }

      #endregion

      public static IEnumerator load(string url, Model.RadioStation station, Model.RecordInfo record)
      {
         if (Util.Helper.isValidURL(url))
         {
            using (UnityWebRequest www = new UnityWebRequest(url))
            {
               DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
               www.downloadHandler = texDl;
               www.timeout = 5;

               //Debug.Log("load: "+ url);

               yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
               if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
               if (!www.isHttpError && !www.isNetworkError)
#endif
               {
                  Texture2D tex = texDl.texture;

                  if (station != null)
                     station.Icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));

                  if (record != null)
                     record.Icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
               }
               else
               {
                  Debug.LogWarning("Could not load image: " + www.error);
               }
            }
         }
         else
         {
            Debug.LogWarning("'url' is invalid: " + url);
         }
      }
   }
}
// © 2018-2021 crosstales LLC (https://www.crosstales.com)