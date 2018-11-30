using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CannonControll : MonoBehaviour {

    public float cannonSpeed = 30f;
    public int power = 50;

    //GUI info
    private Text angleMeter;
    private Text powerMeter;
    private GameObject GUI;
    private GameObject useAngleMeter;
    private GameObject usePowerMeter;

    // Other members
    public int playerId;
    private AudioSource audio;
    private bool clipPlay = false;
    private GameObject gameControllerObj;
    private GameController gameController;
    public bool isReversed;


    // Prefabs
    private GameObject ammo120mm;
    private GameObject cannonFlame;



	// Use this for initialization
	void Start () {
	    gameControllerObj = GameObject.Find("Game Controller");
	    gameController = gameControllerObj.GetComponent<GameController>();
		audio = GetComponent<AudioSource>();
		//Give Meters value
		getGui(playerId);
        powerMeter.text = (power).ToString();
        angleMeter.text = (Mathf.Ceil(transform.localEulerAngles.z)).ToString();

	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) ||
          (isReversed && (transform.localEulerAngles.z < 259.1 || transform.localEulerAngles.z > 359.3)) ||
          (!isReversed && transform.localEulerAngles.z > 109.1f || transform.localEulerAngles.z < 0.3f)) {
            audio.clip = null;
            clipPlay = false;
        }

        if(cannonFlame != null) {
            Destroy(cannonFlame, 0.09f);
        }
	}

	void FixedUpdate () {


	    if(!gameController.hasShoot && playerId == gameController.playerTurn) {
	        if(isReversed) {

	            if((transform.localEulerAngles.z > 258f && transform.localEulerAngles.z <= 359.3f) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) {
                    moveCannonClockWise(false);
                }

                if((transform.localEulerAngles.z >= 259.1f && transform.localEulerAngles.z < 361f) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ) {
                    moveCannonClockWise(true);
                }

	        } else {
                if((transform.localEulerAngles.z < 109.1f && transform.localEulerAngles.z >= -1f) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) {
                    moveCannonClockWise(false);
                }

                if((transform.localEulerAngles.z <= 112f && transform.localEulerAngles.z > 0.3f) && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ) {
                    moveCannonClockWise(true);
                }

	        }

            if(power < 100 && ((Input.GetKey(KeyCode.W)) || Input.GetKey(KeyCode.UpArrow)))  {
                power ++;
                powerMeter.text = (power).ToString();
            }

            if(power > 0 && ((Input.GetKey(KeyCode.S)) || Input.GetKey(KeyCode.DownArrow)))  {
                power --;
                powerMeter.text = (power).ToString();
            }

            if(Input.GetKeyDown(KeyCode.Space)) {
                ammo120mm = Instantiate(Resources.Load("Prefabs/Ammo_120mm", typeof(GameObject)), new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity) as GameObject;
                cannonFlame = Instantiate(Resources.Load("Prefabs/Flame_Cannon", typeof(GameObject)),new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity) as GameObject;
                ammo120mm.transform.parent = transform;
                ammo120mm.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                cannonFlame.transform.parent = transform;
                cannonFlame.transform.localEulerAngles = new Vector3(0f,0f,0f);
                // Reposition on the cannon end


                Rigidbody2D rbAmmo = ammo120mm.GetComponent<Rigidbody2D>();

                if(isReversed) {
                    //Flip all prefabs
                    Vector3 flip = new Vector3(0f, -180f, 0f);
                    ammo120mm.transform.localEulerAngles = flip;
                    cannonFlame.transform.localEulerAngles = flip;
                    ammo120mm.transform.localPosition = new Vector3(ammo120mm.transform.localPosition.x + -3.5f, ammo120mm.transform.localPosition.y + 0.05f, 0f);
                    cannonFlame.transform.localPosition = new Vector3(cannonFlame.transform.localPosition.x -5.8f, cannonFlame.transform.localPosition.y +0.05f, 0f);
                } else {
                    ammo120mm.transform.localPosition = new Vector3(ammo120mm.transform.localPosition.x + 3.5f, ammo120mm.transform.localPosition.y + 0.05f, 0f);
                    cannonFlame.transform.localPosition = new Vector3(cannonFlame.transform.localPosition.x +5.8f, cannonFlame.transform.localPosition.y +0.05f, 0f);
                }

                rbAmmo.AddForce(ammo120mm.transform.right * power * 2);
                // Against wind force
                rbAmmo.AddForce(Vector3.right * gameController.windAmount * gameController.windDirection);

                // Audio tasks
                audio.Stop();
                audio.pitch = 1;
                AudioClip fireShot = Resources.Load<AudioClip>("Audio/120mmBlow");
                audio.clip = fireShot;
                audio.Play();
                ammo120mm.transform.parent = null;
                gameController.hasShoot = true;
            }
	    }
	}

	public void moveCannonClockWise(bool clockWise) {

	    if(clockWise){
            transform.RotateAround(transform.position, -Vector3.forward, cannonSpeed * Time.deltaTime);
	    } else {
	        transform.RotateAround(transform.position, Vector3.forward, cannonSpeed * Time.deltaTime);
	    }

        angleMeter.text = (Mathf.Ceil(transform.localEulerAngles.z)).ToString();

        if(!clipPlay) {
            AudioClip moveCannon = Resources.Load<AudioClip>("Audio/MoveCannon");
            audio.clip = moveCannon;
            audio.Play();
            clipPlay = true;
        }

        if(audio.pitch <= 1.3)audio.pitch += 0.01f;
	}

	public void getGui(int playerId) {
	    GUI = GameObject.Find("GUI");
        GameObject playerInfo = GUI.transform.Find("Player Infos "+ playerId).gameObject;
        GameObject AngleInfo = playerInfo.transform.Find("Angle Info "+ playerId).gameObject;
        GameObject PowerInfo = playerInfo.transform.Find("Power Info "+ playerId).gameObject;
	    useAngleMeter = AngleInfo.transform.Find("Angle Meter " + playerId).gameObject;
        usePowerMeter = PowerInfo.transform.Find("Power Meter " + playerId).gameObject;

        angleMeter = useAngleMeter.GetComponent<Text>();
        powerMeter = usePowerMeter.GetComponent<Text>();
	}
}
