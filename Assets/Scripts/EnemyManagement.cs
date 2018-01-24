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
            Instantiate(enemy);
        }  
	}

    private void resetTimer() {
        refTime = Time.time + Random.Range(minSpawnTime,maxSpawnTime);
    }
}
