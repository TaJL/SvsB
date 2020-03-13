using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public float range;
    public float give_points_speed;
    public int points_to_give = 1;

    private GameObject player;
    private bool taken;

    private const string EVENT_TAKEN = "pick up taken";

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        SetText();
    }

    void Update()
    {
        if ((player.transform.position - transform.position).sqrMagnitude < range && !taken)
        {
            StartCoroutine("Taken");
        }
    }

    IEnumerator Taken()
    {
        taken = true;

        for (int i = 0; i < points_to_give; i++)
        {
            EventManager.TriggerEvent(EVENT_TAKEN);
            yield return new WaitForSeconds(give_points_speed);
        }

        Destroy(gameObject);
    }

    public void SetText()
    {
        transform.Find("Canvas").GetComponentInChildren<Text>().text = points_to_give.ToString(); 
    }
}
