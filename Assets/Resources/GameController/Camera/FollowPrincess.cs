using UnityEngine;
using System.Collections;

public class FollowPrincess : MonoBehaviour {

	public static byte distancez = 25;
	public static int distancey = -150;
	public static byte distancex = 0;


	//bool next = true;


	GameObject[] princess;

	// Use this for initialization
	void Start () {

		Application.targetFrameRate = 33;

		princess = GameObject.FindGameObjectsWithTag("princess");

		//look to object slowly
		//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(princess[0].transform.position - transform.position), 1*Time.deltaTime);
		
		transform.LookAt (princess [0].transform.position);
		
		
		
		
	
	}
	
	// Update is called once per frame
	void Update () {

		//transform.position = new Vector3(princess[0].transform.position.x, transform.position.y, princess[0].transform.position.z + distancey);

		//if(next)
			transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(princess[0].transform.position.x, transform.position.y, princess[0].transform.position.z + distancey), 10*Time.deltaTime);
			

		//next = !next;




	
	}
}
