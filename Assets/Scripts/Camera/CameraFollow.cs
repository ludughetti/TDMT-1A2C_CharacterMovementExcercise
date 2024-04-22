using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offsetFromTarget = new(0f, 2.25f, -2.5f);

    [Header("Settings")]
    [SerializeField] private float cameraSensitivity = 10f;
    [SerializeField] private float verticalMinClamp = -30f;
    [SerializeField] private float verticalMaxClamp = 45f;

    private float _inputRotation = 0f;
    private float _targetDistance = 0f;
    private float _rotX = 0f;

    private void Start()
    {
        if (!AreAllComponentsAssigned())
        {
            enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;

        // Offset camera for third person view
        transform.position = offsetFromTarget;
        if (Vector3.zero.Equals(offsetFromTarget))
            transform.LookAt(target.position);

        // Calculate distance between positions for camera rotation and repositioning
        _targetDistance = Vector3.Distance(transform.position, target.position);
    }

    // We update the camera on LateUpdate to allow for all physics calculations to happen beforehand
    private void LateUpdate()
    {
        MoveCamera();
    }

    // Check if all dependencies are properly set in the UI
    private bool AreAllComponentsAssigned()
    {
        if (!target)
        {
            Debug.Log($"{name}: Camera target is not assigned");
            return false;
        }

        return true;
    }

    private void MoveCamera()
    {
        // If we don't have input, then skip camera calculations
        if (_inputRotation == 0f)
            return;

        // Smooth input rotation
        SmoothRotation();

        // Rotate camera on input
        RotateCamera();

        // Reposition camera to follow the target
        transform.position = target.position - (transform.forward * _targetDistance);
    }

    public float GetCameraSensitivity()
    {
        return cameraSensitivity;
    }

    public void SetInputRotation(float yInput)
    {
        _inputRotation = yInput;
    }

    private void SmoothRotation()
    {
        // We use only input.y as X since vertical rotation (input.y) happens on the X axis
        _rotX += _inputRotation * cameraSensitivity * Time.deltaTime;

        // Clamp rotation so the camera doesn't go through the floor or above max character rotation possibilities 
        _rotX = Mathf.Clamp(_rotX, verticalMinClamp, verticalMaxClamp);

        // Invert mouse input
        _inputRotation = -_rotX;
    }

    private void RotateCamera()
    {
        // Get current camera angle and replace vertical rotation for the one we calculated
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = _inputRotation;

        // Update camera rotation
        transform.eulerAngles = targetRotation;
    }
}