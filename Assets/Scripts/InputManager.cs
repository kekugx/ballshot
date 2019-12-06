using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> TouchStartEvent;
    public event Action<Vector2> TouchMoveEvent;
    public event Action TouchEndEvent;
    private bool _inputSession;
    public bool AbleToInput { get; set; }

    private void Start()
    {
        AbleToInput = true;
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.touchCount > 0 && AbleToInput)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                TouchStartEvent?.Invoke(touch.position);
                _inputSession = true;
            }

            if (_inputSession)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    TouchMoveEvent?.Invoke(touch.position);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    AbleToInput = false;
                    TouchEndEvent?.Invoke();
                    _inputSession = false;
                }
            }
        }
    }
}
