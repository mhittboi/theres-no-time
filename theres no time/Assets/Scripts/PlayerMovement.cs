using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem.EmissionModule particleEmission;

    // speed/movement
    public float playerSpeed = 3.0f;
    public float playerCrouchSpeed = 1.5f;
    public float maxSpeed = 12.0f;
    public float jumpForce = 20.0f;

    // collisions
    public float groundCheckRadius = 0.5f;
    public Transform groundCheckPoint;
    public Transform overheadCheckPoint;
    public LayerMask environmentLayer;
    public LayerMask overheadObstructionLayer;

    // spritesheet animations
    private enum AnimationState { Idle, Running, Crouching };
    private AnimationState currentState;
    public Sprite[] playerSprites;
    private float animationTimer = 0f;
    private float animationInterval = 0.1f;

    // particles
    public ParticleSystem playerParticles;
    private float maxParticleEmission = 500f;

    // character information
    private bool isGrounded;
    private bool isCrouching;
    public bool crownCollected;
    public bool hasMoved = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        particleEmission = playerParticles.emission;

        particleEmission.rateOverTime = 0;
        playerParticles.Stop();

        crownCollected = false;
        PauseController.canPause = true;
    }

    private void Update()
    {
        // check if game is paused to sav resources :)
        if (PauseController.isPaused) return;

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, environmentLayer); // check if currently grounded

        // movement is broken into 3 parts - physical movement, animation, then visual effects. all 3 are called. 
        HandleMovement();
        HandleAnimation();
        HandleParticles();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.DownArrow)) // if down pressed, crouch
        {
            Crouch();
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) // if left pressed, move left
        {
            Move(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // if right pressed, move right
        {
            Move(Vector2.right);
        }
        else // default sprite
        {
            Idle();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded) // if up pressed while grounded, jump
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow)) // try and stand if crouching
        {
            TryStand();
        }

        // limit the character speed, whatever direction, to the max
        LimitSpeed();
    }

    private void Move(Vector2 direction)
    {
        hasMoved = true; // this signals the countdown to begin
        float speed = isCrouching ? playerCrouchSpeed : playerSpeed;  // use the reduced speed if the player is crouching
        rb.AddForce(direction * speed);
        spriteRenderer.flipX = direction == Vector2.left;
        currentState = AnimationState.Running;
        if (isCrouching) // if crouched, ensure that animation remains crouched while moving
        {
            currentState = AnimationState.Crouching;
        }
    }

    private void Jump()
    {
        hasMoved = true;
        rb.velocity = new Vector2(rb.velocity.x * 1.5f, 0);
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    private void Crouch()
    {
        isCrouching = true;
        boxCollider.size = new Vector2(2f, 1f);
        boxCollider.offset = new Vector2(boxCollider.offset.x, -1f);
        currentState = AnimationState.Crouching;
    }

    private void TryStand()
    {
        bool hasOverheadObstruction = Physics2D.OverlapCircle(overheadCheckPoint.position, groundCheckRadius, overheadObstructionLayer); // check to see if adequate room to un-crouch
        if (!hasOverheadObstruction)
        {
            boxCollider.size = new Vector2(1.5f, 3f);
            boxCollider.offset = new Vector2(boxCollider.offset.x, 0f);
            currentState = AnimationState.Idle;
            isCrouching = false;
        }
    }

    private void Idle()
    {
        rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        if (currentState != AnimationState.Crouching)
        {
            currentState = AnimationState.Idle;
        }
    }

    private void HandleAnimation() // displays appropriate sprites based on the current animationstate
    {
        switch (currentState)
        {
            case AnimationState.Idle: 
                spriteRenderer.sprite = playerSprites[8];
                break;

            case AnimationState.Running:
                animationTimer += Time.deltaTime;
                if (animationTimer >= animationInterval)
                {
                    int spriteIndex = (int)((animationTimer / animationInterval) % 7);
                    spriteRenderer.sprite = playerSprites[spriteIndex];
                }
                break;

            case AnimationState.Crouching:
                spriteRenderer.sprite = playerSprites[7];
                break;
        }
    }

    private void HandleParticles() // the particles here are tied to the players speed - faster means more particles. 
    {
        float speedPercentage = rb.velocity.magnitude / maxSpeed;
        float emissionRate = speedPercentage * maxParticleEmission;
        particleEmission.rateOverTime = emissionRate;

        if (rb.velocity.magnitude > 0.1 && !playerParticles.isPlaying)
        {
            playerParticles.Play();
        }
        else if (currentState == AnimationState.Idle && playerParticles.isPlaying)
        {
            playerParticles.Stop();
        }
    }

    private void LimitSpeed()
    {
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
    }
}