using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour {

    public float DeathTimer = 4f;
    public float LightFadeOutTimer = 1f; //how many seconds before the destruction of the object

    private Light pointLight;

	// Use this for initialization
	void Start () {
        pointLight = GetComponentInChildren<Light>(true);
        Destroy(gameObject, DeathTimer);

        if (pointLight)
        { StartCoroutine(FadeOutLight(DeathTimer - LightFadeOutTimer)); }
    }

    
    public IEnumerator FadeOutLight(float delay)
    {
        yield return new WaitForSeconds(delay);
        float currentTime = 0f;
        float initialIntensity = pointLight.intensity;

        while (currentTime < LightFadeOutTimer)
        {
            currentTime += Time.deltaTime;
            pointLight.intensity = Mathf.Lerp(initialIntensity, 0f, currentTime / LightFadeOutTimer);
            yield return null;
        }
        yield break;
    }
}
