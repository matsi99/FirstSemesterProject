using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    public float MovementSpeed;
    public float AttackForce;
    public float HealthPoints;


    public Vector2 direction;
    public float distance; //Abstand zwischen Enemy und Player
    public float aggressionsDistance; //Abstand, ab dem Enemy eine Attacke startet
    public float auszeit; //Mindestzeitraum zwischen zwei Angriffen
    private float letzteAttacke; //Zeit der letzten Attacke
    private Rigidbody2D Player;

    private void Start() {
        letzteAttacke = Time.time;
        var p = FindObjectOfType<PlayerBehaviour>();
        Player = p.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        getDirection();
    }


    //Errechnet Richtung, in der der Spieler vom Enemy aus sich befindet
    public void getDirection() {
        float xPlayerPosition = Player.position.x;
        float yPlayerPosition = Player.position.y;

        direction = new Vector2(xPlayerPosition - this.transform.position.x, yPlayerPosition - this.transform.position.y);

        distance = direction.magnitude;

        //Vektor normieren
        direction.Normalize();
    }



    private void FixedUpdate() {
        if (distance <= aggressionsDistance && auszeit + 1 < Time.time - letzteAttacke) {
            attack();

        }
        else if (distance > aggressionsDistance && auszeit < Time.time - letzteAttacke) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            move();
        }
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //Bewegt Enemy
    private void move() {
        transform.Translate(direction * MovementSpeed * Time.deltaTime);
    }

    //Attacke
    private void attack() {
        Rigidbody2D rb; 
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * AttackForce, ForceMode2D.Impulse);
        letzteAttacke = Time.time;
    }

    public void RemoveHealth(int value) {
        HealthPoints -= value;
        letzteAttacke = Time.time;
        if (HealthPoints <= 0) {
            HealthPoints = 0;
            gameObject.SetActive(false);
        }
    }
}
