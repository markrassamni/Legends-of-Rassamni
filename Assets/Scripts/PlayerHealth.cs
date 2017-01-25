using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] private int startingHealth = 100;
	[SerializeField] private float timeBetweenHits = 2f;
	[SerializeField] private Slider healthSlider;

	private float timer = 0f;
	private CharacterController characterController;
	private Animator animator;
	private int currentHealth;
	private AudioSource audioSource;
	private ParticleSystem blood;



	// public int CurrentHealth {
	// 	get { 
	// 		return currentHealth;
	// 	} set {
	// 		if (value < 0){
	// 			currentHealth = 0;
	// 		} else {
	// 			currentHealth = value;
	// 		}
	// 	}
	// }


	public int CurrentHealth {
		get { 
			return currentHealth;
		} 
	}

	public int StartingHealth {
		get { 
			return startingHealth;
		} 
	}




	void Awake(){
		Assert.IsNotNull(healthSlider);
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		characterController = GetComponent<CharacterController>();
		currentHealth = startingHealth;
		audioSource = GetComponent<AudioSource> ();
		blood = GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Weapon" && timer >= timeBetweenHits && !GameManager.instance.GameOver){
			TakeHit();
			blood.Play();
			timer = 0;
		}
	}

	void TakeHit(){
		if (currentHealth > 0){
			GameManager.instance.PlayerHit(currentHealth);
			animator.Play("Hurt");
			currentHealth -= 10;
			healthSlider.value = currentHealth;
			audioSource.PlayOneShot(audioSource.clip);
		}

		if (currentHealth <= 0){
			KillPlayer();
		}
	}

	void KillPlayer(){
		GameManager.instance.PlayerHit(currentHealth);
		animator.SetTrigger("HeroDie");
		characterController.enabled = false;
	}

	public void PowerUpHealth(){
		if (currentHealth <= startingHealth - 30){
			currentHealth += 30;
		} else {
			currentHealth = startingHealth;
		}
		healthSlider.value = currentHealth;
	}


}
