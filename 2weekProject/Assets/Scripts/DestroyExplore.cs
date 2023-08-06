using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplore : MonoBehaviour
{
    [SerializeField] float destroyTime = 2f;
    float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > destroyTime) 
        {
            Destroy(gameObject);
        }
    }
}
