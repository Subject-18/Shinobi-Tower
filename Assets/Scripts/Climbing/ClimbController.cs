using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    ClimbPoint current;
    EnvironmentScanner environmentScanner;
    PlayerController playerController;

    void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        playerController = GetComponent<PlayerController>();
    }
   private void Update()
   {
        if(!playerController.isHanging)
        {    if(Input.GetButton("Jump") && !playerController.inAction)
            {
            if (environmentScanner.climbLedgeCheck(transform.forward, out RaycastHit ledgeHit))
            {
                    current = GetFirstPoint(ledgeHit.transform, ledgeHit.point);
                    playerController.setControl(false);
                    StartCoroutine(LedgeClimb("LedgeClimb", current.transform, 0.41f, 0.54f));
            }
            }
            if(Input.GetButton("Jump") && !playerController.inAction)
            {
                if(environmentScanner.DropLedgeCheck(out RaycastHit dropHit))
                {
                    current = GetFirstPoint(dropHit.transform, dropHit.point);
                    playerController.setControl(false);
                    StartCoroutine(LedgeClimb("Drop_to_hang", current.transform, 0.31f, 0.45f,
                    Handoffset: new Vector3(0.25f, 0.24f, -0.3f)));

                }
            }
        }
        else{

            if(Input.GetButton("JumpBack") && !playerController.inAction)
            {
                
                StartCoroutine(jumpFromHang());
                return;
            }

            //Ledge to Ledge Jump

            float h = Mathf.Round(Input.GetAxisRaw("Horizontal"));
            float v = Mathf.Round(Input.GetAxisRaw("Vertical"));
            var inpDir = new Vector2(h,v);

            if(playerController.inAction || inpDir == Vector2.zero) return;

            if(current.MountPoint && inpDir.y == 1)
            {
                StartCoroutine(MountfromHang());        // to climbup from hanging
            }

            var neighbour = current.GetNeighbour(inpDir);
            if(neighbour == null)
            return;

            if( neighbour.connectiontype == Connection.jump && Input.GetButton("Jump"))
            {
                current = neighbour.climbPoint;

                if(neighbour.direction.y == 1)
                {
                    // to climb up from ledge to ledge
                    StartCoroutine(LedgeClimb("Hang_Hop_Up", current.transform, 0.34f, 0.65f,Handoffset : new Vector3(0.25f, 0.1f, 0.15f))); 
                }
                else if(neighbour.direction.y == -1)
                {
                    // to drop from ledge to ledge
                    StartCoroutine(LedgeClimb("Hang_Drop", current.transform, 0.31f, 0.65f, Handoffset : new Vector3(0.25f, 0.1f, 0.12f))); 
                }
                else if(neighbour.direction.x == 1)
                {
                    // to jump to right ledge
                    StartCoroutine(LedgeClimb("Hang_Hop_Right", current.transform, 0.18f, 0.51f)); 
                }
                else if(neighbour.direction.x == -1)
                {
                     // to jump to left ledge
                    StartCoroutine(LedgeClimb("Hang_Hop_Left", current.transform, 0.18f, 0.51f));
                }
            }
            else if( neighbour.connectiontype == Connection.move)
            {
                current = neighbour.climbPoint;

                if(neighbour.direction.x == 1)
                {
                    StartCoroutine(LedgeClimb("Shimmy_Right", current.transform, 0.0f, 0.38f, Handoffset : new Vector3(0.25f, 0.05f, 0.15f))); // shimmy on ledge towards right
                }
                else if(neighbour.direction.x == -1)
                {
                    // shimmy on ledge towards left
                    StartCoroutine(LedgeClimb("Shimmy_Left", current.transform, 0.0f, 0.38f, AvatarTarget.LeftHand,
                    Handoffset : new Vector3(0.25f, 0.05f, 0.20f))); 
                }

            }
        }
   }

   IEnumerator LedgeClimb(string anim, Transform Ledge, float MatchStartTime, float MatchFinishTime, 
   AvatarTarget hand = AvatarTarget.RightHand, Vector3? Handoffset = null) // to start climbing action
   {    
        //for target matching
        var matchParams = new MatchTargetP()
        {
            pos = getHandPos(Ledge, hand, Handoffset),
            bodyPart = hand,
            startTime = MatchStartTime,
            finishTime = MatchFinishTime,
            posWeight = Vector3.one

        };

        var targetRotation = Quaternion.LookRotation(-Ledge.forward);

        yield return playerController.doAction(anim, matchParams, targetRotation, true);

        playerController.isHanging = true;
   }

   Vector3 getHandPos(Transform Ledge, AvatarTarget hand, Vector3? Handoffset)
   {
        //to adjust the hand position since hand is being used in target matching
        var offval = (Handoffset != null) ? Handoffset.Value : new Vector3(0.25f, 0.1f, 0.1f);
        var HorDir = (hand == AvatarTarget.RightHand) ? Ledge.right : -Ledge.right;
        return Ledge.position + Ledge.forward * offval.z + Vector3.up * offval.y - HorDir * offval.x;
   }

   IEnumerator jumpFromHang()   // for back ejects
   {
    playerController.isHanging = false;
    yield return StartCoroutine(playerController.doAction("JumpFromHang"));
    playerController.ResetTargetRot();          // give back player control
    playerController.setControl(true);
   }

   IEnumerator MountfromHang() // for climbing up from hanging state
   {
    playerController.isHanging = false;
    yield return playerController.doAction("HangClimbUp");
    playerController.EnableCharacterControl(true); // to avoid clipping of feet in ground
    yield return new WaitForSeconds(0.5f);     // to delay for the time taken to stand
    playerController.ResetTargetRot();         // give back player control 
    playerController.setControl(true);
   }

   ClimbPoint GetFirstPoint(Transform ledge, Vector3 hitpoint)
   {
    var points = ledge.GetComponentsInChildren<ClimbPoint>();
    ClimbPoint nearest = null;

    float firstpointdis = Mathf.Infinity;
    
    foreach( var point in points)
    {
       var distance = Vector3.Distance(point.transform.position, hitpoint);
       if(distance<firstpointdis)
       {
        nearest = point;
        firstpointdis = distance;

       }
    }

    return nearest;

   }
}

