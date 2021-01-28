using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    private float m_worldFriction = 0.05f;
    public Rigidbody2D s_spriteRB;
    private Vector3 m_velocity = Vector3.zero;
    public LayerMask m_groundLayers;
    public LayerMask m_wallLayers;

    private float s_jumpForce = 700f;
    public bool m_midAirControl = false;
    //Grab ceiling and ground check transforms through hard code.
    public Transform s_groundCheck;
    const float c_groundDetectionRadius = 0.2f;
    public bool m_isGrounded;

    public Transform s_wallCheck;
    const float c_wallDetectionRadius = 0.2f;
    public bool m_onWall;

    // Checks for one-way platform, but this can be handled with Platform Effector 2D pretty well too.
    public Transform s_ceilingCheck;
    const float c_ceilingDetectionRadius = 0.2f;



    public UnityEvent OnGround;
    public UnityEvent OnWall;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        s_spriteRB = GetComponent<Rigidbody2D>();

        if (OnGround == null)
        {
            OnGround = new UnityEvent();
        }

        if (OnWall == null)
        {
            OnWall = new UnityEvent();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        bool wasGrounded = m_isGrounded;
        m_isGrounded = false;

        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(s_groundCheck.position, c_groundDetectionRadius, m_groundLayers);

        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != gameObject)
            {
                m_isGrounded = true;

                if (!wasGrounded)
                    OnGround.Invoke();

                //if (!wasGrounded)
                //{
                //    OnGround.Invoke();
                //}
            }
        }

        bool wasOnWall = m_onWall;
        m_onWall = false;

        Collider2D[] wallColliders = Physics2D.OverlapCircleAll(s_wallCheck.position, c_wallDetectionRadius, m_wallLayers);

        for (int i = 0; i < wallColliders.Length; i++)
        {
            if (wallColliders[i].gameObject != gameObject)
            {
                m_onWall = true;
                m_midAirControl = false;
                if (!wasOnWall)
                    OnWall.Invoke();
            }

        }
    }

    public void MoveSprite(float move, bool isJumping, bool isWalling)
    {
        if (m_isGrounded || m_midAirControl)
        {
            Vector3 desiredVelocity = new Vector2(move * 10f, s_spriteRB.velocity.y);
            s_spriteRB.velocity = Vector3.SmoothDamp(s_spriteRB.velocity, desiredVelocity, ref m_velocity, m_worldFriction);

            // Flip the player depending on current axis direction (left-facing for A, right-facing for D).
            // TO-DO: Flip function!

            
        }

        //if (m_onWall && isWalling)
        //{
        //    //Vector3 desiredVelocity = new Vector2(move * 10f, s_spriteRB.velocity.y);
        //    //s_spriteRB.velocity = Vector3.SmoothDamp(s_spriteRB.velocity, desiredVelocity, ref m_velocity, m_worldFriction);

        //    //float wallTimer = 3.0f;
        //    //wallTimer -= Time.deltaTime;

        //    //if (wallTimer == 0)
        //    //{
        //    //    s_spriteRB.velocity = new Vector2(0, -1);
        //    //}

        //    m_onWall = false;

        //    if (isJumping)
        //    {
        //        s_spriteRB.AddForce(new Vector2(s_spriteRB.velocity.x, s_jumpForce));
        //    }
            
        //}

        if ((m_isGrounded || m_onWall) && isJumping)
        {
            m_isGrounded = false;
            m_onWall = false;
            s_spriteRB.AddForce(new Vector2(0f, s_jumpForce));
        }

        if (isWalling)
        {
            int jumpCounter = 0;

            if (isJumping)
            {
                jumpCounter += 1;

                if (jumpCounter >= 1)
                {
                    isJumping = false;
                }
            }
            else
            {
                jumpCounter = 0;
            }
            m_onWall = false;
            //s_spriteRB.AddForce(new Vector2(s_spriteRB.velocity.x, s_jumpForce));
            s_spriteRB.AddForce(new Vector2(move * 10f, s_jumpForce));
        }
    }
}
