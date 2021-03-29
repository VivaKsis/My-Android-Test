using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Charge Param")]
    [SerializeField] private Vector3 _initialScale = new Vector3(3f, 3f, 3f);
    public Vector3 _InitialScale => _initialScale;

    [SerializeField] private Vector3 _decreaseRate = new Vector3(0.01f, 0.01f, 0.01f);
    public Vector3 _DecreaseRate => _decreaseRate;

    [SerializeField] private float _frameRate = 0.01f;
    public float _FrameRate => _frameRate;

    [SerializeField] private Vector3 _livingLimitScale = new Vector3(1f, 1f, 1f);
    public Vector3 _LivingLimitScale => _livingLimitScale;

    [Header("Movement Param")]
    [SerializeField] private float _speed;
    public float _Speed => _speed;

    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody _RigidBody => _rigidbody;

    [Header("-"), Space]
    [SerializeField] private GameObject _explosion;
    public GameObject _Explosion => _explosion;

    [SerializeField] private RaycastEmitter _raycastEmitter;
    public RaycastEmitter _RaycastEmitter => _raycastEmitter;

    [SerializeField] private UIManager _uIManager;
    public UIManager _UIManager => _uIManager;

    [SerializeField] private AudioManager _audioManager;
    public AudioManager _AudioManager => _audioManager;

    private static float EXPLOSION_Y_COORDINATE = 1;

    private PlayerState playerState = PlayerState.playing;
    private Vector3 initialPosition;
    private float chargeTimer;
    private ExplosionProjectile explosion;
    private GameObject explosionObject;
    private ObjectPool.Pool poolTransferer = new ObjectPool.Pool();
    private RaycastHit _raycastHit;
    private float accelerationSpeed;

    public enum PlayerState
    {
        playing,
        won,
        dead
    }

    public void Win()
    {
        playerState = PlayerState.won;
        _audioManager.PlaySound(AudioManager.Sound.playerWin);
        _uIManager.YouWonTextShow();
    }

    private void Lose()
    {
        playerState = PlayerState.dead;
        _audioManager.PlaySound(AudioManager.Sound.playerLose);
        _uIManager.GameOverTextShow();
    }

    public void Charge()
    {
        chargeTimer += Time.deltaTime;

        if (chargeTimer >= _frameRate)
        {
            chargeTimer -= _frameRate;

            gameObject.transform.localScale -= _decreaseRate;

            Vector3 scale = gameObject.transform.localScale;
            if (scale.x <= _livingLimitScale.x || scale.y <= _livingLimitScale.y || scale.y <= _livingLimitScale.z)
            {
                Lose();
            }

            explosion.IncreaseCharge();
        }
    }

    public void Restart()
    {
        if(explosion != null)
        {
            explosion.Explode();
        }

        playerState = PlayerState.playing;
        gameObject.transform.localScale = _initialScale;
        gameObject.transform.position = initialPosition;
    }

    private void Awake()
    {
        gameObject.transform.localScale = _initialScale;
        initialPosition = gameObject.transform.position;
        accelerationSpeed = _speed / 10;
    }

    void Update()
    {
        if(playerState != PlayerState.playing)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(pos: Input.mousePosition);
            if (_raycastEmitter.Raycast(ray: ray, raycastHit: out _raycastHit))
            {
                explosionObject = poolTransferer.Aquire(_explosion);
                explosionObject.transform.position = new Vector3(_raycastHit.point.x, EXPLOSION_Y_COORDINATE, _raycastHit.point.z);

                explosion = explosionObject.GetComponent<ExplosionProjectile>();
                explosion.SetOnExplode(() =>
                {
                    _audioManager.PlaySound(AudioManager.Sound.explosion);
                });
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (explosion != null)
            {
                Charge();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(explosion != null)
            {
                explosion.ShootFrom(transform.position);

                _audioManager.PlaySound(AudioManager.Sound.playerShoot);
            }
            
            chargeTimer = 0;
            explosionObject = null;
        }
    }

    private void FixedUpdate()
    {
        var currentVelocity = _rigidbody.velocity;
        if (currentVelocity.y >= 0f)
        {
            currentVelocity.y = 0f;
        }
        _rigidbody.velocity = currentVelocity;

#if UNITY_EDITOR

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        _rigidbody.AddForce(new Vector3(v, 0, -h) * _speed * Time.deltaTime);

#endif

#if UNITY_ANDROID

        var tilt = Input.acceleration;
        _rigidbody.AddForce(new Vector3(tilt.y * accelerationSpeed, 0, -tilt.x * accelerationSpeed));

#endif
    }
}
