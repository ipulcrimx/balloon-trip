using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class FuguEnemy : ChargingEnemy
{
    public float maxSize = 5;

    private Vector3 _initialSize;
    private Vector3 _currentSie;

    protected override void Start()
    {
        _initialSize = _currentSie = transform.localScale;
        base.Start();
    }

    private void ChargeUpdate()
    {
        switch (_phase)
        {
            case ChargingPhase.Charging:
                {
                    _preChargeTimer += Time.deltaTime;

                    if (_preChargeTimer >= chargingTime)
                    {
                        moveDirection = Vector2.zero;
                        _rigidBody2D.velocity = Vector2.zero;
                        transform.localScale = _currentSie = _initialSize * maxSize;

                        _preChargeTimer = 0;
                        _phase = ChargingPhase.DoCharge;
                    }
                    else
                    {
                        // TODO: making itself big here (preferable using DOTween
                    }
                    break;
                }
            case ChargingPhase.DoCharge:
                {
                    _chargingTimer += Time.deltaTime;

                    if (_chargingTimer >= chargeBoostTime)
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

                    if (true)
                    {
                        _rigidBody2D.velocity *= 0.1f;


                        //transform.localScale = _initialSize;
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
}
