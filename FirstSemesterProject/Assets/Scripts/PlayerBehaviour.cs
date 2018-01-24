using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    public float MovementSpeed;
    public Camera Cam;
    public float RecoilForce;
    public float AttackForce;
    public int HealthPoints;
    public Text HealthText;
    public Text HighscoreText;

    private float horizontalInput;
    private float verticalInput;
    private bool attackInput;
    public static long Highscore;

    private List<EnemyBehaviour> enemiesInRange;

    private void Awake() {
        enemiesInRange = new List<EnemyBehaviour>();
        Highscore = 0;
    }

    private void Update() {
        getInput();
        cameraFollow();
        updateHealthText();
        updateHighscoreText();
        checkForAttack();
    }

    private void getInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        attackInput = Input.GetKeyDown(KeyCode.Space);
    }

    private void cameraFollow() {
        Cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Cam.transform.position.z);
    }

    private void updateHealthText() {
        string text = "";
        for (int i = 0; i < HealthPoints; i++) {
            text += i % 2 == 0 ? "<" : "3";
        }
        HealthText.text = text;
    }

    private void checkForAttack() {
        if (attackInput) {
            EnemyBehaviour[] enemies = new EnemyBehaviour[enemiesInRange.Count];
            enemiesInRange.CopyTo(enemies);
            foreach (var enemy in enemies) {
                    if (enemy == null) {
                        enemiesInRange.Remove(enemy);
                        continue;
                    }

                    attack(enemy);

                    if (enemy.isActiveAndEnabled == false) {
                        Highscore += (int)Time.time * 10;
                        enemiesInRange.Remove(enemy);
                        Destroy(enemy.gameObject);
                    }
            }
        }
    }

    private void updateHighscoreText() {
        HighscoreText.text = string.Format("Highscore: {0:n0}", Highscore);
    }

    private void attack(EnemyBehaviour enemy) {
        Vector2 direction = enemy.direction;
        enemy.RemoveHealth(1);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * -AttackForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate() {
        //move();
        moveGleichschnell();
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //Player bewegt sich diagonal schneller
    private void move() {
        float xMovement = horizontalInput * Time.deltaTime * MovementSpeed;
        float yMovement = verticalInput * Time.deltaTime * MovementSpeed;

        transform.Translate(new Vector3(xMovement, yMovement, transform.position.z));
    }

    //Player bewegt sich auch diagonal mit der gleichen Geschwindigkeit
    private void moveGleichschnell() {

        float speed = MovementSpeed;

        var rb = GetComponent<Rigidbody2D>();
        if (rb.velocity.magnitude > 0.1) {
            speed = MovementSpeed / 2;
        }

        //Richtungsvektor erstellen
        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        //Vektor normieren (auf Länge 1 bringen)
        direction.Normalize();

        transform.Translate(direction * Time.deltaTime * speed);
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            wirdWeggestoßen(collision);
        }
    }

    //Spieler wird weggestoßen
    private void wirdWeggestoßen(Collision2D collision) {
        EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
        Vector2 direction = enemy.direction;

        Rigidbody2D rb;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * RecoilForce, ForceMode2D.Impulse);

        RemoveHealth(1);
    }

    private void RemoveHealth(int value) {
        HealthPoints -= value;
        if (HealthPoints <= 0) {
            HealthPoints = 0;
            SceneManager.LoadScene("GameOver");
        }
        updateHealthText();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            var enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            enemiesInRange.Add(enemy);
        }
        else if (collision.tag == "Heart") {
            HealthPoints += 1;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            var enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            if (enemiesInRange.Contains(enemy)) {
                enemiesInRange.Remove(enemy);
            }
        }
    }
}
