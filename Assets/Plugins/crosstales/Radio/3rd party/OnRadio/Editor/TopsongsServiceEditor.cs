#if UNITY_EDITOR
using UnityEditor;

namespace Crosstales.Radio.OnRadio.EditorExtension
{
   /// <summary>Custom editor for the 'TopsongsService'-class.</summary>
   [CustomEditor(typeof(Service.TopsongsService))]
   public class TopsongsServiceEditor : BaseServiceEditor
   {
      //empty
   }
}
#endif
// © 2020-2021 crosstales LLC (https://www.crosstales.com)