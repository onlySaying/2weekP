using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 5f;

    CharacterController cc;

    float gravity = -20f;
    float yVelocity= 0;
    float jumpPower = 5;    
    
    float maxHp = 100;
    float curHp = 0;
    [SerializeField] Slider hpslider;
    [SerializeField] Text hpText;
    void Start()
    {
        curHp = maxHp;
        cc= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * y;
        Vector3 dir = dirH + dirV;

        //transform.position = transform.position + dir * speed * Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
        }
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * speed * Time.deltaTime);
    }

    public void OnDamaged()
    {
        curHp -= 10;
        hpText.text = "HP : " + curHp;

        float ratio = curHp/ maxHp;
        hpslider.value = ratio;
    }
}
