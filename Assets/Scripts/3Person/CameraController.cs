using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    

    [SerializeField] Transform followTarget;

    [SerializeField] float distance = 5f;

    [SerializeField] float minrotation = -20f;

    [SerializeField] float maxrotation = 90f;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] float speed;

    [SerializeField] bool invert = true;

    //[SerializeField] bool lockCursor = true;

    

    float rotationX;
    float rotationY;
    // Start is called before the first frame update
    void Start()
    {
            Cursor.visible = false;
           // Cursor.lockState = CursorLockMode.Locked;
            
    }

    // Update is called once per frame
    void Update()
    {
        if(!invert)
            {rotationX += Input.GetAxis("Mouse Y") * speed;
            rotationX = Mathf.Clamp(rotationX, minrotation, maxrotation );
            rotationY += Input.GetAxis("Mouse X") * speed;
            var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
            var focus = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
        transform.position = focus - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;


            }
        else    
        {
            rotationX += Input.GetAxis("Mouse Y") * speed;
            rotationX = Mathf.Clamp(rotationX, minrotation, maxrotation );
            rotationY += Input.GetAxis("Mouse X") * speed;

            var targetRotation = Quaternion.Euler(-rotationX, -rotationY, 0);
            var focus = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);
        transform.position = focus - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;

        }
    }

     

    public Quaternion PlanerRotation => Quaternion.Euler(0, rotationY, 0);
}
