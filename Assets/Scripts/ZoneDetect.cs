using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDetect : MonoBehaviour
{

    public GameObject Zone1;
    public GameObject Zone2;
    public GameObject Zone3;
    public GameObject Zone4;
    public GameObject Zone5;

    public GameObject player;
    public string zoneLabel = "";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Zone");
    }
}
