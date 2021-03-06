﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity : MonoBehaviour
{
    public bool isAffected = true;
    public bool isDisturbed = false;
    [Space]
    public GravValueScriptableObject gravityValue;
    public Transform planetTransform;

    private Rigidbody2D _rigidBody;

    /// <summary>
    /// Direction to Object from center of planet
    /// </summary>
    public Vector2 dirFromCenter
    {
        get
        {
            return (transform.position - planetTransform.position).normalized;
        }
    }

    public float distanceFromCenter
    {
        get
        {
            return (transform.position - planetTransform.position).magnitude;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = dirFromCenter;
        if (isAffected)
        {
            _rigidBody.AddForce(-transform.up * gravityValue.gravity * _rigidBody.mass);
        }
        else if (isDisturbed)
        {
            _rigidBody.AddForce(-transform.up * gravityValue.gravity * _rigidBody.mass / 2);
        }
    }
}
