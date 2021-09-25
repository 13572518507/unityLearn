using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    protected Animator ani;
    public void deathAnimation()
    {
        ani.SetTrigger("death");
    }

    private void death()
    {
        Destroy(gameObject);
    }
}
