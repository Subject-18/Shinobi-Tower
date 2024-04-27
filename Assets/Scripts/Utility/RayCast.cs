using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast
{
    public static bool threerayCast(Vector3 origin, Vector3 dir, float spacing, 
    Transform transform, out List<RaycastHit> hits, float distance, LayerMask layer)
     {
        bool centreMaskHit = Physics.Raycast(origin, Vector3.down, out RaycastHit centreHit, distance, layer);
        bool leftMaskHit = Physics.Raycast(origin - transform.right * spacing, Vector3.down, out RaycastHit leftHit, distance, layer);
        bool rightMaskHit = Physics.Raycast(origin + transform.right * spacing, Vector3.down, out RaycastHit rightHit, distance, layer);

        hits = new List<RaycastHit> { centreHit, leftHit, rightHit};

        //Haven't drawn the three raycast lines....

        bool hitFound = centreMaskHit || leftMaskHit || rightMaskHit;

        return hitFound;
     }
}
