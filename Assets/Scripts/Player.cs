using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float ver_speed;
    public float seconds_of_invulnerability;
    public int hp_initial;

    private Vector2 traslation;
    private float invulnerability_timer = 0;
    private int hp;
    private int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = Hp;
            SetText();
        }
    }

    private const string EVENT_PICK_UP_TAKEN = "Pick Up Taken";
    private const string EVENT_DAMAGE_TAKEN = "Damage Taken";
    private const string EVENT_GAME_OVER = "GameOver";

    private void Start()
    {
        EventManager.StartListening(EVENT_PICK_UP_TAKEN , AddSegment);
        EventManager.StartListening(EVENT_DAMAGE_TAKEN , RemoveSegment);
        EventManager.StartListening(EVENT_GAME_OVER , GameOver);

        Hp = hp_initial;
    }

    void Update()
    {
        if (Hp <= 0)
        {
            return;
        }

        GetHorizontalMove();

        if (traslation != Vector2.zero)
        {
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

    void AddSegment()
    {
        Hp ++;
    }

    void RemoveSegment()
    {
        if (invulnerability_timer > 0)
        {
            return;
        }

        Hp--;

        if (Hp <= 0)
        {
            EventManager.TriggerEvent("GameOver");
        }
    }

    void GameOver()
    {
        Hp = hp_initial;
    }

    void SetText()
    {
        transform.Find("Canvas").GetComponentInChildren<Text>().text = Hp.ToString();
    }
}