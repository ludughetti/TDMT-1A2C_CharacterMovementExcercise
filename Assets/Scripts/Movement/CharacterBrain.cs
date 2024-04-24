using UnityEngine;

/* This class controls a body */
public class CharacterBrain : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] new private CameraControl camera;

    [Header("Character")]
    [SerializeField] private CharacterBody body;
    [SerializeField] private CharacterAnimatorView view;

    [Header("Movement")]
    [SerializeField] private float maxMovementSpeed = 12f;
    [SerializeField] private float maxMovementAcceleration = 20f;
    [SerializeField] private float reduceMovementMultiplier = 4f;

    [Header("Player Input")]
    [SerializeField] private InputReader inputReader;

    private float _reducedAcceleration;
    private float _reducedSpeed;
    private Vector3 _desiredDirection = Vector3.zero;

    private void Start()
    {
        _reducedAcceleration = maxMovementAcceleration - (maxMovementAcceleration / reduceMovementMultiplier);
        _reducedSpeed = maxMovementSpeed - (maxMovementSpeed / reduceMovementMultiplier);
    }

    private void OnEnable()
    {
        if(body == null)
        {
            Debug.LogError($"{name}: {nameof(body)} is null!" +
                "\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        inputReader.OnMovementInput += HandleMovementInput;
        inputReader.OnCameraInput += HandleCameraInput;
        inputReader.OnJumpInput += HandleJumpInput;
    }

    private void OnDisable()
    {
        inputReader.OnMovementInput -= HandleMovementInput;
        inputReader.OnCameraInput -= HandleCameraInput;
        inputReader.OnJumpInput -= HandleJumpInput;
    }

    private void HandleMovementInput(Vector2 input)
    {
        if(_desiredDirection.magnitude > Mathf.Epsilon && input.magnitude < Mathf.Epsilon)
        {
            Debug.Log($"{nameof(_desiredDirection)} magnitude: {_desiredDirection.magnitude}\t{nameof(input)} magnitude: {input.magnitude}");
            body.RequestBrake();
        }

        _desiredDirection = new Vector3(input.x, 0f, input.y);
        body.SetMovement(new MovementRequest(_desiredDirection, GetSpeedByDirection(input), GetAccelerationByDirection(input)));
        view.SetMovementDirection(input);
    }

    private void HandleCameraInput(Vector2 input)
    {
        body.SetHorizontalRotation(input.x);
        camera.SetInputRotation(input.y);
    }

    // If character is moving forward, use full speed. Else, reduced.
    private float GetSpeedByDirection(Vector2 input)
    {
        if (input.y > 0f)
            return maxMovementSpeed;

        return _reducedSpeed;
    }

    // If character is moving forward, use full acceleration. Else, reduced.
    private float GetAccelerationByDirection(Vector2 input)
    {
        if (input.y > 0f)
            return maxMovementAcceleration;

        return _reducedAcceleration;
    }

    private void HandleJumpInput()
    {
        body.Jump();
    }
}
