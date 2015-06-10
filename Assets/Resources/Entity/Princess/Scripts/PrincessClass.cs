using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PrincessClass : EntityClass3D {


	// to where the priness should go
	Vector3 newPosition;
	// strart point to walk arround
	Vector3 pivotPoint;
	//switch princess animation
	Animator anim;
	

	//State machine to walk arround
	public enum WalkState : byte {WalkFinished, WalkDelay, WalkDefine, Walking, WalkingStopped };
	public WalkState walkStatus = WalkState.WalkFinished;
	//delay between each walk
	public float   walkDelay = 3;
	//used to calculate the next walk
	float   lastWalkTime = 0;

	
	//public AudioClip punchImpact;
	public AudioClip kickImpact;
	bool attackSound = false; // false kick - true punch


	public Vector3 mouCaPosition;

	//state machine to battle
	public enum FightState : byte {FindInRange, FightOneInRage, Fighting, FightDelay, FightFinished, FightStopped, none};
	public FightState fightStatus = FightState.FindInRange;
	//dely in each meele
	public float	fightDelay = 0.1f;
	//calculate the delay
	float fightTimeCurrent = 0; 
	
	//fx when fighting 
	//GameObject hitBlow;
	
	
	//enemies in fight range
	Collider[] c = null;
	//enemy in target
	Collider targetEnemy = null;

	GameObject hitBlow;

	public short killBonus = 0;
	TextMesh killBonusTXT;
	
	
	bool alternatePunchAndKick = false;
	
	//save the original life bat to calculate the new size 
	public Vector3 lifeBarTOriginalSize;
	
	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();


		killBonusTXT = GameObject.Find("KILLBONUSTXT").GetComponent<TextMesh>();
		
		
		lifeBarT = GameObject.Find("life").transform;
		lifeBarTOriginalSize = lifeBarT.transform.localScale;
		
		hitBlow = Instantiate(Resources.Load("Entity/Princess/Prefabs/hitblow1-prefab")) as GameObject;
		hitBlow.SetActive (false);
		
		//Sync initial position
		newPosition.x = transform.position.x;
		newPosition.z = transform.position.z;
		newPosition.y = transform.position.y;
		
		life = 100;
		damageRate = 0.2f;
		

	}

	// Update is called once per framel
	void Update () {


		if(!isPhaseControlling)
		{

			DebugCombatRange ();
			
			if (life <= 0) {
				StartCoroutine(DieAndFreeze("trigDie", 2.0f)); 
				return;
			}
			
			WaklArround ();
			
			FightInRange ();
		}else{
			pivotPoint = transform.position;
		}

	}
	
	void OnCollisionStay(Collision collision){
		
		if (life <= 0.0f)
			return;
		
		lifeDecrease ();
		
		
		collision.gameObject.GetComponent<Renderer>().material.color = Color.red;;
		lifeBarT.transform.localScale = new Vector3(life * lifeBarTOriginalSize.x / 100f, lifeBarTOriginalSize.y, 0);

		killBonus = 0;
		
	}

	void WaklArround ()
	{
		
		
		switch (walkStatus) {
			
			//makes princess in stand mode
		case WalkState.WalkFinished:
			
			anim.SetTrigger("trigStand");
			anim.SetBool("enableStand",true);
			anim.SetBool("enableWalk",false);
			
			
			walkStatus = WalkState.WalkDelay;
			
			break;
			
		case WalkState.WalkDelay:
			
			//in stand mode. Allow next stage only after the walkDelay
			if(Time.time - lastWalkTime >= walkDelay){				
				walkStatus = WalkState.WalkDefine;
			}
			
			break;
			
		case WalkState.WalkDefine:
		
			
			//define new doordinates to walk
			//newPosition.x = Random.Range( Xmin, Xmax );
			//newPosition.z = Random.Range( Zmin, Zmax );
			newPosition.x = Random.Range( pivotPoint.x + 20, pivotPoint.x - 20 );
			newPosition.z = Random.Range( pivotPoint.z + 20, pivotPoint.z - 20 );
			
			newPosition.y = transform.position.y;
			
			//check if princess is going left or right
			float dist = (transform.position.x - newPosition.x);
			//flip is needed
			if ((dist < 0 && isLeftTuned) || (dist > 0 && !isLeftTuned))
				FlipUnflip ();
			
			//go to next stage
			walkStatus = WalkState.Walking;
			break;
			
		case WalkState.Walking:

			GoTo(newPosition);
			
			
			////check if finished walking
			if (transform.position == newPosition)
				walkStatus = WalkState.WalkFinished;
			
			break;
			
			
		case WalkState.WalkingStopped:
			
			//walking interrupted by anyting			
			newPosition.x = transform.position.x;
			newPosition.z = transform.position.z;
			newPosition.y = transform.position.y;
			transform.position = Vector3.MoveTowards(transform.position, newPosition, walkSpeed*Time.deltaTime);
			
			//go to first stage
			//walkStatus = WalkState.WalkFinished;
			break;
			
		}
	}

	public void addBonus(string monster)
	{

		if(killBonus++ >= 2)
			killBonusTXT.text = "Kill Bonus\n" + killBonus +" X 100";
		else
		{
			killBonusTXT.text = "";
		}

	}




	void FightInRange()
	{
		
		switch (fightStatus) {
			
		case FightState.FindInRange:
			
			//gets all entitis in range
			c = getEntitiesInCombatRange();
			
			//prepare to clean list of objects
			List<Collider> co = new List<Collider>();
			
			//gets only the the enemies
			for(int i = 0; i < c.Length; i++)
				if(c[i].name.Contains("enemy"))
					co.Add(c[i]);
			
			c = co.ToArray();
			
			//fight if possible
			if (c.Length >= 1)
				fightStatus = FightState.FightOneInRage;
			else
			{
				if(walkStatus == WalkState.WalkingStopped)
					walkStatus = WalkState.WalkFinished;
			}
						
			break;
			
			
		case FightState.FightOneInRage:
			
			walkStatus = WalkState.WalkingStopped;
			
			
			targetEnemy = c[Random.Range(0,c.Length-1)];
			
			//check if princess is going left or right
			float dist = (transform.position.x - targetEnemy.transform.position.x);;
			//flip is needed
			if ((dist < 0 && isLeftTuned) || (dist > 0 && !isLeftTuned))
				FlipUnflip ();
			

			if(targetEnemy != null)
				fightStatus = FightState.Fighting;
			
			
			break;
			
			
		case FightState.Fighting:
		
		
			
			//start fight animation
			if(alternatePunchAndKick)
			{	
				GetComponent<Animator> ().SetTrigger ("trigPunch");
				attackSound = true;
			}
			else
			{
				GetComponent<Animator> ().SetTrigger ("trigKick");
				attackSound = true;
			}
			
			alternatePunchAndKick = !alternatePunchAndKick;

			fightTimeCurrent = Time.time;
			
			
			if(targetEnemy.gameObject.tag == "enemy")
				targetEnemy.gameObject.GetComponent<CorpseClass>().lifeDecrease();
			
			
			
			StartCoroutine(HitBlow(0.0225f,targetEnemy.transform.position, 0.1f));
			
			fightStatus = FightState.FightDelay;
			
			
			break;
			
		case FightState.FightDelay:
			
			if (Time.time - fightTimeCurrent > fightDelay)
			{
				if(targetEnemy.gameObject.activeSelf == true)
					fightStatus = FightState.Fighting;
				else
					fightStatus = FightState.FightFinished;
			}
			break;
			
		case FightState.FightFinished:
			
			if (Time.time - fightTimeCurrent > fightDelay){
				fightStatus = FightState.FindInRange;
				walkStatus = WalkState.WalkDefine;
			}
			break;
			
			
		case FightState.FightStopped:
		
			anim.SetTrigger("trigStand");
			anim.SetBool("enableStand",true);
			anim.SetBool("enableWalk",false);
			
			
			fightStatus = FightState.none;
			
			break;
		}
		
		
	}

	
	protected IEnumerator HitBlow(float delay, Vector3 pos, float duration){
		
		
		hitBlow.transform.position = targetEnemy.transform.position;
		yield return new WaitForSeconds (delay);
		
		if(attackSound)
			GetComponent<AudioSource>().PlayOneShot(punchImpact, 0.7F);
		else
			GetComponent<AudioSource>().PlayOneShot(kickImpact, 0.7F);
			
		hitBlow.SetActive (true);
		yield return new WaitForSeconds (duration);
		hitBlow.SetActive (false);
	}
	
	protected IEnumerator DieAndFreeze(string TrigDieAnimation, float delay){
		
		GetComponent<Animator> ().SetTrigger (TrigDieAnimation);
		GetComponent<AudioSource>().PlayOneShot(dieSound, 0.15F);
		Time.timeScale = 0.25f;
		yield return new WaitForSeconds (delay);
		Application.LoadLevel (Application.loadedLevel);
		Time.timeScale = 1.0f;
	}

	public void GoTo(Vector3 position) {
	
		if(transform.position != position)
		{
			//check if princess is going left or right
			float dist = (transform.position.x - position.x);
			//flip is needed
			if ((dist < 0 && isLeftTuned) || (dist > 0 && !isLeftTuned))
				FlipUnflip ();
			
			//define walking animations 
			anim.SetTrigger("trigWalk");
			anim.SetBool("enableWalk",true);
			anim.SetBool("enableStand",false);
			
			//update walking time
			lastWalkTime = Time.time;
			//walk
			transform.position = Vector3.MoveTowards(transform.position, position, walkSpeed*Time.deltaTime);
		}
		else
		{
			anim.SetTrigger("trigStand");
			anim.SetBool("enableStand",true);
			anim.SetBool("enableWalk",false);
			walkStatus = WalkState.WalkFinished;
		}
		
	}
	
	
}
