using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    public UnityEvent OnPicked;
    public int points_to_give = 1;
    public float range;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        if ((player.transform.position - transform.position).sqrMagnitude < range)
            if (OnPicked != null)
            {
                OnPicked.Invoke();  
            }
            Destroy(gameObject);
    }
}
