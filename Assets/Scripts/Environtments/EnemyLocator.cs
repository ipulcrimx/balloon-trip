using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocator : MonoBehaviour
{
    public Camera mainCam;

    [Space]
    [Tooltip("Padding will use value in pixel")]
    public Vector2 detectorPaddingPosition;
    public Transform detectorObject;
    public float lerpDuration = 2f;

    [Space]
    public Transform[] aliens;

    private bool _isActive = false;
    private Vector2 _halfScreen = Vector2.zero;
    private Vector2 _playerPos;
    private Transform _nearestAlien;
    private GameManager _gameManager;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _halfScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        yield return new WaitUntil(() => GameManager.instance != null);
        _gameManager = GameManager.instance;

        _playerPos = _gameManager.player.transform.position;
        aliens = new Transform[_gameManager.enemies.Length];
        for (int i = 0; i < _gameManager.enemies.Length; i++)
        {
            aliens[i] = _gameManager.enemies[i].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_nearestAlien)
        {
            Vector2 alienPos = mainCam.WorldToScreenPoint(_nearestAlien.position);
            Vector2 plPos = mainCam.WorldToScreenPoint(_playerPos);

            if (alienPos.x <= Screen.width && alienPos.y <= Screen.height)
            {
                detectorObject.gameObject.SetActive(false);
                _isActive = false;
            }
            else
            {
                if (!detectorObject.gameObject.activeInHierarchy)
                {
                    detectorObject.gameObject.SetActive(false);
                }

                Vector2 distPos = (Vector2)_nearestAlien.position - _playerPos;
                Vector2 mod = new Vector2(Mathf.Abs(distPos.x / _halfScreen.x), Mathf.Abs(distPos.y / _halfScreen.y));
                float modifier = mod.x > mod.y ? mod.x : mod.y;

                Vector2 newPos = _halfScreen + distPos / modifier;
                float rad = Mathf.Atan2(distPos.normalized.y, distPos.normalized.x);

                if (_isActive)
                {
                    detectorObject.position = Vector3.Lerp(detectorObject.position,
                                              new Vector3(newPos.x * detectorPaddingPosition.x, newPos.y + (1 - detectorPaddingPosition.y) * _halfScreen.y),
                                              Time.deltaTime * lerpDuration);

                    detectorObject.eulerAngles = Vector3.Lerp(detectorObject.eulerAngles,
                                                 new Vector3(0, 0, 270 + Mathf.Rad2Deg * rad),
                                                 Time.deltaTime * lerpDuration);
                }
                else
                {
                    detectorObject.position = new Vector3(newPos.x * detectorPaddingPosition.x, newPos.y + (1 - detectorPaddingPosition.y) * _halfScreen.y);
                    detectorObject.eulerAngles = new Vector3(0, 0, 270 + Mathf.Rad2Deg * rad);
                }

                _isActive = true;
            }
        }

        CheckNearestEnemy();
    }

    private void CheckNearestEnemy()
    {
        if (!_gameManager)
            return;

        float nearestDistance = float.MaxValue;
        int nearestIndex = -1;
        for (int i = 0; i < aliens.Length; i++)
        {
            float dist = Vector2.Distance(_playerPos, aliens[i].position);

            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearestIndex = i;
            }
        }

        if (nearestIndex >= 0 && nearestIndex < aliens.Length)
        {
            _nearestAlien = aliens[nearestIndex];
        }
        else
        {
            Debug.LogWarning("Wrong index value\nValue:" + nearestIndex);
        }
    }
}