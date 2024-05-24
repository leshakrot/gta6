using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

namespace Crosstales.Radio.Provider
{
   /// <summary>Base class for radio providers.</summary>
   [ExecuteInEditMode]
   public abstract class BaseRadioProvider : MonoBehaviour, IRadioProvider
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("ClearStationsOnLoad")] [Header("Load Behaviour")] [Tooltip("Clears all existing stations on 'Load' (default: true)."), SerializeField]
      private bool clearStationsOnLoad = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("LoadOnStart")] [Tooltip("Calls 'Load' on Start (default: true)."), SerializeField]
      private bool loadOnStart = true;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("LoadOnStartInEditor")] [Tooltip("Calls 'Load' on Start in Editor (default: true)."), SerializeField]
      private bool loadOnStartInEditor = true;

      protected readonly System.Collections.Generic.List<string> coRoutines = new System.Collections.Generic.List<string>();

      private System.Collections.Generic.List<Model.RadioStation> stations = new System.Collections.Generic.List<Model.RadioStation>(Util.Constants.INITIAL_LIST_SIZE);

      private bool loadedInEditor = true;

      private bool isReadySent;

      // split chars
      private static readonly char[] splitCharEquals = {'='};
      private static readonly char[] splitCharText = {';'};
      private static readonly char[] splitCharColon = {':'};
      private static readonly char[] splitCharComma = {','};

      #endregion


      #region Properties

      /// <summary>Clears all existing stations on 'Load'.</summary>
      public bool ClearStationsOnLoad
      {
         get => clearStationsOnLoad;
         set => clearStationsOnLoad = value;
      }

      /// <summary>Calls 'Load' on Start.</summary>
      public bool LoadOnStart
      {
         get => loadOnStart;
         set => loadOnStart = value;
      }

      /// <summary>Calls 'Load' on Start in Editor.</summary>
      public bool LoadOnStartInEditor
      {
         get => loadOnStartInEditor;
         set => loadOnStartInEditor = value;
      }

      protected abstract StationsChangeEvent onStationsChanged { get; }

      protected abstract ProviderReadyEvent onProviderReadyEvent { get; }

      #endregion


      #region Events

      /// <summary>An event triggered whenever the stations change.</summary>
      public event StationsChange OnStationsChange;

      /// <summary>An event triggered whenever the provider is ready.</summary>
      public event ProviderReady OnProviderReady;

      #endregion


      #region MonoBehaviour methods

      protected virtual void Start()
      {
         if (LoadOnStart && !Util.Helper.isEditorMode || LoadOnStartInEditor && Util.Helper.isEditorMode)
            Load();

         OnValidate();
      }

      private void Update()
      {
         if (!isReadySent && isReady)
         {
            isReadySent = true;
            onProviderReady();
         }
      }

      protected virtual void OnValidate()
      {
         foreach (Model.Entry.BaseRadioEntry entry in RadioEntries.Where(entry => entry != null))
         {
            if (!entry.isInitialized)
            {
               entry.Format = Model.Enum.AudioFormat.MP3;
               entry.EnableSource = true;

               entry.isInitialized = true;
            }

            entry.Bitrate = entry.Bitrate <= 0 ? Util.Config.DEFAULT_BITRATE : Util.Helper.NearestBitrate(entry.Bitrate, entry.Format);

            if (entry.ChunkSize <= 0)
            {
               entry.ChunkSize = Util.Config.DEFAULT_CHUNKSIZE;
            }
            else if (entry.ChunkSize > Util.Config.MAX_CACHESTREAMSIZE)
            {
               entry.ChunkSize = Util.Config.MAX_CACHESTREAMSIZE;
            }

            if (entry.BufferSize <= 0)
            {
               entry.BufferSize = Util.Config.DEFAULT_BUFFERSIZE;
            }
            else
            {
               switch (entry.Format)
               {
                  case Model.Enum.AudioFormat.MP3:
                  {
                     if (entry.BufferSize < Util.Config.DEFAULT_BUFFERSIZE / 4)
                     {
                        entry.BufferSize = Util.Config.DEFAULT_BUFFERSIZE / 4;
                     }

                     break;
                  }
                  case Model.Enum.AudioFormat.OGG:
                  {
                     if (entry.BufferSize < Util.Constants.MIN_OGG_BUFFERSIZE)
                     {
                        entry.BufferSize = Util.Constants.MIN_OGG_BUFFERSIZE;
                     }

                     break;
                  }
               }

               if (entry.BufferSize < entry.ChunkSize)
               {
                  entry.BufferSize = entry.ChunkSize;
               }
               else if (entry.BufferSize > Util.Config.MAX_CACHESTREAMSIZE)
               {
                  entry.BufferSize = Util.Config.MAX_CACHESTREAMSIZE;
               }
            }
         }
      }

      #endregion


      #region Implemented methods

      public abstract System.Collections.Generic.List<Model.Entry.BaseRadioEntry> RadioEntries { get; }

      public System.Collections.Generic.List<Model.RadioStation> Stations
      {
         get => stations;
         protected set => stations = value;
      }

      public virtual bool isReady
      {
         get
         {
            if (Util.Helper.isEditorMode)
            {
               return loadedInEditor;
            }

            return coRoutines.Count == 0;
         }
      }

      public virtual void Load()
      {
         isReadySent = false;

         if (Util.Helper.isEditorMode)
         {
#if UNITY_EDITOR
            initInEditor();
#endif
         }
         else
         {
            init();
         }
      }

      public void Save(string path)
      {
         if (!string.IsNullOrEmpty(path))
         {
#if (!UNITY_WSA && !UNITY_WEBGL && !UNITY_XBOXONE) || UNITY_EDITOR
            try
            {
               path = path.Replace(Util.Constants.PREFIX_FILE, string.Empty); //remove file://-prefix

               using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
               {
                  file.WriteLine("# " + Util.Constants.ASSET_NAME + " " + Util.Constants.ASSET_VERSION);
                  file.WriteLine("# © 2015-2021 by " + Util.Constants.ASSET_AUTHOR + " (" + Util.Constants.ASSET_AUTHOR_URL + ")");
                  file.WriteLine("#");
                  file.WriteLine("# List of all radio stations from '" + GetType().Name + "'");
                  file.WriteLine("# Created: " + System.DateTime.Now.ToString("dd.MM.yyyy"));
                  file.WriteLine("# Name;Url;DataFormat;AudioFormat;Station (optional);Genres (optional);Bitrate (in kbit/s, optional);Rating (0-5, optional);Description (optional);ExcludeCodec (optional);ChunkSize (in KB, optional);BufferSize (in KB, optional);IconUrl (optional);City (optional);Country (optional);Language (optional)");

                  foreach (Model.RadioStation rs in Stations)
                  {
                     file.WriteLine(rs.ToTextLine());
                  }
               }
            }
            catch (System.IO.IOException ex)
            {
               Debug.LogError("Could not write file: " + ex, this);
            }
#else
            Debug.LogWarning("'Save' is not supported on the currrent platform!", this);
#endif
         }
         else
         {
            Debug.LogWarning("'path' was null or empty! Could not save the data!", this);
         }
      }

      #endregion


      #region Private methods

      protected virtual void init()
      {
         if (ClearStationsOnLoad)
            Stations.Clear();
      }

      protected IEnumerator loadWeb(string uid, Model.Entry.RadioEntryURL entry, bool suppressDoubleStations = false)
      {
         if (!string.IsNullOrEmpty(entry.FinalURL))
         {
            using (UnityWebRequest www = UnityWebRequest.Get(entry.FinalURL))
            {
               www.timeout = Util.Constants.MAX_WEB_LOAD_WAIT_TIME;

               www.downloadHandler = new DownloadHandlerBuffer();
               yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
               if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
               if (!www.isHttpError && !www.isNetworkError)
#endif
               {
                  System.Collections.Generic.List<string> list = Util.Helper.SplitStringToLines(www.downloadHandler.text);

                  yield return null;

                  if (list.Count > 0)
                  {
                     switch (entry.DataFormat)
                     {
                        case Model.Enum.DataFormatURL.M3U:
                           list = Util.Helper.SplitStringToLines(www.downloadHandler.text, false); //dirty workaround
                           fillStationsFromM3U(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                           break;
                        case Model.Enum.DataFormatURL.PLS:
                           fillStationsFromPLS(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                           break;
                        case Model.Enum.DataFormatURL.Text:
                           fillStationsFromText(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                           break;
                        default:
                           Debug.LogWarning("Not implemented!", this);
                           break;
                     }

                     onStationsChange();
                  }
                  else
                  {
                     if (Util.Config.DEBUG)
                        Debug.Log(entry + " - URL: '" + entry.FinalURL + "' does not contain any active radio stations!", this);
                  }
               }
               else
               {
                  Debug.LogWarning(entry + " - Could not load source: '" + entry.FinalURL + "'" + System.Environment.NewLine + www.error + System.Environment.NewLine + "Did you set the correct 'URL'?", this);
               }
            }
         }
         else
         {
            Debug.LogWarning(entry + ": 'URL' is null or empty!" + System.Environment.NewLine + "Please add a valid URL.", this);
         }


         coRoutines.Remove(uid);
      }

      protected IEnumerator loadResource(string uid, Model.Entry.RadioEntryResource entry, bool suppressDoubleStations = false)
      {
         if (entry.Resource != null)
         {
            System.Collections.Generic.List<string> list = Util.Helper.SplitStringToLines(entry.Resource.text);

            yield return null;

            if (list.Count > 0)
            {
               switch (entry.DataFormat)
               {
                  case Model.Enum.DataFormatResource.M3U:
                     list = Util.Helper.SplitStringToLines(entry.Resource.text, false); //dirty workaround
                     fillStationsFromM3U(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                     break;
                  case Model.Enum.DataFormatResource.PLS:
                     fillStationsFromPLS(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                     break;
                  case Model.Enum.DataFormatResource.Text:
                     fillStationsFromText(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                     break;
                  default:
                     Debug.LogWarning("Not implemented!", this);
                     break;
               }

               onStationsChange();
            }
            else
            {
               if (Util.Config.DEBUG)
                  Debug.Log(entry + " - Resource: '" + entry.Resource + "' does not contain any active radio stations!", this);
            }
         }
         else
         {
            Debug.LogWarning(entry + ": resource field 'Resource' is null or empty!" + System.Environment.NewLine + "Please add a valid resource.", this);
         }

         coRoutines.Remove(uid);
      }

      protected IEnumerator loadShoutcast(string uid, Model.Entry.RadioEntryShoutcast entry, bool suppressDoubleStations = false)
      {
         using (UnityWebRequest www = UnityWebRequest.Get(Util.Constants.SHOUTCAST + entry.ShoutcastID.Trim()))
         {
            www.timeout = Util.Constants.MAX_SHOUTCAST_LOAD_WAIT_TIME;

            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.ProtocolError && www.result != UnityWebRequest.Result.ConnectionError)
#else
            if (!www.isHttpError && !www.isNetworkError)
#endif
            {
               System.Collections.Generic.List<string> list = Util.Helper.SplitStringToLines(www.downloadHandler.text);

               yield return null;

               if (list.Count > 0)
               {
                  fillStationsFromPLS(list, entry, 1, suppressDoubleStations);

                  onStationsChange();
               }
               else
               {
                  if (Util.Config.DEBUG)
                     Debug.Log(entry + " - Shoutcast-ID: '" + entry.ShoutcastID + "' does not contain any active radio stations!");
               }
            }
            else
            {
               Debug.LogWarning(entry + " - Could not load Shoutcast-ID: '" + entry.ShoutcastID + "'" + System.Environment.NewLine + www.error + System.Environment.NewLine + "Did you set the correct 'Shoutcast-ID'?", this);
            }
         }

         coRoutines.Remove(uid);
      }

      protected void fillStationsFromM3U(System.Collections.Generic.List<string> list, Model.Entry.BaseRadioEntry entry, int readNumberOfStations = 0, bool suppressDoubleStations = false)
      {
         int stationCount = 0;

         for (int ii = 0; ii < list.Count;)
         {
            string file = string.Empty;
            string title = string.Empty;
            string line = list[ii].Trim();

            if (ii == 0 && !line.CTEquals(Util.Constants.M3U_EXT_ID))
            {
               Debug.LogWarning("File is not in the M3U-format!", this);

               break;
            }

            if (!line.CTContains(Util.Constants.M3U_EXT_ID)) //EXTM3U?
            {
               if (line.CTContains(Util.Constants.M3U_EXT_INF_ID)) //EXTINF?
               {
                  string[] extsplit = line.Split(splitCharColon, System.StringSplitOptions.RemoveEmptyEntries);

                  if (extsplit.Length > 1)
                  {
                     string[] ext2split = extsplit[1].Split(splitCharComma, System.StringSplitOptions.RemoveEmptyEntries);

                     if (ext2split.Length > 1)
                     {
                        title = ext2split[1];
                     }
                  }

                  if (ii + 1 < list.Count)
                  {
                     ii++;
                     line = list[ii];

                     file = line;
                  }
               }
               else if (!string.IsNullOrEmpty(line))
               {
                  file = line;
               }

               if (!string.IsNullOrEmpty(file) && (!entry.AllowOnlyHTTPS || entry.AllowOnlyHTTPS && isHTTPS(file)))
               {
                  Model.RadioStation station = new Model.RadioStation(entry.ForceName ? entry.Name : string.IsNullOrEmpty(title) ? entry.Name : title.Trim(), file.Trim(), entry.Format == Model.Enum.AudioFormat.UNKNOWN ? Util.Helper.AudioFormatFromString(file) : entry.Format, entry.Station, entry.Genres.ToLower(), entry.Bitrate, entry.Rating, entry.Description, entry.Icon, entry.IconUrl, entry.City, entry.Country, entry.Language, entry.ChunkSize, entry.BufferSize, entry.ExcludedCodec);

                  if (!Stations.Contains(station))
                  {
                     Stations.Add(station);

                     stationCount++;

                     if (Util.Constants.DEV_DEBUG)
                        Debug.Log("Station added: " + station, this);

                     if (readNumberOfStations == stationCount)
                     {
                        break;
                     }
                  }
                  else
                  {
                     if (!suppressDoubleStations)
                     {
                        Debug.LogWarning("Station already added: '" + entry + "'", this);
                     }
                  }
               }
            }

            ii++;
         }
      }

      protected void fillStationsFromPLS(System.Collections.Generic.List<string> list, Model.Entry.BaseRadioEntry entry, int readNumberOfStations = 0, bool suppressDoubleStations = false)
      {
         int stationCount = 0;

         for (int ii = 0; ii < list.Count;)
         {
            string line = list[ii].Trim();

            if (ii == 0 && !line.CTEquals("[playlist]"))
            {
               Debug.LogWarning("File is not in the PLS-format!", this);

               break;
            }

            string title = string.Empty;

            if (line.CTContains(Util.Constants.PLS_FILE_ID)) //File?
            {
               string[] filesplit = line.Split(splitCharEquals, System.StringSplitOptions.RemoveEmptyEntries);

               if (filesplit.Length > 1)
               {
                  string file = filesplit[1];

                  if (ii + 1 < list.Count)
                  {
                     ii++;
                     line = list[ii];

                     if (line.CTContains(Util.Constants.PLS_TITLE_ID)) // Title?
                     {
                        string[] titlesplit = line.Split(splitCharEquals, System.StringSplitOptions.RemoveEmptyEntries);

                        if (titlesplit.Length > 1)
                        {
                           title = titlesplit[1];

                           ii++;
                        }
                     }
                  }

                  if (!string.IsNullOrEmpty(file) && (!entry.AllowOnlyHTTPS || entry.AllowOnlyHTTPS && isHTTPS(file)))
                  {
                     Model.RadioStation station = new Model.RadioStation(entry.ForceName ? entry.Name : string.IsNullOrEmpty(title) ? entry.Name : title.Trim(), file.Trim(), entry.Format == Model.Enum.AudioFormat.UNKNOWN ? Util.Helper.AudioFormatFromString(file) : entry.Format, entry.Station, entry.Genres.ToLower(), entry.Bitrate, entry.Rating, entry.Description, entry.Icon, entry.IconUrl, entry.City, entry.Country, entry.Language, entry.ChunkSize, entry.BufferSize, entry.ExcludedCodec);

                     if (!Stations.Contains(station))
                     {
                        Stations.Add(station);

                        stationCount++;

                        if (Util.Constants.DEV_DEBUG)
                           Debug.Log("Station added: " + station, this);

                        if (readNumberOfStations == stationCount)
                        {
                           break;
                        }
                     }
                     else
                     {
                        if (!suppressDoubleStations)
                           Debug.LogWarning("Station already added: '" + entry + "'", this);
                     }
                  }
               }
               else
               {
                  Debug.LogWarning(entry + ": No URL found for '" + Util.Constants.PLS_FILE_ID + "': " + line, this);
               }
            }
            else
            {
               ii++;
            }
         }
      }

      protected void fillStationsFromText(System.Collections.Generic.List<string> list, Model.Entry.BaseRadioEntry entry, int readNumberOfStations = 0, bool suppressDoubleStations = false)
      {
         int stationCount = 0;

         foreach (string line in list)
         {
            string[] content = line.Split(splitCharText, System.StringSplitOptions.None);

            if (content.Length >= 4 && content.Length <= 16)
            {
               if (!entry.AllowOnlyHTTPS || entry.AllowOnlyHTTPS && isHTTPS(content[1]))
               {
                  Model.RadioStation station = new Model.RadioStation(entry.ForceName ? entry.Name : string.IsNullOrEmpty(content[0]) ? entry.Name : content[0].Trim(), content[1].Trim(), Util.Helper.AudioFormatFromString(content[3].Trim()));

                  if (content.Length >= 5)
                     station.Station = content[4].Trim();

                  if (content.Length >= 6)
                     station.Genres = content[5].Trim().ToLower();

                  if (content.Length >= 7)
                     station.Bitrate = int.TryParse(content[6].Trim(), out int bitrate) ? bitrate : entry.Bitrate;

                  if (content.Length >= 8)
                     station.Rating = float.TryParse(content[7].Trim(), out float rating) ? rating : entry.Rating;

                  if (content.Length >= 9)
                     station.Description = content[8].Trim();

                  if (content.Length >= 10)
                     station.ExcludedCodec = Util.Helper.AudioCodecFromString(content[9].Trim());

                  if (content.Length >= 11)
                     station.ChunkSize = int.TryParse(content[10].Trim(), out int chunkSize) ? chunkSize : entry.ChunkSize;

                  if (content.Length >= 12)
                     station.BufferSize = int.TryParse(content[11].Trim(), out int bufferSize) ? bufferSize : entry.BufferSize;

                  if (content.Length >= 13)
                     station.IconUrl = content[12].Trim();

                  if (content.Length >= 14)
                     station.City = content[13].Trim();

                  if (content.Length >= 15)
                     station.Country = content[14].Trim();

                  if (content.Length == 16) //all parameters
                     station.Language = content[15].Trim();

                  if (station.Format == Model.Enum.AudioFormat.OGG && station.BufferSize < Util.Constants.MIN_OGG_BUFFERSIZE)
                  {
                     if (Util.Config.DEBUG)
                        Debug.Log("Adjusted buffer size: " + station, this);

                     station.BufferSize = Util.Constants.MIN_OGG_BUFFERSIZE;
                  }

                  stationCount++;

                  if (content[2].CTEquals("stream"))
                  {
                     if (!Stations.Contains(station))
                     {
                        Stations.Add(station);

                        if (Util.Config.DEBUG)
                           Debug.Log("Station added: " + station, this);
                     }
                     else
                     {
                        if (!suppressDoubleStations)
                           Debug.LogWarning("Station already added: '" + entry + "'", this);
                     }
                  }
                  else if (content[2].CTContains("pls"))
                  {
                     if (Util.Helper.isEditorMode)
                     {
#if UNITY_EDITOR
                        loadWebInEditor(new Model.Entry.RadioEntryURL(station, content[1].Trim(), Model.Enum.DataFormatURL.PLS, 1 /*readNumberOfStations*/));
#endif
                     }
                     else
                     {
                        StartCoroutine(loadWeb(addCoRoutine(), new Model.Entry.RadioEntryURL(station, content[1].Trim(), Model.Enum.DataFormatURL.PLS, 1 /*readNumberOfStations*/)));
                     }
                  }
                  else if (content[2].CTContains("m3u"))
                  {
                     if (Util.Helper.isEditorMode)
                     {
#if UNITY_EDITOR
                        loadWebInEditor(new Model.Entry.RadioEntryURL(station, content[1].Trim(), Model.Enum.DataFormatURL.M3U, 1 /*readNumberOfStations*/));
#endif
                     }
                     else
                     {
                        StartCoroutine(loadWeb(addCoRoutine(), new Model.Entry.RadioEntryURL(station, content[1].Trim(), Model.Enum.DataFormatURL.M3U, 1 /*readNumberOfStations*/)));
                     }
                  }
                  else if (content[2].CTContains("shoutcast"))
                  {
                     if (Util.Helper.isEditorMode)
                     {
#if UNITY_EDITOR
                        loadShoutcastInEditor(new Model.Entry.RadioEntryShoutcast(station, content[1].Trim()));
#endif
                     }
                     else
                     {
                        StartCoroutine(loadShoutcast(addCoRoutine(), new Model.Entry.RadioEntryShoutcast(station, content[1].Trim())));
                     }
                  }
                  else
                  {
                     Debug.LogWarning("Could not determine URL for station: '" + entry + "'" + System.Environment.NewLine + line, this);
                     stationCount--;
                  }

                  if (readNumberOfStations == stationCount)
                     break;
               }
            }
            else
            {
               Debug.LogWarning("Invalid station description: '" + entry + "'" + "'" + System.Environment.NewLine + line, this);
            }
         }
      }

      protected string addCoRoutine()
      {
         string uid = System.Guid.NewGuid().ToString();
         coRoutines.Add(uid);

         return uid;
      }

      private static bool isHTTPS(string url)
      {
         return url.CTContains(Util.Constants.PREFIX_HTTPS);
      }

      #endregion


      #region Event-trigger methods

      protected void onStationsChange()
      {
         if (Util.Config.DEBUG)
            Debug.Log("onStationsChange", this);

         if (!Util.Helper.isEditorMode)
            onStationsChanged?.Invoke();

         OnStationsChange?.Invoke();
      }

      private void onProviderReady()
      {
         if (Util.Config.DEBUG)
            Debug.Log("onProviderReady", this);

         if (!Util.Helper.isEditorMode)
            onProviderReadyEvent?.Invoke();

         OnProviderReady?.Invoke();
      }

      #endregion


      #region Editor-only methods

#if UNITY_EDITOR
      public bool isReadyInEditor => loadedInEditor;

      protected virtual void initInEditor()
      {
         if (Util.Helper.isEditorMode)
            Stations.Clear();
      }

      protected void loadWebInEditor(Model.Entry.RadioEntryURL entry, bool suppressDoubleStations = false)
      {
         if (Util.Helper.isEditorMode)
         {
            loadedInEditor = false;

            if (!string.IsNullOrEmpty(entry.FinalURL))
            {
               try
               {
                  System.Net.ServicePointManager.ServerCertificateValidationCallback = Util.Helper.RemoteCertificateValidationCallback;

                  using (System.Net.WebClient client = new Common.Util.CTWebClient())
                  {
                     string url = entry.FinalURL;

                     using (System.IO.Stream stream = client.OpenRead(url))
                     {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                        {
                           string content = reader.ReadToEnd();

                           System.Collections.Generic.List<string> list = Util.Helper.SplitStringToLines(content);

                           if (list.Count > 0)
                           {
                              switch (entry.DataFormat)
                              {
                                 case Model.Enum.DataFormatURL.M3U:
                                    list = Util.Helper.SplitStringToLines(content, false); //dirty workaround
                                    fillStationsFromM3U(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                                    break;
                                 case Model.Enum.DataFormatURL.PLS:
                                    fillStationsFromPLS(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                                    break;
                                 case Model.Enum.DataFormatURL.Text:
                                    fillStationsFromText(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                                    break;
                                 default:
                                    Debug.LogWarning("Not implemented!", this);
                                    break;
                              }
                           }
                           else
                           {
                              if (Util.Config.DEBUG)
                                 Debug.Log(entry + " - URL: '" + entry.FinalURL + "' does not contain any active radio stations!", this);
                           }
                        }
                     }
                  }
               }
               catch (System.Exception ex)
               {
                  Debug.LogError(ex, this);
               }
            }
            else
            {
               Debug.LogWarning(entry + ": 'URL' is null or empty!" + System.Environment.NewLine + "Please add a valid URL.", this);
            }

            loadedInEditor = true;
         }
      }

      protected void loadResourceInEditor(Model.Entry.RadioEntryResource entry, bool suppressDoubleStations = false)
      {
         if (Util.Helper.isEditorMode)
         {
            loadedInEditor = false;

            if (entry.Resource != null)
            {
               System.Collections.Generic.List<string> list = Util.Helper.SplitStringToLines(entry.Resource.text);

               if (list.Count > 0)
               {
                  switch (entry.DataFormat)
                  {
                     case Model.Enum.DataFormatResource.M3U:
                        list = Util.Helper.SplitStringToLines(entry.Resource.text, false); //dirty workaround
                        fillStationsFromM3U(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                        break;
                     case Model.Enum.DataFormatResource.PLS:
                        fillStationsFromPLS(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                        break;
                     case Model.Enum.DataFormatResource.Text:
                        fillStationsFromText(list, entry, entry.ReadNumberOfStations, suppressDoubleStations);
                        break;
                     default:
                        Debug.LogWarning("Not implemented!", this);
                        break;
                  }
               }
               else
               {
                  if (Util.Config.DEBUG)
                     Debug.Log(entry + " - Resource: '" + entry.Resource + "' does not contain any active radio stations!", this);
               }
            }
            else
            {
               Debug.LogWarning(entry + ": resource field 'Resource' is null or empty!" + System.Environment.NewLine + "Please add a valid resource.", this);
            }

            loadedInEditor = true;
         }
      }

      protected void loadShoutcastInEditor(Model.Entry.RadioEntryShoutcast entry, bool suppressDoubleStations = false)
      {
         if (Util.Helper.isEditorMode)
         {
            loadedInEditor = false;

            try
            {
               System.Net.ServicePointManager.ServerCertificateValidationCallback = Util.Helper.RemoteCertificateValidationCallback;

               using (System.Net.WebClient client = new Common.Util.CTWebClient())
               {
                  string url = Util.Constants.SHOUTCAST + entry.ShoutcastID.Trim();

                  using (System.IO.Stream stream = client.OpenRead(url))
                  {
                     using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                     {
                        string content = reader.ReadToEnd();

                        System.Collections.Generic.List<string> list = Util.Helper.SplitStringToLines(content);

                        if (list.Count > 0)
                        {
                           fillStationsFromPLS(list, entry, 1, suppressDoubleStations);
                        }
                        else
                        {
                           if (Util.Config.DEBUG)
                              Debug.Log(entry + " - Shoutcast-ID: '" + entry.ShoutcastID + "' does not contain any active radio stations!", this);
                        }
                     }
                  }
               }
            }
            catch (System.Exception ex)
            {
               Debug.LogError(ex, this);
            }

            loadedInEditor = true;
         }
      }

#endif

      #endregion
   }
}
// © 2015-2021 crosstales LLC (https://www.crosstales.com)