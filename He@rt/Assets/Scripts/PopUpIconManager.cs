using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpIconManager : MonoBehaviour {

    private Transform Camera;

    private float DestructionTimer = 3.5f;
    private float FadeDuration = 2f;
    private float initialSpeed = 0.25f;

    private float currentTime = 0f;
    private float speed;
    float alpha = 1f;

    private Color myColor = new Color(0.90f, 0.00f, 0.30f, 1f);

    private void Start()
    {
        Camera = GameObject.Find("Main Camera").transform;

        transform.LookAt(Camera);
        transform.transform.eulerAngles = new Vector3(transform.eulerAngles.x + Random.Range(-15f, 15f), transform.eulerAngles.y + Random.Range(-5f, 5f), transform.eulerAngles.z + Random.Range(-10f, 10f));
        speed = initialSpeed;

        StartCoroutine(SpeedAndColorVariation());
        Destroy(gameObject, DestructionTimer);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);

        GetComponent<TextMesh>().color = new Color(myColor.r, myColor.g, myColor.b, alpha);
    }

    public void SetTextAndColor(int number, Color colour)
    {
        GetComponent<TextMesh>().text = number.ToString();
        myColor = colour;
    }

    private IEnumerator SpeedAndColorVariation()
    {
        yield return new WaitForSeconds(DestructionTimer - FadeDuration);

        while (currentTime < FadeDuration)
        {
            currentTime += Time.deltaTime;
            speed = Mathf.Lerp(initialSpeed, 0f, currentTime / FadeDuration);
            alpha = Mathf.Lerp(1f, 0f, currentTime / FadeDuration);
            yield return null;
        }
        yield break;
    }
}
