using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float seconds_of_invulnerability;
    public float horizontalLimits;
    public float horSpeed;
    public float VerSpeed
    {
        set
        {
            verSpeed = value;
        }
    }
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp < 0)
            {
                hp = 0;
            }
            SetText();
        }
    }

    private float verSpeed;
    private Vector2 traslation;
    private float minX;
    private float maxX;
    private float invulnerability_timer = 0;
    private int hp;

    private const string EVENT_PICK_UP_TAKEN = "pick up taken";
    private const string EVENT_DAMAGE_TAKEN = "damage";
    private const string EVENT_GAME_OVER = "game over";
    private const string EVENT_STOP = "player stop";
    private const string EVENT_GO_FORWARD = "player go forward";

    private void Start()
    {
        EventManager.StartListening(EVENT_PICK_UP_TAKEN, AddSegment);
        EventManager.StartListening(EVENT_DAMAGE_TAKEN, RemoveSegment);
        EventManager.StartListening(EVENT_STOP, StopVerticalMove);
        EventManager.StartListening(EVENT_GO_FORWARD, StartVerticalMove);

        minX = horizontalLimits;
        maxX = 10 - horizontalLimits;
        //maxX = Camera.main.orthographicSize * Screen.width / Screen.height - horizontalLimits;
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
            transform.Translate(traslation * Time.deltaTime);
            if (transform.position.x < minX || transform.position.x > maxX)
            {
                transform.position = new Vector2(Mathf.Clamp(transform.position.x,minX,maxX) ,transform.position.y);
            }
        }

        if (invulnerability_timer > 0)
        {
            invulnerability_timer -= Time.deltaTime;
        }
    }
    
    void GetHorizontalMove()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touch_position = Camera.main.ScreenToWorldPoint(touch.position);
                traslation.x = horSpeed * (touch_position.x - transform.position.x);
            }
        }
        else
        {
            Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            traslation.x = mouse_position.x - transform.position.x;
        }
    }

    void StartVerticalMove()
    {
        traslation.y = verSpeed;
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
    }

    void SetText()
    {
        transform.Find("Canvas").GetComponentInChildren<Text>().text = Hp.ToString();
    }
}