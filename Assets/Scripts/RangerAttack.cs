using UnityEngine;
using System.Collections;

public class RangerAttack : MonoBehaviour {

	[SerializeField] private float range = 10f;
	[SerializeField] private float timeBetweenAttacks = 1f;
	[SerializeField] Transform fireLocation;
	[SerializeField] private float arrowSpeed = 25f;

	private Animator animator;
	private GameObject player;
	private bool playerInRange;
	private EnemyHealth enemyHealth;
	private GameObject arrow;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		player = GameManager.instance.Player;
		StartCoroutine(Attack());
		enemyHealth = GetComponent<EnemyHealth> ();
		arrow = GameManager.instance.Arrow;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, player.transform.position) < range && enemyHealth.IsAlive){
			playerInRange = true;
			animator.SetBool("PlayerInRange", true);
			RotateToPlayer(player.transform);
		} else {
			playerInRange = false;
			animator.SetBool("PlayerInRange", false);
		}
	}

	IEnumerator Attack(){
		if (playerInRange && !GameManager.instance.GameOver){
			animator.Play("Attack");
			yield return new WaitForSeconds(timeBetweenAttacks);
		}
		yield return null;
		StartCoroutine(Attack());
	}

	private void RotateToPlayer(Transform player){
		Vector3 direction = (player.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
	}

	public void FireArrow(){
		GameObject newArrow = Instantiate(arrow) as GameObject;
		newArrow.transform.position = fireLocation.position;
		newArrow.transform.rotation = transform.rotation;
		newArrow.GetComponent<Rigidbody>().velocity = transform.forward * arrowSpeed;
	}
}
