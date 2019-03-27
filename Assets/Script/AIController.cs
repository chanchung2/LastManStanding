using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    enum AIAction { Wait, Move };

    private bool isAction = false;

    private Vector3 randomPos;

    private Rigidbody rigidbody;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    private float moveTime = 0;

    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;

    public PhotonView pv;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        TryRandomAction();
    }

    [PunRPC]
    private void TryRandomAction()
    {
        if (!isAction)
        {
            StartCoroutine(ActionTimeCoroutine());
        }
    }

    IEnumerator ActionTimeCoroutine()
    {
        isAction = true;

        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));

        RandomAction();
    }

    private void RandomAction()
    {
        int i;

        i = 1;
        //i = Random.Range(0, 2);

        switch (i)
        {
            case (int)AIAction.Wait:
                Wait();
                break;
            case (int)AIAction.Move:
                Move();
                break;
        }
    }

    private void Wait()
    {
        Debug.Log("Wait");
        isAction = false;
    }
    private void Move()
    {
        StartCoroutine(MoveCoroutine());
        //transform.rotation = Quaternion.Slerp(transform.rotation,
       
        //rigidbody.MovePosition(transform.position + movement);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, rotationSpeed * Time.smoothDeltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, rotationSpeed * Time.smoothDeltaTime);
    }

    IEnumerator MoveCoroutine()
    {
        randomPos = new Vector3(Random.Range(-10.0f, 10.0f), 0,Random.Range(-10.0f, 10.0f));
        animator.SetBool("Move", true);
        navMeshAgent.speed = speed;

        Debug.Log(randomPos);

        while (moveTime < Random.Range(6.0f, 10.0f))
        {
            Debug.Log(Vector3.Distance(transform.position, randomPos));
            moveTime += Time.deltaTime;
            navMeshAgent.SetDestination(transform.position + randomPos);

            //transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.smoothDeltaTime);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(randomPos - transform.position), rotationSpeed * Time.smoothDeltaTime);

            yield return null;
        }

        moveTime = 0;
        animator.SetBool("Move", false);
        isAction = false;
    }

    [PunRPC]
    private void Die()
    {
        animator.SetBool("Die", true);
    }
}
