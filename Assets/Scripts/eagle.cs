using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eagle : enemy
{
    public float speed;

    private Transform tra;
    private float topY;
    private float bottomY;
    private bool faceDown;

    private Rigidbody2D rib;
    private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        rib = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        tra = gameObject.transform;
        topY = tra.position.y + 3;
        bottomY = tra.position.y - 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(tra.position.y > topY)
        {
            faceDown = true;
        }else if(tra.position.y < bottomY)
        {
            faceDown = false;
        }
        if (faceDown)
        {
            rib.velocity = new Vector2(rib.velocity.x, -speed * Time.deltaTime);
        }
        else
        {
            rib.velocity = new Vector2(rib.velocity.x, speed * Time.deltaTime);
        }
    }

}
