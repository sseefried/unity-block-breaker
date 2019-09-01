using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    // cached variables
    [SerializeField] GameSession gameSession;

    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameSession.LoseLife();
        if (gameSession.lives <= 0)
        {
            SceneManager.LoadScene("Game Over");
        }
        else
        {
            FindObjectOfType<Ball>().MoveBallBackToPaddle();
        }
    }
}
