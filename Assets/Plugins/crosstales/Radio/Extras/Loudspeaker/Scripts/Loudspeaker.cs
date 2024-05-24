using UnityEngine;

namespace Crosstales.Radio.Tool
{
   /// <summary>Loudspeaker for a player.</summary>
   [RequireComponent(typeof(AudioSource))]
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_tool_1_1_loudspeaker.html")]
   public class Loudspeaker : MonoBehaviour
   {
      #region Variables

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("Player")] [Tooltip("Origin Player."), SerializeField]
      private BasePlayer player;

      [UnityEngine.Serialization.FormerlySerializedAsAttribute("SilenceSource")] [Tooltip("Silence the origin (default: true)."), SerializeField]
      private bool silenceSource = true;

      private AudioSource audioSource;
      private bool stopped = true;
      private long dataPosition;
      private AudioClip clip;

      #endregion


      #region Properties

      /// <summary>Origin Player.</summary>
      public BasePlayer Player
      {
         get => player;
         set
         {
            stopped = true;

            player = value;

            if (player != null)
               player.CaptureDataStream = true;
         }
      }

      /// <summary>Silence the origin.</summary>
      public bool SilenceSource
      {
         get => silenceSource;
         set => Player.isMuted = silenceSource = value;
      }

      #endregion


      #region MonoBehaviour methods

#if !UNITY_WEBGL || UNITY_EDITOR
      private void Awake()
      {
         audioSource = GetComponent<AudioSource>();
         audioSource.playOnAwake = false;
         audioSource.Stop(); //always stop the AudioSource at startup
      }

      private void OnDisable()
      {
         audioSource.Stop();
         audioSource.clip = null;

         if (clip != null)
            Destroy(clip);

         stopped = true;
      }

      private void Start()
      {
         if (Player == null)
         {
            Debug.LogWarning("No 'Player' added to the Loudspeaker!", this);
         }
         else
         {
            Player.CaptureDataStream = true;
         }
      }

      private void Update()
      {
         if (!audioSource.mute)
         {
            if (Player.isAudioPlaying)
            {
               if (stopped)
               {
                  stopped = false;

                  if (Player.DataStream != null)
                     dataPosition = Player.DataStream.Position;

                  clip = AudioClip.Create(Player.Station.Name, int.MaxValue, Player.Station.Channels, Player.Station.SampleRate, true, readPCMData);

                  audioSource.clip = clip;

                  audioSource.Play();

                  Player.isMuted = silenceSource;
               }
            }
            else
            {
               if (!stopped)
               {
                  OnDisable();
               }
            }
         }
      }
#else
      private void Start()
      {
         Debug.LogWarning("'Loudspeaker' is not supported for the current platform.", this);
      }
#endif

      #endregion


      #region Private methods

      private void readPCMData(float[] data)
      {
         if (data != null)
         {
            if (Player.isAudioPlaying && Player.DataStream != null)
            {
               byte[] buffer = new byte[data.Length * 2];

               int count;
               long tempPosition = Player.DataStream.Position;
               Player.DataStream.Position = dataPosition;

               if ((count = Player.DataStream.Read(buffer, 0, buffer.Length)) > 0)
               {
                  //System.Buffer.BlockCopy(Util.Helper.ConvertByteArrayToFloatArray(buffer, count), 0, data, 0, data.Length * 4);
                  float[] converted = buffer.CTToFloatArray(count);
                  System.Array.Copy(converted, 0, data, 0, converted.Length);

                  dataPosition += count;
               }

               Player.DataStream.Position = tempPosition;
            }
            else
            {
               //System.Buffer.BlockCopy(new float[data.Length], 0, data, 0, data.Length * 4);
               System.Array.Copy(new float[data.Length], 0, data, 0, data.Length);
            }
         }
      }

      #endregion
   }
}
// © 2017-2021 crosstales LLC (https://www.crosstales.com)