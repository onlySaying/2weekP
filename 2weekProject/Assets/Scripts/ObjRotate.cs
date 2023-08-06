using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate : MonoBehaviour
{
    //ȸ�� �� (���콺 ������ ���� ��
    float rotX = 0;
    float rotY = 0;

    [SerializeField]bool useVertical = false;

    float rotSpeed = 100f;
    float rotSpeedV = 100f;
    void Start()
    {
        
    }

    
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if(useVertical)
        {
            rotY += my * Time.deltaTime * rotSpeedV;
        }
        else
        {
            rotX += mx * Time.deltaTime * rotSpeed;
        }
        

        transform.localEulerAngles = new Vector3(-rotY, rotX, 0);
    }
}
