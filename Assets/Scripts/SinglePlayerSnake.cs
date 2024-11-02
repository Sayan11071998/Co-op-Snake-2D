using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerSnake : SnakeController
{
    protected override void HandleInput()
    {
        if (Input.GetKey(KeyCode.D) && currentDirection != Vector2.left)
            nextDirection = Vector2.right;
        else if (Input.GetKey(KeyCode.S) && currentDirection != Vector2.up)
            nextDirection = Vector2.down;
        else if (Input.GetKey(KeyCode.A) && currentDirection != Vector2.right)
            nextDirection = Vector2.left;
        else if (Input.GetKey(KeyCode.W) && currentDirection != Vector2.down)
            nextDirection = Vector2.up;
    }

    protected override void HandleDeath()
    {
        Debug.Log("Handling death for snake: " + gameObject.name);
        //AudioManager.instance.PlayGameOverSound();
        // HandleGameOver();
    }
}
