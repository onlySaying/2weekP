using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] GameObject bombFactory;
    [SerializeField] GameObject firePos;

    [SerializeField] float firePower = 100f;
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1)) 
        {
            GameObject bomb =  Instantiate(bombFactory);
            bomb.transform.position = firePos.transform.position;
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(Camera.main.transform.forward * firePower);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, firePos.transform.forward);
            RaycastHit hitinfo = new RaycastHit();
            if(Physics.Raycast(ray, out hitinfo)) 
            {
                Debug.Log("맞은 오브젝트 : " + hitinfo.transform.name);
                /*
                 * GameObject bulletFactory = Instantitate(bulletFactroy);
                 * bulletImpact.transform.position = hitinnfo.point;
                   방향설정
                 * bulletImpact.transform.forward = hitInfo.normal;
                 * Destroy(bulletImpact,2);
                    */

                if(hitinfo.transform.gameObject.name.Contains("Enemy"))
                {
                    Enemy enemy = hitinfo.transform.GetComponent<Enemy>();
                    enemy.OnDamamaged();
                }
            }
        }
    }
}
