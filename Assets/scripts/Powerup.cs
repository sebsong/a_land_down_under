using UnityEngine;
using System.Collections;

public abstract class Powerup : MonoBehaviour {

	/* TRUE if ability can be used, FALSE if it is a passive ability. */
	public bool IsActive { get; protected set; }

	/* Cooldown for the ability if it is an active. */
	public float Cooldown { get; protected set; }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/* Called on collection of the powerup. */
	void OnCollisionEnter2D (Collision2D coll) {
	}

	/* Upgrade ability when player collects same ability again. */
	public abstract void Upgrade ();
}
