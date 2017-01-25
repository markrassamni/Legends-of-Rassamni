using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float moveSpeed = 6f;
	[SerializeField] private float normalSpeed = 6f;
	[SerializeField] private float boostSpeed = 10f;
	[SerializeField] private float boostLength = 10f;
	[SerializeField] private LayerMask layerMask;
	private CharacterController characterController;
	private Vector3 currentLookTarget = Vector3.zero;
	private Animator animator;
	private BoxCollider[] swordColliders;
	private GameObject fireTrail;
	private ParticleSystem fireTrailParticles;


	// Use this for initialization
	void Start () {
		fireTrail = GameObject.FindWithTag("Fire");
		fireTrail.SetActive(false);
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		swordColliders = GetComponentsInChildren<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!GameManager.instance.GameOver){
			Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
			characterController.SimpleMove(moveDirection * moveSpeed);
			
			if (moveDirection == Vector3.zero){
				animator.SetBool("IsWalking", false);
			} else {
				animator.SetBool("IsWalking", true);
			}

			if (Input.GetMouseButtonDown(0)){
				animator.Play("DoubleChop");
			}

			if (Input.GetMouseButtonDown(1)){
				animator.Play("SpinAttack");
			}
		}

	}

	void FixedUpdate(){
		
		if (!GameManager.instance.GameOver){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			Debug.DrawRay(ray.origin, ray.direction * 500, Color.red);

			if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore)){
				if (hit.point != currentLookTarget) {
					currentLookTarget = hit.point;
				}
				Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
				Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10f * Time.deltaTime);
			}
		}
	}

	public void BeginAttack(){
		foreach (var sword in swordColliders){
			sword.enabled = true;
		}
	}

	public void EndAttack(){
		foreach (var sword in swordColliders){
			sword.enabled = false;
		}
	}

	public void SpeedPowerUp(){
		StartCoroutine(FireTrailRoutine());
	}

	IEnumerator FireTrailRoutine(){
		fireTrail.SetActive(true);
		moveSpeed = boostSpeed;
		yield return new WaitForSeconds(boostLength);
		moveSpeed = normalSpeed;
		
		fireTrailParticles = fireTrail.GetComponent<ParticleSystem>();
		var emission = fireTrailParticles.emission;
		emission.enabled = false;
		yield return new WaitForSeconds(3f);
		emission.enabled = true;
		fireTrail.SetActive(false);
	}

}
