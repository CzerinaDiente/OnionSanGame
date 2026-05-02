using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    public float enemySpeed;
    private PlayerMovement playerMovement;

    public bool isDead = false;
    private int health = 1;

    [Header("Score Penalty")]
    public int penalty = 100;

    void Start()
    {
        // Find the player in the scene (tag must be "Player")
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    { 
        if (isDead || GameManager.Instance.isGameOver) return;

        transform.Translate(Vector2.down * enemySpeed * Time.deltaTime); 
    }

    public void Die()
    {
        if (isDead || GameManager.Instance.isGameOver) return;

        isDead = true;
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (isDead || GameManager.Instance.isGameOver) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //for border damage
    {
        if (isDead || GameManager.Instance.isGameOver) return;

        if (other.CompareTag("Border"))
        {
            isDead = true;

            // Subtract score instead of damaging player
            if (ScorePoints.scorePoints != null)
            {
                ScorePoints.scorePoints.SubtractPoint(penalty);
            }

            Destroy(gameObject);
        }
    }
}
