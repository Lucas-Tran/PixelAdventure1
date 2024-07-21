using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] int boxHealth;
    [SerializeField] GameObject[] allCollectables;
    [SerializeField] int collectables;
    [SerializeField] float collectableSpeed;

    public GameObject collectableParent;

    private void Start() {
        collectableParent = GameObject.FindGameObjectWithTag("CollectableParent");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player" && !animator.GetBool("hit")) {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player.GetPlayerState() == PlayerMovement.PlayerStates.falling) {
                player.Jump(8f);
            }
            animator.SetBool("hit", true);
        }
    }

    public void Hit() {
        animator.SetBool("hit", false);
        boxHealth -= 1;
        if (boxHealth <= 0) {
            for (int i = 0; i < collectables; i++) {
                GameObject newItem = Instantiate(allCollectables[Random.Range(0, allCollectables.Length)], transform.position, transform.rotation, collectableParent.transform);
                Vector2 randomUnitVector = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
                Rigidbody2D newRigidBody = newItem.GetComponent<Rigidbody2D>();
            }
            Destroy(gameObject);
        }
    }
}
