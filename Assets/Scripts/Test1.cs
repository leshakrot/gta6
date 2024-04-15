using UnityEngine;

public class Test1 : MonoBehaviour
{
    public GameObject player;
    public GameObject controller;
    public GameObject camera2;

    private void Start()
    {
        Invoke("Toggle", 10f);
    }
    public void Toggle()
    {
            player.SetActive(false);
            controller.SetActive(false);
            camera2.SetActive(true);
    }
}
