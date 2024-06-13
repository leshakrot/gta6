using UnityEngine;
using Invector.vCamera;

namespace PixelCrushers.DialogueSystem.InvectorSupport
{

    public class InvectorPositionSaver : PositionSaver
    {

        protected override void SetPosition(Vector3 position, Quaternion rotation)
        {
            if (vThirdPersonCamera.instance != null)
            {
                var smooth = vThirdPersonCamera.instance.currentState.smooth;
                var smoothDamp = vThirdPersonCamera.instance.currentState.smoothDamp;
                vThirdPersonCamera.instance.currentState.smooth = 0;
                vThirdPersonCamera.instance.currentState.smoothDamp = 0;
                base.SetPosition(position, rotation);
            }
            else
            {
                base.SetPosition(position, rotation); 
            }
        }

    }
}
