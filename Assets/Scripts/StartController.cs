using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    [SerializeField] GameObject tapToStartObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowStartPrompt());
    }

    // Update is called once per frame
    void Update()
    {
        if (tapToStartObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Gameplay");
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    SceneManager.LoadScene("Gameplay");
                }
            }
        }
    }

    IEnumerator ShowStartPrompt()
    {
        yield return new WaitForSeconds(2f);
        tapToStartObject.SetActive(true);
    }
}
