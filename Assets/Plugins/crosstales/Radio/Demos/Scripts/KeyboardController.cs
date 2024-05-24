using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Crosstales.Radio.Demo
{
   /// <summary>Controls UI elements with keyboard commands.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_keyboard_controller.html")]
   public class KeyboardController : MonoBehaviour
   {
      #region Variables

      public Button ButtonPlay;
      public Button ButtonStop;
      public Button ButtonPrevious;
      public Button ButtonNext;

      public KeyCode Play = KeyCode.F3;
      public KeyCode Stop = KeyCode.F2;
      public KeyCode Previous = KeyCode.F1;
      public KeyCode Next = KeyCode.F4;

      #endregion


      #region MonoBehaviour methods

      private void Update()
      {
         PointerEventData pointer = new PointerEventData(EventSystem.current);

         if (Input.GetKeyDown(Play))
            ExecuteEvents.Execute(ButtonPlay.gameObject, pointer, ExecuteEvents.submitHandler); // more see https://answers.unity.com/questions/820599/simulate-button-presses-through-code-unity-46-gui.html

         if (Input.GetKeyDown(Stop))
            ExecuteEvents.Execute(ButtonStop.gameObject, pointer, ExecuteEvents.submitHandler);

         if (Input.GetKeyDown(Previous))
            ExecuteEvents.Execute(ButtonPrevious.gameObject, pointer, ExecuteEvents.submitHandler);

         if (Input.GetKeyDown(Next))
            ExecuteEvents.Execute(ButtonNext.gameObject, pointer, ExecuteEvents.submitHandler);
      }

      #endregion
   }
}
// © 2018-2021 crosstales LLC (https://www.crosstales.com)