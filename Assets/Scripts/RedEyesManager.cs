using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RedEyesManager : MonoBehaviour
{
    public static RedEyesManager _instance;
    Animator animator;
    [SerializeField] Transform Player;
    [SerializeField] GameObject[] TreeContainers;
    Vector3 newDirection;
    float offset = 2f;
    int lastTree = new int(), currentTree = new int();
    Vector3 direction = new Vector3();
    bool isDead = false;


    void Start()
    {
        _instance = this;
        animator = GetComponent<Animator>();
        TreeContainers = GameObject.FindGameObjectsWithTag("Tree");
        RestartEyes();
    }
    public void RestartEyes() { 
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
            if (!isDead)
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



    public void DeathLocation() {
        isDead = true;
        transform.position= Player.position + Player.forward * 2f;
        direction = transform.position - Player.position;
        newDirection = Vector3.RotateTowards(-transform.up, new Vector3(direction.x, 0, direction.z), 2, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        animator.SetTrigger("Eye");

      
    }

  
}
