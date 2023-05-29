using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Other Components
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    // Player Sprites
    public Sprite standingSprite;
    public Sprite crouchingSprite;

    // Movement Controls & Collision
    public float playerSpeed = 3.0f;
    public float maxSpeed = 12.0f;
    public float jumpForce = 20.0f; // Added a separate jump force for better control over jump height
    public float groundCheckRadius = 0.2f; // Radius of the ground check
    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask environmentLayer;

    // Particles
    public ParticleSystem playerParticles;
    private ParticleSystem.EmissionModule particleEmission;
    private float maxParticleEmission = 500f;

    private void Start()
    {
        // Access & Assign the Other Components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Particles
        particleEmission = playerParticles.emission;
        playerParticles.Stop();
    }

    private void Update()
    {
        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, environmentLayer);

        // Horizontal Movement
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * playerSpeed);
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * playerSpeed);
            spriteRenderer.flipX = false;
        }
        else // Decelerate when no key is pressed
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        }

        // Check the player's current speed and adjust particle emission accordingly
        float speedPercentage = rb.velocity.magnitude / maxSpeed;
        float emissionRate = speedPercentage * maxParticleEmission;
        particleEmission.rateOverTime = emissionRate;
        if (rb.velocity.magnitude > 0.1) // Check if the player is moving
        {
            // Start particles if they are not playing
            if (!playerParticles.isPlaying)
                playerParticles.Play();
        }
        else
        {
            // If the player is not running and particles are playing, stop the particles
            if (playerParticles.isPlaying)
                playerParticles.Stop();
        }

        // Limit horizontal speed to maxSpeed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x * 1.5f, 0); // Preserve horizontal momentum and increase it slightly when jumping
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); // Add jump force
        }

        // Crouch when S pressed
        if (Input.GetKeyDown(KeyCode.S))
        {
            spriteRenderer.sprite = crouchingSprite;
            boxCollider.size = new Vector2(1f, 1f);
            boxCollider.offset = new Vector2(boxCollider.offset.x, -0.5f);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            spriteRenderer.sprite = standingSprite;
            boxCollider.size = new Vector2(1f, 2f);
            boxCollider.offset = new Vector2(boxCollider.offset.x, 0f);
        }
    }
}