using UnityEngine;
using System.Collections;

public class Shrink : Powerup {

	// Use this for initialization
	void Start () {
		IsPassive = false;
		Cooldown = 3f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Upgrade() {
	}
}
