using UnityEngine;
using System.Collections;

public class lvl2 : MonoBehaviour {

	//princess object
	GameObject princess;
	
	//start position in each phase
	public Vector3 startPosition;
	
	//stages of this level
	public enum lvlPhase : byte {NotPhaseControlling, GotoFlorestDisables, GotoFlorest };
	//current stage of this level
	public lvlPhase lvlPhaseStatus = lvlPhase.NotPhaseControlling;
	public lvlPhase lastLVLPlayed;
	
	// Use this for initialization
	void Start () {
	
		GameObject.Find("LVLControl").GetComponent<lvl1>().enabled = false;
	
		//get the princess object
		princess = GameObject.Find("Princess");
		
		startPosition = princess.transform.position;
		
		
		//tell the princess, she will be controletd by the level
		//princess.GetComponent<PrincessClass>().isPhaseControlling = true;
		
		//disable respawn
		//GameObject.Find("SpawnController").GetComponent<SpawnControl>().enabled = false;
		
		
		//each resawn point in a fixed position
		//PrintRespawnPositions();
		
		GameObject.Find("RespawnPoint0").transform.position = new Vector3(946.5005f, 6f, 1278.479f);
		GameObject.Find("RespawnPoint1").transform.position = new Vector3(801.9f, 6f, 1097.51f);
		GameObject.Find("RespawnPoint2").transform.position = new Vector3(882.166f, 6f, 1052.926f);
		GameObject.Find("RespawnPoint3").transform.position = new Vector3(935.8522f, 6f, 1260.464f);
		GameObject.Find("RespawnPoint4").transform.position = new Vector3(954.0012f, 6f, 1068.827f);
		GameObject.Find("RespawnPoint5").transform.position = new Vector3(1002.071f, 6f, 1156.995f);
		GameObject.Find("RespawnPoint6").transform.position = new Vector3(789.1414f, 6f, 1143.355f);
		GameObject.Find("RespawnPoint7").transform.position = new Vector3(982.7614f, 6f, 1107.607f);
		GameObject.Find("RespawnPoint8").transform.position = new Vector3(798.9755f, 6f, 1295.109f);
		GameObject.Find("RespawnPoint9").transform.position = new Vector3(780.3199f, 6f, 1278.511f);
	
				
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//state machine to this level	
		switch(lvlPhaseStatus)
		{
		
		case lvlPhase.GotoFlorestDisables:

			//disabe respawn
			GameObject.Find("SpawnController").GetComponent<SpawnControl>().enabled = false;
			//stop fighing (bug if keep fighting and delete components)
			princess.GetComponent<PrincessClass>().fightStatus = PrincessClass.FightState.none;
			//give princess control to the phase controller
			princess.GetComponent<PrincessClass>().isPhaseControlling = true;
			
			//destroy enemies
			GameObject[] e = GameObject.FindGameObjectsWithTag("enemy");
			foreach(GameObject o in e)
				Destroy(o);


			lastLVLPlayed =  lvlPhase.GotoFlorestDisables;
			lvlPhaseStatus = lvlPhase.GotoFlorest;
			startPosition = new Vector3(1070.907f, 6f, 921.4257f);

			break;

		case lvlPhase.GotoFlorest:

			//skip walk
			if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
				Time.timeScale = 3;


			if(startPosition == princess.transform.position)
			{

				lastLVLPlayed =  lvlPhase.GotoFlorest;
				lvlPhaseStatus = lvlPhase.NotPhaseControlling;
				princess.GetComponent<PrincessClass>().fightStatus = PrincessClass.FightState.FightStopped;

				if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
					Application.LoadLevel ("congrats"); 
			}
			
			princess.GetComponent<PrincessClass>().GoTo(new Vector3(1070.907f, 6f, 921.4257f));
			
			break;
			
			case lvlPhase.NotPhaseControlling:
			
			if(GameObject.Find("SpawnController").GetComponent<SpawnControl>().pointsTotal >= 16000 )
				lvlPhaseStatus = lvlPhase.GotoFlorestDisables;

			
			break;
		}
	}
	
	void PrintRespawnPositions()
	{
		GameObject rp;
		
		for(int i = 0; i < 10; i++)
		{
			rp = GameObject.Find("RespawnPoint"+i);
			print("GameObject.Find(\"RespawnPoint"+i+"\").transform.position = new Vector3(" + rp.transform.position.x + "f, " + rp.transform.position.y + "f, " +  rp.transform.position.z + "f);");
			
			
		}
	}
	
}
