using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    public float scrollSpeed;
    public int value;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject textPrefabPositive;
    [SerializeField] GameObject textPrefabNegative;
    GameObject textObject;
    float newScale;

    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(scrollSpeed, 0);

    }

    bool testing = false;
    // Update is called once per frame
    void Update()
    {
        if (textObject != null)
        {
            textObject.transform.position = transform.position;
            //textObject.transform.localScale = new Vector2(newScale, newScale);
        }
        if (GameController.instance.gameOver)
        {
            rigidbody.velocity = Vector2.zero;
        }

        if (transform.position.x < -GameController.instance.screenSize.x)
        {
            StartDisappear();
        }
    }

    public void SetValue(int val)
    {
        value = val;
        if (val < 0)
        {
            textObject = Instantiate(textPrefabNegative, canvas.transform);
        }
        else
        {
            textObject = Instantiate(textPrefabPositive, canvas.transform);
        }
        
        // Set size based on value
        newScale = Mathf.Lerp(.2f, .8f, Mathf.Abs(value) / 2000f);
        transform.localScale = new Vector2(newScale, newScale);
        float yValue = Random.Range(((200f / Screen.height) * GameController.instance.screenSize.y) + -GameController.instance.screenSize.y + (gameObject.GetComponent<Renderer>().bounds.size.y / 2f), GameController.instance.screenSize.y - (gameObject.GetComponent<Renderer>().bounds.size.y / 2f));
        transform.position = new Vector2(GameController.instance.generationPlace.position.x, yValue);

        // Text object values
        textObject.transform.position = transform.position;
        textObject.transform.localScale = new Vector2(newScale * 4f, newScale * 4f);
        textObject.GetComponent<Text>().text = value.ToString();
    }

    public void StartDisappear()
    {
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(textObject);
        Destroy(gameObject);
    }
}
