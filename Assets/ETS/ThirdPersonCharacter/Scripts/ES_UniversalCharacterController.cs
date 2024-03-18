using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class ES_UniversalCharacterController : MonoBehaviour
{
    public enum CharacterType
    {
        FirstMan,
        ThirdPerson
    }
    public CharacterType GetCharacterType = CharacterType.ThirdPerson;
    public Transform ThirdCam, FirstCam;
    public List<SkinnedMeshRenderer> FirstPersonBodyPartsMeshRender = new List<SkinnedMeshRenderer>();
    public Transform FirstPersonRagdollCollection, ThirdPersonRagdollCollection;
    public Avatar FirstPersonAvatar, ThirdPersonAvatar;
    //[HideInInspector]
    public bool ReturnControls;
    //
    //
    //
    ///
    private float LastPosY;
    private float LastPosX;

    [SerializeField] private bool JustLefttheground;
    private bool DoLerpBob = true;
    [Header("BobSettings")]
    public float Bobcount;
    public float BobSpeed = 10f;
    public float BobCountModifier = .25f;
    public float MaxHeadBobX = .25f;
    public float MaxHeadBobY = .15f;
    public float LerpBobSpeed = 10f;
    public float AimAcurracy = .35f;
    public float HeadHieghtModifier = .15f;
    [Header("End")]
    public float stepvariation;
    public AnimationCurve LerpBobAnimCurve;
    private float newstep, oldstep, mytime, ReturnHeadheight;
    [Tooltip("ReadOnly")]
    public float HeadHeight;
    private Vector3 me;
    private Keyframe animlength;
    private Vector3 LastBodyPos;
    [SerializeField] private float m_RunCycleLegOffset = 0.2f;
    private bool m_Crouching;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float senstivity = 5;
    private float pre_mul = 0;
    private float movecounter;
    private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer, InvertedMouse = true;
    private float FowardValue;
    [SerializeField] private float TurnValue;
    private float gravityValue = -9.81f;
    private Vector3 m_Move;
    private Vector3 rotationlast;
    private Vector3 move;
    [SerializeField] private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;
    private bool m_jump, accuratecheckforgrounded;
    private float H, V;
    private const float k_Half = 0.5f;
    private bool crouch;
    private float m_CapsuleHeight, MouseY, MouseX;
    [SerializeField] private float sensorraduis = 0.5f, sensorposmodifier = 0.5f;
    private Vector3 m_CapsuleCenter, BodyRotation, HeadRotation;
    private Vector3 BackUpFirtPos;
    private float mydeltaTime;
    private Transform obj;
    //
    [SerializeField] private bool NeedsToRetrunHeight, preventpush;
    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        //controller.detectCollisions = false;
        m_Animator = this.GetComponent<Animator>();
        rotationlast = transform.rotation.eulerAngles;
        m_CapsuleHeight = controller.height;
        m_CapsuleCenter = controller.center;
        HeadHeight = -(controller.height / 2f) + HeadHieghtModifier;
        ReturnHeadheight = HeadHeight;
        if (GetCharacterType == CharacterType.FirstMan) BackUpFirtPos = FirstCam.localPosition;
        SwitchCharacterVeiws();
    }
    private void SwitchCharacterVeiws()
    {
        if (ThirdCam == null && FirstCam == null) return;
        if (GetCharacterType == CharacterType.ThirdPerson)
        {
            m_Cam = ThirdCam;
            ThirdCam.GetComponent<Camera>().enabled = true;
            ThirdCam.GetComponent<AudioListener>().enabled = true;
            FirstCam.GetComponent<AudioListener>().enabled = false;
            FirstCam.GetComponent<Camera>().enabled = false;
            FirstPersonRagdollCollection.gameObject.SetActive(false);
            ThirdPersonRagdollCollection.gameObject.SetActive(true);
            m_Animator.avatar = ThirdPersonAvatar;
        }
        else if (GetCharacterType == CharacterType.FirstMan)
        {
            m_Cam = FirstCam;
            FirstCam.GetComponent<Camera>().enabled = true;
            FirstCam.GetComponent<AudioListener>().enabled = true;
            ThirdCam.GetComponent<AudioListener>().enabled = false;
            ThirdCam.GetComponent<Camera>().enabled = false;
            FirstPersonRagdollCollection.gameObject.SetActive(true);
            ThirdPersonRagdollCollection.gameObject.SetActive(false);
            m_Animator.avatar = FirstPersonAvatar;
        }
    }
    //
    public void StopCameraRender()
    {
        if (ThirdCam == null) return; if (FirstCam == null) return;

        FirstCam.GetComponent<Camera>().enabled = false;
        FirstCam.GetComponent<AudioListener>().enabled = false;
        ThirdCam.GetComponent<AudioListener>().enabled = false;
        ThirdCam.GetComponent<Camera>().enabled = false;

    }
    //
    private IEnumerator ReturnInput(float t)
    {
        yield return new WaitForSeconds(t);
        preventpush = false;
    }
    //
    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.collider.attachedRigidbody != null)
        {
            //print(hit.rigidbody.tag);
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
            {
                preventpush = true;
                //m_Move *= 0.01f;
            }
        }
        //
        if (Mathf.Abs(Input.GetAxis("Vertical")) == 0 && Mathf.Abs(Input.GetAxis("Horizontal")) == 0)
        {
            preventpush = false;

        }
    }
