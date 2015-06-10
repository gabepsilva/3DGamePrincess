using UnityEngine;
using System.Collections;

public class intro : MonoBehaviour {

	// Use this for initialization
	int exitcount = 0;

	GameObject o;
	TextMesh t;
	float timer = 10f;

	void Start () {

		o = GameObject.Find("fadePlane");
		t = GameObject.Find("ContinueTXT").GetComponent<TextMesh>();


		
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime * 0.1f;

		GameObject.Find("Objects").transform.position = Vector3.MoveTowards(transform.position, new Vector3(1036.21f,24.39165f, 961.2158f ), 1.5f*Time.deltaTime);

		if(timer >= 0)
		{
			Color a = o.GetComponent<Renderer>().material.color;
			a.a -= 0.25f * Time.deltaTime;
			o.GetComponent<Renderer>().material.color = a;
		}
		else
		{
			timer = 0;
			Destroy(o);


		}
		
		if( Input.GetMouseButtonDown(0) || Input.touches.Length > 0)
			exitcount++;
		
		
		if(exitcount > 0)
		{
			t.text = "Loading ...";
			Application.LoadLevel ("scene1");
		}
		
	}
}
