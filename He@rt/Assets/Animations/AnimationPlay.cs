using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlay : MonoBehaviour {

    private Animator animator = null;
    private float curretnTime = 0f;
    private float nextPlay = 0f;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        curretnTime += Time.deltaTime;

        if (curretnTime > nextPlay)
        {
            animator.Play("HeadTurn");
            nextPlay = Random.Range(4.5f, 10f);
            curretnTime = 0f;
        }
    }
}
