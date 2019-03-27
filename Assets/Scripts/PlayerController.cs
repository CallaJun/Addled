using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [SerializeField] Text text;
    private float upForce = 250f;
    private float gravityScale = 1f;
    float newScale;

    // Ascend and descend
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    [SerializeField] AudioClip ascendClip;
    [SerializeField] AudioClip descendClip;
    [SerializeField] AudioClip flapClip;
    [SerializeField] AudioClip bounceClip;

    bool bufferOn;
    void Start()
    {
        bufferOn = true;

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        transform.position = new Vector2(GameController.instance.screenSize.x * -.5f, 0f);

        StartCoroutine(BufferStart());
    }

    IEnumerator BufferStart()
    {
        yield return new WaitForSeconds(.5f);
        bufferOn = false;
    }

    void Update()
    {
        if (!GameController.instance.gameOver && !bufferOn)
        {
            if (Input.touchCount > 0)
            {
                if (rigidbody.gravityScale == 0f)
                {
                    rigidbody.gravityScale = gravityScale;
                }
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    source2.PlayOneShot(flapClip);
                    rigidbody.velocity = Vector2.zero;
                    rigidbody.AddForce(Vector2.up * upForce);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rigidbody.gravityScale == 0f)
                {
                    rigidbody.gravityScale = gravityScale;
                }
                source2.PlayOneShot(flapClip);
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(Vector2.up * upForce);
            }
        }

        if (GameController.instance.playerValue < 0)
        {
            EndGame();
        }

        newScale = Mathf.Lerp(.2f, .8f, Mathf.Abs(GameController.instance.playerValue) / 2000f);
        transform.localScale = new Vector2(newScale, newScale);

        text.text = GameController.instance.playerValue.ToString();
        text.gameObject.transform.position = transform.position;
        text.gameObject.transform.localScale = new Vector2(newScale * 4f, newScale * 4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Number" && !GameController.instance.gameOver)
        {
            StartCoroutine(Adding(collision.gameObject.GetComponent<Tile>().value));
            collision.gameObject.GetComponent<Tile>().StartDisappear();
            GameController.instance.collected++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BottomCollider" || collision.gameObject.name == "TopCollider")
        {
            collision.gameObject.GetComponent<AudioSource>().PlayOneShot(bounceClip);
            EndGame();
        }
    }

    public void StartGame()
    {
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Number"))
        {
            tile.GetComponent<Tile>().StartDisappear();
        }
        GameController.instance.collected = 0;
        GameController.instance.playerValue = 0;
        rigidbody.gravityScale = 0;
        transform.position = new Vector2(GameController.instance.screenSize.x * -.5f, 0f);
        StartCoroutine(BufferStart());
    }

    void EndGame()
    {
        GameController.instance.gameOver = true;
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Number"))
        {
            tile.GetComponent<Tile>().StartDisappear();
        }
        bufferOn = true;
    }

    IEnumerator Adding(int delta)
    {
        if (delta >= 0)
        {
            source1.PlayOneShot(ascendClip);
        }
        else
        {
            source1.PlayOneShot(descendClip);
        }

        float duration = 1f;
        int prevInc = 0;
        int incVal = 0;
        float durationStep = 0f;
        int absDelta = Mathf.Abs(delta);
        while (incVal < absDelta)
        {
            durationStep += Time.deltaTime / duration;
            incVal = Mathf.RoundToInt(Mathf.Lerp(0, absDelta, durationStep));
            if (delta >= 0)
            {
                GameController.instance.playerValue += (incVal - prevInc);
            }
            else
            {
                GameController.instance.playerValue -= (incVal - prevInc);
            }
            prevInc = incVal;
            yield return null;
        }
    }
}
