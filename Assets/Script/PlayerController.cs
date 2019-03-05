using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private float h;
    private float v;

    private Vector3 horizontal;
    private Vector3 vertical;
    private Vector3 movement;
    private Quaternion quaternion;

    [SerializeField] private float range; // 공격 사거리
    [SerializeField] private LayerMask layerMask;

    private bool isMove = false;
    private bool isLife = true;
    private bool isAttack = false;

    private Rigidbody rigidbody;
    private BoxCollider boxCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform camera;

    private RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLife)
        {
            Move();
            TryAttack();
        }
    }

    private void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            isMove = true;
            quaternion.eulerAngles = new Vector3(0, camera.rotation.eulerAngles.y, 0);
        }
        else
            isMove = false;

        if (v > 0)
        {
            vertical = camera.transform.forward;
        }
        else if (v < 0)
        {
            vertical = -camera.transform.forward;
            quaternion.eulerAngles += new Vector3(0, 180, 0);
        }
        else
        {
            vertical = Vector3.zero;
        }

        if (h > 0)
        {
            horizontal = camera.transform.right;
            if (v > 0)
            {
                quaternion.eulerAngles += new Vector3(0, 45, 0);
            }
            else if (v < 0)
            {
                quaternion.eulerAngles += new Vector3(0, 315, 0);
            }
            else
            {
                quaternion.eulerAngles += new Vector3(0, 90, 0);
            }
        }
        else if (h < 0)
        {
            horizontal = -camera.transform.right;
            if (v > 0)
            {
                quaternion.eulerAngles += new Vector3(0, 315, 0);
            }
            else if (v < 0)
            {
                quaternion.eulerAngles += new Vector3(0, 45, 0);
            }
            else
            {
                quaternion.eulerAngles += new Vector3(0, 270, 0);
            }
        }
        else
        {
            horizontal = Vector3.zero;
        }

        animator.SetBool("Move", isMove);
        movement = (horizontal + vertical) * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + movement);

        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, rotationSpeed * Time.deltaTime);
    }

    private void TryAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            isAttack = false;
        }

        if (!isAttack)
        {
            if (Input.GetButton("Fire1"))
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        isAttack = true;
        animator.SetTrigger("Attack");

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
            hitInfo.transform.GetComponent<PlayerController>().Die();
        }
    }

    private void Die()
    {
        isLife = false;
        animator.SetBool("Die", true);
    }
}

