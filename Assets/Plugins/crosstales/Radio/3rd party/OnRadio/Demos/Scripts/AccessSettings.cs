using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.Radio.OnRadio.Demo
{
   /// <summary>Set the access settings for OnRadio.</summary>
   [HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_on_radio_1_1_demo_1_1_access_settings.html")]
   public class AccessSettings : MonoBehaviour
   {
      #region Variables

      public Service.BaseService Service;

      public GameObject SettingsPanel;

      public InputField Token;

      public Button OkButton;

      private string enteredToken = string.Empty;

      private static string lastToken;

      private Color okColor;

      #endregion


      #region MonoBehaviour methods

      private void Start()
      {
         okColor = OkButton.image.color;

         if (!string.IsNullOrEmpty(lastToken))
            Service.Token = lastToken;

         if (string.IsNullOrEmpty(Service.Token))
         {
            ShowSettings();
         }
         else
         {
            HideSettings();
            enteredToken = lastToken = Token.text = Service.Token;
         }

         SetOkButton();
      }

      #endregion


      #region Public methods

      public void OnTokenEntered(string key)
      {
         enteredToken = string.IsNullOrEmpty(key) ? string.Empty : key.Trim();
         SetOkButton();
      }

      public void HideSettings()
      {
         SettingsPanel.SetActive(false);

         if (!string.IsNullOrEmpty(enteredToken) && !enteredToken.Equals(lastToken))
         {
            lastToken = Service.Token = enteredToken;

            if (Service.QueryOnStart)
               Service.Query();
         }
      }

      public void ShowSettings()
      {
         SettingsPanel.SetActive(true);
      }

      public void SetOkButton()
      {
         if (enteredToken.Length >= 10 && enteredToken.CTisInteger())
         {
            OkButton.interactable = true;
            OkButton.image.color = okColor;
         }
         else
         {
            OkButton.interactable = false;
            OkButton.image.color = Color.gray;
         }
      }

      #endregion
   }
}
// © 2020-2021 crosstales LLC (https://www.crosstales.com)