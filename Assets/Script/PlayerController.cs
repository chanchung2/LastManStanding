using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.PunBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private float h;
    private float v;

    private Vector3 horizontal;
    private Vector3 vertical;
    private Vector3 movement;
    private Quaternion quaternion;
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    [SerializeField] private float range; // 공격 사거리
    [SerializeField] private LayerMask layerMask;

    private bool isMove = false;
    private bool isLife = true;
    private bool isAttack = false;

    private Rigidbody rigidbody;
    private BoxCollider boxCollider;
    private Transform camera;
    private Animator animator;

    private RaycastHit hitInfo;
    private PhotonView pv = null;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();

        pv = GetComponent<PhotonView>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;   // 데이터 전송타입
        pv.ObservedComponents[0] = this;  // 스크립트 연결
        currPos = transform.position;
        currRot = transform.rotation;

        if (pv.isMine)
        {
            camera = Camera.main.transform;
            Camera.main.GetComponent<MainCamera>().player = transform.GetChild(1).GetComponent<Transform>(); // CameraPos;
        }
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
        if (pv.isMine)
        {
            if (!isAttack)
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
                movement = (horizontal + vertical) * speed * Time.smoothDeltaTime;

                rigidbody.MovePosition(transform.position + movement);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, rotationSpeed * Time.smoothDeltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, rotationSpeed * Time.smoothDeltaTime);
            }
        }
        else  // 원격 플레이어 수신 받은 위치
        {
            //Debug.Log(currPos);
            transform.position = Vector3.Lerp(transform.position, currPos, speed * Time.smoothDeltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, currRot, 500f * Time.smoothDeltaTime);
            //rigidbody.MovePosition(transform.position + (currPos - transform.position) * speed * Time.smoothDeltaTime);
             //transform.rotation = Quaternion.Slerp(transform.rotation, currRot, rotationSpeed * Time.smoothDeltaTime);
        }
    }

    private void TryAttack()
    {
        if (pv.isMine)
        {
            if (!isAttack)
            {
                if (Input.GetButton("Fire1"))
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    [PunRPC]
    private void Attack()
    {
        Debug.Log("ang");
        animator.SetTrigger("Attack");

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "AIPlayer")
            {
                hitInfo.transform.GetComponent<AIController>().pv.RPC("Die", PhotonTargets.All, null);
            }
            else
            {
                hitInfo.transform.GetComponent<PlayerController>().pv.RPC("Die", PhotonTargets.All, null);
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        pv.RPC("Attack", PhotonTargets.All, null);

        yield return new WaitForSeconds(1.1f);

        isAttack = false;
    }

    [PunRPC]
    private void Die()
    {
        animator.SetBool("Die", true);
        isLife = false;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else // 원격 플레이어 위치 정보 수신
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}

