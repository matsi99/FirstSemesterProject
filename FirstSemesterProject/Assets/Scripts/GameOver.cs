using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text ReplayInfo;
    public Text Highscore;

	void Start () {
        ReplayInfo.enabled = false;
        Highscore.text = string.Format("Highscore: {0:n0}", PlayerBehaviour.Highscore); 
        StartCoroutine(wait());
	}
	
	void Update () {
        if (ReplayInfo.enabled && Input.anyKeyDown) {
            SceneManager.LoadScene("Main");
        }
	}

    IEnumerator wait() {
        yield return new WaitForSeconds(2f);
        ReplayInfo.enabled = true;
    }

}
