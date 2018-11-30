using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PopUpTemp : MonoBehaviour {

    private GameObject gameControllerObj;
    private GameController gameController;
    private Text text;

	// Use this for initialization
	void Start () {
		gameControllerObj = GameObject.Find("Game Controller");
		gameController = gameControllerObj.GetComponent<GameController>();
		gameController.destroyMe(gameObject, 2.5f);
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.up * Time.deltaTime / 3);
        text.color =  new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.007f);
	}
}
