using Invector.vCharacterController;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out vThirdPersonController player))
        {
            transform.GetChild(0).gameObject.transform.Rotate(0, -105, 0);
            _sfx.PlayOneShot(_openSound);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out vThirdPersonController player))
        {
            transform.GetChild(0).gameObject.transform.Rotate(0, 105, 0);
            _sfx.PlayOneShot(_closeSound);
        }
    }
}
