using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	/* Current health of the player. */
	public float Health { get; private set; }

	/* The depth of the player. */
	public float Depth { get; private set; }

	/* How fast the player is propelled. */
	public float Speed { get; private set; }

	/* True if Player is in small state, false otherwise. */
	bool isSmall;

	/* Set of abilities that the player currently posesses. */
	private HashSet<Powerup> abilities;

	/* Player's rigidbody componennt. */
	private Rigidbody2D rb;

	/* Player's cabin light. */
	private Light cabinLight;


	/* Powerups */

	bool canShrink;

	/* Camera Transform */
//	private Transform camera;

	/* How far the camera is offset from the player. */
//	private float cameraOffset;

	// Use this for initialization
	void Start () {
		Health = 100f;
		Depth = 0f;
		Speed = 4f;
		isSmall = false;
		abilities = new HashSet<Powerup> ();
		rb = GetComponent<Rigidbody2D> ();
		cabinLight = GameObject.FindGameObjectWithTag ("CabinLight").GetComponent<Light> ();
//		camera = GameObject.FindGameObjectWithTag ("MainCamera").transform;
//		cameraOffset = Vector3.Distance (transform.position, camera.position);
		canShrink = false;
	}
	
	// Update is called once per frame
	void Update () {

		/* Health check. */
		if (Health < 0) {
			//death animation
			//game over screen
		}

		/* Player movement. */
		float horizontalForce = -Input.GetAxis ("Horizontal");
		float verticalForce = -Input.GetAxis ("Vertical");
		rb.AddForce (new Vector2 (horizontalForce, verticalForce) * Speed);

		/* Player abilities. */
		if (canShrink && Input.GetKeyDown ("space")) {
			if (isSmall) {
				transform.localScale *= 5;
				cabinLight.range *= 5;
			} else {
				transform.localScale /= 5;
				cabinLight.range /= 5;
			}
			isSmall = !isSmall;
		}

		/* Ambient lighting control. */
		RenderSettings.ambientLight += Color.black * Time.deltaTime;

		/* Camera control. */
//		cameraOffset = Vector3.Distance (transform.position, camera.position);
	}

	/* Reduce Health by DMG. */
	public void TakeDamage (float dmg) {
		Health -= dmg;
	}

	public void AddAbility (Powerup ability) {
		if (!abilities.Contains (ability)) {
			abilities.Add (ability);
		} else {
			ability.Upgrade ();
		}
	}

	void OnTriggerEnter2D (Collider2D coll) {
		print (coll.tag);
		switch (coll.tag) {
		case ("Shrink"):
			canShrink = true;
			break;
		default:
			break;
		}
	}
}
