using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour {


    public EnemyBehaviour enemy;
    public float minSpawnTime;
    public float maxSpawnTime;

    private float refTime;

	// Use this for initialization
	void Start () {
        resetTimer();
	}
	
	// Update is called once per frame
	void Update () {
        if (refTime < Time.time)
        {
            resetTimer();
            float x = Random.Range(-19f, 19f);
            float y = Random.Range(-9f, 9f);
            enemy.transform.position = new Vector3(x, y, 0);
            Instantiate(enemy);
        }  
	}

    private void resetTimer() {
        refTime = Time.time + Random.Range(minSpawnTime,maxSpawnTime);
    }
}
