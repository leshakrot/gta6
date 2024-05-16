using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace EasyBuildSystem.Packages.Addons.BuildingMenu
{
    public class Demo_OrbitCamera : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] float m_NormalMovementSpeed = 8f;
        [SerializeField] float m_SprintMovementSpeed = 16f; // Скорость при зажатом Shift

        [Header("Rotation Settings")]
        [SerializeField] float m_RotateSpeed = 15;

        Vector3 m_PanMovement;
        Vector3 m_LastMousePosition;

        void Update()
        {
            m_PanMovement = Vector3.zero;

            float currentMovementSpeed = Input.GetKey(KeyCode.LeftShift) ? m_SprintMovementSpeed : m_NormalMovementSpeed; // Проверяем, зажат ли Shift

#if ENABLE_INPUT_SYSTEM

            if (Keyboard.current.wKey.isPressed)
            {
                m_PanMovement += Vector3.forward * currentMovementSpeed * Time.deltaTime;
            }

            if (Keyboard.current.sKey.isPressed)
            {
                m_PanMovement -= Vector3.forward * currentMovementSpeed * Time.deltaTime;
            }

            if (Keyboard.current.aKey.isPressed)
            {
                m_PanMovement += Vector3.left * currentMovementSpeed * Time.deltaTime;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                m_PanMovement += Vector3.right * currentMovementSpeed * Time.deltaTime;
            }

            transform.Translate(transform.TransformDirection(m_PanMovement), Space.World);

            Vector2 inputAxis = Mouse.current.position.ReadValue();

            if (Mouse.current.rightButton.isPressed)
            {
                Vector3 mouseDelta;

                if (m_LastMousePosition.x >= 0 &&
                    m_LastMousePosition.y >= 0 &&
                    m_LastMousePosition.x <= Screen.width &&
                    m_LastMousePosition.y <= Screen.height)
                    mouseDelta = new Vector3(inputAxis.x, inputAxis.y, 0) - m_LastMousePosition;
                else
                {
                    mouseDelta = Vector3.zero;
                }

                float rotationX = -mouseDelta.y * m_RotateSpeed * Time.deltaTime;
                float rotationY = mouseDelta.x * m_RotateSpeed * Time.deltaTime;

                transform.RotateAround(transform.position, transform.right, rotationX);
                transform.RotateAround(transform.position, Vector3.up, rotationY);
            }

            m_LastMousePosition = new Vector3(inputAxis.x, inputAxis.y, 0);
#else
            if (Input.GetKey(KeyCode.W))
            {
                m_PanMovement += Vector3.forward * currentMovementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                m_PanMovement -= Vector3.forward * currentMovementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A))
            {
                m_PanMovement += Vector3.left * currentMovementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                m_PanMovement += Vector3.right * currentMovementSpeed * Time.deltaTime;
            }

            transform.Translate(transform.TransformDirection(m_PanMovement), Space.World);

            if (Input.GetMouseButton(1))
            {
                Vector3 mouseDelta;

                if (m_LastMousePosition.x >= 0 &&
                    m_LastMousePosition.y >= 0 &&
                    m_LastMousePosition.x <= Screen.width &&
                    m_LastMousePosition.y <= Screen.height)
                    mouseDelta = Input.mousePosition - m_LastMousePosition;
                else
                {
                    mouseDelta = Vector3.zero;
                }

                float rotationX = -mouseDelta.y * m_RotateSpeed * Time.deltaTime;
                float rotationY = mouseDelta.x * m_RotateSpeed * Time.deltaTime;

                transform.RotateAround(transform.position, transform.right, rotationX);
                transform.RotateAround(transform.position, Vector3.up, rotationY);
            }

            m_LastMousePosition = Input.mousePosition;
#endif
        }
    }
}
