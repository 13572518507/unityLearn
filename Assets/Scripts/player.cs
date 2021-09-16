using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public Collider2D coll;
    public double collections;
    public Text cherText;
    private Rigidbody2D rib;
    private Animator ani;
    private float horizontal;
    private bool jumpping;
    private bool isHurt;



    void Start()
    {
        rib = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
        Move();
        switchAnimation();
       
    }

    void FixedUpdate()
    {
        if (!isHurt)
        {
            if (horizontal != 0)
            {
                rib.velocity = new Vector2(horizontal * speed * Time.deltaTime, rib.velocity.y);
            }
        }
        
    }


    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        float hori = Input.GetAxisRaw("Horizontal");
        if(hori != 0){
            transform.localScale = new Vector3(hori, 1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (coll.IsTouchingLayers(ground))
            {
                ani.SetBool("jump", true);
                rib.velocity = new Vector2(rib.velocity.x, jumpForce);
                jumpping = true;
            }

        }

        if (horizontal != 0)
        {
            if (!jumpping)
            {
                ani.SetFloat("running", Math.Abs(horizontal));
                ani.SetBool("idle", false);
            }
            else
            {
                ani.SetFloat("running", 0);
            }
        }
        else
        {
            ani.SetBool("idle", true);
            ani.SetFloat("running", 0);
        }



        if (rib.velocity.y < 0 && ani.GetBool("jump"))
        {
            ani.SetBool("jump", false);
            ani.SetBool("fall", true);
        }

    }

    void switchAnimation()
    {
       
        
        if(coll.IsTouchingLayers(ground) && ani.GetBool("fall"))
        {
            ani.SetBool("fall", false);
            jumpping = false;
            ani.SetBool("idle", true);
        }
        if(isHurt && Math.Abs(rib.velocity.x) < 0.1f)
        {
            ani.SetBool("idle", true);
            ani.SetBool("hurt", false);
            ani.SetFloat("running", 0);
            isHurt = false;
        }
        //if(coll.IsTouchingLayers(ground) && rib.velocity.x == 0)
        //{
        //    ani.SetBool("idle", true);
        //    ani.SetBool("hurt", false);
        //    ani.SetFloat("running", 0);
        //    isHurt = false;
        //}

    }

    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "cherry")
        {
            col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(col.gameObject);
            collections = collections + 1;
            cherText.text = collections.ToString();
            Debug.Log("Triggered by Cherry");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (ani.GetBool("fall"))
            {
                ani.SetBool("jump", true);
                rib.velocity = new Vector2(rib.velocity.x, jumpForce);
                //jumpping = true;
                Destroy(collision.gameObject);
            }
            else
            {
                if(collision.gameObject.transform.position.x > gameObject.transform.position.x)
                {
                    isHurt = true;
                    rib.velocity = new Vector2(-5, rib.velocity.y);
                    ani.SetBool("hurt", true);
                }
                else
                {
                    isHurt = true;
                    rib.velocity = new Vector2(5, rib.velocity.y);
                    ani.SetBool("hurt", true);
                }
            }
        }
    }

}
