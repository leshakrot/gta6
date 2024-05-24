using System.Linq;
using UnityEngine;

namespace Crosstales.Radio.Set
{
   /// <summary>RadioSet consists of 1-n providers.</summary>
   [ExecuteInEditMode]
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_set_1_1_radio_set.html")]
   public class RadioSet : MonoBehaviour, ISet
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Providers")] [Header("General Settings"), Tooltip("Radio station providers for this set."), SerializeField]
      private Provider.BaseRadioProvider[] providers;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Filter")] [Tooltip("Global RadioFilter (active if no explicit filter is given)."), SerializeField]
      private Model.RadioFilter filter;

      private int stationIndex = -1;
      private int randomStationIndex = -1;

      private System.Collections.Generic.List<Model.RadioStation> allStations = new System.Collections.Generic.List<Model.RadioStation>(Util.Constants.INITIAL_LIST_SIZE);
      private System.Collections.Generic.List<Model.RadioStation> randomStations = new System.Collections.Generic.List<Model.RadioStation>(Util.Constants.INITIAL_LIST_SIZE);

      private bool cleared = true;
      private bool clearedRandom = true;

      private Model.RadioFilter lastStationFilter;
      private Model.RadioFilter lastRandomStationFilter;
      private System.Collections.Generic.List<Model.RadioStation> lastFilteredStations;
      private System.Collections.Generic.List<Model.RadioStation> lastFilteredRandomStations;

      // Filter specific fields
      private Model.RadioFilter stationsByNameFilterDesc;
      private Model.RadioFilter stationsByNameFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByNameDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByNameAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter stationsByURLFilterDesc;
      private Model.RadioFilter stationsByURLFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByURLDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByURLAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter stationsByFormatFilterDesc;
      private Model.RadioFilter stationsByFormatFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByFormatDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByFormatAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter stationsByStationFilterDesc;
      private Model.RadioFilter stationsByStationFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByStationDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByStationAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter stationsByBitrateFilterDesc;
      private Model.RadioFilter stationsByBitrateFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByBitrateDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByBitrateAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter stationsByGenresFilterDesc;
      private Model.RadioFilter stationsByGenresFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByGenresDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByGenresAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter stationsByCitiesFilterDesc;
      private Model.RadioFilter stationsByCitiesFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByCitiesDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByCitiesAsc = new System.Collections.Generic.List<Model.RadioStation>();


      private Model.RadioFilter stationsByCountriesFilterDesc;
      private Model.RadioFilter stationsByCountriesFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByCountriesDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByCountriesAsc = new System.Collections.Generic.List<Model.RadioStation>();


      private Model.RadioFilter stationsByLanguagesFilterDesc;
      private Model.RadioFilter stationsByLanguagesFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByLanguagesDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByLanguagesAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter stationsByRatingFilterDesc;
      private Model.RadioFilter stationsByRatingFilterAsc;
      private System.Collections.Generic.List<Model.RadioStation> stationsByRatingDesc = new System.Collections.Generic.List<Model.RadioStation>();
      private System.Collections.Generic.List<Model.RadioStation> stationsByRatingAsc = new System.Collections.Generic.List<Model.RadioStation>();

      private Model.RadioFilter currentFilter;

      #endregion


      #region Properties

      /// <summary>Radio station providers for this set.</summary>
      public Provider.BaseRadioProvider[] Providers
      {
         get => providers;
         set
         {
            unregister();

            providers = value;

            register();
         }
      }

      /// <summary>Global RadioFilter (active if no explicit filter is given).</summary>
      public Model.RadioFilter Filter
      {
         get => filter;
         set
         {
            filter = value;

            currentFilter = new Model.RadioFilter(filter);

            onFilterChange();
         }
      }

      public System.Collections.Generic.List<Model.RadioStation> Stations
      {
         get
         {
            if (allStations.Count < 1)
            {
               System.Collections.Generic.List<Model.RadioStation> result = new System.Collections.Generic.List<Model.RadioStation>();


               foreach (Model.RadioStation station in from rp in Providers.Where(rp => rp != null && rp.Stations != null) from station in rp.Stations where !result.Contains(station) select station)
               {
                  result.Add(station);
               }

               allStations = result.OrderBy(s => s.Name).ToList();
               randomStations.AddRange(allStations);

               RandomizeStations();
            }

            return allStations;
         }
      }

