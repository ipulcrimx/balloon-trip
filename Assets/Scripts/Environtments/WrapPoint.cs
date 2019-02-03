using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WrapPoint : MonoBehaviour
{
    public Transform wrapPoint;
    public Vector2 offset;

    public Vector2 wrapPosition
    {
        get
        {
            return (Vector2)wrapPoint.position + offset;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy")
        {
            col.transform.position = new Vector3(wrapPosition.x, col.transform.position.y, 0);
            //Debug.Break();
        }
    }
}
