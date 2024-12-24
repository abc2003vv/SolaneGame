using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.SlotRacer;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private bool isMoving;
    private Vector2 input;
    bool isRunning;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            if (input != Vector2.zero)    
            {
                var target = transform.position;
                target.x += input.x * moveSpeed * Time.deltaTime;
                target.y += input.y * moveSpeed * Time.deltaTime;
                StartCoroutine(Move(target));
            }
        }
        // Xoay nhân vật theo hướng
        if (input.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Hướng phải
        }
        else if (input.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Hướng trái
        }
        // Kiểm tra trạng thái chạy
        isRunning = input.magnitude > 0;
        anim.SetBool("isRunning", isRunning);
    }

    IEnumerator  Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}
