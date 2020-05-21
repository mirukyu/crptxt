using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PathFollowingMode
{
    BackAndForth,
    Loop
}

public class PathFollower : MonoBehaviour
{
    [SerializeField] private PathFollowingMode mode = PathFollowingMode.BackAndForth;
    [SerializeField] private Transform path = null;

    private int targetCheckpointIndex = -1;
    private Transform targetCheckpoint = null;

    private int step = 1;
    private float speed = 0.5f;

    [SerializeField] private float checkpointTolerance = 2f;

	// Use this for initialization
	void Start ()
    {
        if (path != null && path .childCount > 0)
        {
            targetCheckpointIndex = 0;
            targetCheckpoint = path.GetChild(0);
        }

        path.parent = null;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (TraversalManager.MoveAllowed && targetCheckpoint != null)
        {
            if (Vector3.Distance(transform.position, targetCheckpoint.position) <= checkpointTolerance)
            {
                targetCheckpointIndex += (1 * step);
                if (targetCheckpointIndex == -1)
                { targetCheckpointIndex = 1; step *= -1; }

                if (targetCheckpointIndex < path.childCount)
                    targetCheckpoint = path.GetChild(targetCheckpointIndex);

                else // targetCheckpointIndex >= path.childCount
                {
                    if (mode == PathFollowingMode.BackAndForth)
                    {
                        step *= -1;
                        targetCheckpointIndex -= 1;
                        targetCheckpoint = path.GetChild(targetCheckpointIndex);
                    }
                    else // mode == PathFollowingMode.Loop
                    {
                        targetCheckpointIndex = 0;
                        targetCheckpoint = path.GetChild(0);
                    }
                }
            }

            //Vector3 smoothing = Vector3.Lerp(transform.position, targetCheckpoint.position, speed);
            //transform.position = smoothing;

            transform.position = Vector3.MoveTowards(transform.position, targetCheckpoint.position, speed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(targetCheckpoint.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
	}
}
