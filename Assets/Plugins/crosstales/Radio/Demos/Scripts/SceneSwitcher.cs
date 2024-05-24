using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crosstales.Radio.Demo
{
   /// <summary>Very simple scene switcher.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/DJ/api/class_crosstales_1_1_d_j_1_1_demo_1_1_scene_switcher.html")] //TODO update
   public class SceneSwitcher : MonoBehaviour
   {
      public int Index;

      /// <summary>Switches the scene to the given index.</summary>
      public void Switch()
      {
         SceneManager.LoadScene(Index);
      }
   }
}
// © 2016-2021 crosstales LLC (https://www.crosstales.com)