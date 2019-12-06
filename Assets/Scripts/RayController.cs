using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour
{
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Vector3 _thirdPoint;
    private Vector3 _touchStartPosition;
    private Vector3 _touchEndPosition;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _pointCount = 50;
    [SerializeField] private Vector3[] _positions = new Vector3[50];
    [SerializeField] private GameObject _particlePrefab;
    private GameObject[] _particles = new GameObject[50];

    public event Action<Vector3[]> ShootBallEvent;
    
    private void Start()
    {
        var inputManager = FindObjectOfType<InputManager>();
        inputManager.TouchStartEvent += InputManagerOnTouchStartEvent;
        inputManager.TouchMoveEvent += InputManagerOnTouchMoveEvent;
        inputManager.TouchEndEvent += InputManagerOnTouchEndEvent;
        for (int i = 0; i < 50; i++)
        {
            _particles[i] = Instantiate(_particlePrefab, Vector3.zero, Quaternion.identity);
            _particles[i].SetActive(false);
        }
    }

    private void InputManagerOnTouchStartEvent(Vector2 vector2)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var star = GameObject.FindGameObjectWithTag("Star");
        var playerParent = player.transform.parent;
        player.transform.SetParent(null);
        var starParent = star.transform.parent;
        star.transform.SetParent(null);
        _startPoint = player.transform.position;
        _endPoint = star.transform.position;
        player.transform.SetParent(playerParent);
        star.transform.SetParent(starParent);

        _touchStartPosition = vector2;
        _touchEndPosition = vector2;
        _thirdPoint = new Vector3((_startPoint.x + _endPoint.x) / 2f,
            (_startPoint.y + _endPoint.y) / 2f + 2f, (_startPoint.z + _endPoint.z) / 2f);
        DrawArc();
    }

    private void InputManagerOnTouchMoveEvent(Vector2 vector2)
    {
        _touchEndPosition = vector2;
        var midPoint = Camera.main.ScreenToViewportPoint(vector2).x;
        midPoint = (midPoint - 0.5f) * 23;
        _thirdPoint = new Vector3((_startPoint.x + _endPoint.x) / 2f + midPoint,
            (_startPoint.y + _endPoint.y) / 2f + 2f, (_startPoint.z + _endPoint.z) / 2f);
        DrawArc();
    }

    private void InputManagerOnTouchEndEvent()
    {
        foreach (var particle in _particles)
        {
            particle.SetActive(false);
        }
        ShootBallEvent?.Invoke(_positions);
    }

    private void DrawArc()
    {
        for (int i = 1; i < _pointCount + 1; i++)
        {
            float t = i / (float) _pointCount;
            _positions[i - 1] = CalculateArc(t, _startPoint, _thirdPoint,  _endPoint );
            _particles[i - 1].SetActive(true);
            _particles[i - 1].transform.position = CalculateArc(t, _startPoint, _thirdPoint,  _endPoint );
        }
//        _lineRenderer.SetPositions(_positions);
    }

    private Vector3 CalculateArc(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}