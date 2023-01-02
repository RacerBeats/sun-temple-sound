using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonePlayer : MonoBehaviour
{
    //initialize variables
    FMOD.Studio.EventInstance Zone_Player;

    public GameObject player;
    public GameObject start;
    public GameObject goal;
    //public GameObject light;

    private float Zone1 = 0.0f;
    private float Zone2 = 1.0f;
    private float Zone3 = 2.0f;
    private float Zone4 = 3.0f;
    //private float Zone4 = 4.0f;

    private float startToGoalDistance;
    private float playerToGoalDistance;
    public float playerProgressPercent;

    // Start is called before the first frame update
    private void Start()
    {
        Zone_Player = FMODUnity.RuntimeManager.CreateInstance("event:/Zone Player");
        Zone_Player.start();
        startToGoalDistance = Vector3.Distance(goal.transform.position, start.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        playerToGoalDistance = Vector3.Distance(goal.transform.position, player.transform.position);
        playerProgressPercent = playerToGoalDistance / startToGoalDistance;
        playerProgressPercent = 1.0f - playerProgressPercent;
        Zone_Player.setParameterByName("Zones", playerProgressPercent);
        GetComponent<Light>().transform.eulerAngles = new Vector3(-10 + (40 * playerProgressPercent), 0, 0);
    }

}
