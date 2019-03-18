using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public PhotonView pv;

    [PunRPC]
    private void Die()
    {
        animator.SetBool("Die", true);
    }
}
