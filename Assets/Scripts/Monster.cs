using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Monster : MonoBehaviour
{
    [SerializeField] Sprite _deadSprite;
    [SerializeField] ParticleSystem _particleSystem;
    public int hp = 1;
    public bool isjumping;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpTime = 0.5f;

    bool _hasDied;

    Animator animator;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDieFromCollision(collision))
        {
            StartCoroutine(Die());
        }        
    }

    bool ShouldDieFromCollision(Collision2D collision)
    {
        if (_hasDied)
        {
            return false;
        }
        
        Bird bird = collision.collider.GetComponent<Bird>();
        if (bird != null)
        {
            return true;
        }

        //Pokud to jde shora, padá to
        if (collision.contacts[0].normal.y < -0.5)
        {
            return true;
        }

        return false;
    }

    IEnumerator Die()
    {
        _hasDied = true;
        GetComponent<SpriteRenderer>().sprite = _deadSprite;
        GetComponent<Animator>().enabled = false;
        _particleSystem.Play();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    internal void TakeDamage(float damage)
    {
        hp -= (int)damage;
        if (hp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    void Update()
    {
        if (!_hasDied)
        {
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump()
    {
        if (!isjumping)
        {
            isjumping = true;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(jumpTime);
            isjumping = false;
        }
    }
}
