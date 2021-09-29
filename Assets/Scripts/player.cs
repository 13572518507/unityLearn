using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed;
    public float climpSpeed;
    public float jumpForce;
    public float gravityNum;
    public LayerMask ground;
    public LayerMask landder;
    public Collider2D coll;
    public double collections;
    public Text cherText;
    public GameObject healthState;
    private Rigidbody2D rib;
    private Animator ani;
    private Transform headNode;
    private float horizontal;
    private bool jumpping;
    private bool isHurt;
    private bool crouching;
    private bool climb;

    public float sumBlood = 180;

    public GameObject landLine;



    void Start()
    {
        rib = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        headNode = GameObject.Find("headPoint").gameObject.transform;
        gravityNum = rib.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        checkLandder();
        //if (!climb)
        //{
            Move();
            switchAnimation();
        //}
        isClimbing();
        
        

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
    void checkLandder()
    {
        climb = coll.IsTouchingLayers(landder);
    }

    void isClimbing()
    {
        if (climb)
        {
            rib.gravityScale = 0; //重力为零
            landLine.GetComponent<BoxCollider2D>().enabled = false;
            
            //Vector3 pos = rib.gameObject.transform.position;
            //rib.gameObject.transform.position = new Vector3(landLine.transform.position.x,pos.y,0);
            float moveY = Input.GetAxis("Vertical");
            if (moveY > 0.5f || moveY < -0.5f)
            {
                rib.velocity = new Vector2(0, moveY * climpSpeed);
                ani.SetBool("climb", true);
            }
            else
            {
                ani.SetBool("climb", false);
                rib.velocity = new Vector2(0, 0);
            }

            if (ani.GetBool("fall") || ani.GetBool("jump"))
                ani.SetBool("climb", true);
        }
        else
        {
            if(ani.GetBool("climb"))
                ani.SetBool("climb", false);
            rib.gravityScale = gravityNum;
            landLine.GetComponent<BoxCollider2D>().enabled = true;
        }
        
    }

    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        float hori = Input.GetAxisRaw("Horizontal");
        if (hori != 0)
        {
            transform.localScale = new Vector3(hori, 1, 1);
        }

        if (Input.GetButtonDown("Crouch") && !climb)
        {
            Debug.Log("我蹲下了");
            ani.SetBool("crouch", true);
            ani.SetBool("idle", false);
            GetComponent<BoxCollider2D>().enabled = false; //禁用
            crouching = true;
        }
        
        if (Input.GetButtonUp("Crouch"))
        {
            if (!Physics2D.OverlapCircle(headNode.position, 0.2f, ground))
            {
                Debug.Log("我又站起来了");
                if (ani.GetBool("crouch"))
                {
                    ani.SetBool("crouch", false);
                    ani.SetBool("idle", true);
                    GetComponent<BoxCollider2D>().enabled = true; 
                }
            }
            crouching = false;
        }

        if (!Physics2D.OverlapCircle(headNode.position, 0.2f, ground) && !crouching) //头上没有刚体
        {
            if (ani.GetBool("crouch"))
            {
                ani.SetBool("crouch", false);
                ani.SetBool("idle", true);
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if (Input.GetButtonDown("Jump")&&!climb)
        {
            if ((ani.GetBool("idle")|| ani.GetFloat("running")!=0) && !jumpping)
            {
                ani.SetBool("jump", true);
                rib.velocity = new Vector2(rib.velocity.x, jumpForce);
                GetComponent<BoxCollider2D>().enabled = false;
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
            if (!ani.GetBool("crouch")&&!ani.GetBool("hurt"))
            {
                ani.SetBool("idle", true);
                ani.SetFloat("running", 0);
            }
        }

        
    }

    void switchAnimation()
    {
        //下落判断
        if (rib.velocity.y < 0 && ani.GetBool("jump"))
        {
            ani.SetBool("jump", false);
            ani.SetBool("fall", true);
        }

        //下落接触地面
        //if (coll.IsTouchingLayers(ground) && ani.GetBool("fall"))
        if (rib.velocity.y == 0 && ani.GetBool("fall"))
        {
            ani.SetBool("fall", false);
            jumpping = false;
            GetComponent<BoxCollider2D>().enabled = true;
            ani.SetBool("idle", true);
        }

        //受伤后重返idle
        if (isHurt && Math.Abs(rib.velocity.x) < 0.1f)
        {
            Debug.Log("wtm");
            ani.SetBool("idle", true);
            ani.SetBool("hurt", false);
            ani.SetFloat("running", 0);
            isHurt = false;
        }

        

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
                ani.SetBool("fall", false);
                rib.velocity = new Vector2(rib.velocity.x, jumpForce);
                //jumpping = true;
                //Destroy(collision.gameObject);
                collision.gameObject.GetComponent<enemy>().deathAnimation();
            }
            else
            {
                if (collision.gameObject.transform.position.x > gameObject.transform.position.x)
                {
                    isHurt = true;
                    rib.velocity = new Vector2(-5, rib.velocity.y);
                    ani.SetBool("hurt", true);
                    ani.SetBool("idle", false);
                }
                else
                {
                    isHurt = true;
                    rib.velocity = new Vector2(5, rib.velocity.y);
                    ani.SetBool("hurt", true);
                    ani.SetBool("idle", false);
                }
                bloodState();
            }
        }

    }

    private void bloodState()
    {
        Debug.Log("我被攻击了");
        sumBlood = sumBlood - 40;
        if (sumBlood < 0)
        {
            sumBlood = 0;
            Destroy(gameObject);
        }
        healthState.GetComponent<RectTransform>().sizeDelta = new Vector2(sumBlood, 20);

    }

}
