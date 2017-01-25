using UnityEngine;

public class EnemyMove : MonoBehaviour {

	private Transform player;

	private NavMeshAgent nav;
	private Animator animator;
	private EnemyHealth enemyHealth;


	// Use this for initialization
	void Start () {
		player = GameManager.instance.Player.transform;
		nav = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		enemyHealth = GetComponent<EnemyHealth> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.GameOver && enemyHealth.IsAlive){
			nav.SetDestination(player.position);
		} else if (!enemyHealth.IsAlive){
			nav.enabled = false;
		} else if (GameManager.instance.GameOver && enemyHealth.IsAlive){
			animator.Play("Idle");
			nav.enabled = false;
		}
		
	}


}
