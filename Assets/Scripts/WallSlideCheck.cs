using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideCheck : MonoBehaviour {
    [SerializeField] LayerMask layerMask;

    bool wallSlide = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((layerMask & (1 << collision.gameObject.layer)) != 0) {
            wallSlide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        wallSlide = false;
    }

    public bool WallSliding() {
        return wallSlide;
    }
}