using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float bulletSpeed = 15;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    private bool hasHit = false;

    void Start(){ Destroy(gameObject, 1f); }
    
    void FixedUpdate()
    { 
        rb.MovePosition(rb.position + (Vector2)transform.up * bulletSpeed * Time.fixedDeltaTime); // Keep velocity consistent with physics updates
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit || GameManager.Instance.isGameOver) return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            hasHit = true;

            damageable.TakeDamage(1);

            ScorePoints.scorePoints.AddPoint();

            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);

            Destroy(gameObject);
            Destroy(effect, 1f);
        }
    }
}