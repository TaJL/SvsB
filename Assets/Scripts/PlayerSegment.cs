using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSegment : MonoBehaviour
{
    public GameObject following_to;
    public int memory;
    private List<Vector2> future_positions;

    void Update()
    {
        if (future_positions.Count == memory)
        {
            transform.Translate(future_positions[0]);
            future_positions.RemoveAt(0);
        }
        future_positions.Add(following_to.transform.position);
    }
}
