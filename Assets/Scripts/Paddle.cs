using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;
    [SerializeField] float screenWidthInUnits = 16f;

    GameSession thisGameSession;
    Ball thisBall;

    private void Start()
    {
        thisGameSession = FindObjectOfType<GameSession>();
        thisBall = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 paddlePos = transform.position;
        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = paddlePos;
    }

    private float GetXPos()
    {
        if (thisGameSession.IsAutoPlayEnabled())
        {
            return thisBall.transform.position.x;
        } else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
        }
    }

}
