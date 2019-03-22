using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    enum AIAction { Wait, Move };

    private bool isAction = false;

    [SerializeField] private Animator animator;
    public PhotonView pv;

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

        yield return new WaitForSeconds(Random.Range(0, 3.0f));

        RandomAction();

        isAction = false;
    }

    private void RandomAction()
    {
        int i;

        Debug.Log("ang");
        i = Random.Range(0, 2);

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
    }
    private void Move()
    {
        Debug.Log("Move");
    }

    [PunRPC]
    private void Die()
    {
        animator.SetBool("Die", true);
    }
}
