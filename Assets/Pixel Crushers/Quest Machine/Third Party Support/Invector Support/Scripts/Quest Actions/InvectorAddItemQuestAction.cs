using UnityEngine;
using PixelCrushers.QuestMachine.InvectorSupport;

namespace PixelCrushers.QuestMachine
{

    public class InvectorAddItemQuestAction : QuestAction
    {

        [Tooltip("ID of item to add or remove.")]
        [SerializeField]
        private int m_itemID = 0;

        [Tooltip("Amount to give or remove from player. (Negative value removes from player.)")]
        [SerializeField]
        private QuestNumber m_amount = new QuestNumber();

        [Tooltip("If adding, auto-equip.")]
        [SerializeField]
        private bool m_autoEquip = false;

        public int itemID
        {
            get { return m_itemID; }
            set { m_itemID = value; }
        }

        public QuestNumber amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }

        public bool autoEquip
        {
            get { return m_autoEquip; }
            set { m_autoEquip = value; }
        }

        public override string GetEditorName()
        {
            var x = amount.GetValue(quest);
            return (x >= 0) ? ("Invector add " + x + " of item ID " + itemID) : ("Invector remove " + -x + "  of item ID " + itemID);
        }

        public override void Execute()
        {
            base.Execute();
            QuestMachine_InvectorUtils.AddItem(itemID, amount.GetValue(quest), autoEquip);
        }

    }

}
