using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private float dist = 1.5f;

    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;

    [SerializeField] private float yMinLimit;
    [SerializeField] private float yMaxLimit;

    [SerializeField] private Transform player;

    private float x; // 카메라의 초기값.
    private float y;

    Quaternion rotation;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.eulerAngles.y;
        y = transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {

        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        rotation = Quaternion.Euler(y, x, 0);
        position = rotation * new Vector3(0, 0, -dist) + player.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
