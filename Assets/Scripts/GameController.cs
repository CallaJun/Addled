using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public bool gameOver;
    public int collected = 0;
    public int playerValue = 0;
    float speed = 1.5f;
    public Transform generationPlace;
    public Vector2 screenSize;

    // Instructions
    [SerializeField] GameObject handImage;
    [SerializeField] Text instructionsText;

    // Game
    [SerializeField] public GameObject positiveNumberObject;
    [SerializeField] public GameObject negativeNumberObject;
    [SerializeField] public Text score;
    [SerializeField] GameObject restartTextObject;
    [SerializeField] PlayerController player;

    // Audio
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    [SerializeField] AudioClip startClip;
    [SerializeField] AudioClip endClip;

    // Booleans
    bool restarted;
    bool readyToStart;

    // Ads
    [SerializeField] Admob admob;
    public bool restartReady;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

        restartTextObject.SetActive(false);

        restarted = false;
        readyToStart = false;

        generationPlace.transform.position = new Vector2(screenSize.x + positiveNumberObject.GetComponent<Renderer>().bounds.size.x, 0f);

        if (StatsManager.instance.state.highScore < 10)
        {
            // Show tutorial
            StartCoroutine(ShowInstructions());
        }
        else
        {
            StartCoroutine(LaunchTiles());
        }

        restartReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Not playing
        if (gameOver)
        {
            if (!restarted)
            {
                // End background music
                source2.Stop();
                // Play lose game sound
                source1.PlayOneShot(endClip);
                // Restart
                restartTextObject.SetActive(true);
                score.gameObject.SetActive(false);
                CancelInvoke();
                StartCoroutine(ShowRestartPrompt());
                restarted = true;
            }

            if (readyToStart)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (restartReady)
                        {
                            restartReady = false;
                            StartCoroutine(PlayAndRestart());
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (restartReady)
                    {
                        restartReady = false;
                        StartCoroutine(PlayAndRestart());
                    }
                }
            }
        }
        // Playing
        else
        {
            score.text = "Score: " + collected.ToString();
        }
    }

    IEnumerator PlayAndRestart()
    {
        if (StatsManager.instance.state.numPlays % 1 != 0)
        {
            restartReady = true;
            Restart();
        }
        else
        {
            admob.ShowInterstitial();
            yield return new WaitUntil(() => restartReady == true);
            Restart();
        }
    }

    IEnumerator ShowInstructions()
    {
        StartCoroutine(ShowHand());
        yield return new WaitForSeconds(.5f);
        instructionsText.gameObject.SetActive(true);
        instructionsText.text = "Clash with orbs to absorb their values";
        yield return new WaitForSeconds(2.5f);
        instructionsText.text = "Keep your value above zero";
        yield return new WaitForSeconds(2.5f);
        instructionsText.text = "Clash as many orbs as you can!";
        yield return new WaitForSeconds(2.5f);
        instructionsText.gameObject.SetActive(false);
        StartCoroutine(LaunchTiles());
    }

    IEnumerator ShowHand()
    {
        for (int i = 0; i < 6; i++)
        {
            handImage.SetActive(true);
            yield return new WaitForSeconds(.8f);
            handImage.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator LaunchTiles()
    {
        source1.PlayOneShot(startClip);
        yield return new WaitForSeconds(startClip.length);
        source2.Play();
        while (!gameOver)
        {
            yield return new WaitForSeconds(Mathf.Lerp(2.5f, .4f, speed / 5.5f));
            LaunchTile();
        }
    }

    IEnumerator ShowRestartPrompt()
    {
        StatsManager.instance.Save();
        StatsManager.instance.addScore(collected);
        restartTextObject.GetComponent<Text>().text = "Score: " + collected.ToString() + "\nAverage: " + StatsManager.instance.averageScore() + "\nBest: " + StatsManager.instance.state.highScore;
        yield return new WaitForSeconds(3);
        restartTextObject.GetComponent<Text>().text = restartTextObject.GetComponent<Text>().text + "\nTap to restart";
        readyToStart = true;
    }

    void Restart()
    {
        restartTextObject.SetActive(false);
        gameOver = false;
        StartCoroutine(LaunchTiles());
        score.gameObject.SetActive(true);
        player.StartGame();
        readyToStart = false;
        restarted = false;
    }

    void LaunchTile()
    {

        // Generate number
        int value = 0;
        if (playerValue > 10)
        {
            value = Random.Range(-playerValue, playerValue);
        }
        else
        {
            value = Random.Range(-10, 10);
        }
        GameObject tile;
        if (value >= 0)
        {
            tile = Instantiate(positiveNumberObject, new Vector3(generationPlace.position.x, 0f, 0f), Quaternion.identity);
        }
        else
        {
            tile = Instantiate(negativeNumberObject, new Vector3(generationPlace.position.x, 0f, 0f), Quaternion.identity);
        }

        speed = Mathf.Lerp(1.5f, 5.5f, collected / 100f);
        tile.GetComponent<Tile>().scrollSpeed = -speed;
        tile.tag = "Number";

        tile.GetComponent<Tile>().SetValue(value);
    }
}
