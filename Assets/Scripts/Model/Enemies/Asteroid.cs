using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    public float rotateSpeed;

    [Header("Random Angle")]
    public float randomAngle;
    public float minAngle;
    public float maxAngle;

    [Header("Fall Force")]
    public float randomForce;
    public float minForce;
    public float maxForce;

    public UnityAction OnHitPlanet = delegate { };
    public UnityAction OnStartShoot = delegate { };

    private bool _isActive;
    private Transform _childSprite;
    private Rigidbody2D _rigidBody2d;

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();

        if(!_childSprite)
            _childSprite = transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomizeParameter();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            if (_childSprite)
            {
                _childSprite.Rotate(new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Planet")
        {
            OnHitPlanet();
        }
    }

    public void Shoot()
    {
        float rndX = Mathf.Sin(Mathf.Deg2Rad * randomAngle);
        float rndY = Mathf.Cos(Mathf.Deg2Rad * randomAngle);

        _rigidBody2d.AddForce(new Vector2(rndX, rndY) * randomForce);
        OnStartShoot();
    }

    private void RandomizeParameter()
    {
        randomAngle = Random.Range(minAngle, maxAngle);
        randomForce = Random.Range(minForce, maxForce);
    }

    private void OnDisable()
    {
        _isActive = false;
    }

    private void OnEnable()
    {
        _isActive = true;
    }

    private void OnDestroy()
    {
        OnHitPlanet = null;
        OnStartShoot = null;
    }
}
