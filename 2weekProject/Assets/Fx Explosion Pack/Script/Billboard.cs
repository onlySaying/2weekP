using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //자신의 앞 방향과 카메라의 앞방향과 같게한다.
        transform.forward= Camera.main.transform.forward;
    }
}
