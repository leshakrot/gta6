using Invector.vCharacterController;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            audio.enabled = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            audio.enabled = false;
        }
    }
}
