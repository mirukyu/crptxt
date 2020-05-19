using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject goSpawn;
    public float fDifficulty = 7.5f;

    float fSpawn = 0;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!DiveIntoHeccManager.StopPlaying)
        {
            fSpawn += fDifficulty * Time.deltaTime;
            fDifficulty += Time.deltaTime * 1.01f;

            while (fSpawn > 0)
            {

                fSpawn -= 1;
                Vector3 v3pos = new Vector3(Random.value * 40f - 20f, 0, Random.value * 40f - 20f) + transform.position;
                Vector3 v3scale = new Vector3(Random.value + 0.1f, 20f, Random.value * 3f);
                GameObject goCreate = Instantiate(goSpawn, v3pos, Quaternion.Euler(0, Random.value * 360f, Random.value * 50f));
                goCreate.transform.localScale = v3scale;

                Destroy(goCreate, 7f);
            }
        }
	}
}
