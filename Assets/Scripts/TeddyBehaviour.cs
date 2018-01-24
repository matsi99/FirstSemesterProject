using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBehaviour : MonoBehaviour {

    public float MovementSpeed;
    public float HealthPoints;
    public Vector2 direction;
    public float distance; //Abstand zwischen Enemy und Player
    private Rigidbody2D Player;


    // Use this for initialization
    void Start () {
        var p = FindObjectOfType<PlayerBehaviour>();
        Player = p.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        getDirection();
	}

    //Errechnet Richtung, in der der Spieler vom Enemy aus sich befindet
    public void getDirection()
    {
        float xPlayerPosition = Player.position.x;
        float yPlayerPosition = Player.position.y;

        direction = new Vector2(xPlayerPosition - this.transform.position.x, yPlayerPosition - this.transform.position.y);

        //distance = direction.magnitude;

        //Vektor normieren
        direction.Normalize();
    }


    private void FixedUpdate()
    {
        move();
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //Bewegt Enemy
    private void move()
    {
        transform.Translate(direction * MovementSpeed * Time.deltaTime);
    }

    private void attack ()
    {
        Player.GetComponent<Renderer>().enabled = false;
    }

}
