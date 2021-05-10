using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour {
    private AISpawner AIManager;

    private bool hasTarget = false;
    private bool isTurning;

    private Vector3 waypoint;
    private Vector3 lastWaypoint = new Vector3(0,0,0);

    private Animator animator;
    private float speed;

    // Use this for initialization
    void Start () {
        AIManager = transform.parent.GetComponentInParent<AISpawner>();
        animator = GetComponentInChildren<Animator>();

        SetUp();
    }
	public void SetUp()
    {
        float scale = Random.Range(0f, 0.25f);
        transform.localScale += new Vector3(scale, scale, scale);
    }
	// Update is called once per frame
	void Update () {
        if (!hasTarget)
        {
            hasTarget = CanFindTarget();
        }
        else
        {
            RotateNPC(waypoint, speed);
            transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
        }
        if(transform.position == waypoint)
        {
            hasTarget = false;
        }
	}

    bool CanFindTarget()
    {
        waypoint = AIManager.RandomWayPoint();
        if (lastWaypoint == waypoint)
        {
            waypoint = AIManager.RandomWayPoint();
            return false;
        }
        else{
            lastWaypoint = waypoint;
            speed = Random.Range(1, 7);
            return true;
        }
    }

    void RotateNPC(Vector3 waypoint, float currentSpeed)
    {
        float turnSpeed = currentSpeed * Random.Range(1f, 3f);
        Vector3 lookAt = waypoint - this.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), turnSpeed * Time.deltaTime);
    }
}
