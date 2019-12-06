using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraMovement[] _cameraMovements;
    private int currentPosition = 0;
    private bool _canMove;
    private InputManager _inputManager;
    private bool _checkPosition;

    private void Start()
    {
        _inputManager = FindObjectOfType<InputManager>();
        GameManager.instance.SectionClearedEvent += InstanceOnSectionClearedEvent;
    }

    private void Update()
    {
        if (_canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                _cameraMovements[currentPosition].cameraPosition.position,
                _cameraMovements[currentPosition].moveSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, _cameraMovements[currentPosition].cameraPosition.position) < 0.001f &&
            _checkPosition)
        {
            _checkPosition = false;
            transform.position = _cameraMovements[currentPosition].cameraPosition.position;
            _canMove = false;
            _inputManager.AbleToInput = true;
        }
    }

    private void InstanceOnSectionClearedEvent()
    {
        currentPosition++;
        if (currentPosition == _cameraMovements.Length)
        {
            return;
        }

        _checkPosition = true;
        _canMove = true;
        _inputManager.AbleToInput = false;
    }
}

[Serializable]
public struct CameraMovement
{
    public Transform cameraPosition;
    public float moveSpeed;
}