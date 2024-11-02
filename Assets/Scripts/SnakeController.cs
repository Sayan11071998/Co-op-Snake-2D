using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public abstract class SnakeController : MonoBehaviour
{
    // // Message fields for game over scenarios
    // [SerializeField] private string singlePlayerGameOverMessage = "";
    // private string player1DeathMessage = "Player 1 Died. Player 2 Wins!";
    // private string player2DeathMessage = "Player 2 Died. Player 1 Wins!";

    // // Scene and manager references
    // [SerializeField] private string singlePlayerSceneName = "Singleplayer";
    // [SerializeField] private GameOverManager gameOverManager;
    // [SerializeField] private Snake otherSnake;

    // // Audio manager
    // private AudioManager audioManager;

    // Movement and direction variables
    protected Vector2 currentDirection = Vector2.right;
    protected Vector2 nextDirection;
    private const float BaseMoveDelay = 0.1f;
    private float timer;
    private const float Scale = 0.2f;
    private bool ate = false;

    // Tail management
    public GameObject tailPrefab;
    private List<Transform> tail = new List<Transform>();

    // // Power-up states and durations
    // private bool isShieldActive = false;
    // private bool isScoreBoostActive = false;
    // private bool isSpeedUpActive = false;
    // private float shieldDuration = 10f;
    // private float speedUpDuration = 10f;
    // private float scoreBoostDuration = 10f;
    // private float powerUpCooldown = 3f;

    // // Power-up cooldown trackers
    // private float shieldCooldownTimer = 0f;
    // private float scoreBoostCooldownTimer = 0f;
    // private float speedUpCooldownTimer = 0f;

    // // Power-up effects
    // private float scoreMultiplier = 1f;
    private float moveDelay = BaseMoveDelay;

    // // Power-up prefabs
    // [SerializeField] private GameObject shieldPrefab;
    // [SerializeField] private GameObject scoreBoostPrefab;
    // [SerializeField] private GameObject speedUpPrefab;

    // Game boundaries
    [SerializeField] private GameObject topBorder;
    [SerializeField] private GameObject bottomBorder;
    [SerializeField] private GameObject leftBorder;
    [SerializeField] private GameObject rightBorder;
    private float topBorderY;
    private float bottomBorderY;
    private float leftBorderX;
    private float rightBorderX;

    // Scoring system
    // private int score = 0;
    // [SerializeField] private TextMeshProUGUI scoreText;
    // [SerializeField] private GameObject scoreManagerObject;
    // private ScoreManager scoreManager;
    // [SerializeField] private int massGainerScore = 1;
    // [SerializeField] private int massBurnerScore = -2;
    // [SerializeField] private float scoreBoostMultiplier = 2f;
    // [SerializeField] private float defaultScoreMultiplier = 1f;

    // Player identifier
    public int playerNumber;

    // Initialization
    void Start()
    {
        nextDirection = currentDirection;

        // Border initialization
        topBorderY = topBorder.transform.position.y;
        bottomBorderY = bottomBorder.transform.position.y;
        leftBorderX = leftBorder.transform.position.x;
        rightBorderX = rightBorder.transform.position.x;

        // // Score manager reference
        // scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
    }

    // Main update loop
    void Update()
    {
        HandleInput();
        // UpdatePowerUpCooldowns();
    }

    // Movement and collision checks in FixedUpdate for smoother movement
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= moveDelay)
        {
            UpdateDirection();
            Move();
            timer = 0f;
        }

        // Adjust speed based on power-up status
        // moveDelay = isSpeedUpActive ? BaseMoveDelay / 2 : BaseMoveDelay;
    }

    // Abstract method for handling input - implemented in derived classes
    protected abstract void HandleInput();

    // Update snake direction based on player input
    protected void UpdateDirection()
    {
        currentDirection = nextDirection;
    }

    // Move the snake, handle collisions, and manage tail growth
    protected void Move()
    {
        Vector2 previousPosition = transform.position;
        Vector2 newPosition = CalculateNewPosition();

        // // Check for collisions with self and other snake
        // if (CheckSelfCollision(newPosition) || CheckOtherSnakeCollision(newPosition))
        // {
        //     if (isShieldActive)
        //     {
        //         isShieldActive = false;
        //     }
        //     else
        //     {
        //         HandleDeath();
        //         return;
        //     }
        // }

        // Apply screen wrapping
        newPosition = ApplyScreenWrap(newPosition);
        transform.position = newPosition;

        // if (ate)
        // {
        //     GrowTail(previousPosition);
        //     ate = false;
        // }
        // else if (tail.Count > 0)
        // {
        //     MoveTail(previousPosition);
        // }
    }

    // Calculate the new position for the snake's head
    protected Vector2 CalculateNewPosition()
    {
        return new Vector2(
            transform.position.x + currentDirection.x * Scale,
            transform.position.y + currentDirection.y * Scale
        );
    }

    // // Check for collisions with the snake's own tail
    // protected bool CheckSelfCollision(Vector2 headPosition)
    // {
    //     foreach (Transform tailPart in tail)
    //     {
    //         if (headPosition == (Vector2)tailPart.position)
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // Check for collisions with the other snake (head and tail)
    // protected bool CheckOtherSnakeCollision(Vector2 headPosition)
    // {
    //     if (otherSnake != null)
    //     {
    //         // Collision with other snake's head
    //         if (headPosition == (Vector2)otherSnake.transform.position)
    //         {
    //             HandleDeath();
    //             return true;
    //         }

    //         // Collision with other snake's tail
    //         foreach (Transform tailPart in otherSnake.tail)
    //         {
    //             if (headPosition == (Vector2)tailPart.position)
    //             {
    //                 HandleDeath();
    //                 return true;
    //             }
    //         }
    //     }
    //     return false;
    // }

    // Handle screen wrapping based on border positions
    protected Vector2 ApplyScreenWrap(Vector2 position)
    {
        if (position.y > topBorderY)
            position.y = bottomBorderY;
        else if (position.y < bottomBorderY)
            position.y = topBorderY;

        if (position.x < leftBorderX)
            position.x = rightBorderX;
        else if (position.x > rightBorderX)
            position.x = leftBorderX;

        return position;
    }

    // // Grow the tail by adding a new segment
    // protected void GrowTail(Vector2 position)
    // {
    //     GameObject newTailPart = Instantiate(tailPrefab, position, Quaternion.identity);
    //     tail.Insert(0, newTailPart.transform);
    // }

    // Move each tail segment to follow the head
    // protected void MoveTail(Vector2 previousHeadPosition)
    // {
    //     tail.Last().position = previousHeadPosition;
    //     tail.Insert(0, tail.Last());
    //     tail.RemoveAt(tail.Count - 1);
    // }

    // Abstract method to handle snake's death - implemented in derived classes
    protected abstract void HandleDeath();

    // Trigger game over screen based on game mode
    // protected void HandleGameOver()
    // {
    //     string message = SceneManager.GetActiveScene().name == singlePlayerSceneName
    //         ? singlePlayerGameOverMessage
    //         : (playerNumber == 1 ? player1DeathMessage : player2DeathMessage);

    //     gameOverManager.ShowGameOverScreen(message);
    //     Destroy(gameObject);
    //     AudioManager.instance.PlaySFX(AudioManager.instance.gameOverSound);
    // }

    // Collision handling for food and power-ups
    // void OnTriggerEnter2D(Collider2D coll)
    // {
    //     if (coll.TryGetComponent(out Food food))
    //     {
    //         HandleFoodCollision(food);
    //     }
    //     else if (coll.TryGetComponent(out PowerUp powerUp))
    //     {
    //         HandlePowerUpCollision(powerUp);
    //     }
    // }

    // // Handle collision with food items
    // private void HandleFoodCollision(Food food)
    // {
    //     ate = food.type == FoodType.MassGainer;
    //     UpdateScore(food.type == FoodType.MassGainer ? massGainerScore : massBurnerScore);
    //     AudioManager.instance.PlaySFX(AudioManager.instance.eatSound);
    //     Destroy(food.gameObject);
    // }

    // // Handle collision with power-ups
    // private void HandlePowerUpCollision(PowerUp powerUp)
    // {
    //     ActivatePowerUp(powerUp.type);
    //     AudioManager.instance.PlaySFX(AudioManager.instance.powerUpSound);
    //     Destroy(powerUp.gameObject);
    // }

    // // Activate specific power-up based on type
    // private void ActivatePowerUp(PowerUpType type)
    // {
    //     switch (type)
    //     {
    //         case PowerUpType.Shield:
    //             if (shieldCooldownTimer <= 0) StartCoroutine(ActivateShield());
    //             break;
    //         case PowerUpType.ScoreBoost:
    //             if (scoreBoostCooldownTimer <= 0) StartCoroutine(ActivateScoreBoost());
    //             break;
    //         case PowerUpType.SpeedUp:
    //             if (speedUpCooldownTimer <= 0) StartCoroutine(ActivateSpeedUp());
    //             break;
    //     }
    // }

    // Power-up activation and deactivation coroutines
    // private IEnumerator ActivateShield()
    // {
    //     isShieldActive = true;
    //     shieldCooldownTimer = shieldDuration + powerUpCooldown;
    //     yield return new WaitForSeconds(shieldDuration);
    //     isShieldActive = false;
    // }

    // private IEnumerator ActivateScoreBoost()
    // {
    //     isScoreBoostActive = true;
    //     scoreMultiplier = scoreBoostMultiplier;
    //     scoreBoostCooldownTimer = scoreBoostDuration + powerUpCooldown;
    //     yield return new WaitForSeconds(scoreBoostDuration);
    //     scoreMultiplier = defaultScoreMultiplier;
    //     isScoreBoostActive = false;
    // }

    // private IEnumerator ActivateSpeedUp()
    // {
    //     isSpeedUpActive = true;
    //     speedUpCooldownTimer = speedUpDuration + powerUpCooldown;
    //     yield return new WaitForSeconds(speedUpDuration);
    //     isSpeedUpActive = false;
    // }

    // // Update score and UI
    // private void UpdateScore(int deltaScore)
    // {
    //     score += Mathf.RoundToInt(deltaScore * scoreMultiplier);
    //     scoreText.text = "Score: " + score;
    // }

    // // Update cooldowns for power-ups
    // private void UpdatePowerUpCooldowns()
    // {
    //     shieldCooldownTimer = Mathf.Max(0, shieldCooldownTimer - Time.deltaTime);
    //     scoreBoostCooldownTimer = Mathf.Max(0, scoreBoostCooldownTimer - Time.deltaTime);
    //     speedUpCooldownTimer = Mathf.Max(0, speedUpCooldownTimer - Time.deltaTime);
    // }
}