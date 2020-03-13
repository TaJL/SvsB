using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float ver_speed;
    public float seconds_of_invulnerability;

    private Vector2 traslation;
    private float invulnerability_timer = 0;
    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            SetText();
        }
    }
    private const string EVENT_PICK_UP_TAKEN = "pick up taken";
    private const string EVENT_DAMAGE_TAKEN = "damage";
    private const string EVENT_GAME_OVER = "game over";
    private const string EVENT_STOP = "player stop";
    private const string EVENT_GO_FORWARD = "player go forward";

    private void Start()
    {
        EventManager.StartListening(EVENT_PICK_UP_TAKEN , AddSegment);
        EventManager.StartListening(EVENT_DAMAGE_TAKEN , RemoveSegment);
        EventManager.StartListening(EVENT_STOP, StopVerticalMove);
        EventManager.StartListening(EVENT_GO_FORWARD, StartVerticalMove);
    }

    void Update()
    {
        if (Hp <= 0)
        {
            return;
        }

        if (traslation.y != 0)
        {
            GetHorizontalMove();
            Debug.Log(traslation);
            transform.Translate(traslation * Time.deltaTime);
        }

        if (invulnerability_timer > 0)
        {
            invulnerability_timer -= Time.deltaTime;
        }
    }
    
    void GetHorizontalMove()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touch_position = Camera.main.ScreenToWorldPoint(touch.position);
            traslation.x = touch_position.x - transform.position.x;
        }
    }

    void StartVerticalMove()
    {
        traslation.y = ver_speed;
    }

    void StopVerticalMove()
    {
        traslation.y = 0;
    }

    void AddSegment()
    {
        if (Hp <= 0)
        {
            return;
        }

        Hp ++;
    }

    void RemoveSegment()
    {
        if (invulnerability_timer > 0 || Hp <= 0)
        {
            return;
        }

        Hp--;

        if (Hp <= 0)
        {
            EventManager.TriggerEvent(EVENT_GAME_OVER);
        }
    }

    void SetText()
    {
        transform.Find("Canvas").GetComponentInChildren<Text>().text = Hp.ToString();
    }
}