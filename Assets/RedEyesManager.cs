using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEyesManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform Player;
    [SerializeField] Transform[] TreeContainers;
    Vector3 newDirection;
    float offset = 1f;
    int lastTree = new int(), currentTree = new int();
    Vector3 direction = new Vector3();
    float duration = 3f;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentTree = Random.Range(0, TreeContainers.Length);
        transform.position = TreeContainers[currentTree].position;
        StopCoroutine(SpawnEyes());
        StartCoroutine(SpawnEyes());
    }


    void SetPosition() {

        currentTree = Random.Range(0, TreeContainers.Length);
        if (lastTree == currentTree)
            SetPosition();

        lastTree = currentTree;

        transform.position = TreeContainers[currentTree].position;
    }



    IEnumerator SpawnEyes() {
        float time;
      
        float duration = 3f;
        while (true)
        {

            yield return new WaitForSeconds(duration);
            SetPosition();

           
            animator.SetTrigger("Eye");
            time = Time.time;

            while (time + duration > Time.time)
            {
                yield return null;
                SetRotaion();
            }
        }
    }


    void SetRotaion() {
        direction = transform.position - Player.position;
        newDirection = Vector3.RotateTowards(-transform.up, new Vector3(direction.x, 0, direction.z), 2, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
