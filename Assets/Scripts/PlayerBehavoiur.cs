using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavoiur : MonoBehaviour {

    public float MovementSpeed;
    public float HungerFrequency;
    public Text SaturationText;

    private float horizontalInput;
    private float verticalInput;

    private const int MAX_SATURATION = 100;
    private int saturation = MAX_SATURATION;
    private float hungerTime = 0;

    private void Update() {
        getInput();
        updateSaturation();
    }

    private void getInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void updateSaturation() {
        if (verticalInput != 0 || horizontalInput != 0) {
            hungerTime += Time.deltaTime * 2;
        }
        else {
            hungerTime += Time.deltaTime;
        }


        if (hungerTime >= HungerFrequency) {
            hungerTime = 0;
            saturation--;

            updateSaturationText();
        }
    }

    private void updateSaturationText() {
        if (saturation <= 0) {
            saturation = 0;
            Destroy(gameObject);
            SaturationText.text = "Tod";
        }
        else {
            SaturationText.text = "Sättigung: " + saturation + "/" + MAX_SATURATION;
        }
    }

    private void FixedUpdate() {
        move();
    }

    private void move() {
        float xMovement = horizontalInput * Time.deltaTime * MovementSpeed;
        float yMovement = verticalInput * Time.deltaTime * MovementSpeed;

        transform.Translate(new Vector3(xMovement, yMovement, transform.position.z));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Food") {
            Destroy(collision.gameObject);
            saturation += 8;
            if (saturation > MAX_SATURATION) {
                saturation = MAX_SATURATION;
            }
            updateSaturationText();
        }
    }

}
