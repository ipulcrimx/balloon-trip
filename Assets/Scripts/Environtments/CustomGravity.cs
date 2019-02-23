using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity : MonoBehaviour
{
    public bool isAffected = true;
    [Space]
    public GravValueScriptableObject gravityValue;
    public Transform planetTransform;

    private Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAffected)
        {
            transform.up = (transform.position - planetTransform.position).normalized;

            _rigidBody.AddForce(-transform.up * gravityValue.gravity * _rigidBody.mass);
        }
    }
}
