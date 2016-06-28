using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameObject player;
	public float cameraHeight = 5f;
	public float mousePanRestriction = 200f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
//		Camera.main.orthographicSize = cameraHeight;
	}

	// Update is called once per frame
	void Update () {
		Vector3 mouseOffset = (Vector3) (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position));
		transform.position = player.transform.position + mouseOffset / mousePanRestriction + Vector3.back * cameraHeight;
	}

}
