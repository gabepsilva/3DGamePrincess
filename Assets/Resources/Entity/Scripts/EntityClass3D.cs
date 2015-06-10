using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EntityClass3D : MonoBehaviour {

	//BY DEFAUL YOU SHOULD ADD ALL SPRITES TURNED LEFT
	public bool isLeftTuned = true;

	//range to detect elements 
	//getEntitiesInCombatRange_LeftRight
	public float radiusCombatlRange = 8f;
	//public float yCombatlRange = 1.02f;

	//horizontal speed
	public int walkSpeed = 10;

	//princess phisical battledamage
	public float damageRate = 0.15f;
	// reduce damage on touch (dragpointer)
	public float damageRateTouch = 0.02f;
	//keep 100 to make things easier
	public float life = 100;

	//life bar of each enity
	public Transform lifeBarT;
	
	public AudioClip dieSound;
	
	public bool isPhaseControlling = false;


	//Stipid unit, use variable makes operations with floats more precise and correct
	protected static float ZERO = 0f;
	// Use this for initialization


	public AudioClip punchImpact;

	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//flip images 2d left and right
	protected void FlipUnflip()
	{
		isLeftTuned = !isLeftTuned;
		Vector3 tmp = transform.localScale;
		tmp.x *= -1;
		transform.localScale = tmp;
	}

	//life decreases
	public void  lifeDecrease()
	{
		life -= damageRate;
	}
	public void  lifeDecreaseTouch()
	{
		life -= damageRateTouch;
	}

	//return matrix with the colliders arround the entity
	protected Collider[] getEntitiesInCombatRange()
	{

		Collider[] c;//= new Collider[1];

		c = Physics.OverlapSphere(transform.position, radiusCombatlRange);

		return c;
	
	}
	//implement a function to make objects die.
	protected void Die()
	{

		//transform.position = new Vector3 (20, 0);
		gameObject.SetActive (false);
		Destroy(this.gameObject, 0.5f);	
	}

	//on click return the clicked object
	protected RaycastHit[] ScreenMouseRay()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);;

		Debug.DrawRay (ray.origin, ray.direction*9999, Color.yellow);


		 return Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

	}
	
	
	//on click return the touched object
	protected RaycastHit[] ScreenTouchRay()
	{


		Ray ray;
		RaycastHit[] hits;

		List<RaycastHit> l = new List<RaycastHit> ();

		for (int i = 0; i < Input.touchCount; i++) {


			if (Input.GetTouch(i).phase == TouchPhase.Moved) 
			{
				ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

				hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

				foreach(RaycastHit hit in hits)
					l.Add(hit);
			}

		}

		return l.ToArray();

	}

	//handles inputs such as mouse and touch
	protected RaycastHit[] Inputed(){

		RaycastHit[] hit;

		if (Application.platform == RuntimePlatform.Android || 
		    Application.platform == RuntimePlatform.IPhonePlayer ||
		    Application.platform == RuntimePlatform.WP8Player)
		{
			hit = ScreenTouchRay ();

			if (hit.Length > 0)
				return hit;
		}
		else
		{
			if (Input.GetMouseButtonDown (0))
			{
				hit = ScreenMouseRay();

				if(hit.Length > 0)
					return hit;
			
			}
		}
		return null;
	}

	


	//draw retangles arround the entity to facilitate debug
	protected void DebugCombatRange (){

		//sides
		Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x + radiusCombatlRange, transform.position.y, transform.position.z), Color.cyan);
		Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x + radiusCombatlRange * -1, transform.position.y, transform.position.z), Color.cyan);
		//up and Down
		Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y + radiusCombatlRange, transform.position.z), Color.cyan);
		Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y + radiusCombatlRange * -1, transform.position.z), Color.cyan);
		//back and for
		Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, transform.position.z+ radiusCombatlRange), Color.cyan);
		Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, transform.position.y, transform.position.z + radiusCombatlRange * -1), Color.cyan);
		
		//indicate the direction
		int dir = isLeftTuned ? -1 : 1;
		Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x + radiusCombatlRange * dir, transform.position.y, transform.position.z), Color.yellow);
		
	}
}
