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


        // Use this for initialization
        protected void Start() {
            var p = FindObjectOfType<PlayerBehaviour>();
            Player = p.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        protected void Update() {
            getDirection();
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
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        //Bewegt Enemy
        protected void move() {
            transform.Translate(direction * MovementSpeed * Time.deltaTime);
        }

        public virtual void RemoveHealth(int value) {
            HealthPoints -= value;
            if (HealthPoints <= 0) {
                HealthPoints = 0;
                gameObject.SetActive(false);
            }
        }
}