      public System.Collections.Generic.List<Model.RadioStation> RandomStations
      {
         get
         {
            if (allStations.Count < 1)
            {
               System.Collections.Generic.List<Model.RadioStation> result = new System.Collections.Generic.List<Model.RadioStation>();

               foreach (Model.RadioStation station in from rp in Providers.Where(rp => rp != null && rp.Stations != null) from station in rp.Stations where !result.Contains(station) select station)
               {
                  result.Add(station);
               }

               allStations = result.OrderBy(s => s.Name).ToList();
               randomStations.AddRange(allStations);

               RandomizeStations();
            }

            return randomStations;
         }
      }

      public bool isReady => Providers?.All(provider => provider == null || provider.isReady) != false;

      public int CurrentStationIndex
      {
         get => stationIndex;
         set => stationIndex = Mathf.Clamp(value, 0, Stations.Count - 1);
      }

      public int CurrentRandomStationIndex
      {
         get => randomStationIndex;
         set => randomStationIndex = Mathf.Clamp(value, 0, Stations.Count - 1);
      }

      #endregion


      #region Events

      [Header("Events")] public FilterChangeEvent OnFilterChanged;
      public StationsChangeEvent OnStationsChanged;
      public ProviderReadyEvent OnProviderReadyEvent;

      /// <summary>An event triggered whenever the filter changes.</summary>
      public event FilterChange OnFilterChange;

      /// <summary>An event triggered whenever the stations change.</summary>
      public event StationsChange OnStationsChange;

      /// <summary>An event triggered whenever all providers are ready.</summary>
      public event ProviderReady OnProviderReady;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         if (Filter != null)
            currentFilter = new Model.RadioFilter(Filter);

         //StartCoroutine(init());
      }

      private void Update()
      {
         if (Filter == null && currentFilter != null)
         {
            currentFilter = null;
            onFilterChange();
         }
         else if (Filter != null && currentFilter == null || currentFilter?.Equals(Filter) == false)
         {
            currentFilter = new Model.RadioFilter(Filter);
            onFilterChange();
         }
      }

      private void OnEnable()
      {
         register();
      }

      private void OnDisable()
      {
         unregister();
      }

      #endregion


      #region Public methods

      public void Load()
      {
         if (Providers != null)
         {
            foreach (Provider.BaseRadioProvider rp in Providers.Where(rp => rp != null && rp.isActiveAndEnabled))
            {
               rp.Load();
            }
         }

/*
         if (Util.Helper.isEditorMode)
         {
#if UNITY_EDITOR
            new System.Threading.Thread(() => initInEditor()).Start();
#endif
         }
         else
         {
            StartCoroutine(init());
         }
*/
      }

      public void Save(string path, Model.RadioFilter _filter = null)
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

                  foreach (Model.RadioStation rs in StationsByStation(false, getFilter(_filter)))
                  {
                     file.WriteLine(rs.ToTextLine());
                  }
               }
            }
            catch (System.Exception ex)
            {
               Debug.LogError("Could not save file: " + path + System.Environment.NewLine + ex, this);
            }
#else
                Debug.LogWarning("'Save' is not supported on the current platform!", this);
