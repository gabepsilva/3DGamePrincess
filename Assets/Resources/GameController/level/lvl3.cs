using UnityEngine;
using System.Collections;

public class lvl3 : MonoBehaviour {

	// Use this for initialization
	int exitcount = 0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
			exitcount++;


		if(exitcount > 3)
			Application.Quit();
	
	}
}
