using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {



	public AudioClip sd;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PlaySd(AudioClip sd){
	
		
		GetComponent<AudioSource>().PlayOneShot(sd, 0.1F);
	
	}
}
