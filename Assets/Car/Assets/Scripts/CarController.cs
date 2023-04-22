using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private bool _useDesktop = false;
    [Header("References")]
    [SerializeField] private Rigidbody _sphereRigidBody;
    [SerializeField] private WheelInteractable _steeringWheel;
    [SerializeField] private Vector3 _followOffset;
    [SerializeField] private Transform _leftFrontWheel;
    [SerializeField] private Transform _rightFrontWheel;
    [Header("Movement parameters")]
    [SerializeField] private float _forwardAcceleration = 8f;
    [SerializeField] private float _reverseAcceleration = 4f;
    [SerializeField] private float _maxSpeed = 50f;
    [Header("Turn parameters")]
    [SerializeField] private float _turnStrength = 100f;
    [SerializeField] private float _minimalSpeedToTurn = 1f;
    [Header("Sideways deceleration")]
    [SerializeField] private int _maxSidewaysAngle = 30;
    [SerializeField] private float _dragSideways = 5f;
    [Header("Wheels")]
    [SerializeField] private float _maxWheelTurn = 25f;
    [SerializeField] private float _wheelTurnSpeedHorizontal = 4f;
    [SerializeField] private WheelRotation[] _wheelRotations;

    private DesktopInputs _desktopInputs;
    private float _moveInput;
    private float _turnInput;

    private float _initialDrag;
    private float _initialLeftWheelYRotation;
    private float _initialRightWheelYRotation;
    private float _initialSteeringWheelRotation;

    private float _sqrMinimalSpeedToTurn;
    private float _sqrMaxSpeed;

    private bool _isDriving = false;
    private static readonly int TurningAnimationHash = Animator.StringToHash("turning");

    private void Awake()
    {
        if (_useDesktop)
        {
            _desktopInputs = new DesktopInputs();

            _desktopInputs.Player.Move.performed += ctx =>
            {
                _moveInput = ctx.ReadValue<Vector2>().y;
                _turnInput = ctx.ReadValue<Vector2>().x;
            };
            _desktopInputs.Player.Move.canceled += ctx =>
            {
                _moveInput = 0f;
                _turnInput = 0f;
            };
        }

        _initialDrag = _sphereRigidBody.drag;
        _initialLeftWheelYRotation = _leftFrontWheel.rotation.eulerAngles.y;
        _initialRightWheelYRotation = _rightFrontWheel.rotation.eulerAngles.y;

        _initialSteeringWheelRotation = _steeringWheel.Anchor.transform.localEulerAngles.z;
        while (_initialSteeringWheelRotation >= 360 || _initialSteeringWheelRotation < 0)
        {
            if (_initialSteeringWheelRotation > 360)
                _initialSteeringWheelRotation -= 360;
            else if (_initialSteeringWheelRotation < 0)
                _initialSteeringWheelRotation += 360;
        }

        _sqrMinimalSpeedToTurn = _minimalSpeedToTurn * _minimalSpeedToTurn;
        _sqrMaxSpeed = _maxSpeed * _maxSpeed;
    }

    private void OnEnable()
    {
        if (_useDesktop)
            _desktopInputs.Player.Enable();
    }

    private void OnDisable()
    {
        if (_useDesktop)
            _desktopInputs.Player.Disable();
    }

    private void Start()
    {
        _sphereRigidBody.transform.parent = null;
    }

    private void Update()
    {
        if (!_useDesktop)
        {
            float steeringWheelRotation = _steeringWheel.Anchor.localEulerAngles.z - _initialSteeringWheelRotation;

            _turnInput = steeringWheelRotation / _steeringWheel.MaxRotationOffset;
        }

        if (new Vector2(_sphereRigidBody.velocity.x, _sphereRigidBody.velocity.z).sqrMagnitude > _sqrMinimalSpeedToTurn)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * _turnInput * _turnStrength * Time.deltaTime);

        if (Vector3.Angle(_sphereRigidBody.velocity, transform.forward) > _maxSidewaysAngle)
        {
            _sphereRigidBody.drag = _dragSideways;
        }
        else
        {
            _sphereRigidBody.drag = _initialDrag;
        }

        Quaternion nextLeftRotation = Quaternion.Euler(_leftFrontWheel.localRotation.eulerAngles.x, _turnInput * _maxWheelTurn + _initialLeftWheelYRotation, _leftFrontWheel.localRotation.eulerAngles.z);
        Quaternion nextRightRotation = Quaternion.Euler(_rightFrontWheel.localRotation.eulerAngles.x, _turnInput * _maxWheelTurn + _initialRightWheelYRotation, _rightFrontWheel.localRotation.eulerAngles.z);
        
        _leftFrontWheel.localRotation = Quaternion.Lerp(_leftFrontWheel.localRotation, nextLeftRotation, Time.deltaTime * _wheelTurnSpeedHorizontal);
        _rightFrontWheel.localRotation = Quaternion.Lerp(_rightFrontWheel.localRotation, nextRightRotation, Time.deltaTime * _wheelTurnSpeedHorizontal);

        if (!_isDriving && _sphereRigidBody.velocity.sqrMagnitude > 0.1f)
        {
            _isDriving = true;
            foreach (var wheelRotation in _wheelRotations) 
                wheelRotation.IsRotating = true;
        }
        else if (_isDriving && _sphereRigidBody.velocity.sqrMagnitude < 0.1f)
        {
            _isDriving = false;
            foreach (var wheelRotation in _wheelRotations) 
                wheelRotation.IsRotating = false;
        }
        
        transform.position = _sphereRigidBody.transform.position + transform.InverseTransformVector(_followOffset);
    }

    private void FixedUpdate()
    {
        if (_moveInput != 0 && _sphereRigidBody.velocity.sqrMagnitude < _sqrMaxSpeed)
        {
            _sphereRigidBody.AddForce(transform.forward * (_moveInput * (_moveInput > 0 ? _forwardAcceleration : _reverseAcceleration) * _sphereRigidBody.mass));
        }
    }

    public void StartMoving()
    {
        _moveInput = 1;
    }

    public void StopMoving()
    {
        _moveInput = 0;
    }
}
