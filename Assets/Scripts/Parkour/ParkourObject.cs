using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ParkourObject", menuName = "Parkour System/New Parkour Action", order = 0)]
public class ParkourObject : ScriptableObject {

    [SerializeField] string animname;

    [SerializeField] float minHeight;

    [SerializeField] float maxHeight;

    [SerializeField] string obstacleTag;

    [SerializeField] bool rotateToObstacle;

    [Header("Target Matching")]
    [SerializeField] bool targetmatching = true;

    [SerializeField] AvatarTarget matchBodyPart;

    [SerializeField] float matchStartTime;

    [SerializeField] float matchFinishTime;

    [SerializeField] Vector3 matchPosweight = new Vector3( 0, 1, 0);

    [SerializeField] float postDelay;

    public Quaternion TargetRotation { get; set;} 

    public Vector3 MatchPosition { get; set;}



    public bool CheckifPossible(ObstacleData hitData, Transform player)
    {
        //check tag
        if(!string.IsNullOrEmpty(obstacleTag) && hitData.forwardHit.transform.tag != obstacleTag)
            return false;

        //check height
        var height = hitData.heightHit.point.y - player.position.y;

        if(height<minHeight || height>maxHeight){
            return false;
        }

        if(rotateToObstacle)
        {
            TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);
        }
        if(targetmatching)
        {
            MatchPosition = hitData.heightHit.point;
        }
        return true;
    }

    public string Animname => animname;
    public bool RotateToObstacle => rotateToObstacle;

    public bool TargetMatching => targetmatching;

    public AvatarTarget MatchBodyPart => matchBodyPart;

    public float MatchStartTime => matchStartTime;

    public float MatchFinishTime => matchFinishTime;

    public Vector3 MatchPosWeight => matchPosweight;

    public float Postdelay => postDelay;
    
}

