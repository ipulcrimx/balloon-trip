using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Camera mainCam;

    [Space]
    public Transform topWall;
    public Transform leftWall;
    public Transform rightWall;

    public Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        int height = Screen.height;
        int width = Screen.width;

        Vector2 top = mainCam.ScreenToWorldPoint(new Vector3(width / 2, height));
        Vector2 left = mainCam.ScreenToWorldPoint(new Vector3(0, height / 2));
        Vector2 right = mainCam.ScreenToWorldPoint(new Vector3(width, height / 2));

        topWall.position = top + Vector2.up * offset.y;
        leftWall.position = left - Vector2.right * offset.x;
        rightWall.position = right + Vector2.right * offset.x;
    }
}
