using UnityEngine;
using System.Collections;

public class ScrCal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static float PWidth(float percent)
	{
		return ((Screen.width/100)*percent);
	}

	public static float PHeight(float percent)
	{
		return ((Screen.height/100)*percent);
	}
}
