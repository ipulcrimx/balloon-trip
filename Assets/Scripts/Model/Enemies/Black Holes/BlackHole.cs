using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BlackHole : MonoBehaviour
{
    public float moveSpeed;
    [Space]
    public float damageArea = 15f;
    public float suckPower;

    private GameManager _gameManager;
    private AsteroidManager _asteroidManager;

    private Player _player;
    private Alien[] _aliens;
    private GameObject[] _asteroids
    {
        get
        {
            return _asteroidManager.asteroids;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

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
            Transform tr = col.transform;

            tr.position = Vector3.Lerp(tr.position, transform.position, Time.deltaTime * suckPower);
            tr.GetComponent<CustomGravity>().isDisturbed = true;

            if (Vector2.Distance(tr.position, transform.position) <= damageArea)
            {
                Destroy(tr.gameObject);
            }
        }
        else if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {
            Transform tr = col.transform;
            tr.position = Vector3.Lerp(tr.position, transform.position, Time.deltaTime * suckPower);
        }
        else if (col.gameObject.tag == "Asteroid")
        {
            Transform tr = col.transform;
            tr.position = Vector3.Lerp(tr.position, transform.position, (Time.deltaTime * 3) * suckPower);

            if (Vector2.Distance(tr.position, transform.position) <= damageArea)
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
           col.gameObject.tag == "Player"||
           col.gameObject.tag == "Enemy" || col.gameObject.tag == "Enemies")
        {
            col.gameObject.GetComponent<CustomGravity>().isDisturbed = false;
        }
    }
}