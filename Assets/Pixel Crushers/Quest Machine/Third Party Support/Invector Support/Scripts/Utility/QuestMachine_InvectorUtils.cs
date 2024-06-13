using UnityEngine;

namespace PixelCrushers.QuestMachine.InvectorSupport
{
    public static class QuestMachine_InvectorUtils
    {

        public static Invector.vCharacterController.vThirdPersonController GetController()
        {
            return GameObject.FindObjectOfType<Invector.vCharacterController.vThirdPersonController>();
        }

        public static void AddHealth(int v)
        {
            var controller = GetController();
            if (controller != null) controller.ChangeHealth(v);
        }

        public static void AddMaxHealth(int v)
        {
            var controller = GetController();
            if (controller != null) controller.ChangeMaxHealth(v);
        }


#if USE_INVECTOR_INVENTORY

        public static Invector.vItemManager.vItemManager GetItemManager()
        {
            return GameObject.FindObjectOfType<Invector.vItemManager.vItemManager>();
        }

        public static int GetItemAmount(int itemID)
        {
            var itemManager = GetItemManager();
            if (itemManager)
            {
                int total = 0;
                var allItems = itemManager.GetItems(itemID);
                foreach (var item in allItems)
                {
                    if (item.id == itemID)
                    {
                        total += item.amount;
                    }
                }
                return total;
            }
            return 0;
        }

        public static void AddItem(int itemID, int amount, bool autoEquip)
        {
            var itemManager = GetItemManager();
            if (itemManager == null || amount == 0) return;
            if (amount > 0)
            {
                var reference = new Invector.vItemManager.ItemReference(itemID);
                reference.amount = amount;
                reference.autoEquip = autoEquip;
                reference.addToEquipArea = autoEquip;
                itemManager.AddItem(reference);
            }
            else
            {
                var allItems = itemManager.GetItems();
                for (int i = allItems.Count - 1; i >= 0; i--)
                {
                    var item = allItems[i];
                    if (item.id == itemID)
                    {
                        itemManager.DestroyItem(item, -amount);
                    }
                }
            }
        }

#else

        public static int GetItemAmount(int itemID)
        {
            Debug.LogError("You must define the scripting symbol USE_INVECTOR_INVENTORY to use Invector inventory features in Quest Machine.");
            return 0;
        }

        public static void AddItem(int itemID, int amount, bool autoEquip)
        {
            Debug.LogError("You must define the scripting symbol USE_INVECTOR_INVENTORY to use Invector inventory features in Quest Machine.");
        }

#endif

    }
}
