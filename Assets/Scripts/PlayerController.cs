using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 movement;

    public string prevScene = "TrainStationScene";
    public bool canMove = true;

    private static PlayerController instance;

    private Animator animator; 

    private void Awake()
    {
        // Singleton check to prevent duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // A player already exists, destroy this one
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0) movement.y = 0; // Prevent diagonal movement
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            if(movement.x != 0 || movement.y != 0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", movement.x);
                animator.SetFloat("moveY", movement.y);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            
        }         
    }
}
