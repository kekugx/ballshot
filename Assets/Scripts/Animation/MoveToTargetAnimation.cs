using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetAnimation : MonoBehaviour
{
    public Vector3 targetVector3;
    public float moveSpeed;
    public bool useLocalPosition = false;
    private bool _canMove;
    private Vector3 _transformTargerPosition;

    private void OnEnable()
    {
        _canMove = true;
    }

    private void Update()
    {
        if (_canMove)
        {
            _transformTargerPosition = Vector3.MoveTowards(transform.position,
                targetVector3, moveSpeed * Time.deltaTime);
            
            if (useLocalPosition)
            {
                transform.localPosition = _transformTargerPosition;
            }
            else
            {
                transform.position = _transformTargerPosition;
            }
            
            if (Vector3.Distance(useLocalPosition ? transform.localPosition : transform.position, targetVector3) < 0.001f)
            {
                if (useLocalPosition)
                {
                    transform.localPosition = targetVector3;
                }
                else
                {
                    transform.position = targetVector3;
                }
                
                _canMove = false;
                Destroy(this);
            }
        }

    }
}