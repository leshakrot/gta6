using UnityEngine;

public class VehicleBody : MonoBehaviour
{
    [SerializeField] private GameObject _destroyParticle;
    private Renderer[] _vehicleRenderer;

    private ESVehicleAI _vehicleAI;
    private BCG_EnterExitVehicle _enterExitVehicle;
    private void Start()
    {
        _vehicleAI = GetComponent<ESVehicleAI>();
        _enterExitVehicle = GetComponent<BCG_EnterExitVehicle>();
        _vehicleRenderer = GetComponentsInChildren<Renderer>();
        Debug.Log(_vehicleRenderer);
    }
    public void DestroyVehicle()
    {
        foreach(Renderer r in _vehicleRenderer)
        {
            r.material.color = Color.black;
        }      
        _destroyParticle.SetActive(true);
        _vehicleAI.enabled = false;
        _enterExitVehicle.enabled = false;
    }
}
