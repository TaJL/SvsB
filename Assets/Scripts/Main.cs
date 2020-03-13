using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public float player_ver_speed;
    public int BaseHp;
    public int level;

    static int baseHp;

    private Player player;
    private MainCamera mainCamera;
    private GameObject goal;
    private Text levelText;
    private Text title;
    private static int hpLastLevel;
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
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        mainCamera = Camera.main.GetComponent<MainCamera>();
        goal = GameObject.FindGameObjectsWithTag("Goal")[0];
        levelText = GameObject.FindGameObjectsWithTag("LevelText")[0].GetComponent<Text>();
        title = GameObject.FindGameObjectsWithTag("Title")[0].GetComponent<Text>();

        player.VerSpeed = player_ver_speed;
        mainCamera.VerSpeed = player_ver_speed;

        baseHp = BaseHp;
        if (hpLastLevel == 0)
            hpLastLevel = baseHp;

        state = states.Ready;
        NextState();

        EventManager.StartListening(EVENT_GAME_OVER, SetStateGameOver);
    }

    IEnumerator ReadyState()
    {
        EventManager.TriggerEvent(EVENT_PLAYER_STOP);

        TextVisibility(true, true);
        title.text = "SNAKE\nVS\nBLOCK";
        levelText.text = "Level " + level.ToString() + "\n\nTouch to Start";

        player.Hp = hpLastLevel;

        while (state == states.Ready)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonUp(0))
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
        title.text = "GAME\nOVER";

        hpLastLevel = baseHp;
        EventManager.TriggerEvent(EVENT_PLAYER_STOP);

        while (state == states.LevelComplete)
        {
            if ((Input.touchCount > 0) || Input.GetMouseButtonUp(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            yield return 0;
        }
        NextState();
    }

    IEnumerator LevelCompleteState()
    {
        TextVisibility(true, false);
        title.text = "LEVEL \n COMPLETE";

        level++;
        hpLastLevel = player.Hp;

        while (state == states.LevelComplete)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonUp(0))
            {
                state = states.Ready;
            }

            yield return 0;
        }
        NextState();
    }

    void NextState()
    {
        string funcName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(funcName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
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
