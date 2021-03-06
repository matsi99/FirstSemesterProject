﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Enemy {

    public float AttackForce;
    public GameObject Heart;


    public float aggressionsDistance; //Abstand, ab dem Enemy eine Attacke startet
    public float auszeit; //Mindestzeitraum zwischen zwei Angriffen
    private float letzteAttacke; //Zeit der letzten Attacke

    private Animator animator;


    private new void Start() {
        base.Start();
        letzteAttacke = Time.time;
        animator = GetComponent<Animator>();
    }

    private new void FixedUpdate() {
        if (distance <= aggressionsDistance && auszeit + 1 < Time.time - letzteAttacke) {
            attack();
            lockRotation = true;
        }
        else if (distance > aggressionsDistance && auszeit < Time.time - letzteAttacke) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            move();
        }

        if(auszeit-0.5f < Time.time - letzteAttacke) {
            lockRotation = false;
        }
        //transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //Attacke
    private void attack() {       
        Rigidbody2D rb; 
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * AttackForce, ForceMode2D.Impulse);
        letzteAttacke = Time.time;
        animator.Play("attack", -1, 0f);
    }

    public override void RemoveHealth(int value) {
        HealthPoints -= value;
        letzteAttacke = Time.time;
        if (HealthPoints <= 0) {
            HealthPoints = 0;
            gameObject.SetActive(false);
            Heart.transform.position = transform.position;
            Instantiate(Heart);
        }
    }


}
