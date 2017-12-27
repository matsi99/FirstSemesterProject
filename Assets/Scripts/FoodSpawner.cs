using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    public GameObject FoodPrefab;
    public float SpawnFrequency;

    private float currentTime = 0;
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;

        if(currentTime > SpawnFrequency) {
            currentTime = 0;
            spawnFood();
        }
	}

    private void spawnFood() {

        float x = Random.Range(-5.95f, 5.95f);
        float y = Random.Range(-4.3f, 4.3f);

        FoodPrefab.transform.position = new Vector3(x, y, 0);
        Instantiate(FoodPrefab);
    }
}
