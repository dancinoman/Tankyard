using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletIntegrity : MonoBehaviour {

    private GameObject leftBound;
    private GameObject rightBound;
    private GameObject bottomBound;
    private GameController gameController;
    private PolygonCollider2D polyCol2D;


	// Use this for initialization
	void Start () {
	    leftBound = GameObject.Find("Left Bound");
	    rightBound = GameObject.Find("Right Bound");
	    bottomBound = GameObject.Find("Bottom Bound");
		GameObject gameControllerObj = GameObject.Find("Game Controller");
        gameController = gameControllerObj.GetComponent<GameController>();
        polyCol2D = GetComponent<PolygonCollider2D>();
        polyCol2D.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x < leftBound.transform.position.x || transform.position.x > rightBound.transform.position.x || transform.position.y < bottomBound.transform.position.y) {
            gameController.startDelay = true;
            gameController.destroyMe(gameObject, 0f);
		}
	}
}
