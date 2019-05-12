using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGeneration : MonoBehaviour {

    [Header("Настройка области")]
    public float spawnRadius;
    public float exceptionRadius;

    [Header("Параметры спавна")]
    public float contactRadius;
    public int treeValue;
    private int curTreeValue = 0;

    [Header("Объекты спавна")]
    public GameObject[] tree;

    int stopValueStep = 20;
    int curValueStep = 0;

    // Use this for initialization
    void Start () {
        SpawnTrees();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnTrees()
    {
        for(curValueStep = 0; curValueStep < stopValueStep &&  curTreeValue < treeValue; curValueStep++)
        {
            Vector3 offsetPos;
            int randVec = Random.Range(0, 4);
            if (randVec == 0) offsetPos = new Vector3(Random.Range(exceptionRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
            else if(randVec == 1) offsetPos = new Vector3(Random.Range(-spawnRadius, -exceptionRadius), 0, Random.Range(-spawnRadius, spawnRadius));
            else if(randVec == 2) offsetPos =  new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(exceptionRadius, spawnRadius));
            else offsetPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, -exceptionRadius));
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position + offsetPos, contactRadius);
            if (hitColliders.Length > 0) continue;
            else
            {
                Instantiate(tree[Random.Range(0, tree.Length)], gameObject.transform.position + offsetPos, Quaternion.AngleAxis(Random.Range(0,360),Vector3.up));
                curTreeValue++;
                curValueStep = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position, new Vector3(spawnRadius,spawnRadius,spawnRadius) * 2);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(exceptionRadius, exceptionRadius, exceptionRadius) * 2);
    }
}
