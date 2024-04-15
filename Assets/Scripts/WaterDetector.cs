using Invector.vCharacterController;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    [SerializeField] private GameObject _halo;
    [SerializeField] private GameObject _bread;
    [SerializeField] private GameObject _vine;

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
            _halo.SetActive(true);
            _vine.SetActive(true);
            _bread.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out vThirdPersonController player))
        {
            audio.enabled = false;
            _halo.SetActive(false);
            _vine.SetActive(false);
            _bread.SetActive(false);
        }
    }
}
