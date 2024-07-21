using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    private Animator animator;

    private void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player" && !animator.GetBool("collected")) {
            animator.SetBool("collected", true);
        }
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }
}