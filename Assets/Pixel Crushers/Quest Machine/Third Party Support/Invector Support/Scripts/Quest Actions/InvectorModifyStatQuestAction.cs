using UnityEngine;
using PixelCrushers.QuestMachine.InvectorSupport;

namespace PixelCrushers.QuestMachine
{

    public enum InvectorStat { Health, MaxHealth }

    public class InvectorModifyStatQuestAction : QuestAction
    {

        [Tooltip("Stat to change.")]
        [SerializeField]
        private InvectorStat m_stat = InvectorStat.Health;

        [Tooltip("Amount to add or remove.")]
        [SerializeField]
        private QuestNumber m_amount = new QuestNumber();

        public InvectorStat stat
        {
            get { return m_stat; }
            set { m_stat = value; }
        }

        public QuestNumber amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }

        public override string GetEditorName()
        {
            var x = amount.GetValue(quest);
            return (x >= 0) ? ("Invector add " + x + " to " + stat) : ("Invector remove " + -x + "  from " + stat);
        }

        public override void Execute()
        {
            base.Execute();
            switch (stat)
            {
                case InvectorStat.Health:
                    QuestMachine_InvectorUtils.AddHealth(amount.GetValue(quest));
                    break;
                case InvectorStat.MaxHealth:
                    QuestMachine_InvectorUtils.AddMaxHealth(amount.GetValue(quest));
                    break;
            }
        }
    }
}
