using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float currentSpeed = 3f; // 이동 속도

    private float maxSpeed = 0.5f;
    private float minSpeed = 0.1f;

    private float direction = -1.0f; // 1 또는 -1


    private void Start()
    {

    }

    private void Update() 
    {
        if (!GameManager.instance.isGameover)
        {
            transform.Translate(Vector3.left * currentSpeed * direction * Time.deltaTime);
        }
    }
}
