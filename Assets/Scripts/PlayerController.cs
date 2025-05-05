using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 movement;

    public string prevScene = "TrainStationScene";

    private static PlayerController instance;

    private Animator animator;

    [Header("Footstep Audio")]
    public bool isInterior = false;
    public List<AudioClip> grassAudios;
    public List<AudioClip> tilesAudios;
    private AudioSource audioSource;
    public float pitchVariation = 0.1f;
    public float footstepInterval = 0.4f; // Time between footstep sounds

    private float footstepTimer = 0f;

    public bool canMove = false;
    public bool isFakeMoving = true;
    private Vector2 fakeMovement;

    [Header("Story variables")]
    public bool farming_task = false;
    public bool harvest_task = false;
    public bool fountain_task = false;

    private void Awake()
    {
        // Singleton check to prevent duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isFakeMoving)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0) movement.y = 0; // Prevent diagonal movement
        }  
    }

    private void FixedUpdate()
    {
        if (canMove || isFakeMoving)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", movement.x);
                animator.SetFloat("moveY", movement.y);

                footstepTimer -= Time.fixedDeltaTime;
                if (footstepTimer <= 0f)
                {
                    PlayFootstep();
                    footstepTimer = footstepInterval;
                }
            }
            else
            {
                animator.SetBool("isMoving", false);
                footstepTimer = 0f;
            }
        }
    }

    private void PlayFootstep()
    {
        List<AudioClip> currentClips = isInterior ? tilesAudios : grassAudios;

        if (currentClips.Count == 0 || audioSource == null)
            return;

        AudioClip clip = currentClips[Random.Range(0, currentClips.Count)];
        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(clip);
    }

    public void FakeRightMovement()
    {
        if (rb == null || animator == null) return;

        isFakeMoving = true;
        canMove = false;
        movement.x = 1;
        movement.y = 0;
    }

    public void StopFakeMovement()
    {
        isFakeMoving = false;
        canMove = false;
        movement.x = 0; 
        movement.y = 0;
        animator.SetBool("isMoving", false);
        footstepTimer = 0f;
    }
}
