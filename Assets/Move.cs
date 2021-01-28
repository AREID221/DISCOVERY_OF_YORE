using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // 2D character controller,
    // contains move and jump.
    public CharacterController2D controller;

    // Left/right movement, jumping velocity.
    private float m_xMovement = 0f;
    private float m_yMovement = 0f;

    // Left/right move speed.
    private float s_moveSpeed = 100f;

    // Jump force.
    private float s_jumpForce = 30f;

    // Isn't used.
    //public Rigidbody2D rB;
    //public float force = 10f;
    
    private void Awake()
    {
        //rB = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Take this out and add colliders to player (one for collision and one for ground check).
        //rB.isKinematic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        m_xMovement = Input.GetAxisRaw("Horizontal") * s_moveSpeed;
        //m_yMovement = Input.GetAxisRaw("Vertical"); // Jumping instead.
    }

    private void FixedUpdate()
    {
        //                    Left/right,  jump.
        controller.MoveSprite(m_xMovement * Time.fixedDeltaTime, false);
    }
}
