using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : MonoBehaviour
{

    public Animator animator;
    public BoxCollider2D collide;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(sinkRun());
    }
    private IEnumerator sinkRun()
    {
        animator.Play("Running Sink");
        collide.enabled = true;
        yield return new WaitForSeconds(5f);
        StartCoroutine(sinkStop());
    }
    private IEnumerator sinkStop()
    {
        animator.Play("Sink Stop");
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        collide.enabled = false;
        yield return new WaitForSeconds(3f);
        StartCoroutine(sinkRun());
    }
}
