using PixelCrushers.QuestMachine;
using UnityEngine;

public class BarnCheckpoint : MonoBehaviour
{
    QuestControl questControl;

    private void Start()
    {
        questControl = GetComponent<QuestControl>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Farmer player))
        {
            Farmer.onHarvestDelivered?.Invoke();
            if(GetComponent<QuestControl>().IsConditionMet())
            questControl.SendToMessageSystem("Got:Harvest");
            gameObject.SetActive(false);
        }
    }
}
