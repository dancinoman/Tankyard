using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    private GameController gameController;

    public GameObject mainCamera;
    public GameObject background;
    public GameObject prefabTerrainSmallHills;
    public GameObject prefabPlayerTank;
    public GameObject prefabPlayerTank2;
    private GameObject currentPlayerTank;
    private GameObject currentPlayerTank2;
    public GameObject prefabCannonControll;
    public GameObject prefabCannonControll2;
    private GameObject currentCannonControll;
    private GameObject currentCannonControll2;
    public Transform targetCamera;
    private bool loading = true;
    private Vector3 currentPosPlayer1;
    private Vector3 currentPosPlayer2;
    private Vector3 lastPosPlayer1;
    private Vector3 lastPosPlayer2;
    private bool instantiateObject = true;

	// Use this for initialization
	void Start () {
		gameController = this.transform.parent.gameObject.GetComponent<GameController>();
	}

	void FixedUpdate () {
		if(gameController.gameStart == true) {

            if(instantiateObject) {
                Instantiate(prefabTerrainSmallHills, new Vector3(Random.Range(-14.8f, 14.8f),0.5f,0f), Quaternion.identity);
                currentPlayerTank = Instantiate(prefabPlayerTank, new Vector3(-7f, 4.5f,0f), Quaternion.identity);
                currentPlayerTank2 = Instantiate(prefabPlayerTank2, new Vector3(7f, 4.5f, 0f), Quaternion.identity);
                currentCannonControll = Instantiate(prefabCannonControll, new Vector3 (currentPlayerTank.transform.position.x, currentPlayerTank.transform.position.y, 0f), Quaternion.identity);
                currentCannonControll2 = Instantiate(prefabCannonControll2, new Vector3 (currentPlayerTank2.transform.position.x, currentPlayerTank2.transform.position.y, 0f), Quaternion.identity);
                currentCannonControll.transform.parent = currentPlayerTank.transform;
                currentCannonControll2.transform.parent = currentPlayerTank2.transform;
                currentCannonControll.transform.localEulerAngles = new Vector3(0f, 0f, 8);
                currentCannonControll2.transform.localEulerAngles = new Vector3(0f, 0f, -8f);
                instantiateObject = false;
            }
            /*
            if(loading) {
                currentPosPlayer1 = currentPlayerTank.transform.position;
                currentPosPlayer2 = currentPlayerTank2.transform.position;

                if(currentPosPlayer1 == lastPosPlayer1 && currentPosPlayer2 == lastPosPlayer2) {
                    GameObject loadingScreenImage = GameObject.Find("Load Screen");
                    loadingScreenImage.SetActive(false);
                    loading = false;
                }

                lastPosPlayer1 = currentPosPlayer1;
                lastPosPlayer2 = currentPosPlayer2;
            }*/

            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetCamera.position, 5 * Time.deltaTime);

            if(mainCamera.transform.position.y != targetCamera.position.y) {
                background.transform.position = Vector3.MoveTowards(background.transform.position, targetCamera.position, 10 * Time.deltaTime);
            }
        }
    }

}



