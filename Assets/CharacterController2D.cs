using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    private float m_worldFriction = 0.05f;
    public Rigidbody2D s_spriteRB;
    private Vector3 m_velocity = Vector3.zero;
    public bool flipped = false;
    
    

    private float s_jumpForce = 700f;
    private float s_wallJumpForce = 700f;
    public bool m_midAirControl = false;



    //Grab ceiling and ground check transforms through hard code.
    public LayerMask m_groundLayers;
    public Transform s_groundCheck;
    const float c_groundDetectionRadius = 0.2f;
    public bool m_isGrounded;



    public LayerMask m_wallLayers;
    public Transform s_wallCheck0;
    public Transform s_wallCheck1;
    const float c_wallDetectionRadius0 = 0.2f;
    const float c_wallDetectionRadius1 = 0.2f;
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

            }
        }

        bool wasOnWall = m_onWall;
        m_onWall = false;

        Collider2D[] wallCollider0 = Physics2D.OverlapCircleAll(s_wallCheck0.position, c_wallDetectionRadius0, m_wallLayers);

        for (int i = 0; i < wallCollider0.Length; i++)
        {
            if (wallCollider0[i].gameObject != gameObject && m_isGrounded == false)
            {
                m_onWall = true;
                //m_midAirControl = false;
                if (!wasOnWall)
                    OnWall.Invoke();
            }
        }

        Collider2D[] wallCollider1 = Physics2D.OverlapCircleAll(s_wallCheck1.position, c_wallDetectionRadius1, m_wallLayers);

        for (int i = 0; i < wallCollider1.Length; i++)
        {
            if (wallCollider1[i].gameObject != gameObject && m_isGrounded == false)
            {
                m_onWall = true;
                //m_midAirControl = false;
                if (!wasOnWall)
                    OnWall.Invoke();
            }
        }
    }

    public void MoveSprite(float move, bool isOnGround, bool isJumping, bool isWalling, bool isWallJumping)
    {
        if (m_isGrounded || m_midAirControl)
        {
            Vector3 desiredVelocity0 = new Vector2(move * 10f, s_spriteRB.velocity.y);
            s_spriteRB.velocity = Vector3.SmoothDamp(s_spriteRB.velocity, desiredVelocity0, ref m_velocity, m_worldFriction);

            if (move > 0 && flipped)
            {
                FlipSprite();
                flipped = false;
            }
            else if (move < 0 && !flipped)
            {
                FlipSprite();
                flipped = true;
            }
        }


        if (isWalling)
        {
            float spriteVelocity = s_spriteRB.velocity.x;
            m_midAirControl = true;
            s_spriteRB.AddForce(new Vector2(0f, s_spriteRB.velocity.y - 100));
            spriteVelocity = 0;
        }


        if ((isWalling && flipped) && isWallJumping)
        {
            m_onWall = false;
            s_spriteRB.AddForce(new Vector2(s_wallJumpForce * 1.13f, s_wallJumpForce * 1.1f));
            m_midAirControl = false;
        }
        
        if ((isWalling && !flipped) && isWallJumping)
        {
            m_onWall = false;
            s_spriteRB.AddForce(new Vector2(-s_wallJumpForce * 1.13f, s_wallJumpForce * 1.1f));
            m_midAirControl = false;
        }
        //else
        //{
        //    m_midAirControl = true;
        //}

        if ((m_isGrounded && isOnGround) && isJumping)
        {
            m_isGrounded = false;            
            s_spriteRB.AddForce(new Vector2(0f, s_jumpForce));
        }
    }

    private void FlipSprite()
    {
        Vector3 objScale = transform.localScale;
        objScale.x *= -1;
        transform.localScale = objScale;
    }
}
