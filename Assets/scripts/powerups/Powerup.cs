using UnityEngine;
using System.Collections;

public abstract class Powerup : MonoBehaviour {

	/* TRUE if ability is a passive ability, FALSE if can be used for an active effect. */
	public bool IsPassive { get; protected set; }

	/* Cooldown for the ability if it is an active. */
	public float Cooldown { get; protected set; }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/* Called on collection of the powerup. */
	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "Player") {
			gameObject.SetActive (false);
		}
	}

	/* Upgrade ability when player collects same ability again. */
	public abstract void Upgrade ();
}
