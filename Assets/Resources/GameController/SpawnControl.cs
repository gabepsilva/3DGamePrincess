using UnityEngine;
using System.Collections;

public class SpawnControl : MonoBehaviour {
	

	//max number of corpses on screen
	public int MAX_CORPSES = 30;
	
	public int corpseCounter = 0;
	
	public int pointsTotal = 0;
	int pointsSumAnimation = 0;
	
	public GUIText pointsText;
	
	//var auxiliar to define random positions
	Vector3 newPosition;


	//no need to check respawn ever frame, vars created to
	//make respawn be echced every X milisecond
	public float	respwnDelay = 0.4f;
	//calculate the delay
	float respwnTimeCurrent = 0; 

	//Vector to count existing enemies
	GameObject[] enemies;
	//aux
	GameObject aux;


	
	// Use this for initialization
	void Start () {
	
		//GameObject princess = GameObject.Find("Princess");
		
		//transform.position = princess.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		//check if need respawn every X miliseconds
		if(Time.time - respwnTimeCurrent > respwnDelay)
		{
			//make the respawn on objects
			RespawnCorpses();
			

			//update time
			respwnTimeCurrent = Time.time;
		}
		PointsCalculation();		
		
	}

	void RespawnCorpses(){
	
	
		enemies = GameObject.FindGameObjectsWithTag("enemy");
	
	
		//for(int i = enemies.Length; i < MAX_CORPSES; i++ ) {
		if(enemies.Length < MAX_CORPSES) {
			//define the position to de new enemy
			newPosition = GameObject.Find("RespawnPoint" + Random.Range (0, 10)).transform.position;

			

			//instaciate prefab
			aux = Instantiate (Resources.Load ("Entity/Enemy/corpse/prefabs/enemy-corpse-prefab")) as GameObject;
			//set new position
			aux.transform.position = newPosition;
			//randomize d name
			aux.name = "enemy-corpse" + Random.Range (0, 99999);

			//set parent
			//GameObject.Find(aux.name).transform.parent = transform;
			aux.transform.parent = transform;
			
			corpseCounter++;

		}
	}
	
	void PointsCalculation(){
	

		pointsTotal = corpseCounter * 100;
		
		if(pointsSumAnimation < pointsTotal)
		{
			pointsSumAnimation += 6;
			pointsText.text = "" +  pointsSumAnimation;
		}

	}
	
}
