using UnityEngine;
using PixelCrushers.QuestMachine.InvectorSupport;
using Invector.vItemManager;

namespace PixelCrushers.QuestMachine
{

    public class InvectorHasItemsQuestCondition : QuestCondition
    {

        [Tooltip("ID of item to check.")]
        [SerializeField]
        private int m_itemID = 0;

        [Tooltip("How the player's current amount applies to the required amount.")]
        [SerializeField]
        private CounterValueConditionMode m_valueMode = CounterValueConditionMode.AtLeast;

        [Tooltip("Required amount.")]
        [SerializeField]
        private QuestNumber m_requiredAmount = new QuestNumber();

        [Tooltip("If assigned, keep this quest counter updated while waiting for this condition to be true. Inspect the quest's main info to view/edit counters.")]
        [SerializeField]
        private int m_counterIndex;

        public int itemID
        {
            get { return m_itemID; }
            set { m_itemID = value; }
        }

        public CounterValueConditionMode valueMode
        {
            get { return m_valueMode; }
            set { m_valueMode = value; }
        }

        public QuestNumber requiredAmount
        {
            get { return m_requiredAmount; }
            set { m_requiredAmount = value; }
        }

        public int counterIndex
        {
            get { return m_counterIndex; }
            set { m_counterIndex = value; }
        }

        private QuestCounter counter { get; set; }

        public override string GetEditorName()
        {
            return "Invector player has " + valueMode + " " + requiredAmount.GetValue(quest) + " of item ID " + itemID;
        }

#if USE_INVECTOR_INVENTORY

        public override void StartChecking(System.Action trueAction)
        {
            base.StartChecking(trueAction);
            counter = quest.GetCounter(counterIndex);
            var itemManager = QuestMachine_InvectorUtils.GetItemManager();
            if (itemManager != null)
            {
                itemManager.onAddItem.AddListener(OnHandleItem);
                itemManager.onChangeItemAmount.AddListener(OnHandleItem);
                itemManager.onDestroyItem.AddListener(OnChangeItem);
                itemManager.onDropItem.AddListener(OnChangeItem);
                CheckItemCount();
            }
        }

        public override void StopChecking()
        {
            base.StopChecking();
            var itemManager = QuestMachine_InvectorUtils.GetItemManager();
            if (itemManager != null)
            {
                itemManager.onAddItem.RemoveListener(OnHandleItem);
                itemManager.onChangeItemAmount.RemoveListener(OnHandleItem);
                itemManager.onDestroyItem.RemoveListener(OnChangeItem);
                itemManager.onDropItem.RemoveListener(OnChangeItem);
            }
        }

        private void OnHandleItem(vItem item)
        {
            CheckItemCount();
        }

        private void OnChangeItem(vItem item, int amount)
        {
            CheckItemCount();
        }

        public void CheckItemCount()
        {
            var currentAmount = QuestMachine_InvectorUtils.GetItemAmount(itemID);
            if (counter != null) counter.currentValue = currentAmount;
            var actualRequiredAmount = requiredAmount.GetValue(quest);
            bool isRequirementMet = false;
            switch (valueMode)
            {
                default:
                case CounterValueConditionMode.AtLeast:
                    isRequirementMet = currentAmount >= actualRequiredAmount;
                    break;
                case CounterValueConditionMode.AtMost:
                    isRequirementMet = currentAmount <= actualRequiredAmount;
                    break;
            }
            if (isRequirementMet)
            {
                if (QuestMachine.debug) Debug.Log("Quest Machine: InvectorHasItemsQuestCondition player has " + valueMode + " " + requiredAmount.GetValue(quest) + " of item ID " + itemID + " is true.");
                StopChecking();
                SetTrue();
            }
        }

#else

        public override void StartChecking(System.Action trueAction)
        {
            base.StartChecking(trueAction);
            Debug.LogError(GetType().Name + ": You must define the scripting symbol USE_INVECTOR_INVENTORY to use Invector inventory features in Quest Machine.");
        }

#endif

    }
}
