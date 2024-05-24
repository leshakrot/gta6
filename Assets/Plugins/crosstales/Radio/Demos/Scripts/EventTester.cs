using UnityEngine;

namespace Crosstales.Radio.Demo
{
   /// <summary>Simple test script for all UnityEvent-callbacks.</summary>
   [ExecuteInEditMode]
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_event_tester.html")]
   public class EventTester : MonoBehaviour
   {
      public Crosstales.Radio.Set.RadioSet Set;

      public void OnPlaybackStart(string _name, int hash)
      {
         Debug.Log("OnPlaybackStart: " + _name + " - " + hash);

         Debug.LogWarning(Set.StationFromHashCode(hash));
      }

      public void OnPlaybackEnd(string _name, int hash)
      {
         Debug.Log("OnPlaybackEnd: " + _name + " - " + hash);

         //Debug.LogWarning(Set.StationFromHashCode(hash));
      }

      public void OnRecordChange(string _name, int hash)
      {
         Debug.Log("OnRecordChange: " + _name + " - " + hash);

         Debug.LogWarning(Set.StationFromHashCode(hash));
      }

      public void OnStationChange(string _name, int hash)
      {
         Debug.Log("OnStationChange: " + _name + " - " + hash);

         Debug.LogWarning(Set.StationFromHashCode(hash));
      }

      public void OnFilterChange()
      {
         Debug.Log("OnFilterChange");
      }

      public void OnError(string _name, int hash, string info)
      {
         Debug.LogWarning("OnError: " + _name + " - " + hash + " - " + info);

         //Debug.LogWarning(Set.StationFromHashCode(hash));
      }

      public void OnStationsChange()
      {
         Debug.Log("OnStationsChange");
      }

      public void OnProviderReady()
      {
         Debug.Log("OnProviderReady");
      }

      public void OnQueryComplete(string id)
      {
         Debug.Log("OnQueryComplete: " + id);
      }
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)