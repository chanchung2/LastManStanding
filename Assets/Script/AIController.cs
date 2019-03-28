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

    private Vector3 currPos;
    private Quaternion currRot;

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
        if(pv.isMine)
        {
            TryRandomAction();
        }
    }

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

        yield return new WaitForSeconds(Random.Range(0.0f, 5.0f));

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
        Vector3 targetPos;

        randomPos = new Vector3(Random.Range(-1.0f, 1.0f), 0,Random.Range(-1f, 1f));
        animator.SetBool("Move", true);
        navMeshAgent.speed = speed;

        targetPos = transform.position + randomPos * 10;

        //Debug.Log(randomPos);

        while (moveTime < Random.Range(6.0f, 10.0f))
        {
            moveTime += Time.deltaTime;
            navMeshAgent.SetDestination(transform.position + randomPos * 10);
            if (Vector3.Distance(targetPos, transform.position) < 1)
            {
                Debug.Log("break");
                break;
            }
            yield return null;
            //transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.smoothDeltaTime);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(randomPos - transform.position), rotationSpeed * Time.smoothDeltaTime);
        }

        navMeshAgent.speed = 0;
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
