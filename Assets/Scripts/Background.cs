using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.backgroundColor = new Color(135f/255f, 206f / 250f, 235f / 255f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
