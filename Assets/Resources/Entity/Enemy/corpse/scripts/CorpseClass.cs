using UnityEngine;
using System.Collections;

public class CorpseClass : EntityClass3D {



	GameObject[] princess;
	
	public float speed = 8;


	public Vector3 lifeBarTOriginalSize;
	




	// Use this for initialization
	void Start () {

		life = 100;
		damageRate = 95f;
		damageRateTouch = 9f;

		princess = GameObject.FindGameObjectsWithTag("princess");
		transform.position = Vector3.MoveTowards(transform.position, princess[0].transform.position, speed*Time.deltaTime);

		lifeBarT = transform.Find ("LifeBar/lifeG");
		lifeBarTOriginalSize = lifeBarT.localScale;

		lifeBarT.gameObject.SetActive (false);
		transform.Find ("LifeBar/lifeR").gameObject.SetActive (false);


		if (Application.platform != RuntimePlatform.Android && 
		    Application.platform != RuntimePlatform.IPhonePlayer &&
		    Application.platform != RuntimePlatform.WP8Player)
		{
			damageRateTouch = damageRate;
		}

	}
	
	// Update is called once per frame
	void Update () {

		//flip if needed
		float dist = (transform.position.x - princess[0].transform.position.x);
		if ((dist < 0 && isLeftTuned) || (dist > 0 && !isLeftTuned))
			FlipUnflip ();

		//go to princess
		transform.position = Vector3.MoveTowards(transform.position, princess[0].transform.position, speed*Time.deltaTime);

		//Damage on click
		RaycastHit[] o = Inputed ();
		if (o != null ){
			foreach(RaycastHit hit in o){
				if(hit.collider.gameObject.name == name){
					lifeDecreaseTouch();

				}
			}
		}

		//activate lifebar
		if(!lifeBarT.gameObject.activeSelf){
			if(life < 100){
				lifeBarT.gameObject.SetActive (true);
				transform.Find ("LifeBar/lifeR").gameObject.SetActive (true);
			}
		}

		if(life < EntityClass3D.ZERO)
		{
			Camera.main.GetComponent<PlaySound>().PlaySd(dieSound);
			Die();
			princess[0].GetComponent<PrincessClass>().addBonus("corpse");
		}

		//if(enabled == false)
		//	enabled = true;
		
		lifeBarT.transform.localScale = new Vector3(life * lifeBarTOriginalSize.x / 100f, lifeBarTOriginalSize.y, 0);
	
	}
	

	
	
	
}
