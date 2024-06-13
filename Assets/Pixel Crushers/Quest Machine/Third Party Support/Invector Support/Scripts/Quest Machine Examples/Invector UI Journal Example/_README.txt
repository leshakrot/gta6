How to Connect Quest Machine's Quest Journal to Invector's Inventory UI:

1. Set the Quest Machine > Quest Machine Canvas > Quest Journal UI's
   - Send Message On Open: Always
   - Open Message: "Opened Quest Journal" (without quotes)
   - Close Message: "Closed Quest Journal" (without quotes)

2. Make a copy of the Inventory_ShooterMelee prefab.
   - Assign the copy to the character's vItemManager component.
   - Add the InventoryQuestJournalUtility component.
   - Add a Journal UI button inside InventoryWindow > MainWindow.
     Configure the events to call InventoryQuestJournalUtility.ShowQuestJournal.