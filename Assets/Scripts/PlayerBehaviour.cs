using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

    public float MovementSpeed;
    public Camera Cam;

    private float horizontalInput;
    private float verticalInput;

    private void Update() {
        getInput();
        cameraFollow();
    }

    private void getInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void cameraFollow() {
        Cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Cam.transform.position.z);
    }

    private void FixedUpdate() {
        move();
    }

    private void move() {
        float xMovement = horizontalInput * Time.deltaTime * MovementSpeed;
        float yMovement = verticalInput * Time.deltaTime * MovementSpeed;

        transform.Translate(new Vector3(xMovement, yMovement, transform.position.z));
    }

}
