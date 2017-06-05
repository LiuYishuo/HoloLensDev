using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeNumber : MonoBehaviour {

	public TextMesh numberMesh;
	private int currNum;

	// Use this for initialization
	void Start () {
		numberMesh.text = "0";
		currNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			currNum = currNum - 1;
			if (currNum < 0)
				currNum = 10 + currNum;
			numberMesh.text = currNum.ToString ();
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			currNum = (currNum + 1) % 10;
			numberMesh.text = currNum.ToString ();
		}
	}
}
