using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] float rotationSpeed = 500f;

    [SerializeField] float groundCheckRadius  = 0.2f;

    [SerializeField] Vector3 groundCheckOffset;

    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    bool hasControl=true;
    Vector3 moveDir;

    Vector3 desmoveDir;

    Vector3 velocity;

    float ySpeed;
    Quaternion targetRotation;
    CameraController cameraController;

    Animator animator;

    CharacterController characterController;

    EnvironmentScanner environmentScanner;

    public bool isOnLedge {get; set;}

    public LedgeData ledgedata {get; set;}

    public bool inAction {get; private set;}

    public bool isHanging { get; set;}


    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        environmentScanner = GetComponent<EnvironmentScanner>();
    }
    public void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        

        float moveamt = Mathf.Clamp(Mathf.Abs(h) + Mathf.Abs(v), 0, 1);
        var moveInput = (new Vector3( h, 0, v)).normalized;

         desmoveDir = cameraController.PlanerRotation * moveInput;
         moveDir = desmoveDir;

        if(!hasControl)
            return;

        if(isHanging)
            return;

         velocity = Vector3.zero ;

        GroundCheck();
        animator.SetBool("isGrounded", isGrounded);

        if(isGrounded)
        {
            ySpeed = -0.5f;

            velocity = moveDir * moveSpeed;

            isOnLedge = environmentScanner.ledgeCheck(moveDir, out LedgeData ledgeData);

            if(isOnLedge)
            {
                ledgedata = ledgeData;
                ledgeMovement();

            }
            animator.SetFloat("moveamt", velocity.magnitude/moveSpeed, 0.2f, Time.deltaTime);
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;

            velocity = transform.forward * moveSpeed/2;
            

        }

       

        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if(moveamt>0 && moveDir.magnitude>0.2f)
        {
            
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
         rotationSpeed * Time.deltaTime);
    }

    public void ResetTargetRot()
    {
        targetRotation = transform.rotation;
    }

    void GroundCheck(){
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset),
         groundCheckRadius, groundLayer);
    }

    void ledgeMovement()
    {
        float signedangle = Vector3.SignedAngle(ledgedata.surfaceHit.normal, desmoveDir, Vector3.up);
        float angle = Mathf.Abs(signedangle);
        if(Vector3.Angle(desmoveDir, transform.forward)>=80)
        {
            velocity = Vector3.zero;
            return;
        }
        if(angle < 90 )
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
        else if(angle<60)
        {
            var left = Vector3.Cross(Vector3.up, ledgedata.surfaceHit.normal);
            var dir = left * Mathf.Sign(signedangle);
            velocity = velocity.magnitude * dir;
            moveDir = dir;  
        }
    }

    public IEnumerator doAction(string Animname,MatchTargetP matchp=null, 
    Quaternion targetRotation = new Quaternion(), 
    bool rotate =false, float postDelay = 0f){

        inAction = true;


        animator.CrossFadeInFixedTime(Animname, 0.2f);

        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(0);

        if(!animState.IsName(Animname))
        {
            Debug.Log("Wrong parkour");
        }

        float RotateStartTime = (matchp != null) ? matchp.startTime : 0f;

        float timer = 0f;
        while (timer <= animState.length)
        {
            timer+=Time.deltaTime;
            float normTime = timer / animState.length;

            if(rotate && timer > RotateStartTime)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                targetRotation, Rotation * Time.deltaTime);
            }

            if(matchp != null)
            {
                MatchTarget(matchp);
            }
            if(animator.IsInTransition(0) && timer>0.5f )
                break;

            yield return null;
        }
        yield return new WaitForSeconds(postDelay);


        inAction = false;
    }

    void MatchTarget(MatchTargetP mp){

        if (animator.isMatchingTarget)
            return;
        if (animator.IsInTransition(0))
        return;

        animator.MatchTarget(mp.pos, transform.rotation, 
        mp.bodyPart, new MatchTargetWeightMask(mp.posWeight, 0), 
        mp.startTime, mp.finishTime);
        

    }

    public void setControl(bool hasControl)
    {

        this.hasControl = hasControl;
        characterController.enabled = hasControl;

        if(!hasControl)
        {
            animator.SetFloat("moveamt", 0f);
            targetRotation = transform.rotation;

        }
    }
// to give control to character controller and not the player so as to avoid clipping
    public void EnableCharacterControl(bool enable) 
    {
        characterController.enabled = enable;
    }

    public bool HasControl{
        get => hasControl;
        set => hasControl = value;
    }

     private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius );
    }

    public float Rotation => rotationSpeed;

}

public class MatchTargetP{
    public Vector3 pos;
    public AvatarTarget bodyPart;
    public Vector3 posWeight;

    public float startTime;

    public float finishTime;
}
