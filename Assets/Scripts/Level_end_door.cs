using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_end_door : MonoBehaviour
{
        void OnTriggerEnter(Collider col)
        {
            if(col.gameObject.name == "endDoor")
            {
                SceneManager.LoadScene(2);
            }
        }
    
}
