using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float bulletSpeed = 20;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    private bool hasHit = false;

    void Start(){ rb.velocity = transform.up * bulletSpeed; Destroy(gameObject, 1f); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit || GameManager.Instance.isGameOver) return;

        if (other.gameObject.tag == "Enemy")
        {
            hasHit = true;

            ScorePoints.scorePoints.AddPoint(); 
            Destroy(other.gameObject);

            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation); //Impact Effect 
            
            Destroy(this.gameObject);
            Destroy(effect, 1f); 
        }
    }
}