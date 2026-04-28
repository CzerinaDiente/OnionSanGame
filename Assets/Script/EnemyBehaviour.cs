using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float enemySpeed;
    private PlayerMovement playerMovement;

    public bool isDead = false;

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

    private void OnTriggerEnter2D(Collider2D other)
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
