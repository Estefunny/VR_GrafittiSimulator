using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayTest : MonoBehaviour {

    public Sprayer sprayer;
    private float movementSpeed = 5;
    private Color[] colors = new Color [] { Color.blue, Color.red, Color.yellow, Color.clear };
    private int colorIndex = 0;

    private void Update() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.up * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.position += Vector3.down * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.PageUp)) {
            transform.position += Vector3.back * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.PageDown)) {
            transform.position += Vector3.forward * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            colorIndex = (colorIndex + 1) % colors.Length;
            print("Color Selected: " + colors[colorIndex]);
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            SprayTarget.undoStep();
        }
    }

    private void FixedUpdate() {
        sprayer.setSprayColor(colors[colorIndex]);
        sprayer.sprayerUpdate(transform.position + Vector3.forward * 0.5f, Vector3.forward, Input.GetKey(KeyCode.Space) ? 1 : 0);
    }

}
