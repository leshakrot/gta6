using UnityEngine;
using System.Linq;

namespace Crosstales.Radio.OnRadio.Provider
{
   /// <summary>Provider for OnRadio service results.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_provider_1_1_radio_provider_on_radio.html")]
   public class RadioProviderOnRadio : Crosstales.Radio.Provider.BaseRadioProvider
   {
      #region Variables

      /// <summary>OnRadio services from the scene.</summary>
      [Header("Source Settings")] [Tooltip("OnRadio services from the scene.")] public OnRadio.Service.BaseService[] Services;

      private bool ready;

      private int readyCounter;

      #endregion


      #region Properties

      public override System.Collections.Generic.List<Crosstales.Radio.Model.Entry.BaseRadioEntry> RadioEntries => new System.Collections.Generic.List<Crosstales.Radio.Model.Entry.BaseRadioEntry>(); //TODO improve?

      public override bool isReady => ready;

      protected override StationsChangeEvent onStationsChanged => OnStationsChanged;

      protected override ProviderReadyEvent onProviderReadyEvent => OnProviderReadyEvent;

      #endregion


      #region Events

      [Header("Events")] public StationsChangeEvent OnStationsChanged;
      public ProviderReadyEvent OnProviderReadyEvent;

      #endregion


      #region MonoBehaviour methods

      private void OnEnable()
      {
         foreach (OnRadio.Service.BaseService service in Services.Where(service => service != null))
         {
            service.OnQueryComplete += onQueryComplete;
         }
      }

      private void OnDisable()
      {
         foreach (OnRadio.Service.BaseService service in Services.Where(service => service != null))
         {
            service.OnQueryComplete -= onQueryComplete;
         }
      }

      #endregion

      public override void Load()
      {
         readyCounter = 0;
         ready = false;
         base.Load();
      }

      #region Private methods

      private void onQueryComplete(string id)
      {
         readyCounter++;
         init();

         foreach (OnRadio.Model.RadioStationExt station in Services.Where(service => service != null).SelectMany(service => service.Stations))
         {
            Stations.Add(station);
         }

         if (readyCounter >= Services.Length)
            ready = true;

         onStationsChange();
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)