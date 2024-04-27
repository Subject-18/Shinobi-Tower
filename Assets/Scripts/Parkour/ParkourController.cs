using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkourController : MonoBehaviour
{   
    [SerializeField] List<ParkourObject> parkourObjects;

    [SerializeField] ParkourObject LedgeJump;
    EnvironmentScanner environmentScanner;
    Animator animator;

    PlayerController playerController;

    void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        var hitData = environmentScanner.ObstacleCheck();
        if(Input.GetButton("Jump") && !playerController.inAction && !playerController.isHanging){
            if(hitData.forwardHitFound)
            {
                foreach(var actions in parkourObjects)
                {
                    if(actions.CheckifPossible(hitData, transform)){
                        StartCoroutine(doParkour(actions));
                        break;
                    }
                }
               
            }
        }

        if(playerController.isOnLedge && !playerController.inAction && !hitData.forwardHitFound && Input.GetButton("Jump"))
        {
            if(playerController.ledgedata.angle <= 50)
            {
                playerController.isOnLedge = false;
                StartCoroutine(doParkour(LedgeJump));
            }
        }
    }

    IEnumerator doParkour(ParkourObject actions){


        playerController.setControl(false);

        MatchTargetP matchp = null;
        if(actions.TargetMatching)
        {
            matchp = new MatchTargetP()
            {
                pos = actions.MatchPosition,
                bodyPart = actions.MatchBodyPart,
                posWeight = actions.MatchPosWeight,
                startTime = actions.MatchStartTime,
                finishTime = actions.MatchFinishTime
                
            };
        }


        yield return playerController.doAction(actions.Animname, matchp, 
        actions.TargetRotation, actions.RotateToObstacle, actions.Postdelay);

        playerController.setControl(true);

    }

    void MatchTarget(ParkourObject actions){

        if (animator.isMatchingTarget)
            return;
        if (animator.IsInTransition(0))
        return;

        animator.MatchTarget(actions.MatchPosition, transform.rotation, 
        actions.MatchBodyPart, new MatchTargetWeightMask(actions.MatchPosWeight, 0), 
        actions.MatchStartTime, actions.MatchFinishTime);
        

    }
}
    
    
