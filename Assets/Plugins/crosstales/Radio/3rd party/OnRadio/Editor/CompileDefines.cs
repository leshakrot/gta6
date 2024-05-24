#if UNITY_EDITOR
using UnityEditor;

namespace Crosstales.Radio.OnRadio
{
   /// <summary>Adds the given define symbols to PlayerSettings define symbols.</summary>
   [InitializeOnLoad]
   public class CompileDefines : Common.EditorTask.BaseCompileDefines
   {
      private const string symbol = "CT_RADIO_ONRADIO";

      static CompileDefines()
      {
         addSymbolsToAllTargets(symbol);
      }
   }
}
#endif
// © 2020-2021 crosstales LLC (https://www.crosstales.com)