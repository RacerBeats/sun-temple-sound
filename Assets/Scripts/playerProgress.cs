using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProgress : MonoBehaviour
{

    public FMOD.Studio.EventInstance fmodInstance;

    public GameObject player;
    public GameObject start;
    public GameObject goal;
    public GameObject light;

    private float startToGoalDistance;
    private float playerToGoalDistance;
    public float playerProgressPercent;

    // Start is called before the first frame update
    void Start()
    {
        // CHANGE TO YOUR EVENT
        fmodInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
        fmodInstance.start();
        startToGoalDistance = Vector3.Distance(goal.transform.position, start.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        playerToGoalDistance = Vector3.Distance(goal.transform.position, player.transform.position);
        playerProgressPercent = playerToGoalDistance / startToGoalDistance;
        playerProgressPercent = 1.0f - playerProgressPercent;
        fmodInstance.setParameterByName("Progress", playerProgressPercent);
        light.transform.eulerAngles = new Vector3(-10 + (40 * playerProgressPercent), 0, 0);
    }
}