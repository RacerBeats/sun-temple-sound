using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlayer2 : MonoBehaviour
{
    public bool closed = true;
    public bool open = false;

    public AudioSource OpenSound;
    public AudioSource CloseSound;

    public void pickSound()
    {
        if (closed)
        {
            OpenSound.Play(0);
            closed = false;
            open = true;
        }
        if (open)
        {
            CloseSound.Play(0);
            closed = true;
            open = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OpenSound = GetComponent<AudioSource>();
        CloseSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            pickSound();
        }
    }
}
