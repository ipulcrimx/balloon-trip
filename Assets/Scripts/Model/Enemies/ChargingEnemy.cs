using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ChargingEnemy : Enemy
{
    [Header("Charging Parameter")]
    [Tooltip("Duration of enemy standing still before it charge to player")]
    public float chargingTime = 2.5f;
    [Tooltip("Duration of enemy charging toward player")]
    public float chargeBoostTime = 1f;
    [Tooltip("Multiplier speed when enemy charges to player")]
    public float forceMultiplier = 15;

    [Space]
    public float chargeDelay = 8;
    public float minChargeDelayChange;
    public float maxChargeDelayChange;

    protected bool _chargePhase;
    protected float _preChargeTimer = 0;
    protected float _chargingTimer = 0;
    protected Player _player;

    protected ChargingPhase _phase = ChargingPhase.None;

    protected override void Start()
    {
        base.Start();

        _chargePhase = false ;
        _phase = ChargingPhase.None;
        _preChargeTimer = 0;
        _chargingTimer = 0;
    }

    void Update()
    {
        if(_hasBalloon)
        {
            if(_chargePhase)
            {
                ChargeUpdate();
            }
            else
            {
                if (_timer >= duration)
                {
                    ChangeDuration();
                    ChangeDirection(Vector2.zero);
                }
                else
                {
                    _timer += Time.deltaTime;
                }

                if(_preChargeTimer >= chargeDelay)
                {
                    _chargePhase = true;
                    _phase = ChargingPhase.Charging;
                    _preChargeTimer = 0;
                    _chargingTimer = 0;
                }
                else
                {
                    _chargingTimer = 0;
                    _preChargeTimer += Time.deltaTime;
                }
                if(_phase != ChargingPhase.DoCharge &&
                    moveDirection.sqrMagnitude > moveSpeed * moveSpeed)
                {
                    moveDirection = Vector3.Lerp(moveDirection, moveDirection.normalized * moveSpeed, Time.deltaTime);
                }
            }
            _rigidBody2D.AddForce(moveDirection);
        }
        else
        {
            Inflating();
        }

        if (_invincibleTimer < invincibleDuration)
        {
            _invincibleTimer += Time.deltaTime;
        }
    }

    private void ChargeUpdate()
    {
        switch(_phase)
        {
            case ChargingPhase.Charging:
                if(_preChargeTimer < chargingTime)
                {
                    moveDirection = Vector2.zero;
                    _rigidBody2D.velocity = Vector2.zero;

                    _preChargeTimer += Time.deltaTime;
                }
                else
                {
                    _preChargeTimer = 0;
                    _phase = ChargingPhase.DoCharge;
                }
                break;
            case ChargingPhase.DoCharge:
                if (_chargingTimer <= 0)
                {
                    Vector2 playerDir = GetDirectionToPlayer();
                    moveDirection = playerDir * forceMultiplier;
                    _chargingTimer += Time.deltaTime;
                }
                else if (_chargingTimer >= chargeBoostTime)
                {

                    _chargingTimer = 0;
                    _phase = ChargingPhase.PostCharge;
                }
                else
                {
                    _chargingTimer += Time.deltaTime;
                }
                break;
            case ChargingPhase.PostCharge:
                _rigidBody2D.velocity *= 0.15f;

                _phase = ChargingPhase.None;
                _chargePhase = false;
                ChangeDuration();
                break;
            case ChargingPhase.ChargeBack:
                //float rndAngle = Random.Range(40f, 150f);
                //float x = Mathf.Sin(rndAngle);
                //float y = Mathf.Cos(rndAngle);

                //_rigidBody2D.velocity = new Vector2(x, y) * (_rigidBody2D.velocity / 4);

                moveDirection = -moveDirection.normalized * moveSpeed;
                _phase = ChargingPhase.None;
                _chargePhase = false;
                //ChangeDuration();
                break;
            case ChargingPhase.None:
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (_invincibleTimer <= invincibleDuration)
                return;

            Player pl = col.gameObject.GetComponent<Player>();

            if (pl)
            {
                if (_chargePhase)
                {
                    _phase = ChargingPhase.ChargeBack;
                }
                else
                {
                    if(_hasBalloon)
                    {
                        OnBallonDestroyed();
                    }
                    else
                    {
                        OnEnemyKilled();
                    }
                }
            }
            else
            {
                Debug.LogWarning("There's no Player component on " + col.gameObject.name + " gameObject.");
            }
        }
        else if (col.gameObject.tag == "Players Balloon")
        {
            PlayerBalloon balloon = col.gameObject.GetComponent<PlayerBalloon>();

            if (balloon)
            {
                balloon.OnBalloonPoppedUp();
            }
            else
            {
                Debug.LogWarning("There's no Player Balloon component on " + col.gameObject.name + " gameObject.");
            }
        }
        else
        {
            OnCollideWithObstacle(col.contacts[0].point);
        }
    }

    protected override void ChangeDuration()
    {
        base.ChangeDuration();
        if (_chargePhase)
        {
            chargeDelay = Random.Range(minChargeDelayChange, maxChargeDelayChange);
        }
    }

    [ContextMenu("Get angle")]
    protected Vector2 GetDirectionToPlayer()
    {
        // TODO: this angle calculation is still inaccurated... please change
        Vector2 angle = new Vector2();

        if (!_player) _player = GameManager.instance.player;
        angle = (_player.transform.position - transform.position).normalized;

        return angle;
    }
}

public enum ChargingPhase
{
    None = -1,
    Charging = 0,
    DoCharge = 1,
    PostCharge = 2,
    ChargeBack = 3
}