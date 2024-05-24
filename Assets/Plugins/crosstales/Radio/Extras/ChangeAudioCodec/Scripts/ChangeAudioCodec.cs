using UnityEngine;

namespace Crosstales.Radio.Tool
{
   /// <summary>Changes the default audio codec under Windows.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_tool_1_1_change_audio_codec.html")]
   public class ChangeAudioCodec : MonoBehaviour
   {
      #region Variables

      public Model.Enum.AudioCodec Codec = Model.Enum.AudioCodec.MP3_NLayer;

      #endregion


      #region MonoBehaviour methods

      private void Update()
      {
         Util.Constants.DEFAULT_CODEC_MP3_WINDOWS = Codec;
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)