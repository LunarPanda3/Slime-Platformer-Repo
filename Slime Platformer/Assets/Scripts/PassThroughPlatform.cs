using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    private Collider2D col;
    private bool playerOnPlatform;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<PlayerMovementScript>();
        if (player != null) {
            playerOnPlatform = value;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        SetPlayerOnPlatform(col, true);
    }

    void OnCollisionExit2D(Collision2D col) 
    {
        SetPlayerOnPlatform(col, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            col.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }
}
