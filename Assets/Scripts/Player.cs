using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 speed;
    public Vector2 target_pos;

    void Update()
    {
        HorizontalMove();
        VecticalMove();
    }
    
    void HorizontalMove()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touch_position = Camera.main.ScreenToWorldPoint(touch.position);
            target_pos.x = touch_position.x;
        }

        if (target_pos != (Vector2)transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target_pos.x, transform.position.y), speed.x * Time.deltaTime);
        }
    }

    void VecticalMove()
    {
        target_pos.y = transform.position.y + 1;
        if (target_pos != (Vector2)transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target_pos.y), speed.y * Time.deltaTime);
        }
    }

    void NewSegment()
    {

    }
}
