using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using Random = UnityEngine.Random;

public class GlassHolder : MonoBehaviour
{
    [SerializeField] private GameObject glass, brokenGlass;
    private GameManager _gameManager;
    private Material[] _materials;
    private int _currentMaterial;
    [SerializeField] private float colorChangeTime = 1f;
    private float _lastMaterialUpdateTime;
    private bool _ChangeColor = true;
    private RayController _rayController;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rayController = FindObjectOfType<RayController>();
        _materials = _gameManager.WallColors;
        _currentMaterial = Random.Range(0, _gameManager.WallColors.Length);
        var selectedMaterial = _materials[_currentMaterial];
        ChangeMaterial(selectedMaterial);
        
        _rayController.ShootBallEvent += OnShootBallEvent;
    }

    private void OnShootBallEvent(Vector3[] obj)
    {
        _ChangeColor = false;
    }

    private void ChangeMaterial(Material selectedMaterial)
    {
        if (_ChangeColor)
        {
            glass.GetComponent<MeshRenderer>().material = selectedMaterial;
            for (int i = 0; i < brokenGlass.transform.childCount; i++)
            {
                brokenGlass.transform.GetChild(i).GetComponent<MeshRenderer>().material = selectedMaterial;
            }
        }
    }

    private void Update()
    {
        if (Time.time - _lastMaterialUpdateTime  > colorChangeTime)
        {
            _currentMaterial = _currentMaterial == _materials.Length - 1 ? 0 : _currentMaterial + 1;
            var nextMaterial = _materials[_currentMaterial];
            ChangeMaterial(nextMaterial);
            _lastMaterialUpdateTime = Time.time;
        }
    }

    private void OnEnable()
    {
        _gameManager.SectionClearedEvent += InstanceOnSectionClearedEvent;
    }

    private void OnDestroy()
    {
        _gameManager.SectionClearedEvent -= InstanceOnSectionClearedEvent;
    }

    private void InstanceOnSectionClearedEvent()
    {
        glass.SetActive(false);
        brokenGlass.SetActive(true);
        Vector3 explosionPosition = transform.position;
        Rigidbody[] rbs = brokenGlass.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(10f, explosionPosition, 5f, 1f,ForceMode.Impulse );
        }
        Destroy(gameObject, 2f);
    }
}