using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayTest : MonoBehaviour {

    public Sprayer sprayer;
    private float movementSpeed = 5;
    private Color[] colors = new Color [] { Color.blue, Color.red, Color.yellow, Color.clear };
    private int colorIndex = 0;
    private float[] capRadi = new float[] { 1, 1.4f, 0.8f };
    private float[] capDist = new float[] { 1, 0.8f, 1.4f };
    private float[] capStrength = new float[] { 1, 0.4f, 1 };
    private int capIndex = 0;

    private void Start() {
        sprayer.setSprayColor(colors[colorIndex]);
    }

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
            sprayer.setSprayColor(colors[colorIndex]);
            print("Color Selected: " + colors[colorIndex]);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            capIndex = (capIndex + 1) % capDist.Length;
            sprayer.setMultipliers(capDist[capIndex], capRadi[capIndex]);
            print("Cap Selected: Distance " + capDist[capIndex] + ", Radius " + capRadi[capIndex]);
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            SprayTarget.undoStep();
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            SprayTarget.clear();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            FindObjectOfType<SprayTarget>().gameObject.AddComponent<SprayDrip>().initialize(new Vector2(0.5f, 0.5f), FindObjectOfType<SprayTarget>(), colors[colorIndex]);
        }
    }

    private void FixedUpdate() {
        sprayer.sprayerUpdate(transform.position + Vector3.forward * 0.5f, Vector3.forward, capStrength[capIndex] * (Input.GetKey(KeyCode.Space) ? 1 : 0));
    }

}
