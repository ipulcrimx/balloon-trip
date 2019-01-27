using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool _isCharging;
    private float _preChargeTimer = 0;
    private float _chargingTimer = 0;

    private ChargingPhase _phase = ChargingPhase.None;

    private void Update()
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
                        _phase = ChargingPhase.None;
                        _isCharging = false;
                        ChangeDuration();
                        ChangeDirection(Vector2.zero);
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
}

internal enum ChargingPhase
{
    None = -1,
    Charging = 0,
    DoCharge = 1,
    PostCharge
}