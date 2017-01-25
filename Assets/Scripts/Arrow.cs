using UnityEngine;
using System.Collections;

public class RangerArrow : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		Destroy (gameObject);
	}

}
