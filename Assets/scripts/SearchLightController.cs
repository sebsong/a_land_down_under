using UnityEngine;
using System.Collections;

public class SearchLightController : MonoBehaviour {

	Light searchLight;

	// Use this for initialization
	void Start () {
//		searchLight = GetComponentInChildren<Light> ();
//		searchLight.intensity = 50f;
	}
	
	// Update is called once per frame
	void Update () {
		Rotate ();
	}

	void Rotate() {
		Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2 (dir.x, dir.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.back);
	}

}
