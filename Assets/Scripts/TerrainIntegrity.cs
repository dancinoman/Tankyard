using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainIntegrity : MonoBehaviour {

    private GameObject ammo120mm;
    private GameObject explosion120mm;
    private TankIntegrity tankIntegrity;
    private GameController gameController;
    private AudioSource audio;
    private PolygonCollider2D polCollider2D;
    private GameObject pinPointObj;
    private float[][] explosionCheckPointPercent = new float[16][];
    private List<Vector2> rectangleEdge = new List<Vector2>();
    private List<Vector2> explosionEdge = new List<Vector2>();
    private List<Vector2> newTerrainPoints = new List<Vector2>();
    private List<GameObject> tempArray = new List<GameObject>();
    private int lowestIndexPoint;

	// Use this for initialization
	void Start () {
	    polCollider2D = GetComponent<PolygonCollider2D>();
	    audio = GetComponent<AudioSource>();
		GameObject gameControllerObj = GameObject.Find("Game Controller");
		gameController = gameControllerObj.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {

	    if(pinPointObj != null) {
	        if(pinPointObj.tag == "Circle Pin Point") {
                GameObject[] pinPoints = GameObject.FindGameObjectsWithTag("Circle Pin Point");
                // Once all pinpoints are Spawned
                if(pinPoints.Length == 16 && tempArray.Count > 1) {

                    tempArray.Sort((x, y) => (x.GetComponent<PinPointId>().orderId).CompareTo(y.GetComponent<PinPointId>().orderId));

                    for(int i = 0; i < tempArray.Count; i++) {
                        newTerrainPoints.Insert(lowestIndexPoint, tempArray[i].transform.localPosition);
                    }

                    polCollider2D.SetPath(0, newTerrainPoints.ToArray());

                    tempArray.Clear();
                    newTerrainPoints.Clear();
                    foreach(GameObject pinPoint in pinPoints) {
                        Destroy(pinPoint);
                    }
                }
	        }

	        if(pinPointObj.tag == "Rectangle Pin Point") {
                GameObject[] recPinPoints = GameObject.FindGameObjectsWithTag("Rectangle Pin Point");
                // Once all pinpoints are Spawned
                if(recPinPoints.Length == 12 && tempArray.Count > 1) {
                    tempArray.Sort((x, y) => (x.GetComponent<PinPointId>().orderId).CompareTo(y.GetComponent<PinPointId>().orderId));
                    for(int i = 0; i < tempArray.Count; i++) {
                        newTerrainPoints.Insert(lowestIndexPoint, tempArray[i].transform.localPosition);
                    }

                    polCollider2D.SetPath(0, newTerrainPoints.ToArray());
                    tempArray.Clear();
                    newTerrainPoints.Clear();
                    rectangleEdge.Clear();
                    foreach(GameObject pinPoint in recPinPoints) {
                        Destroy(pinPoint);
                    }
                }
	        }
	    }
	}

	void OnTriggerEnter2D(Collider2D other) {
	    // Giving explosion effect
	    if(other.gameObject.tag == "Ammo") {
	        Destroy(other.gameObject);
	        AudioClip explosionTerrain = Resources.Load<AudioClip>("Audio/TerrainExplosion");
	        audio.clip = explosionTerrain;
	        audio.Play();
	        Vector3 otherPosition = other.gameObject.transform.position;
	        explosion120mm = Instantiate(Resources.Load("Prefabs/Explosion_120mm", typeof(GameObject)), new Vector3(otherPosition.x, otherPosition.y, 0f), Quaternion.identity) as GameObject;
	        checkOtherTankDistance(explosion120mm);
	    }

        // Creating a crater
	    if(other.gameObject.tag == "Explosion") {

            spawnCirclePinPoint(other.gameObject);

	        determinePolyColliderVerticles(other.gameObject);

            other.GetComponent<CircleCollider2D>().enabled = false;
        }

        if(other.gameObject.tag == "Circle Pin Point" || other.gameObject.tag == "Rectangle Pin Point") {
            other.transform.parent = transform;
            tempArray.Add(other.gameObject);
        }

        if(other.gameObject.tag == "Pin Point Spawner") {

            GameObject platFormSkeleton = Instantiate(Resources.Load("Prefabs/PlatForm_Skeleton", typeof(GameObject)), new Vector2(other.transform.position.x, other.transform.position.y + 0.2f), Quaternion.identity) as GameObject;

            determinePolyColliderVerticles(platFormSkeleton);

            spawnRectanglePinPoint(platFormSkeleton);

            Destroy(other.gameObject);
        }

	}


	void spawnCirclePinPoint(GameObject thisObject) {
	    //The coordinates around a circle
        explosionCheckPointPercent[0] = new float[] {0f, 1f};
        explosionCheckPointPercent[1] = new float[] {-0.4f,0.9f};
        explosionCheckPointPercent[2] = new float[] {-0.7f,0.7f};
        explosionCheckPointPercent[3] = new float[] {-0.9f,0.4f};
        explosionCheckPointPercent[4] = new float[] {-1f,0f};
        explosionCheckPointPercent[5] = new float[] {-0.9f,-0.4f};
        explosionCheckPointPercent[6] = new float[] {-0.7f,-0.7f};
        explosionCheckPointPercent[7] = new float[] {-0.4f,-0.9f};
        explosionCheckPointPercent[8] = new float[] {0f,-1f};
        explosionCheckPointPercent[9] = new float[] {0.4f,-0.9f};
        explosionCheckPointPercent[10] = new float[] {0.7f,-0.7f};
        explosionCheckPointPercent[11] = new float[] {0.9f,-0.4f};
        explosionCheckPointPercent[12] = new float[] {1f,0f};
        explosionCheckPointPercent[13] = new float[] {0.9f,0.4f};
        explosionCheckPointPercent[14] = new float[] {0.7f,0.7f};
        explosionCheckPointPercent[15] = new float[] {0.4f,0.9f};

	    Vector2 explosionSize = thisObject.GetComponent<SpriteRenderer>().bounds.extents;
        // Define explosion ray size;
        float explosionRay = explosionSize.x;

        // Add intersection point for collider on terrain
        for(int i = 0; i < 16; i++) {
            explosionEdge.Add(new Vector2(thisObject.transform.position.x + (explosionRay * explosionCheckPointPercent[i][0]) ,
                                        thisObject.transform.position.y + (explosionRay * explosionCheckPointPercent[i][1])));

            pinPointObj = Instantiate(Resources.Load("Prefabs/Pin_Point", typeof (GameObject)), explosionEdge[i], Quaternion.identity) as GameObject;
            pinPointObj.tag = "Circle Pin Point";
            pinPointObj.GetComponent<PinPointId>().orderId = i;
        }
	}

    void spawnRectanglePinPoint(GameObject thisObject) {
        Vector2 rectangleSize = thisObject.GetComponent<SpriteRenderer>().bounds.extents;
        // Add rectangle shape verticles

        for(float d = 1; d >= 0 ; d-= 0.2f){
            rectangleEdge.Add(new Vector2(thisObject.transform.position.x + rectangleSize.x, thisObject.transform.position.y - rectangleSize.y + rectangleSize.y * d * 2));
        }

        for(float d = 0; d <= 1 ; d+= 0.2f){
            rectangleEdge.Add(new Vector2(thisObject.transform.position.x - rectangleSize.x, thisObject.transform.position.y - rectangleSize.y + rectangleSize.y * d * 2));
        }

        rectangleEdge.Reverse();

        for(int i = 0; i < rectangleEdge.Count; i++) {
            pinPointObj = Instantiate(Resources.Load("Prefabs/Pin_Point", typeof (GameObject)), rectangleEdge[i], Quaternion.identity) as GameObject;
            pinPointObj.GetComponent<PinPointId>().orderId = i;
            pinPointObj.tag = "Rectangle Pin Point";
        }
        thisObject.GetComponent<Collider2D>().enabled = false;
    }

	void determinePolyColliderVerticles(GameObject thisObject) {
        Vector2 objectSize = thisObject.GetComponent<SpriteRenderer>().bounds.extents;
        // Define explosion ray size;
        float explosionRay = objectSize.x;

	    //thisObject.transform.parent = transform;
        // Remove all surrounding the explosion intersecting the terrain collider
        bool noPointIntersects = true;
        Collider2D thisObjectCol = thisObject.GetComponent<Collider2D>();
        lowestIndexPoint = polCollider2D.points.Length;

        for(int i = 0; i < polCollider2D.points.Length; i++) {

            Vector3 localPoint = transform.TransformPoint(polCollider2D.points[i]);

            if(noPointIntersects && localPoint.x < (thisObject.transform.position.x - objectSize.x)) {
                lowestIndexPoint = i;
            }


            if(thisObjectCol.bounds.Contains(localPoint)) {
                if(i < lowestIndexPoint) {
                    lowestIndexPoint = i;
                    noPointIntersects = false;
                }
            } else {
                newTerrainPoints.Add(polCollider2D.points[i]);
            }
        }
        Debug.Log("lowest index"+ lowestIndexPoint);
	}

    //Calculate dammage for nearby explosion if apply;
    void checkOtherTankDistance(GameObject explosion) {
        GameObject[] tanksHull = GameObject.FindGameObjectsWithTag("Tank Hull");

        foreach(GameObject tankHull in tanksHull) {
            float dist = Vector3.Distance(explosion.transform.position, tankHull.transform.position);

            if(dist <= gameController.ammo120mmBlast) {
                tankIntegrity = tankHull.GetComponent<TankIntegrity>();
                int damage = Mathf.RoundToInt(gameController.ammo120mmDamage / dist / 2.2f);
                tankIntegrity.applyDamage(damage);
            }
        }
    }

}
