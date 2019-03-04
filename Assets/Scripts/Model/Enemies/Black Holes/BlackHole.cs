using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BlackHole : MonoBehaviour
{
    public float moveSpeed;
    [Space]
    public float damageArea = 15f;
    public float suckPower;
    [Header("For Player Only")]
    public float[] distanceLevels;

    private float _currentPower = 0;
    private GameManager _gameManager;
    private AsteroidManager _asteroidManager;

    // Start is called before the first frame update
    void Start()
    {
        _currentPower = suckPower;
    }

    // Update is called once per frame
    void Update()
    {
        float move = CrossPlatformInputManager.GetAxis("Horizontal");
        transform.Translate(Vector3.right * move * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _currentPower += Time.deltaTime;
            Transform tr = col.transform;
            Player pl = tr.GetComponent<Player>();

            pl.OnEnterBlackHole();
            if (pl.distanceFromInitialPosition < distanceLevels[0])
            {
                tr.position = Vector3.Lerp(tr.position, (Vector2)tr.position + GetDirection(tr.position), Time.deltaTime * _currentPower);
            }
            else if (pl.distanceFromInitialPosition < distanceLevels[1])
            {
                tr.position = Vector3.Lerp(tr.position, (Vector2)tr.position + GetDirection(tr.position), Time.deltaTime * _currentPower / 3);
            }
            else if (pl.distanceFromInitialPosition < distanceLevels[2])
            {
                tr.position = Vector3.Lerp(tr.position, (Vector2)tr.position + GetDirection(tr.position), Time.deltaTime * _currentPower / 5);
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, (Vector2)tr.position + GetDirection(tr.position), Time.deltaTime * _currentPower / 30);
            }

            tr.GetComponent<CustomGravity>().isDisturbed = true;

            if (Vector2.Distance(tr.position, transform.position) <= damageArea)
            {
                Destroy(tr.gameObject);
            }
        }
        else if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {
            _currentPower += Time.deltaTime;
            Transform tr = col.transform;
            tr.position = Vector3.Lerp(tr.position, transform.position, Time.deltaTime * _currentPower);
        }
        else if (col.gameObject.tag == "Asteroid")
        {
            _currentPower += Time.deltaTime;
            Transform tr = col.transform;
            tr.position = Vector3.Lerp(tr.position, transform.position, (Time.deltaTime * 1.75f) * _currentPower);

            if (Vector2.Distance(tr.position, transform.position) <= damageArea * 2.25f)
            {
                Destroy(tr.gameObject);
            }
        }
        else
        {
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Asteroid" ||
           col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {
            col.gameObject.GetComponent<CustomGravity>().isDisturbed = false;
        }
        else if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<CustomGravity>().isDisturbed = false;
            col.gameObject.GetComponent<Player>().OnExitBlackHole();
        }

        _currentPower = suckPower;
    }

    private Vector2 GetDirection(Vector2 startPos)
    {
        Vector2 dir = (Vector2)transform.position - startPos;
        float length = dir.magnitude;
        dir = dir.normalized;


        return dir * (length * 2.5f);
    }
}