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
    //private float m_yMovement = 0f;
    public bool grounded = false;
    public bool jumping = false;
    // Left/right move speed.
    private float s_moveSpeed = 100f;

    public bool walling = false;
    public bool wallJumping = false;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        m_xMovement = Input.GetAxisRaw("Horizontal") * s_moveSpeed;

        if (controller.m_isGrounded)
        {
            grounded = true;
            controller.m_midAirControl = true;
        }

        if (controller.m_onWall)
        {
            walling = true;
        }

        if (Input.GetButton("Jump") && !controller.m_onWall)
        {
            jumping = true;
        }
        else if (Input.GetButton("Jump") && controller.m_onWall)
        {
            wallJumping = true;
        }
        



    }

    private void FixedUpdate()
    {
        controller.MoveSprite(m_xMovement * Time.fixedDeltaTime, grounded, jumping, walling, wallJumping);
        jumping = false;
        wallJumping = false;
        walling = false;
        grounded = false;
    }
}
