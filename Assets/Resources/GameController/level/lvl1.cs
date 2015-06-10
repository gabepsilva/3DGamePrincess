using UnityEngine;
using System.Collections;

/**

THIS SCRIPT MAKES THE LVL CONTROL

**/

public class lvl1 : MonoBehaviour {
	
	
	//princess object
	GameObject princess;
	
	//start position in each phase
	public Vector3 startPosition;
	
	//stages of this level
	public enum lvlPhase : byte {PhaseIntroCAM1, PhaseIntroPrincessWalk, NotPhaseControlling,GotoPhase2Disables, GotoPhase2WalkToGate, GotoPhase2WalkToVilla};
	//current stage of this level
	public lvlPhase lvlPhaseStatus = lvlPhase.PhaseIntroCAM1;
	public lvlPhase lastLVLPlayed;
	
	// Use this for initialization
	void Start () {
	
		
		//get the princess object
		princess = GameObject.Find("Princess");
		
		//Disable follow script to make first animation
		Camera.main.GetComponent<FollowPrincess>().enabled = false;
		//sets camera very high (to make it come down later)
		Camera.main.transform.position = new Vector3(372.9437f, 292.82047f, 1142.953f);
		//final position of camera
		startPosition = new Vector3(372.9437f, 92.82047f, 1142.953f);
		
		//tell the princess, she will be controletd by the level
		princess.GetComponent<PrincessClass>().isPhaseControlling = true;
		
		//disable respawn
		GameObject.Find("SpawnController").GetComponent<SpawnControl>().enabled = false;
		
		
		//each resawn point in a fixed position
		GameObject.Find("RespawnPoint0").transform.position = new Vector3(365.516f, 6f, 1161.765f);
		GameObject.Find("RespawnPoint1").transform.position = new Vector3(377.222f, 6f, 1294.306f);
		GameObject.Find("RespawnPoint2").transform.position = new Vector3(393.8194f, 6f, 1267.792f);
		GameObject.Find("RespawnPoint3").transform.position = new Vector3(382.2294f, 6f, 1206.403f);
		GameObject.Find("RespawnPoint4").transform.position = new Vector3(456.7072f, 6f, 1157.463f);
		GameObject.Find("RespawnPoint5").transform.position = new Vector3(592.3174f, 6f, 1215.721f);
		GameObject.Find("RespawnPoint6").transform.position = new Vector3(590.7265f, 6f, 1286.833f);
		GameObject.Find("RespawnPoint7").transform.position = new Vector3(634.3718f, 6f, 1338.274f);
		GameObject.Find("RespawnPoint8").transform.position = new Vector3(620.5444f, 6f, 1421.906f);
		GameObject.Find("RespawnPoint9").transform.position = new Vector3(533.7917f, 6f, 1159.611f);
	
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		//state machine to this level	
		switch(lvlPhaseStatus)
		{
			
		case lvlPhase.PhaseIntroCAM1:
			
			//skip intro
			if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
				Time.timeScale = 3;
			
			//if animation finished
			if(startPosition == Camera.main.transform.position)
			{
				//princess go to
				startPosition = new Vector3(475.7f, 6f, 1245.8f);
				//enable camre follow
				Camera.main.GetComponent<FollowPrincess>().enabled = true;
				//next stage
				lvlPhaseStatus = lvlPhase.PhaseIntroPrincessWalk;
			}
			
			//while in this stage
			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, startPosition, 100*Time.deltaTime);
			Camera.main.transform.LookAt (princess.transform.position);
			
			break;
			//princess walks to her fight point
		case lvlPhase.PhaseIntroPrincessWalk:
			
			//skip intro
			if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
				Time.timeScale = 3;
			
			//if ready to start game
			if(startPosition == princess.transform.position)
			{
				Time.timeScale = 1;
				princess.GetComponent<PrincessClass>().isPhaseControlling = false;
				GameObject.Find("SpawnController").GetComponent<SpawnControl>().enabled = true;
				lvlPhaseStatus = lvlPhase.NotPhaseControlling;
				lastLVLPlayed =  lvlPhase.PhaseIntroPrincessWalk;
				
			}
			
			//go to fight position
			princess.GetComponent<PrincessClass>().GoTo(startPosition);
			
			break;
			
			//Closes phase and prepareto go to next phase
		case lvlPhase.GotoPhase2Disables:
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
			
			//sets new position to walk
			startPosition = new Vector3(534.7769f, 6f, 1127.862f);
			
			lastLVLPlayed =  lvlPhase.GotoPhase2Disables;
			lvlPhaseStatus = lvlPhase.GotoPhase2WalkToGate;
			
			break;
			
			// make princess to walk to next fight point.
		case lvlPhase.GotoPhase2WalkToGate:
			
			//skip walk
			if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
				Time.timeScale = 3;
			
			if(startPosition == princess.transform.position)
			{
				//ponit in village
				startPosition = new Vector3(876.1296f, 6f, 1135.743f);
				lvlPhaseStatus = lvlPhase.GotoPhase2WalkToVilla;
				lastLVLPlayed = lvlPhase.GotoPhase2WalkToGate;
				princess.GetComponent<PrincessClass>().fightStatus = PrincessClass.FightState.FightStopped;
				
			}
			
			
			princess.GetComponent<PrincessClass>().GoTo(startPosition);
			
			
			break;
			
		case lvlPhase.GotoPhase2WalkToVilla:
		
			//skip walk
			if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
				Time.timeScale = 3;
			
			//make her ready to fight
			if(startPosition == princess.transform.position)
			{
				Time.timeScale = 1;
				lvlPhaseStatus = lvlPhase.NotPhaseControlling;
				lastLVLPlayed = lvlPhase.GotoPhase2WalkToVilla;
				
				//princess control herself
				princess.GetComponent<PrincessClass>().isPhaseControlling = false;
				//respawn enabled
				GameObject.Find("SpawnController").GetComponent<SpawnControl>().enabled = true;
				//enable fighting
				princess.GetComponent<PrincessClass>().fightStatus = PrincessClass.FightState.FightFinished;
				
				//enable next level control			
				GameObject.Find("LVLControl").GetComponent<lvl2>().enabled = true;
				
			}
			//while in this step
			princess.GetComponent<PrincessClass>().GoTo(startPosition);
			
			break;
			
		case lvlPhase.NotPhaseControlling:
			
			if(GameObject.Find("SpawnController").GetComponent<SpawnControl>().pointsTotal >= 8000 && lastLVLPlayed == lvlPhase.PhaseIntroPrincessWalk )
				lvlPhaseStatus = lvlPhase.GotoPhase2Disables;
				
			break;
		}
		
		
		//
		
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

