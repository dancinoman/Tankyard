using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    //Damage Ammo
    public int ammo120mmDamage = 50;
    public float ammo120mmBlast = 1f;

    //Set Wind
    public int windMaximumStrength = 12;
    public int windDirection;
    public int windAmount;
    private List<int> directionList = new List<int>();

    // Class members
    public float secondsBeforeNextTurn = 2f;
    public bool gameStart = false;
    public int playerTurn = 1;
    public bool turnIsOver = false;
    public GameObject loadScreenImage;
    public GameObject playerCannon1;
    public GameObject playerCannon2;
    private int maxPlayers = 2;
    private List<int> playerList = new List<int>();
    public bool hasShoot = false;
    private IEnumerator coroutine;
    private IEnumerator coroutine2;
    public bool startDelay = false;
    private AudioSource audio;

    // Text Effect
    private bool changeTextSizeEffect = false;
    public float textEffectSpeed = 0.2f;
    public float maximumSize = 1.5f;
    private bool raise = true;
    private bool oneTimeOnly = true;

    // Menus
    public GameObject mainMenu;
    public GameObject titleMenu;
    public GameObject sliderTinyAmmoObj;

    // GUI
    public GameObject GUI;
    public GameObject windInfoObj;
    public GameObject imageWindDirection;
    public GameObject gameOverObj;
    private TextMeshProUGUI windInfo;

    // Message Box
    public GameObject messageBox;
    public GameObject messageHolder;
    public GameObject confirmButton;
    private Text confirmText;
    private TextMeshProUGUI messageText;

	// Use this for initialization
	void Start () {
	    Slider sliderTinyAmmo = sliderTinyAmmoObj.GetComponent<Slider>();
	    sliderTinyAmmo.enabled = false;
	    audio = GetComponent<AudioSource>();
	    windInfo = windInfoObj.GetComponent<TextMeshProUGUI>();
	    confirmText = confirmButton.GetComponent<Text>();
        messageText = messageHolder.GetComponent<TextMeshProUGUI>();
	    gameOverObj.SetActive(false);
        setRandomWind(false);
	    coroutine = turnOverDelay(secondsBeforeNextTurn);
	    coroutine2 = textEffect(secondsBeforeNextTurn, textEffectSpeed, maximumSize, windInfo.fontSize);
	    StartCoroutine(coroutine);
	    StartCoroutine(coroutine2);
		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(turnIsOver) {
		    if(playerTurn == maxPlayers) {
		        playerTurn = 1;
		    } else {
		        playerTurn ++;
		    }
            setGui(playerTurn);
            hasShoot = false;
            turnIsOver = false;
		}
	}

    public void hideSubMenus() {
        GameObject[] subMenus;
        subMenus = GameObject.FindGameObjectsWithTag("Sub Menu");
        foreach(GameObject subMenu in subMenus) {
            subMenu.SetActive(false);
        }
    }

    // Popup message box to give infos
    public void popUpMessage(string messageInfo, string buttonInfo) {
        messageBox.SetActive(true);
        confirmText.text = buttonInfo;
        messageText.text = messageInfo;
    }

    // All values will reset here before the new game
	public void resetTheGame() {

	}

	public void setRandomWind(bool noticeChange) {
	    windAmount = Random.Range(0, windMaximumStrength);
	    windInfo.text = windAmount.ToString();
        directionList.Add(-1);
        directionList.Add(1);
	    int index = Random.Range(0, directionList.Count);
	    windDirection = directionList[index];
	    Image imageWindDirectionCom = imageWindDirection.GetComponent<Image>();
        if(windAmount == 0) {
            imageWindDirectionCom.sprite = Resources.Load<Sprite>("Sprites/Hud_WindNull");
        } else if(windDirection == directionList[1]) {
            imageWindDirectionCom.sprite = Resources.Load<Sprite>("Sprites/Hud_WindRight");
        } else if(windDirection == directionList[0]) {
            imageWindDirectionCom.sprite = Resources.Load<Sprite>("Sprites/Hud_WindLeft");
        }

        if(noticeChange) {
            raise = true;
            oneTimeOnly = true;
            changeTextSizeEffect = noticeChange;
        }

	}

    public void destroyMe(GameObject me, float seconds){
        Destroy(me, seconds);
    }


    public void setGui(int playerTurn) {
        GameObject[] currentGUIS = GameObject.FindGameObjectsWithTag("Player GUI");

        foreach(GameObject gui in currentGUIS) {
            gui.SetActive(false);
        }

        GUI = GameObject.Find("GUI");
        GameObject useGUIPlayer = GUI.transform.Find("Player Infos " + playerTurn).gameObject;
        useGUIPlayer.SetActive(true);
    }

    public void gameOver(int playerLost) {
        playerList.RemoveAt(playerLost - 1);

        if(playerList.Count == 1) {
            Text gameOverText = gameOverObj.GetComponent<Text>();
            gameOverText.text = "Player " + playerList[0].ToString() + " has won!";
            gameOverObj.SetActive(true);
        }

    }

	/*----- FOR MENU BUTTONS -----*/

    public void manageSlider(GameObject sliderObj) {
        Slider slider = sliderObj.GetComponent<Slider>();
        GameObject valueObj = sliderObj.transform.Find("Value").gameObject;
        Text value = valueObj.GetComponent<Text>();

        if(sliderObj.tag == "Qty Infinite" && slider.value == slider.maxValue) {
            value.text = "Infinity";
        } else {
            value.text = (slider.value).ToString();
        }

    }

	public void closeMessageBox() {
	    messageBox.SetActive(false);
	}

	public void playButton() {
	    //loadScreenImage.SetActive(true);
        hideSubMenus();
        titleMenu.SetActive(false);
        GUI.SetActive(true);

        // Add list of players in game
        for(int i = 1 ; i <= maxPlayers ; i++) {
            playerList.Add(i);
        }

        popUpMessage("TankYard! Be aware soldier! An enemy is standing on your sight ready to be blown! Take controll of your cannon's tank A or D,", "Understood!");
        gameStart = true;
        Time.timeScale = 1;
	}

	public void switchMenu(GameObject currentMenu) {
        if(currentMenu.name == "Main Menu") {
            mainMenu.SetActive(true);
            hideSubMenus();
        } else {
            currentMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
	}

	public void selectItems(GameObject thisButton) {

	}

	// Enumerators

	private IEnumerator turnOverDelay(float delay) {
        while(true) {
            if(startDelay) {
                yield return new WaitForSeconds(delay);
                turnIsOver = true;
                startDelay = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator textEffect(float secondsBeforeNextTurn, float speed, float maximum, float minimum) {
        while(true) {
            if(changeTextSizeEffect) {

                if(oneTimeOnly) {
                    yield return new WaitForSeconds(secondsBeforeNextTurn);
                    AudioClip wind = Resources.Load<AudioClip>("Audio/WindEffect");
                    audio.clip = wind;
                    audio.Play();
                    oneTimeOnly = false;
                }
                yield return new WaitForSeconds(speed);
                if(windInfo.fontSize >= maximum && raise) {
                    raise = false;
                } else if(windInfo.fontSize < maximum && raise) {
                    windInfo.fontSize += 2f;
                } else if(raise == false && windInfo.fontSize > minimum) {
                    windInfo.fontSize -= 2f;
                } else if(windInfo.fontSize >= minimum) {
                    changeTextSizeEffect = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

}
