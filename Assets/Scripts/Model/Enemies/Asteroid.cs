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
    public UnityAction<Transform> OnStartShoot = delegate { };

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
        OnHitPlanet += OnLand;
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
        RandomizeParameter();

        float rndX = Mathf.Sin(randomAngle);
        float rndY = Mathf.Cos(randomAngle);

        Vector2 rnd = new Vector2(rndX, rndY > 0? -rndY:rndY) * randomForce;
        Debug.Log(rnd);
        _rigidBody2d.AddForce(rnd );
        OnStartShoot(transform);
    }

    private void RandomizeParameter()
    {
        randomAngle = Random.Range(minAngle, maxAngle);
        randomForce = Random.Range(minForce, maxForce);
    }

    private void OnLand()
    {
        gameObject.SetActive(false);
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
