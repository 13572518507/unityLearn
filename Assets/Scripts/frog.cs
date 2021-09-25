using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : enemy
{

    //private Animator ani;
    private Rigidbody2D rb;
    private Collider2D coll;
    public LayerMask ground;

    private float leftPoint, rightPoint;
    private bool faceRight = false;

    public float speed;
    public float jumpForce;


    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        leftPoint =  GameObject.Find("leftpoint").transform.position.x;
        rightPoint = GameObject.Find("rightpoint").transform.position.x;

    }

    void Update()
    {
        //moveMent(); idle动画事件函数后调用
        switchAni();
    }

    void moveMent()
    {
        if(transform.position.x > rightPoint)
        {
            faceRight = false;
        }else if(transform.position.x < leftPoint)
        {
            faceRight = true;
        }
        if (coll.IsTouchingLayers(ground)){
            if (faceRight)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 0);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            ani.SetBool("jump", true);
        }
        
    }

    void switchAni()
    {
        if (!coll.IsTouchingLayers(ground))
        {
            if (Mathf.Abs(rb.velocity.y) < 0.1)
            {
                ani.SetBool("fall", true);
                ani.SetBool("jump", false);

            }
        }
        else
        {
            if (ani.GetBool("fall"))
            {
                ani.SetBool("fall", false);
            }
        }
    }

    //private void death()
    //{
    //    Destroy(gameObject);
    //}

    //public  void deathAnimation()
    //{
        
    //    ani.SetTrigger("death");

    //}
}
