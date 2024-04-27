using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFall : MonoBehaviour
{
    public GameObject raindrop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject g = Instantiate(raindrop, new Vector3(Random.Range(15, 20), 37, Random.Range(38, 50)), transform.rotation) as GameObject;

        Destroy(g, 2f);
    }
}
