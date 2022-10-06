using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float fireRate;
    //[SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject impactEffect;
    [SerializeField] float impactForce = 30f;
    GameObject Target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        //2D
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right);
        if (hitInfo != null)
        {
            Debug.Log(hitInfo.transform.name);
        }
        //if hitted object has tag monster
        if (hitInfo.collider.CompareTag("Monster"))
        {
            //get monster script
            Target = hitInfo.collider.gameObject;
            Monster monster = Target.GetComponent<Monster>();
            //call TakeDamage function
            monster.TakeDamage(damage);
            //move with impact force
            Rigidbody2D rb = Target.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(transform.right * impactForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Rigidbody2D rb = hitInfo.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(transform.right * impactForce, ForceMode2D.Impulse);
            }
        }
    }
}
