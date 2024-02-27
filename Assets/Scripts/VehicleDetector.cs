using Invector;
using Invector.vCharacterController.AI;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class VehicleDetector : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody[] _rbs;
    private RCC_CarControllerV3 _vehicleController;

    private void Start()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();       
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if(collision.gameObject.TryGetComponent(out VehicleBody vehicle))
        {
            _vehicleController = vehicle.gameObject.GetComponent<RCC_CarControllerV3>();

            transform.root.SendMessage("ActivateRagdoll", SendMessageOptions.DontRequireReceiver);

            Vector3 punchDirection = new Vector3(transform.position.x - collision.transform.position.x, 1f, transform.position.z - collision.transform.position.z).normalized;
            var vehicleMagnitude = _vehicleController.speed;

            foreach (var bone in _rbs)
            {
                bone.AddForce(punchDirection * vehicleMagnitude * 3f, ForceMode.Impulse);
            }
        }
    }
}
