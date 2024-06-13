using System.Collections;
using UnityEngine;

namespace PixelCrushers.QuestMachine
{
    /// <summary>
    /// This component updates quest UIs at the end of the Start loop
    /// when a scene starts. 
    /// </summary>
    public class UpdateQuestUIsOnStart : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            QuestMachineMessages.RefreshUIs(this);
        }
    }
}