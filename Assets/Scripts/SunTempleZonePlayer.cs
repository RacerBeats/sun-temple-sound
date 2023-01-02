using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTempleZonePlayer : MonoBehaviour
{
    //UNIQUE CHALLENGE: IMPLEMENTING X AND Y INTO FMOD ZONE DETECTION

    //Initialize Values
    FMOD.Studio.EventInstance Sun_Temple_Zone_Player;

    public GameObject player;
    static public GameObject start;
    static public GameObject goal;


    // Start is called before the first frame update
    void Start()
    {
        Sun_Temple_Zone_Player = FMODUnity.RuntimeManager.CreateInstance("event:/Sun Temple Zone Player");
        Sun_Temple_Zone_Player.start();
    }

    // Update is called once per frame
    void Update()
    {
        Sun_Temple_Zone_Player.setParameterByName("SunTemple X", player.transform.position.x);
        Sun_Temple_Zone_Player.setParameterByName("SunTemple Y", player.transform.position.y);

        Debug.Log($"player position: {player.transform.position.x}, {player.transform.position.y}");
    }
}
