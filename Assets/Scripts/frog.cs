using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : MonoBehaviour
{

    private Animator ani;


    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void death()
    {
        Destroy(gameObject);
    }

    public void deathAnimation()
    {
        ani.SetTrigger("death");

    }
}
