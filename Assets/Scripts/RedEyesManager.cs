using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEyesManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform Player;
    [SerializeField] GameObject[] TreeContainers;
    Vector3 newDirection;
    float offset = 2f;
    int lastTree = new int(), currentTree = new int();
    Vector3 direction = new Vector3();
 
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        TreeContainers = GameObject.FindGameObjectsWithTag("Tree");
        currentTree = Random.Range(0, TreeContainers.Length);
        StopCoroutine(SpawnEyes());
        StartCoroutine(SpawnEyes());
    }


    void SetPosition() {

        currentTree = Random.Range(0, TreeContainers.Length);
        if (lastTree == currentTree)
            SetPosition();

        lastTree = currentTree;

        transform.position = TreeContainers[currentTree].transform.position + (Player.position.normalized * offset);
    }



    IEnumerator SpawnEyes() {

      
        float duration = 5f;
        while (true)
        {

            yield return new WaitForSeconds(duration);
            SetPosition();

           
            animator.SetTrigger("Eye");
 
        }
    }

    private void FixedUpdate()
    {
        SetRotaion();
    }
    void SetRotaion() {
        direction = transform.position - Player.position;
        newDirection = Vector3.RotateTowards(-transform.up, new Vector3(direction.x, 0, direction.z), 2, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
