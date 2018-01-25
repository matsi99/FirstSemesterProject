using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float MovementSpeed;
    public float HealthPoints;
    public Vector2 direction;
    public float distance; //Abstand zwischen Enemy und Player
    protected Rigidbody2D Player;
    private float zRotation = 0;
    protected bool lockRotation = false;


    // Use this for initialization
    protected void Start() {
        var p = FindObjectOfType<PlayerBehaviour>();
        Player = p.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update() {
        getDirection();
        rotateToDirection();
    }

    //Errechnet Richtung, in der der Spieler vom Enemy aus sich befindet
    public void getDirection() {
        float xPlayerPosition = Player.position.x;
        float yPlayerPosition = Player.position.y;

        direction = new Vector2(xPlayerPosition - this.transform.position.x, yPlayerPosition - this.transform.position.y);

        //distance = direction.magnitude;

        //Vektor normieren
        direction.Normalize();
    }


    protected void FixedUpdate() {
        move();
        //transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //Bewegt Enemy
    protected void move() {
        transform.Translate(direction * MovementSpeed * Time.deltaTime, Space.World);
    }

    public virtual void RemoveHealth(int value) {
        HealthPoints -= value;
        if (HealthPoints <= 0) {
            HealthPoints = 0;
            gameObject.SetActive(false);
        }
    }

    private void rotateToDirection() {
        if (lockRotation) {
            return;
        }

        // Rotation -> ^
        float deltaInput = 0.4f;
        float horizontalInput = direction.normalized.x;
        float verticalInput = direction.normalized.y;

        if (horizontalInput > deltaInput && verticalInput > deltaInput) {
            zRotation = 135;
        }
        else if (horizontalInput > deltaInput && verticalInput < -deltaInput) {
            zRotation = 45;
        }
        else if (horizontalInput < -deltaInput && verticalInput > deltaInput) {
            zRotation = 225;
        }
        else if (horizontalInput < -deltaInput && verticalInput < -deltaInput) {
            zRotation = 315;
        }
        else if (horizontalInput > 0 && Mathf.Abs(verticalInput) < deltaInput) {
            zRotation = 90;
        }
        else if (horizontalInput < 0 && Mathf.Abs(verticalInput) < deltaInput) {
            zRotation = 270;
        }
        else if (verticalInput > 0 && Mathf.Abs(horizontalInput) < deltaInput) {
            zRotation = 180;
        }
        else if (verticalInput < 0 && Mathf.Abs(horizontalInput) < deltaInput) {
            zRotation = 0;
        }

        Quaternion currentRotation = transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(0, 0, zRotation);

        transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * 360 * 2);
    }
}
