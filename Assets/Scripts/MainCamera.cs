using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float VerSpeed
    {
        set
        {
            verSpeed = value;
        }
    }

    private float verSpeed;
    private bool going_forward = false;

    private const string EVENT_STOP = "player stop";
    private const string EVENT_GO_FORWARD = "player go forward";

    void Start()
    {
        EventManager.StartListening(EVENT_STOP, StopVerticalMove);
        EventManager.StartListening(EVENT_GO_FORWARD, StartVerticalMove);
    }

    void StartVerticalMove()
    {
        going_forward = true;

        StartCoroutine(GoForward());
    }

    void StopVerticalMove()
    {
        going_forward = false;
    }

    IEnumerator GoForward()
    {
        while (going_forward)
        {
            transform.Translate(new Vector2(0, verSpeed) * Time.deltaTime);
            yield return 0;
        }
    }
}
