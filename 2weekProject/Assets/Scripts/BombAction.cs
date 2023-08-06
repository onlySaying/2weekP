using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    [SerializeField] GameObject expFactory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject expo = Instantiate(expFactory);

        expo.transform.position = transform.position;

        
        Destroy(gameObject);
    }
}
