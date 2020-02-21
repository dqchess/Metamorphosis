using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProgressBarScript : MonoBehaviour
{

    private Transform MainCamera;  
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(MainCamera);
    }
}
