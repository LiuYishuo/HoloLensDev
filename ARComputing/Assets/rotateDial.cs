using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateDial : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate( 50f * Time.deltaTime, 0, 0);
		}
		else if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate( (-1)*50f * Time.deltaTime, 0, 0);
		}
	}
}
