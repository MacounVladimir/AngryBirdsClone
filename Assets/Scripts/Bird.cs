using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _launchForce = 500f;
    [SerializeField] float _maxDragDistance = 5f;

    Vector2 _startPosition;
    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;

    public Vector2 direction;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidbody.position;
        _rigidbody.isKinematic = true;
    }

    void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;  
    }

    void OnMouseUp()
    {
        Vector2 currentPosition = _rigidbody.position;
        direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(direction * _launchForce);

        _spriteRenderer.color = Color.white;
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;

        float distance = Vector2.Distance(desiredPosition, _startPosition);
        if (distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }

        _rigidbody.position = desiredPosition;

        //DrawTrajectory(desiredPosition);
        Debug.DrawLine(_startPosition, desiredPosition, Color.red);
    }

    //private void DrawTrajectory(Vector2 startPosition)
    //{
    //    Vector2 start = startPosition;
    //    Vector2 dir = direction;
    //    float force = _launchForce;
    //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    float gravity = Physics2D.gravity.magnitude;

    //    Vector3[] points = new Vector3[10];
    //    for (int i = 0; i < points.Length; i++)
    //    {
    //        float time = i * 0.1f;
    //        float x = start.x + (dir.x * force * time * Mathf.Cos(angle * Mathf.Deg2Rad));
    //        float y = start.y + (dir.y * force * time * Mathf.Sin(angle * Mathf.Deg2Rad)) - (0.5f * gravity * time * time);
    //        points[i] = new Vector3(x, y, 0);
    //    }

    //    LineRenderer lineRenderer = GetComponent<LineRenderer>();
    //    lineRenderer.positionCount = points.Length;
    //    lineRenderer.SetPositions(points);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay(2f));
    }

    private IEnumerator ResetAfterDelay(float v)
    {
        yield return new WaitForSeconds(v);
        _rigidbody.position = _startPosition;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector2.zero;
    }
}
