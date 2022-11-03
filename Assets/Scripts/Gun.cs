using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float timeBetweenShots = 1f;
    bool canShoot = true;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem impactEffect;
    [SerializeField] float impactForce = 30f;
    GameObject Target;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine(ShootCooldown());
            muzzleFlash.Play();
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right);
            if (hitInfo)
            {
                //play impact effect at hit point
                impactEffect.transform.position = hitInfo.point;
                impactEffect.Play();
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
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }
}
