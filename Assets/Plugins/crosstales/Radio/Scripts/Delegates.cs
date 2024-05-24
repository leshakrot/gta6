namespace Crosstales.Radio
{
   #region BasePlayer

   [System.Serializable]
   public class PlaybackStartEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   [System.Serializable]
   public class PlaybackEndEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   [System.Serializable]
   public class BufferingStartEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   [System.Serializable]
   public class BufferingEndEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   [System.Serializable]
   public class AudioStartEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   [System.Serializable]
   public class AudioEndEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   [System.Serializable]
   public class RecordChangeEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   [System.Serializable]
   public class ErrorEvent : UnityEngine.Events.UnityEvent<string, int, string>
   {
   }

   public delegate void PlaybackStart(Model.RadioStation station);

   public delegate void PlaybackEnd(Model.RadioStation station);

   public delegate void BufferingStart(Model.RadioStation station);

   public delegate void BufferingEnd(Model.RadioStation station);

   public delegate void BufferingProgressUpdate(Model.RadioStation station, float progress);

   public delegate void AudioStart(Model.RadioStation station);

   public delegate void AudioEnd(Model.RadioStation station);

   public delegate void AudioPlayTimeUpdate(Model.RadioStation station, float playtime);

   public delegate void RecordChange(Model.RadioStation station, Model.RecordInfo newRecord);

   public delegate void RecordPlayTimeUpdate(Model.RadioStation station, Model.RecordInfo record, float playtime);

   public delegate void NextRecordChange(Model.RadioStation station, Model.RecordInfo nextRecord, float delay);

   public delegate void NextRecordDelayUpdate(Model.RadioStation station, Model.RecordInfo nextRecord, float delay);

   public delegate void ErrorInfo(Model.RadioStation station, string info);

   #endregion


   #region SimplePlayer

   [System.Serializable]
   public class StationChangeEvent : UnityEngine.Events.UnityEvent<string, int>
   {
   }

   public delegate void StationChange(Model.RadioStation newStation);

   #endregion


   #region Set, RadioManager and SimplePlayer

   [System.Serializable]
   public class FilterChangeEvent : UnityEngine.Events.UnityEvent
   {
   }

   public delegate void FilterChange();

   #endregion


   #region Provider, Set, RadioManager and SimplePlayer

   [System.Serializable]
   public class StationsChangeEvent : UnityEngine.Events.UnityEvent
   {
   }

   [System.Serializable]
   public class ProviderReadyEvent : UnityEngine.Events.UnityEvent
   {
   }

   public delegate void StationsChange();

   public delegate void ProviderReady();

   #endregion
}
// © 2018-2021 crosstales LLC (https://www.crosstales.com)