using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchGround;

    private Animator playerAnimation;

    private Vector3 respawnPoint; 
    public GameObject fallDetector;
    public Text scoreText;
    public HealthBar healthBar;
    private int score = 0;

 
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        respawnPoint = transform.position;
        scoreText.text = "Score: " + Scoring.totalScore;
    }

   
    void Update()
    {
        isTouchGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (direction > 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(0.246905804f, 0.246905804f);
        }
        else if (direction < 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(-0.246905804f, 0.246905804f);
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isTouchGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }

            playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
            playerAnimation.SetBool("OnGround", isTouchGround);

            fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        else if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
          
            respawnPoint = transform.position;
        }
        else if (collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "CompleteLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
        }

        else if (collision.tag == "Crystal")
        {
            Scoring.totalScore += 1;
            scoreText.text = "Score: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
        }

        }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Spikes")
        {
            transform.position = respawnPoint;
            healthBar.Damage(0.1f);
        }
    }
}