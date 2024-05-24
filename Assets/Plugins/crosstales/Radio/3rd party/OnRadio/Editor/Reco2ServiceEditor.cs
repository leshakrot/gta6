#if UNITY_EDITOR
using UnityEditor;

namespace Crosstales.Radio.OnRadio.EditorExtension
{
   /// <summary>Custom editor for the 'Reco2Service'-class.</summary>
   [CustomEditor(typeof(Service.Reco2Service))]
   public class Reco2ServiceEditor : BaseServiceEditor
   {
      //empty
   }
}
#endif
// © 2020-2021 crosstales LLC (https://www.crosstales.com)