#endif
         }
         else
         {
            Debug.LogWarning("'path' was null or empty! Could not save the data!", this);
         }
      }

      public System.Collections.Generic.List<Model.RadioStation> GetStations(bool random = false, Model.RadioFilter _filter = null)
      {
         return filterStations(random, getFilter(_filter)).ToList();
      }

      public int CountStations(Model.RadioFilter _filter = null)
      {
         return filterStations(false, getFilter(_filter)).Count();
         //return getFilter(filter) == null ? Stations.Count : filterStations(false, getFilter(filter)).ToList().Count;
      }

      public Model.RadioStation StationFromIndex(bool random = false, int index = -1, Model.RadioFilter _filter = null)
      {
         return stationFromIndex(random, index, _filter);
      }

      public Model.RadioStation StationFromHashCode(int hashCode)
      {
         return stationFromHashCode(hashCode);
      }

      public Model.RadioStation NextStation(bool random = false, Model.RadioFilter _filter = null)
      {
         if (Stations != null && Stations.Count > 0)
         {
            return nextStation(random, getFilter(_filter));
         }

         Debug.LogWarning("No 'Stations' found: returning null.", this);

         return null;
      }

      public Model.RadioStation PreviousStation(bool random = false, Model.RadioFilter _filter = null)
      {
         if (Stations != null && Stations.Count > 0)
         {
            return previousStation(random, getFilter(_filter));
         }

         Debug.LogWarning("No 'Stations' found: returning null.", this);

         return null;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByName(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByNameFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByName with filter DESC: CACHED!", this);

                  return stationsByNameDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByName with filter DESC: NOT cached!", this);

               stationsByNameDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Name));
               stationsByNameFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByNameDesc;
            }

            if (stationsByNameDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByName without filter DESC: CACHED!", this);

               return stationsByNameDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByName without filter DESC: NOT cached!", this);

            stationsByNameDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Name));
            stationsByNameFilterDesc = null;

            return stationsByNameDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByNameFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByName with filter ASC: CACHED!", this);

               return stationsByNameAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByName with filter ASC: NOT cached!", this);

            stationsByNameAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Name));
            stationsByNameFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByNameAsc;
         }

         if (stationsByNameAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByName without filter ASC: CACHED!", this);

            return stationsByNameAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByName without filter ASC: NOT cached!", this);

         stationsByNameAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Name));
         stationsByNameFilterAsc = null;

         return stationsByNameAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByURL(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByURLFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByURL with filter DESC: CACHED!", this);

                  return stationsByURLDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByURL with filter DESC: NOT cached!", this);

               stationsByURLDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Url).ThenBy(entry => entry.Name));
               stationsByURLFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByURLDesc;
            }

            if (stationsByURLDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByURL without filter DESC: CACHED!", this);

               return stationsByURLDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByURL without filter DESC: NOT cached!", this);

            stationsByURLDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Url).ThenBy(entry => entry.Name));
            stationsByURLFilterDesc = null;

            return stationsByURLDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByURLFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByURL with filter ASC: CACHED!", this);

               return stationsByURLAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByURL with filter ASC: NOT cached!", this);

            stationsByURLAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Url).ThenBy(entry => entry.Name));
            stationsByURLFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByURLAsc;
         }

         if (stationsByURLAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByURL without filter ASC: CACHED!", this);

            return stationsByURLAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByURL without filter ASC: NOT cached!", this);

         stationsByURLAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Url).ThenBy(entry => entry.Name));
         stationsByURLFilterAsc = null;

         return stationsByURLAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByFormat(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByFormatFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByFormat with filter DESC: CACHED!", this);

                  return stationsByFormatDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByFormat with filter DESC: NOT cached!", this);

               stationsByFormatDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Format).ThenBy(entry => entry.Name));
               stationsByFormatFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByFormatDesc;
            }

            if (stationsByFormatDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByFormat without filter DESC: CACHED!", this);

               return stationsByFormatDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByFormat without filter DESC: NOT cached!", this);

            stationsByFormatDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Format).ThenBy(entry => entry.Name));
            stationsByFormatFilterDesc = null;

            return stationsByFormatDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByFormatFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByFormat with filter ASC: CACHED!", this);

               return stationsByFormatAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByFormat with filter ASC: NOT cached!", this);

            stationsByFormatAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Format).ThenBy(entry => entry.Name));
            stationsByFormatFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByFormatAsc;
         }

         if (stationsByFormatAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByFormat without filter ASC: CACHED!", this);

            return stationsByFormatAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByFormat without filter ASC: NOT cached!", this);

         stationsByFormatAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Format).ThenBy(entry => entry.Name));
         stationsByFormatFilterAsc = null;

         return stationsByFormatAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByStation(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByStationFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByStation with filter DESC: CACHED!", this);

                  return stationsByStationDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByStation with filter DESC: NOT cached!", this);

               stationsByStationDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Station).ThenBy(entry => entry.Name));
               stationsByStationFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByStationDesc;
            }

            if (stationsByStationDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByStation without filter DESC: CACHED!", this);

               return stationsByStationDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByStation without filter DESC: NOT cached!", this);

            stationsByStationDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Station).ThenBy(entry => entry.Name));
            stationsByStationFilterDesc = null;

            return stationsByStationDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByStationFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByStation with filter ASC: CACHED!", this);

               return stationsByStationAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByStation with filter ASC: NOT cached!", this);

            stationsByStationAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Station).ThenBy(entry => entry.Name));
            stationsByStationFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByStationAsc;
         }

         if (stationsByStationAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByStation without filter ASC: CACHED!", this);

            return stationsByStationAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByStation without filter ASC: NOT cached!", this);

         stationsByStationAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Station).ThenBy(entry => entry.Name));
         stationsByStationFilterAsc = null;

         return stationsByStationAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByBitrate(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByBitrateFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByBitrate with filter DESC: CACHED!", this);

                  return stationsByBitrateDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByBitrate with filter DESC: NOT cached!", this);

               stationsByBitrateDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Bitrate).ThenBy(entry => entry.Name));
               stationsByBitrateFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByBitrateDesc;
            }

            if (stationsByBitrateDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByBitrate without filter DESC: CACHED!", this);

               return stationsByBitrateDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByBitrate without filter DESC: NOT cached!", this);

            stationsByBitrateDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Bitrate).ThenBy(entry => entry.Name));
            stationsByBitrateFilterDesc = null;

            return stationsByBitrateDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByBitrateFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByBitrate with filter ASC: CACHED!", this);

               return stationsByBitrateAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByBitrate with filter ASC: NOT cached!", this);

            stationsByBitrateAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Bitrate).ThenBy(entry => entry.Name));
            stationsByBitrateFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByBitrateAsc;
         }

         if (stationsByBitrateAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByBitrate without filter ASC: CACHED!", this);

            return stationsByBitrateAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByBitrate without filter ASC: NOT cached!", this);

         stationsByBitrateAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Bitrate).ThenBy(entry => entry.Name));
         stationsByBitrateFilterAsc = null;

         return stationsByBitrateAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByGenres(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByGenresFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByGenres with filter DESC: CACHED!", this);

                  return stationsByGenresDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByGenres with filter DESC: NOT cached!", this);

               stationsByGenresDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Genres).ThenBy(entry => entry.Name));
               stationsByGenresFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByGenresDesc;
            }

            if (stationsByGenresDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByGenres without filter DESC: CACHED!", this);

               return stationsByGenresDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByGenres without filter DESC: NOT cached!", this);

            stationsByGenresDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Genres).ThenBy(entry => entry.Name));
            stationsByGenresFilterDesc = null;

            return stationsByGenresDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByGenresFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByGenres with filter ASC: CACHED!", this);

               return stationsByGenresAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByGenres with filter ASC: NOT cached!", this);

            stationsByGenresAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Genres).ThenBy(entry => entry.Name));
            stationsByGenresFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByGenresAsc;
         }

         if (stationsByGenresAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByGenres without filter ASC: CACHED!", this);

            return stationsByGenresAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByGenres without filter ASC: NOT cached!", this);

         stationsByGenresAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Genres).ThenBy(entry => entry.Name));
         stationsByGenresFilterAsc = null;

         return stationsByGenresAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByCities(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByCitiesFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByCities with filter DESC: CACHED!", this);

                  return stationsByCitiesDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByCities with filter DESC: NOT cached!", this);

               stationsByCitiesDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.City).ThenBy(entry => entry.Name));
               stationsByCitiesFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByCitiesDesc;
            }

            if (stationsByCitiesDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByCities without filter DESC: CACHED!", this);

               return stationsByCitiesDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByCities without filter DESC: NOT cached!", this);

            stationsByCitiesDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.City).ThenBy(entry => entry.Name));
            stationsByCitiesFilterDesc = null;

            return stationsByCitiesDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByCitiesFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByCities with filter ASC: CACHED!", this);

               return stationsByCitiesAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByCities with filter ASC: NOT cached!", this);

            stationsByCitiesAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.City).ThenBy(entry => entry.Name));
            stationsByCitiesFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByCitiesAsc;
         }

         if (stationsByCitiesAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByCities without filter ASC: CACHED!", this);

            return stationsByCitiesAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByCities without filter ASC: NOT cached!", this);

         stationsByCitiesAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.City).ThenBy(entry => entry.Name));
         stationsByCitiesFilterAsc = null;

         return stationsByCitiesAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByCountries(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByCountriesFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByCountries with filter DESC: CACHED!", this);

                  return stationsByCountriesDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByCountries with filter DESC: NOT cached!", this);

               stationsByCountriesDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Country).ThenBy(entry => entry.Name));
               stationsByCountriesFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByCountriesDesc;
            }

            if (stationsByCountriesDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByCountries without filter DESC: CACHED!", this);

               return stationsByCountriesDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByCountries without filter DESC: NOT cached!", this);

            stationsByCountriesDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Country).ThenBy(entry => entry.Name));
            stationsByCountriesFilterDesc = null;

            return stationsByCountriesDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByCountriesFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByCountries with filter ASC: CACHED!", this);

               return stationsByCountriesAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByCountries with filter ASC: NOT cached!", this);

            stationsByCountriesAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Country).ThenBy(entry => entry.Name));
            stationsByCountriesFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByCountriesAsc;
         }

         if (stationsByCountriesAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByCountries without filter ASC: CACHED!", this);

            return stationsByCountriesAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByCountries without filter ASC: NOT cached!", this);

         stationsByCountriesAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Country).ThenBy(entry => entry.Name));
         stationsByCountriesFilterAsc = null;

         return stationsByCountriesAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByLanguages(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByLanguagesFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByLanguages with filter DESC: CACHED!", this);

                  return stationsByLanguagesDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByLanguages with filter DESC: NOT cached!", this);

               stationsByLanguagesDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Language).ThenBy(entry => entry.Name));
               stationsByLanguagesFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByLanguagesDesc;
            }

            if (stationsByLanguagesDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByLanguages without filter DESC: CACHED!", this);

               return stationsByLanguagesDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByLanguages without filter DESC: NOT cached!", this);

            stationsByLanguagesDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Language).ThenBy(entry => entry.Name));
            stationsByLanguagesFilterDesc = null;

            return stationsByLanguagesDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByLanguagesFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByLanguages with filter ASC: CACHED!", this);

               return stationsByLanguagesAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByLanguages with filter ASC: NOT cached!", this);

            stationsByLanguagesAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Language).ThenBy(entry => entry.Name));
            stationsByLanguagesFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByLanguagesAsc;
         }

         if (stationsByLanguagesAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByLanguages without filter ASC: CACHED!", this);

            return stationsByLanguagesAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByLanguages without filter ASC: NOT cached!", this);

         stationsByLanguagesAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Language).ThenBy(entry => entry.Name));
         stationsByLanguagesFilterAsc = null;

         return stationsByLanguagesAsc;
      }

      public System.Collections.Generic.List<Model.RadioStation> StationsByRating(bool desc = false, Model.RadioFilter _filter = null)
      {
         Model.RadioFilter _currentFilter = getFilter(_filter);

         if (desc)
         {
            if (_currentFilter != null)
            {
               if (_currentFilter.Equals(stationsByRatingFilterDesc))
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("StationsByRating with filter DESC: CACHED!", this);

                  return stationsByRatingDesc;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByRating with filter DESC: NOT cached!", this);

               stationsByRatingDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderByDescending(entry => entry.Rating).ThenBy(entry => entry.Name));
               stationsByRatingFilterDesc = new Model.RadioFilter(_currentFilter);

               return stationsByRatingDesc;
            }

            if (stationsByRatingDesc.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByRating without filter DESC: CACHED!", this);

               return stationsByRatingDesc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByRating without filter DESC: NOT cached!", this);

            stationsByRatingDesc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderByDescending(entry => entry.Rating).ThenBy(entry => entry.Name));

            return stationsByRatingDesc;
         }

         if (_currentFilter != null)
         {
            if (_currentFilter.Equals(stationsByRatingFilterAsc))
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("StationsByRating with filter ASC: CACHED!", this);

               return stationsByRatingAsc;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByRating with filter ASC: NOT cached!", this);

            stationsByRatingAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations(false, _currentFilter).OrderBy(entry => entry.Rating).ThenBy(entry => entry.Name));
            stationsByRatingFilterAsc = new Model.RadioFilter(_currentFilter);

            return stationsByRatingAsc;
         }

         if (stationsByRatingAsc.Count > 0)
         {
            if (Util.Constants.DEV_DEBUG)
               Debug.Log("StationsByRating without filter ASC: CACHED!", this);

            return stationsByRatingAsc;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("StationsByRating without filter ASC: NOT cached!", this);

         stationsByRatingAsc = new System.Collections.Generic.List<Model.RadioStation>(filterStations().OrderBy(entry => entry.Rating).ThenBy(entry => entry.Name));
         stationsByRatingFilterAsc = null;

         return stationsByRatingAsc;
      }

      public void RandomizeStations(bool resetIndex = true)
      {
         int seed = Random.Range(0, int.MaxValue);
         randomStations.CTShuffle(seed);

         if (resetIndex)
            randomStationIndex = 0;
      }

      #endregion


      #region Private methods

      private void register()
      {
         foreach (Provider.BaseRadioProvider rp in Providers?.Where(rp => rp != null))
         {
            rp.OnStationsChange += onStationsChange;
         }
      }

      private void unregister()
      {
         foreach (Provider.BaseRadioProvider rp in Providers?.Where(rp => rp != null))
         {
            rp.OnStationsChange -= onStationsChange;
         }
      }

      private System.Collections.Generic.IEnumerable<Model.RadioStation> filterStations(bool random = false, Model.RadioFilter _filter = null)
      {
         //Debug.Log("Filter: " + (_filter?.isFiltering == true));

         if (random)
         {
            if (_filter?.isFiltering == true)
            {
               clearedRandom = false;

               if (_filter.Equals(lastRandomStationFilter) && lastFilteredRandomStations.Count > 0)
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("filterStations RND: CACHED!", this);

                  return lastFilteredRandomStations;
               }

               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("filterStations RND: NOT Cached!", this);

               System.Collections.Generic.IEnumerable<Model.RadioStation> _stations = from entry in RandomStations
                  where (string.IsNullOrEmpty(_filter.Names) || entry.Name.CTContainsAny(_filter.Names)) &&
                        (string.IsNullOrEmpty(_filter.Stations) || entry.Station.CTContainsAny(_filter.Stations)) &&
                        (string.IsNullOrEmpty(_filter.Urls) || entry.Url.CTContainsAll(_filter.Urls)) &&
                        (string.IsNullOrEmpty(_filter.Genres) || entry.Genres.CTContainsAny(_filter.Genres)) &&
                        (string.IsNullOrEmpty(_filter.Cities) || entry.City.CTContainsAny(_filter.Cities)) &&
                        (string.IsNullOrEmpty(_filter.Countries) || entry.Country.CTContainsAny(_filter.Countries)) &&
                        (string.IsNullOrEmpty(_filter.Languages) || entry.Language.CTContainsAny(_filter.Languages)) &&
                        entry.Format.ToString().CTContainsAny(_filter.Format) &&
                        entry.Bitrate >= _filter.BitrateMin && entry.Bitrate <= _filter.BitrateMax &&
                        entry.Rating >= _filter.RatingMin && entry.Rating <= _filter.RatingMax &&
                        (!_filter.ExcludeUnsupportedCodecs || entry.ExcludedCodec == Model.Enum.AudioCodec.None || entry.ExcludedCodec != Util.Helper.AudioCodecForAudioFormat(entry.Format))
                  select entry;

               System.Collections.Generic.List<Crosstales.Radio.Model.RadioStation> radioStations = _stations.ToList();
               lastFilteredRandomStations = _filter.Limit != 0 && radioStations.Count > _filter.Limit ? radioStations.GetRange(0, _filter.Limit) : radioStations;
               lastRandomStationFilter = new Model.RadioFilter(_filter);

               clearFilters(false, false);

               return lastFilteredRandomStations;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("filterStations RND: No filtering!", this);

            if (!clearedRandom)
            {
               clearFilters();
               clearedRandom = true;
            }

            return RandomStations;
         }

         if (_filter?.isFiltering == true)
         {
            cleared = false;

            if (_filter.Equals(lastStationFilter) && lastFilteredStations.Count > 0)
            {
               if (Util.Constants.DEV_DEBUG)
                  Debug.Log("filterStations: CACHED!", this);

               return lastFilteredStations;
            }

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("filterStations: NOT Cached!", this);

            System.Collections.Generic.IEnumerable<Model.RadioStation> _stations = from entry in Stations
               where (string.IsNullOrEmpty(_filter.Names) || entry.Name.CTContainsAny(_filter.Names)) &&
                     (string.IsNullOrEmpty(_filter.Stations) || entry.Station.CTContainsAny(_filter.Stations)) &&
                     (string.IsNullOrEmpty(_filter.Urls) || entry.Url.CTContainsAll(_filter.Urls)) &&
                     (string.IsNullOrEmpty(_filter.Genres) || entry.Genres.CTContainsAny(_filter.Genres)) &&
                     (string.IsNullOrEmpty(_filter.Cities) || entry.City.CTContainsAny(_filter.Cities)) &&
                     (string.IsNullOrEmpty(_filter.Countries) || entry.Country.CTContainsAny(_filter.Countries)) &&
                     (string.IsNullOrEmpty(_filter.Languages) || entry.Language.CTContainsAny(_filter.Languages)) &&
                     entry.Format.ToString().CTContainsAny(_filter.Format) &&
                     entry.Bitrate >= _filter.BitrateMin && entry.Bitrate <= _filter.BitrateMax &&
                     entry.Rating >= _filter.RatingMin && entry.Rating <= _filter.RatingMax &&
                     (!_filter.ExcludeUnsupportedCodecs || entry.ExcludedCodec == Model.Enum.AudioCodec.None || entry.ExcludedCodec != Util.Helper.AudioCodecForAudioFormat(entry.Format))
               select entry;

            System.Collections.Generic.List<Crosstales.Radio.Model.RadioStation> radioStations = _stations.ToList();
            lastFilteredStations = _filter.Limit != 0 && radioStations.Count > _filter.Limit ? radioStations.GetRange(0, _filter.Limit) : radioStations;
            lastStationFilter = new Model.RadioFilter(_filter);

            clearFilters(false, false);

            return lastFilteredStations;
         }

         if (Util.Constants.DEV_DEBUG)
            Debug.Log("filterStations: No filtering!", this);

         if (!cleared)
         {
            clearFilters();
            cleared = true;
         }

         return Stations;
      }

      private void clearFilters(bool clearLastFilter = true, bool clearLastRandomFilter = true)
      {
         if (clearLastFilter)
         {
            lastFilteredStations = null;
            lastStationFilter = null;
         }

         if (clearLastRandomFilter)
         {
            lastFilteredRandomStations = null;
            lastRandomStationFilter = null;
         }

         stationsByNameFilterDesc = null;
         stationsByNameFilterAsc = null;

         stationsByURLFilterDesc = null;
         stationsByURLFilterAsc = null;

         stationsByFormatFilterDesc = null;
         stationsByFormatFilterAsc = null;

         stationsByStationFilterDesc = null;
         stationsByStationFilterAsc = null;

         stationsByBitrateFilterDesc = null;
         stationsByBitrateFilterAsc = null;

         stationsByGenresFilterDesc = null;
         stationsByGenresFilterAsc = null;

         stationsByCitiesFilterDesc = null;
         stationsByCitiesFilterAsc = null;

         stationsByCountriesFilterDesc = null;
         stationsByCountriesFilterAsc = null;

         stationsByLanguagesFilterDesc = null;
         stationsByLanguagesFilterAsc = null;

         stationsByRatingFilterDesc = null;
         stationsByRatingFilterAsc = null;

         resetFilterLists();
      }

      private void resetFilterLists()
      {
         stationsByNameDesc.Clear();
         stationsByNameAsc.Clear();

         stationsByURLDesc.Clear();
         stationsByURLAsc.Clear();

         stationsByFormatDesc.Clear();
         stationsByFormatAsc.Clear();

         stationsByStationDesc.Clear();
         stationsByStationAsc.Clear();

         stationsByBitrateDesc.Clear();
         stationsByBitrateAsc.Clear();

         stationsByGenresDesc.Clear();
         stationsByGenresAsc.Clear();

         stationsByCitiesDesc.Clear();
         stationsByCitiesAsc.Clear();

         stationsByCountriesDesc.Clear();
         stationsByCountriesAsc.Clear();

         stationsByLanguagesDesc.Clear();
         stationsByLanguagesAsc.Clear();

         stationsByRatingDesc.Clear();
         stationsByRatingAsc.Clear();
      }

      private Model.RadioStation stationFromIndex(bool random = false, int index = -1, Model.RadioFilter _filter = null)
      {
         System.Collections.Generic.List<Crosstales.Radio.Model.RadioStation> stations = new System.Collections.Generic.List<Model.RadioStation>(filterStations(random, _filter));

         if (stations.Count > 0)
         {
            if (random)
            {
               if (index > -1 && index < stations.Count)
               {
                  randomStationIndex = index;
               }
               else
               {
                  randomStationIndex = Random.Range(0, stations.Count - 1);
               }

               return stations[randomStationIndex];
            }

            if (index > -1 && index < stations.Count)
            {
               stationIndex = index;
            }
            else
            {
               stationIndex = 0;
            }

            return stations[stationIndex];
         }

         return null;
      }

      private Model.RadioStation stationFromHashCode(int hashCode)
      {
         return Stations.Count > 0 ? Stations.FirstOrDefault(station => station.GetHashCode() == hashCode) : null;
      }

      private Model.RadioStation nextStation(bool random = false, Model.RadioFilter _filter = null)
      {
         System.Collections.Generic.List<Crosstales.Radio.Model.RadioStation> stations = new System.Collections.Generic.List<Model.RadioStation>(filterStations(random, _filter));

         if (random)
         {
            if (randomStationIndex > -1 && randomStationIndex + 1 < stations.Count)
            {
               randomStationIndex++;
            }
            else
            {
               randomStationIndex = 0;
            }

            if (stations.Count > 0)
            {
               return stations[randomStationIndex];
            }
         }
         else
         {
            if (stationIndex > -1 && stationIndex + 1 < stations.Count)
            {
               stationIndex++;
            }
            else
            {
               stationIndex = 0;
            }

            if (stations.Count > 0)
               return stations[stationIndex];
         }

         return null;
      }

      private Model.RadioStation previousStation(bool random = false, Model.RadioFilter _filter = null)
      {
         System.Collections.Generic.List<Crosstales.Radio.Model.RadioStation> stations = new System.Collections.Generic.List<Model.RadioStation>(filterStations(random, _filter));

         if (random)
         {
            if (randomStationIndex > 0 && randomStationIndex < stations.Count)
            {
               randomStationIndex--;
            }
            else
            {
               randomStationIndex = stations.Count - 1;
            }

            if (stations.Count > 0)
               return stations[randomStationIndex];
         }
         else
         {
            if (stationIndex > 0 && stationIndex < stations.Count)
            {
               stationIndex--;
            }
            else
            {
               stationIndex = stations.Count - 1;
            }

            if (stations.Count > 0)
               return stations[stationIndex];
         }

         return null;
      }

      private Model.RadioFilter getFilter(Model.RadioFilter _filter)
      {
         if (_filter?.isFiltering == true)
            return _filter;

         return Filter.isFiltering ? Filter : null;
      }

      #endregion


      #region Event-trigger methods

      private void onProviderReady()
      {
         if (Util.Config.DEBUG)
            Debug.Log("onProviderReady", this);

         if (!Util.Helper.isEditorMode)
            OnProviderReadyEvent?.Invoke();

         OnProviderReady?.Invoke();
      }

      private void onFilterChange()
      {
         if (Util.Config.DEBUG)
            Debug.Log("onFilterChange", this);

         clearFilters(false, false);

         if (!Util.Helper.isEditorMode)
            OnFilterChanged?.Invoke();

         OnFilterChange?.Invoke();
      }

      private void onStationsChange()
      {
         if (Util.Config.DEBUG)
            Debug.Log("onStationsChange", this);

         resetFilterLists();

         allStations.Clear();
         randomStations.Clear();

         if (!Util.Helper.isEditorMode)
            OnStationsChanged?.Invoke();

         OnStationsChange?.Invoke();

         onProviderReady();
      }

      #endregion


      #region Overridden methods

      public override string ToString()
      {
         System.Text.StringBuilder result = new System.Text.StringBuilder();

         result.Append(GetType().Name);
         result.Append(Util.Constants.TEXT_TOSTRING_START);

         result.Append("Providers='");
         result.Append(Providers);
         result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER);

         result.Append("Filter='");
         result.Append(Filter);
         result.Append(Util.Constants.TEXT_TOSTRING_DELIMITER_END);

         result.Append(Util.Constants.TEXT_TOSTRING_END);

         return result.ToString();
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)