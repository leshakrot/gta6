using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBody : MonoBehaviour
{
    public Camera mainCamera;

    private vThirdPersonController _player;

    void Start()
    {
        _player = FindObjectOfType<vThirdPersonController>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, _player.transform.position) > 20f)
        {
            HideObject();
        }
    }

    void HideObject()
    {
        if (!IsObjectVisible(gameObject))
        {
            gameObject.SetActive(false);
        }
    }

    bool IsObjectVisible(GameObject obj)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(obj.transform.position);

        bool isVisible = screenPoint.x >= 0 && screenPoint.x <= 1 &&
                         screenPoint.y >= 0 && screenPoint.y <= 1 &&
                         screenPoint.z > 0;

        return isVisible;
    }
}