*/
    //
    public void StartCameraRender()
    {
        if (ThirdCam == null) return; if (FirstCam == null) return;

        if (GetCharacterType == CharacterType.FirstMan)
        {
            FirstCam.GetComponent<Camera>().enabled = true;
            FirstCam.GetComponent<AudioListener>().enabled = true;
            ThirdCam.GetComponent<AudioListener>().enabled = false;
            ThirdCam.GetComponent<Camera>().enabled = false;
        }
        else
        {
            ThirdCam.GetComponent<Camera>().enabled = true;
            ThirdCam.GetComponent<AudioListener>().enabled = true;
            FirstCam.GetComponent<Camera>().enabled = false;
            FirstCam.GetComponent<AudioListener>().enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        Gizmos.color = Color.red;
        Vector3 pos = this.transform.position;
        pos += transform.up * sensorposmodifier;
        Gizmos.DrawWireSphere(pos, sensorraduis);
    }
    void FixedUpdate()
    {
        if (!this.CompareTag("Player"))
        {
            ReturnControls = true;
        }
        RaycastHit myhit = new RaycastHit();
        if (this.CompareTag("Player"))
        {
            if (Physics.SphereCast(transform.position, sensorraduis, transform.up, out myhit, sensorposmodifier, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                ///print(myhit.transform.tag);
                if (myhit.collider.attachedRigidbody != null)
                {
                    //print(hit.rigidbody.tag);
                    if (myhit.collider.attachedRigidbody.GetComponent<ESPedestrainCtrl>() == null)
                    {
                        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                        {
                            obj = myhit.collider.transform;
                            preventpush = true;
                        }
                    }
                }
                //
            }

        }
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            m_jump = false;
            playerVelocity.y = 0f;
        }
        if (preventpush == false)
        {
            H = ReturnControls ? 0 : Input.GetAxis("Horizontal");
            V = ReturnControls ? 0 : Input.GetAxis("Vertical");
        }
        else
        {
            H = 1;
            V = 1;
        }

        pre_mul = V > 0 ? 1 : -1;
        if (m_Cam != null)
        {
            if (preventpush)
            {
                H = H * -1;
                V = V * -1;
                if (obj != null)
                {
                    if (UCTMath.CalculateVector3Distance(this.transform.position, obj.position) > 1.5f * 1.5f)
                    {
                        obj = null;
                        preventpush = false;
                    }
                }
            }
            if (GetCharacterType == CharacterType.ThirdPerson)
            {

                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = V * m_CamForward + H * m_Cam.right;
            }
            else
            {
                MouseY += Input.GetAxis("Mouse Y") * senstivity;
                MouseX = Input.GetAxis("Mouse X") * senstivity;
                BodyRotation = new Vector3(0f, MouseX, 0f);
                HeadRotation = m_Cam.localEulerAngles;
                MouseY = Mathf.Clamp(MouseY, -65, 65);
                if (InvertedMouse)
                {
                    HeadRotation.x = -MouseY;
                }
                else if (InvertedMouse)
                {
                    HeadRotation.x = MouseY;
                }
                m_Cam.localRotation = Quaternion.Euler(HeadRotation);
                m_Move = transform.rotation * m_Move;
                transform.Rotate(BodyRotation);
                m_Move = V * transform.forward;
            }
        }
        else
        {
            m_Move = V * Vector3.forward + H * Vector3.right;
        }
        //m_Move = V * Vector3.forward + H * Vector3.right;
        if (m_Move.magnitude > 1f) m_Move.Normalize();
        if (GetCharacterType == CharacterType.ThirdPerson)
        {
            if (Input.GetKey(KeyCode.LeftShift) || m_Crouching) m_Move *= 0.5f;
        }
        else if (GetCharacterType == CharacterType.FirstMan)
        {
            if (Input.GetKey(KeyCode.LeftShift) || m_Crouching || V < 0) m_Move *= 0.5f;
        }
        // move = transform.InverseTransformDirection(move);
        Move(m_Move);
        HeadBob();
        UpdateAnimator();
    }
    //
    private void Update()
    {
        if (ReturnControls) return;
        if (Input.GetKey(KeyCode.C))
        {
            crouch = true;
        }
        else
        {
            crouch = false;
        }
    }
    //
    void Move(Vector3 moveplayer)
    {
        // m_Move = v*Vector3.forward + h*Vector3.right;
        move = new Vector3(moveplayer.x, 0, moveplayer.z);
        //move = moveplayer;
        //
        if (controller.enabled == true)
            controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero && GetCharacterType == CharacterType.ThirdPerson)
        {
            //gameObject.transform.forward = move;
            Quaternion direction = Quaternion.LookRotation(move);
            this.transform.rotation = Quaternion.Lerp(transform.rotation, direction, 5f * Time.deltaTime);
        }

        // Changes the height position of the player..
        if (ReturnControls == false)
        {
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                m_jump = true;
            }
        }
        //
        playerVelocity.y += gravityValue * Time.deltaTime;
        if (controller.enabled == true)
            controller.Move(playerVelocity * Time.deltaTime);

        FowardValue = move.magnitude;
        Vector3 relativepoint = transform.InverseTransformDirection(move);
        TurnValue = GetCharacterType == CharacterType.FirstMan ?
          Mathf.Atan2(MouseX, Input.GetAxis("Mouse Y")) : Mathf.Atan2(relativepoint.x, relativepoint.z);
        //prevent character from standing in tight places
        if (!m_Crouching)
        {

            Ray crouchRay = new Ray(controller.transform.position + Vector3.up * controller.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - controller.radius * k_Half;
            if (Physics.SphereCast(crouchRay, controller.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_Crouching = true;
            }
        }
        //scale charactercontroller while crounching

        if (groundedPlayer && crouch)
        {
            if (m_Crouching) return;
            controller.height = controller.height / 2f;
            controller.center = controller.center / 2f;
            m_Crouching = true;
            NeedsToRetrunHeight = true;
        }
        else
        {
            Ray crouchRay = new Ray(controller.transform.position + Vector3.up * controller.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - controller.radius * k_Half;
            if (Physics.SphereCast(crouchRay, controller.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_Crouching = true;
                NeedsToRetrunHeight = true;
                return;
            }
            if (NeedsToRetrunHeight)
            {
                controller.height = m_CapsuleHeight;
                controller.center = m_CapsuleCenter;
                NeedsToRetrunHeight = false;
            }
            m_Crouching = false;
        }
    }
    //
    private void HeadBob()
    {
        if (m_Cam == null) return;
        if (GetCharacterType == CharacterType.ThirdPerson) return;
        if (controller.isGrounded)
        {
            //HeadBobSpeedMultiplier = FPSCTRL.m_Running ? HeadBobSpeedMultiplier = BobRunMultipier : HeadBobSpeedMultiplier = BobWalkMultipier;       
            // print(BobCountModifier);
            Bobcount += (Vector3.Distance(LastBodyPos, transform.position) * BobSpeed) * (BobCountModifier);
            me = m_Cam.localPosition;
            oldstep = me.x;
            //curve of carmera in x axis
            me.x = Mathf.Sin(Bobcount) * MaxHeadBobX * AimAcurracy;
            // curve of camera in y axis
            if (!JustLefttheground)
            {
                me.y = (Mathf.Cos(Bobcount * 2) * MaxHeadBobY * -1 * AimAcurracy) + (controller.height / 2f - HeadHeight);
            }

            if (JustLefttheground && DoLerpBob)
            {
                mytime += LerpBobSpeed * Time.deltaTime;
                me = new Vector3(m_Cam.localPosition.x, LerpBobAnimCurve.Evaluate(mytime), m_Cam.localPosition.z);
                animlength = LerpBobAnimCurve[LerpBobAnimCurve.length - 1];
                if (mytime > animlength.time)
                {
                    JustLefttheground = false;
                    //DoLerpBob = false;
                    mytime = 0f;
                }
            }
            newstep = me.x;
            stepvariation = oldstep - newstep;
            if (stepvariation > 0)
            {
                //  print ("right");
            }
            else if (stepvariation < 0)
            {
                // print ("left");
            }
            //gets last position of the carmera
            GetLastHeadPos();
            // returns the  postion value to the carmera
            m_Cam.localPosition = me;
            LastPosY = m_Cam.localPosition.y;
            LastPosX = m_Cam.localPosition.x;
        }
        /*
                if (move.magnitude > 0.5f)
                {
                    Vector3 leanfwdvec = BackUpFirtPos;
                    leanfwdvec.z *= 2f;
                    leanfwdvec.y = m_Cam.localPosition.y;
                    leanfwdvec.x = m_Cam.localPosition.x;
                    m_Cam.localPosition = leanfwdvec;
                }
                else
                {
                    Vector3 leanfwdvec = BackUpFirtPos;
                    BackUpFirtPos.y = m_Cam.localPosition.y;
                    BackUpFirtPos.x = m_Cam.localPosition.x;
                    m_Cam.localPosition = BackUpFirtPos;
                }
                */
    }

    private void GetLastHeadPos()
    {
        // gets last positoin of the bobdy i.e the players capsule
        LastBodyPos = transform.position;
    }






    //
    void UpdateAnimator()
    {
        m_Animator.SetFloat("Forward", FowardValue, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetFloat("Turn", TurnValue, 0.1f, Time.deltaTime);

        if (GetCharacterType == CharacterType.FirstMan)
        {
            if (V < 0 && !m_Crouching && accuratecheckforgrounded)
            {
                m_Animator.SetFloat("Vertical", V);
            }
            else
            {
                m_Animator.SetFloat("Vertical", 1);
            }
            //m_Animator.SetFloat("ReverseMagnitude", FowardValue, 0.1f, Time.deltaTime);
        }

        if (!m_jump)
            accuratecheckforgrounded = !groundedPlayer && Mathf.Abs(playerVelocity.y) < 1f ? true : groundedPlayer;
        else
            accuratecheckforgrounded = groundedPlayer;


        m_Animator.SetBool("OnGround", accuratecheckforgrounded);
        if (!accuratecheckforgrounded)
        {
            JustLefttheground = true;
            m_Animator.SetFloat("Jump", playerVelocity.y);
        }
        //        print(playerVelocity.y);
        float runCycle =
              Mathf.Repeat(
                  m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * FowardValue;
        if (groundedPlayer)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }
    }
}
