using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public int level = 1;
    public int playerBaseHp;
    public int transitionStateTime;

    private GameObject player;
    private GameObject playerPlayer;
    private GameObject goal;
    private Text levelText;
    private Text title;
    private int playerHpLastLevel;
    private enum states
    {
        Ready,
        Playing,
        GameOver,
        LevelComplete,
    }
    private states state;


    private const string EVENT_PLAYER_STOP = "player stop";
    private const string EVENT_PLAYER_GO_FORWARD = "player go forward";
    private const string EVENT_GAME_OVER = "game over";

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        goal = GameObject.FindGameObjectsWithTag("Goal")[0];
        levelText = GameObject.FindGameObjectsWithTag("LevelText")[0].GetComponent<Text>();
        title = GameObject.FindGameObjectsWithTag("Title")[0].GetComponent<Text>();

        state = states.Ready;
        NextState();

        EventManager.StartListening(EVENT_GAME_OVER, SetStateGameOver);
    }

    IEnumerator ReadyState()
    {
        EventManager.TriggerEvent(EVENT_PLAYER_STOP);

        TextVisibility(true, true);
        title.text = "TOUCH \n TO \n START";
        levelText.text = "LEVEL " + level.ToString();

        if (playerHpLastLevel == 0)
        {
            player.GetComponent<Player>().Hp = playerBaseHp;
        }
        else
        {
            player.GetComponent<Player>().Hp = playerHpLastLevel;
        }

        while (state == states.Ready)
        {
            if (Input.touchCount > 0)
            {
                state = states.Playing;
            }

            yield return 0;
        }
        NextState();
    }

    IEnumerator PlayingState()
    {
        TextVisibility(false, false);
        EventManager.TriggerEvent(EVENT_PLAYER_GO_FORWARD);

        while (state == states.Playing)
        {
            if (goal.transform.position.y < player.transform.position.y)
            {
                state = states.LevelComplete;
            }
            yield return 0;
        }
        NextState();
    }

    IEnumerator GameOverState()
    {
        TextVisibility(true, false);
        title.text = "GAME \n OVER";

        playerHpLastLevel = 0;
        EventManager.TriggerEvent(EVENT_PLAYER_STOP);

        yield return new WaitForSeconds(transitionStateTime);
        NextState();
    }

    IEnumerator LevelCompleteState()
    {
        TextVisibility(true, false);
        title.text = "LEVEL \n COMPLETE";

        level++;
        playerHpLastLevel = player.GetComponent<Player>().Hp;

        yield return new WaitForSeconds(transitionStateTime);
        NextState();
    }

    void NextState()
    {
        string funcName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(funcName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    void SetStateGameOver()
    {
        state = states.GameOver;
    }

    void TextVisibility(bool titleVisible, bool levelTextVisible)
    {
        title.transform.gameObject.SetActive(titleVisible);
        levelText.transform.gameObject.SetActive(levelTextVisible);
    }
}
