#pragma strict

function Start () {


print (Time.time);
	// Waits 5 seconds
	yield WaitForSeconds (5);
	// Prints 5.0
	print (Time.time);

}

function Update () {

}