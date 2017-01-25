using UnityEngine;
using System.Collections;

public class SpeedPowerUp : MonoBehaviour {


	private GameObject player;
	private PlayerController playerController;


	// Use this for initialization
	void Start () {
		player = GameManager.instance.Player;
		playerController = player.GetComponent<PlayerController>();
		GameManager.instance.RegisterPowerUp();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if (other.gameObject == player){
			playerController.SpeedPowerUp();
			Destroy(gameObject);
		}
	}



}
