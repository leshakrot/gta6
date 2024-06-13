using UnityEngine;
using UnityEngine.Events;
using PixelCrushers.InvectorSupport;

namespace PixelCrushers.QuestMachine.InvectorSupport
{

    /// <summary>
    /// Handles pausing and unpausing the Invector player character,
    /// for example in dialogue and journal UIs.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Quest Machine/Third Party/Invector/Quest Machine Pause Player")]
    public class QuestMachinePausePlayer : MonoBehaviour, IMessageHandler
    {
        public UnityEvent onPause = new UnityEvent();
        public UnityEvent onUnpause = new UnityEvent();

        protected virtual void OnEnable()
        {
            MessageSystem.AddListener(this, "Pause Player", string.Empty);
            MessageSystem.AddListener(this, "Unpause Player", string.Empty);
        }

        protected virtual void OnDisable()
        {
            MessageSystem.RemoveListener(this);
        }

        public void OnMessage(MessageArgs messageArgs)
        {
            switch (messageArgs.message)
            {
                case "Pause Player":
                    InvectorPlayerUtility.PausePlayer();
                    onPause.Invoke();
                    break;
                case "Unpause Player":
                    InvectorPlayerUtility.UnpausePlayer();
                    onUnpause.Invoke();
                    break;
            }
        }

    }
}
