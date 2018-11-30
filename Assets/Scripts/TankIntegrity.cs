using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TankIntegrity : MonoBehaviour {

    private int hullPoints = 100;
    public int playerId;

    //GUI info
    private Text hullMeter;
    private GameObject useHullMeter;

    // Other memebers
    private GameController gameController;
    private GameObject GUI;
    private GameObject gameControllerObj;
    private GameObject explosion120mm;
    private GameObject dammageTextObj;
    private Text dammageText;
    private AudioSource audio;
    private IEnumerator coroutine;
    public float waitSeconds;


	// Use this for initialization
	void Start () {

	    coroutine = waitBeforePlay(waitSeconds);
	    StartCoroutine(coroutine);
	    audio = GetComponent<AudioSource>();
	    GameObject GUI = GameObject.Find("GUI");
	    gameControllerObj = GameObject.Find("Game Controller");
	    gameController = gameControllerObj.GetComponent<GameController>();
	    setHullGui(playerId, GUI);
		hullMeter.text = hullPoints.ToString();
	}
	
	// Update is called once per frame
	void Update () {


	}

	public void setHullGui(int playerId, GameObject GUI) {
	    GameObject playerInfo = GUI.transform.Find("Player Infos "+ playerId).gameObject;
        GameObject HullInfo = playerInfo.transform.Find("Hull Info "+ playerId).gameObject;
        useHullMeter = HullInfo.transform.Find("Hull Meter " + playerId).gameObject;

        hullMeter = useHullMeter.GetComponent<Text>();
	}

	void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Ammo") {
            Destroy(other.gameObject);
            Vector3 otherPosition = other.gameObject.transform.position;
            explosion120mm = Instantiate(Resources.Load("Prefabs/Explosion_120mm", typeof(GameObject)), new Vector3(otherPosition.x, otherPosition.y, 0f), Quaternion.identity) as GameObject;
            applyDamage(gameController.ammo120mmDamage);

        }
    }

    public void applyDamage(int ammount) {
        GameObject GUI = GameObject.Find("GUI");
        dammageTextObj = Instantiate(Resources.Load("Prefabs/Damage_Text", typeof(GameObject)), new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity) as GameObject;
        dammageTextObj.transform.SetParent(GUI.transform, false);
        dammageTextObj.transform.position = new Vector3(transform.position.x, transform.position.y +0.5f, 0f);
        dammageText = dammageTextObj.GetComponent<Text>();

        AudioClip explosionTank = Resources.Load<AudioClip>("Audio/TankExplosion");
        audio.clip = explosionTank;
        audio.Play();
        hullPoints -= ammount;
        dammageText.text = "-" + ammount.ToString();
        hullMeter.text = hullPoints.ToString();

        // Reset the wind
        gameController.setRandomWind(true);

        if(hullPoints <= 0) {
            gameController.gameOver(playerId);
        }
    }

    private IEnumerator waitBeforePlay(float waitSeconds) {
        yield return new WaitForSeconds(waitSeconds);
        Instantiate(Resources.Load("Prefabs/Pin_Point_Platform", typeof(GameObject)), new Vector2(transform.position.x, transform.position.y -0.5f), Quaternion.identity);
        GetComponent<Rigidbody2D>().gravityScale = 0.25f;
    }
}
