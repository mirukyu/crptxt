using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivoting : MonoBehaviour {

	[SerializeField] private Transform pivot = null;

    private float rotationSpeed = 15f;
    private float InitialCountdown = 20f;
    private float Countdown = 0f;

    private float r;
    private float x;
    private float y;
    private float z;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(pivot.position, Vector3.up, rotationSpeed * Time.deltaTime);

        if (Countdown > 0)
        { Countdown -= Time.deltaTime; }
        else
        {
            Countdown = InitialCountdown;

            r = Random.Range(7.5f, 10f);
            y = Random.Range(1f, 5f);

            x = Random.Range(0f, Mathf.Sqrt(r * r - y * y));
            z = Mathf.Sqrt(r*r - x*x - y*y);

            transform.position = new Vector3((Random.Range(0,2)*2 -1) * x, y, (Random.Range(0, 2) * 2 - 1) * z);
            transform.LookAt(pivot);

            rotationSpeed = Random.Range(-20f, 20f);
        }
    }
}
