using UnityEngine;

public class NPCVehicleDetector : MonoBehaviour
{
    private Rigidbody[] _rbs;
    private RCC_CarControllerV3 _vehicleController;

    private void Start()
    {
        _rbs = GetComponentsInChildren<Rigidbody>();      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out VehicleBody vehicle))
        {
            Debug.Log("Collision");
            _vehicleController = vehicle.gameObject.GetComponent<RCC_CarControllerV3>();

            SendMessage("ActivateRagdoll", SendMessageOptions.DontRequireReceiver);

            Vector3 punchDirection = new Vector3(transform.position.x - collision.transform.position.x, 1f, transform.position.z - collision.transform.position.z).normalized;
            var vehicleMagnitude = _vehicleController.speed;

            foreach (var bone in _rbs)
            {
                bone.AddForce(punchDirection * vehicleMagnitude * 3f, ForceMode.Impulse);
            }
        }
    }
}
