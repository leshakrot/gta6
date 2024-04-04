
using Invector.vCharacterController;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            if (player.isGrounded)
            {
                player.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 700f, ForceMode.Impulse);
            }
        }
    }
}