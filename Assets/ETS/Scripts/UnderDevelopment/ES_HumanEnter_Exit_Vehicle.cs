using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ES_HumanEnter_Exit_Vehicle : MonoBehaviour
{
    public ESGrabVehicle grabVehicle;
    public Text GrabText;
    [HideInInspector] public Animator animator;
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        if (GrabText != null)
        {
            GrabText.text = "";
        }
    }
    void CorrectCharacterControllerCoordinate(Vector3 JumpPos, Transform BodyHipBone)
    {
        BodyHipBone.position -= JumpPos;
        transform.position += JumpPos;
    }
    private void OnAnimatorIK()
    {
        if (grabVehicle == null) return;
        if (animator == null) return;

        if (grabVehicle.OnSit)
        {
            if (grabVehicle.UseSitpoint)
                animator.bodyPosition = grabVehicle.SitPoint.position;
        }
    }

}
