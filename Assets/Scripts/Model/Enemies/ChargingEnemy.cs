using System.Collections;
using System.Collections.Generic;
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

    protected bool _isCharging;
    protected float _preChargeTimer = 0;
    protected float _chargingTimer = 0;
    protected Player _player;

    protected ChargingPhase _phase = ChargingPhase.None;
    
    void Update()
    {
        if (_hasBalloon)
        {
            if (_isCharging)
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
                    _isCharging = true;
                    _phase = ChargingPhase.Charging;
                    _preChargeTimer = 0;

                    ChangeDuration();
                }
                else
                {
                    _chargingTimer = 0;
                    _preChargeTimer += Time.deltaTime;
                }
            }

            //transform.position += (Vector3)moveDirection * Time.deltaTime;
            _rigidBody2D.AddForce(moveDirection);
        }
    }

    private void ChargeUpdate()
    {
        switch (_phase)
        {
            case ChargingPhase.Charging:
                {
                    _preChargeTimer += Time.deltaTime;

                    if(_preChargeTimer >= chargingTime)
                    {
                        moveDirection = Vector2.zero;
                        _rigidBody2D.velocity = Vector2.zero;

                        _preChargeTimer = 0;
                        _phase = ChargingPhase.DoCharge;
                    }
                    break;
                }
            case ChargingPhase.DoCharge:
                {
                    _chargingTimer += Time.deltaTime;

                    if(_chargingTimer >= chargeBoostTime)
                    {
                        Vector2 direction = GetDirectionToPlayer();
                        moveDirection = direction * forceMultiplier;

                        _chargingTimer = 0;
                        _phase = ChargingPhase.PostCharge;
                    }
                    break;
                }
            case ChargingPhase.PostCharge:
                {
                    // TODO: do something here, like slowing down after charge

                    if(true)    
                    {
                        _rigidBody2D.velocity *= 0.1f;

                        _phase = ChargingPhase.None;
                        _isCharging = false;
                        ChangeDuration();
                        //ChangeDirection(Vector2.zero);
                    }
                    break;
                }
            default: break;
        }
    }

    protected override void ChangeDuration()
    {
        base.ChangeDuration();
        if(_isCharging) chargeDelay = Random.Range(minChargeDelayChange, maxChargeDelayChange);
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
    PostCharge
}