using UnityEngine;

public class BuildingToggleHolder : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _buildingModule;
    [SerializeField] private GameObject _buildingCamera;
    [SerializeField] private Vector3 _cameraOffset;

    private bool _isBuildingActive;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _buildingCamera.transform.position = _player.transform.position + _cameraOffset;
            _isBuildingActive = !_isBuildingActive;
            _player.SetActive(!_isBuildingActive);
            _playerCamera.SetActive(!_isBuildingActive);
            _buildingModule.SetActive(_isBuildingActive);
        }
    }
}
