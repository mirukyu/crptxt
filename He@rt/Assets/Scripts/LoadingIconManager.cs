using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIconManager : MonoBehaviour {

    private float MovementDuration = 0.5f;
    private float maxTravelDistance = 20f;
    private float minTravelDistance = 0f;

    private Vector2 startPosition;
    private float currentDistance = 0f;
    private float currentTime = 0f;
    private float speed;

    private void Start()
    {
        startPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    private void FixedUpdate()
    {
        Vector2 position = GetComponent<RectTransform>().anchoredPosition;
        position.y = startPosition.y + currentDistance;
        GetComponent<RectTransform>().anchoredPosition = position;

        if (currentTime < MovementDuration)
        {
            currentTime += Time.deltaTime;
            currentDistance = Mathf.Lerp(minTravelDistance, maxTravelDistance, currentTime / MovementDuration);
        }
        else
        {
            float tmp = minTravelDistance;
            minTravelDistance = maxTravelDistance;
            maxTravelDistance = tmp;
            currentTime = 0f;
        }
    }
}
