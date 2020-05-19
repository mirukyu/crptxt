using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class player : MonoBehaviour {
    Rigidbody rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!DiveIntoHeccManager.StopPlaying)
        {
            Time.timeScale += Time.fixedDeltaTime * 0.01f;
            transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * 7f);
            rigid.velocity += transform.rotation * (Input.GetAxisRaw("Horizontal") * Vector3.right * 10f) * Time.deltaTime;
            rigid.velocity += transform.rotation * (Input.GetAxisRaw("Vertical") * Vector3.up * 10f) * Time.deltaTime;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(rigid);
        StartCoroutine(GameObject.Find("Manager").GetComponent<DiveIntoHeccManager>().Died());
    }
}
