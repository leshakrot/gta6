using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.OnRadio.Demo
{
   public class OrderManager : MonoBehaviour
   {
      /// <summary>'GUIOnRadio' from the scene.</summary>
      [Header("Settings")] [Tooltip("'GUIOnRadio' from the scene.")] public GUIOnRadio GuiOnRadio;

      /// <summary>Record prefab for the radio list.</summary>
      [Tooltip("Record prefab for the radio list.")] public GameObject RecordPrefab;

      /// <summary>Station prefab for the radio list.</summary>
      [Tooltip("Station prefab for the radio list.")] public GameObject StationPrefab;

      public Text ButtonText;

      private const string recordText = " <b>Record</b>";
      private const string stationText = " <b>Station</b>";

      private bool orderByStation = true;

      public void SwitchOrder()
      {
         orderByStation = !orderByStation;

         ButtonText.text = orderByStation ? stationText : recordText;
         GuiOnRadio.ItemPrefab = orderByStation ? StationPrefab : RecordPrefab;

         GuiOnRadio.Rebuild();
      }
   }
}