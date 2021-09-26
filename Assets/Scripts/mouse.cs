using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : enemy
{
    private Rigidbody2D rib;
    private Collider2D coll;
    public GameObject player;
    private bool move;
    public float speed;

    void Start()
    {
        
        rib = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<Collider2D>();
        ani = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (transform.position.x - player.transform.position.x < 13)
            move = true;
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            rib.velocity = new Vector2(-speed * Time.fixedDeltaTime, rib.velocity.y);
        }
    }
}
