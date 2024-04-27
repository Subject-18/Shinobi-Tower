using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    [SerializeField] public bool lockcursor;

    [SerializeField] public float sensitivity = 10;

    [SerializeField] public Transform target;

    Vector2 pitchMinMax = new Vector2(-20, 85);

    public float smoothing = 0.12f;

     Vector3 rotateSmoothvelocity;
     Vector3 currentRotation;

     float yaw;

     float pitch;

     Vector3 cameraDir;

     public float cameraDistance;

     Vector2 cameraDistanceMinMax = new Vector2(0.5f, 5);

     public Camera cam;


     void Start()
     {

            cameraDir = cam.transform.localPosition.normalized;
            cameraDistance = cameraDistanceMinMax.y;
            if(lockcursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
     }

     void LateUpdate()
     {
        yaw+=Input.GetAxis("Mouse X") * sensitivity;
        pitch+=Input.GetAxis("Mouse Y") * sensitivity;

        pitch=Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotateSmoothvelocity, smoothing);
        transform.eulerAngles = currentRotation;

        transform.position = Vector3.MoveTowards(transform.position, target.position, 0.5f);

        CheckCameraCollsionAndOcclusion(cam);
     }

     public void CheckCameraCollsionAndOcclusion(Camera cam){
        Vector3 desiredcamPos = transform.TransformPoint(cameraDir * cameraDistanceMinMax.y);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, desiredcamPos, out hit))
        {
            cameraDistance = Mathf.Clamp(hit.distance, cameraDistanceMinMax.x, cameraDistanceMinMax.y);

        }
        else
        {
            cameraDistance = cameraDistanceMinMax.y;
        }

        cam.transform.localPosition = cameraDir * cameraDistance;
     }



}
