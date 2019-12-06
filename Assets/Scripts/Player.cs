using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private RayController _rayController;
    [SerializeField] private float _speed;
    private Rigidbody _rigidbody;
    [SerializeField] private bool _useRigidbody;
    private int _selectedMaterial;
    private Material[] _wallColors;

    private void Start()
    {
        var colors = GameManager.instance.PlayerColors;
        _wallColors = GameManager.instance.WallColors;
        _selectedMaterial = Random.Range(0, colors.Length);
        var currentMaterial = colors[_selectedMaterial];
        GetComponent<MeshRenderer>().material = currentMaterial;
        GetComponent<TrailRenderer>().material.color = currentMaterial.color;
    }

    private void Update()
    {
        if (transform.position.x < GameManager.instance.Min || transform.position.x > GameManager.instance.Max)
        {
            GameManager.instance.RequestGameOverEvent();
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        _rayController = FindObjectOfType<RayController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rayController.ShootBallEvent += OnShootBallEvent;
    }

    private void OnDisable()
    {
        _rayController.ShootBallEvent -= OnShootBallEvent;
    }
    
    /// <summary>
    /// Executes events that should run on ShootBallEvent.
    /// </summary>
    /// <param name="obj"></param>
    private void OnShootBallEvent(Vector3[] obj)
    {
        transform.GetComponentInChildren<ParticleSystem>().Play();
        StartCoroutine(BallMoveAnimation(obj));
    }

    /// <summary>
    /// Moving ball coroutine.
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    IEnumerator BallMoveAnimation(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (_useRigidbody)
            {
                _rigidbody.MovePosition(positions[i]);
            }
            else
            {
                gameObject.transform.position = positions[i];
            }
            yield return new WaitForSeconds(_speed * Time.fixedDeltaTime);
        }
    }
    
    /// <summary>
    /// Topun herhangi bir yere temas etmesi
    /// </summary>
    /// <param name="triggerCollider"></param>
    private void OnCollisionEnter(Collision triggerCollider)
    {
        if (triggerCollider.transform.CompareTag("Star"))
        {
            var targetMaterial = triggerCollider.gameObject.GetComponent<MeshRenderer>().material;
            int wallMaterialId = -1;
            for (int i = 0; i < _wallColors.Length; i++)
            {
                if (_wallColors[i].color == targetMaterial.color)
                {
                    wallMaterialId = i;
                }
            }
            GameObject.Find("Konfeti").GetComponent<ParticleSystem>().Play();
            if (wallMaterialId == _selectedMaterial)
            {
                GameManager.instance.RequestSectionClearedEvent();
            }
            else
            {
                GameManager.instance.RequestGameOverEvent();
            }
            Destroy(gameObject);
        }
        else if (triggerCollider.transform.CompareTag("Wall"))
        {
            GameManager.instance.RequestGameOverEvent();
            Destroy(gameObject);
        }
    }
}
