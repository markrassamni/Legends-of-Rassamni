using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	[SerializeField] private int startingHealth = 20;
	[SerializeField] private float timeBetweenHits = .5f;
	[SerializeField] private float despawnSpeed = 2f;

	private AudioSource audioSource;
	private ParticleSystem blood;
	private float timer = 0f;
	private Animator animator;
	private NavMeshAgent nav;
	private bool isAlive;
	private Rigidbody rigidBody;
	private CapsuleCollider capsuleCollider;
	private bool readyToDespawn = false;
	private int currentHealth;

	public bool IsAlive {
		get {
			return isAlive;
		}
	}

	// Use this for initialization
	void Start () {
		GameManager.instance.RegisterEnemy(this);
		rigidBody = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
		nav = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		isAlive = true;
		currentHealth = startingHealth;
		blood = GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (readyToDespawn){
			transform.Translate(Vector3.down * despawnSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider other){
		if (timer >= timeBetweenHits && !GameManager.instance.GameOver && other.tag == "PlayerWeapon"){
			TakeHit();
			blood.Play();
			timer = 0;
		}
	}

	void TakeHit(){
		if (currentHealth > 0){
			audioSource.PlayOneShot(audioSource.clip);
			animator.Play("Hurt");
			currentHealth -= 10;
		} else if (currentHealth <= 0){
			isAlive = false;
			KillEnemy();
		}
	}

	void KillEnemy(){
		GameManager.instance.KilledEnemy(this);
		capsuleCollider.enabled = false;
		nav.enabled = false;
		animator.SetTrigger("EnemyDie");
		rigidBody.isKinematic = true;
		StartCoroutine(DespawnEnemy());
	}

	IEnumerator DespawnEnemy(){
		yield return new WaitForSeconds(4f);
		readyToDespawn = true;
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}

}
