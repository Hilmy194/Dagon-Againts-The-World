using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip shootSound;

    private Rigidbody2D rig;
    private Animator anim;
    private AudioSource audioSource;

    private bool isJumping;
    private bool isOnGround;

    private float lastPositionX;
    public float speedIncrement = 0.1f; // Besarnya kenaikan kecepatan
    private int lastScoreMilestone = 0; // Skor milestone terakhir

    public GameObject bulletPrefab; // Prefab peluru
    public Transform bulletSpawnPoint; // Posisi spawn peluru
    public float shootCooldown = 0.5f; // Waktu jeda antar tembakan

    private float lastShootTime; // Waktu tembakan terakhir

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        lastPositionX = transform.position.x;

        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found! Please attach an AudioSource to the GameObject.");
        }
    }

    private void Update()
    {
        // Read input
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
            {
                isJumping = true;
                if (jumpSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                    Debug.Log("Jump sound played!");
                }
                else
                {
                    Debug.LogWarning("Jump sound or AudioSource is missing!");
                }
            }
        }

        // Change animation
        anim.SetBool("isOnGround", isOnGround);

        // Calculate score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        if (Input.GetMouseButtonDown(1) && Time.time > lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time; // Perbarui waktu tembakan terakhir
        }

        // Increase speed every 50 points
        int currentScore = Mathf.FloorToInt(score.GetCurrentScore());
        if (currentScore >= lastScoreMilestone + 50)
        {
            maxSpeed += speedIncrement; // Tingkatkan kecepatan maksimum
            lastScoreMilestone += 50; // Perbarui milestone
        }

        // Game over
        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Periksa jika player menyentuh obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    private void FixedUpdate()
    {
        // Raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if (hit)
        {
            if (!isOnGround && rig.velocity.y <= 0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        // Calculate velocity vector
        Vector2 velocityVector = rig.velocity;

        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;
    }

    private void Shoot()
    {
        // Spawn peluru di posisi spawn point
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
                Debug.Log("Shoot sound played!");
            }
            else
            {
                Debug.LogWarning("Shoot sound or AudioSource is missing!");
            }
        }
        else
        {
            Debug.LogError("BulletPrefab or BulletSpawnPoint is missing!");
        }
    }

    private void GameOver()
    {
        // Set high score
        score.FinishScoring();

        // Stop camera movement
        gameCamera.enabled = false;

        // Show game over
        gameOverScreen.SetActive(true);

        // Disable this script
        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
