using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

	/* Current health of the enemy. */
	public float Health { get; private set; }

	/* How fast the enemy can move. */
	public float Speed { get; private set; }

	/* How much health the player gains by consuming the enemy. */
	public float ConsumeHealth { get; private set; }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Health <= 0) {
			//death animation
			gameObject.SetActive(false);
		}
	
	}

	public void TakeDamage (float dmg) {
		Health -= dmg;
	}
}
