using Invector.vItemManager;
using UnityEngine;

namespace PixelCrushers.QuestMachine
{

    public class InvectorQuestJournalUtility : MonoBehaviour, IMessageHandler
    {
        public KeyCode journalToggleKey = KeyCode.J;
        public string journalToggleButton = string.Empty;

        private bool wasInventoryOpen = false;

        private void OnEnable()
        {
            MessageSystem.AddListener(this, "Opened Quest Journal", string.Empty);
            MessageSystem.AddListener(this, "Closed Quest Journal", string.Empty);
        }

        private void OnDisable()
        {
            MessageSystem.RemoveListener(this);
        }

        public void OnMessage(MessageArgs messageArgs)
        {
            var vInventory = GetComponent<vInventory>();
            switch (messageArgs.message)
            {
                case "Opened Quest Journal":
                    wasInventoryOpen = vInventory.isOpen;
                    vInventory.CloseInventory();
                    MessageSystem.SendMessage(this, "Pause Player", string.Empty);
                    if (InputDeviceManager.deviceUsesCursor) InputDeviceManager.instance.ForceCursor(true);
                    break;
                case "Closed Quest Journal":
                    MessageSystem.SendMessage(this, "Unpause Player", string.Empty);
                    if (wasInventoryOpen)
                    {
                        vInventory.OpenInventory();
                    }
                    break;
            }
        }

        public void ShowQuestJournal()
        {
            FindObjectOfType<QuestJournal>().ShowJournalUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(journalToggleKey) ||
                (!string.IsNullOrEmpty(journalToggleButton) && Input.GetButtonDown(journalToggleButton)))
            {
                FindObjectOfType<QuestJournal>().ToggleJournalUI();
            }
        }

    }
}