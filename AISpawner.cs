using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AIObject
{
    public string AIGroupName { get { return m_aiGroupName; } }
    public GameObject objectPrefab { get { return m_prefab; } }
    public int maxAI { get { return m_maxAI; } }


    [Header("AI Group Stats")]
    [SerializeField]
    private string m_aiGroupName;
    [SerializeField]
    private GameObject m_prefab;
    [SerializeField]
    [Range(0f,20f)]
    private int m_maxAI;

}
public class AISpawner : MonoBehaviour {

    public List<Transform> Waypoints = new List<Transform>();
    [Header("AI Groups Settings")]
    public AIObject[] AIObject = new AIObject[6];
    public Vector3 spawnArea { get { return m_spawnArea; } }

    [SerializeField]
    private Vector3 m_spawnArea = new Vector3(20f,10f,20f);

    // Use this for initialization
    void Start () {
        GetWayPoints();
        for(int i= 0; i<25; i++)
        {
            CreateAIObject(0);
        }
        InvokeRepeating("CreateAIObject0", 1f, 1f);
        InvokeRepeating("DestroyAIObject0", 1f, 1f);
    }

    public GameObject FindRandomTarget()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("clone");
        int rand = Random.Range(0, gos.Length);
        return gos[rand];
    }

    void GetWayPoints() {
        Transform[] wplist = this.transform.GetComponentsInChildren<Transform>();
        for(int i=0; i < wplist.Length; i++)
        {
            if(wplist[i].tag == "waypoint")
            {
                Waypoints.Add(wplist[i]);
            }
        }
     }

    public Vector3 RandomPosition()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z));
        randomPosition = transform.TransformPoint(randomPosition * .5f);
        return randomPosition;
    } 


    void CreateAIObject(int i)
    {
        Quaternion randomRotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(0, 360), 0);
        GameObject newObject = Instantiate(AIObject[i].objectPrefab, RandomPosition(), randomRotation);
        newObject.transform.parent = this.transform;
        newObject.AddComponent<AIMove>();
    }

    public Vector3 RandomWayPoint()
    {
        int randomWP = Random.Range(0, (Waypoints.Count - 1));
        Vector3 randomWayPoint = Waypoints[randomWP].transform.position;
        return randomWayPoint;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CreateAIObject(0);
        }
	}
    public void CreateAIObject0()
    {
        CreateAIObject(0);
    }
    public void DestroyAIObject0()
    {
        GameObject rgo = FindRandomTarget();
        if (rgo != null)
        {
            StartCoroutine(ScaleToTargetCoroutine(Vector3.zero, 0.5f, rgo));
        }
    }

    private IEnumerator ScaleToTargetCoroutine(Vector3 targetScale, float duration, GameObject rgo)
    {
        Vector3 startScale = transform.localScale;
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            //smoother step algorithm
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            rgo.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        yield return null;
        Destroy(rgo);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, spawnArea);
    }
}
