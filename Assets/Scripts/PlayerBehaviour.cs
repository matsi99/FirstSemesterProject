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



    //Healthbar

    public Image[] Hearts;

    private bool attacking = false;
    private float horizontalInput;
    private float verticalInput;
    private bool attackInput;
    public static long Highscore;
    public float attackCooldown; //Mindestzeitabstand zwischen 2 Attacken; verhindert spammen
    private float letzteAttacke; //Zeit der letzten Attacke
    public float zeitGefressen; //Zeit die der Spieler gefressen verbringt
    private string state;

    private List<Enemy> enemiesInRange;
    //private List<TeddyBehaviour> teddiesInRange;
    private List<Enemy> currentAttackList;

    private Animator animator;
    private Vector3 defaultScale;

    private void Awake() {
        enemiesInRange = new List<Enemy>();
        currentAttackList = new List<Enemy>();
        //teddiesInRange = new List<TeddyBehaviour>();
        Highscore = 0;
        animator = GetComponent<Animator>();
        defaultScale = transform.localScale;
    }

    private void Update() {
        getInput();
        cameraFollow();
        if (state != "gefressen") {
            updateHealthText();
            updateHighscoreText();
            checkForAttack();
        }
        updateHealthbar();
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

    private void updateHealthbar() {
        for (int i = 0; i < HealthPoints; i++) {
            Hearts[i].enabled = true;
        }
        for (int i = HealthPoints; i < 10; i++) {
            Hearts[i].enabled = false;
        }

    }

    private void checkForAttack() {
        if (attackCooldown < Time.time - letzteAttacke) {
            attacking = false;
        }

        if (attackInput && attacking == false) {
            attacking = true;
            animator.Play("attack", -1, 0f);
            letzteAttacke = Time.time;

            Enemy[] enemies = new Enemy[enemiesInRange.Count];
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

    private void attack(Enemy enemy) {
        Vector2 direction = enemy.direction;
        enemy.RemoveHealth(1);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * -AttackForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate() {
        //move();
        if (state != "gefressen") {
            moveGleichschnell();
            //transform.rotation = new Quaternion(0, 0, 0, 0);
        }
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

        // -> ^
        if (verticalInput > 0.1) {
            transform.localScale = new Vector3(defaultScale.x, -defaultScale.y, defaultScale.z);
        }else if (verticalInput < -0.1) {
            transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy" && state != "gefressen") {
            wirdWeggestoßen(collision);
        }
        else if (collision.gameObject.tag == "Teddy" && state != "gefressen") {
            //Debug.Log("Teddy collision    " + Time.time);
            StartCoroutine(wirdGefressen(collision));
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

    //Spieler wird gefressen
    IEnumerator wirdGefressen(Collision2D collision) {
        state = "gefressen";

        TeddyBehaviour teddy = collision.gameObject.GetComponent<TeddyBehaviour>();
        Vector2 direction = teddy.direction;
        teddy.PlayAttackAnimation();

        SpriteRenderer sr;
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        yield return new WaitForSeconds(zeitGefressen);
        sr.enabled = true;
        //Ausspucken
        

        Rigidbody2D rb;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 50, ForceMode2D.Impulse);

        state = "";

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
        if (collision.tag == "Enemy" || collision.tag == "Teddy") {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemiesInRange.Add(enemy);
            if (attacking) {
                attack(enemy);
            }
        }
        else if (collision.tag == "Heart") {
            HealthPoints += 1;
            if (HealthPoints > 10) {  //Healthpoints auf maximal 10 begrenzen
                HealthPoints = 10;
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy" || collision.tag == "Teddy") {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemiesInRange.Contains(enemy)) {
                enemiesInRange.Remove(enemy);
            }
        }
    }
}
