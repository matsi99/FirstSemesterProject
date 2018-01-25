using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBehaviour : Enemy {

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    //private void attack ()
    //{
    //    Player.GetComponent<Renderer>().enabled = false;
        
    //}

    public void PlayAttackAnimation() {
        animator.Play("attack", -1, 0f);
    }

}
