using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    private GameController gameController;
    private GameObject gameControllerObj;
    private IEnumerator coroutine;
    public float secondsDelay = 2f;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
	    spriteRenderer = GetComponent<SpriteRenderer>();
	    coroutine = explosionDelay(secondsDelay);
	    StartCoroutine(coroutine);
	    gameControllerObj = GameObject.Find("Game Controller");
		gameController = gameControllerObj.GetComponent<GameController>();
        if(!gameController.startDelay) {
            gameController.startDelay = true;
        }

	}
	
	// Update is called once per frame
	void Update () {

	}

	private IEnumerator explosionDelay(float secondsDelay) {

        yield return new WaitForSeconds(secondsDelay);
        spriteRenderer.enabled = false;
        StopCoroutine(coroutine);

	}
}
