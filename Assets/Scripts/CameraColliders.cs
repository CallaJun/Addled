using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliders : MonoBehaviour
{
    // Colliders
    //private Transform rightCollider;
    private Transform leftCollider;

    // Triggers
    public Transform topCollider;
    public Transform bottomCollider;

    // Locations and sizes
    private Vector3 cameraPos;
    private Vector2 screenSize;
    public float zPosition = 0f;
    public float colDepth = 1f;

    // Start is called before the first frame update
    void Awake()
    {

        // Generate empty game objects
        leftCollider = new GameObject().transform;
        topCollider = new GameObject().transform;
        bottomCollider = new GameObject().transform;

        // Name game objects
        topCollider.name = "TopCollider";
        bottomCollider.name = "BottomCollider";
        leftCollider.name = "LeftCollider";

        // Add colliders
        leftCollider.gameObject.AddComponent<BoxCollider2D>();
        topCollider.gameObject.AddComponent<BoxCollider2D>();
        bottomCollider.gameObject.AddComponent<BoxCollider2D>();

        // Make top and bottom colliders into triggers
        leftCollider.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        // Make them the child of the camera
        topCollider.parent = transform;
        bottomCollider.parent = transform;
        leftCollider.parent = transform;

        // Generate world space point information for position and scale calculations
        cameraPos = Camera.main.transform.position;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

        // Change our scale and positions to match the edges of the screen
        leftCollider.localScale = new Vector3(colDepth, screenSize.y * 2, colDepth);
        leftCollider.position = new Vector3(cameraPos.x - screenSize.x - (leftCollider.localScale.x * 0.5f), cameraPos.y, zPosition);
        topCollider.localScale = new Vector3(screenSize.x * 2, colDepth, colDepth);
        topCollider.position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (topCollider.localScale.y * 0.5f), zPosition);
        bottomCollider.localScale = new Vector3(screenSize.x * 2, colDepth, colDepth);
        bottomCollider.position = new Vector3(cameraPos.x, cameraPos.y - screenSize.y - (bottomCollider.localScale.y * 0.5f), zPosition);

        // Add audio sources
        topCollider.gameObject.AddComponent<AudioSource>();
        bottomCollider.gameObject.AddComponent<AudioSource>();
    }
}
