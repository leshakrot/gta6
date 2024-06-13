using UnityEngine;
using Invector;

namespace PixelCrushers.InvectorSupport
{

    /// <summary>
    /// Save System saver for vSimpleDoor.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Save System/Savers/Invector/Invector Simple Door Saver")]
    [RequireComponent(typeof(vSimpleDoor))]
    public class InvectorSimpleDoorSaver: Saver
    {

        private vSimpleDoor door;

        public override void Awake()
        {
            base.Awake();
            door = GetComponent<vSimpleDoor>();
        }

        public override string RecordData()
        {
            var isOpen = door.state == vSimpleDoor.DoorState.Opened || door.state == vSimpleDoor.DoorState.Opening;
            return isOpen ? "1" : "0";
        }

        public override void ApplyData(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            if (s == "1") door.Open();
            else door.Close();
        }

    }
}
