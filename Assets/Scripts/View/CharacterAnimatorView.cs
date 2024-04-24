using UnityEngine;

public class CharacterAnimatorView : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private CharacterBody body;

    [Header("Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationSpeed = 4f;
    [SerializeField] private string directionXParam = "dir_x";
    [SerializeField] private string directionZParam = "dir_z";
    [SerializeField] private string horizontalMovSpeedParam = "move_speed";
    [SerializeField] private string jumpParam = "is_jump";
    [SerializeField] private string slopeAngleParam = "slope_angle";

    private Vector2 _currentDirection = Vector2.zero;
    private Vector2 _nextDirection = Vector2.zero;
    private Vector2 _previousDirection = Vector2.zero;

    private void Update()
    {
        SmoothMovementValues();
        SetMovementAnimations();
    }

    private void SetMovementAnimations()
    {
        animator.SetFloat(directionXParam, _currentDirection.x);
        animator.SetFloat(directionZParam, _currentDirection.y);
        animator.SetFloat(horizontalMovSpeedParam, body.GetHorizontalVelocityNormalized());
        animator.SetFloat(slopeAngleParam, body.GetDescendingSlopeAngle());
        animator.SetBool(jumpParam, body.GetIsJumping());
    }

    public void SetMovementDirection(Vector2 input)
    {
        _nextDirection = input;
    }

    private void UpdateCurrentDirection()
    {
        if (body.HasHorizontalMovement())
            _nextDirection = Vector2.zero;
    }

    /*
     * This method lerps input(X,Y) values between changes to smooth animation transitions.
     * If a change comes from a negative position (e.g. [-1,0]) to a positive one (e.g. [1,0]), 
     * then the current position needs to be increased.
     * However, if a change comes from a positive position (e.g. [1,0]) to a negative one (e.g. [-1,0]), 
     * then the current position needs to be decreased.
     * 
     * _previousDirection will be updated once _currentInput reaches _nextDirection's values 
     * (AKA the actual input value provided by InputReader).
     * 
     * This is calculated both for input.x as well as input.y.
     */
    private void SmoothMovementValues()
    {

        // Lerp on X depending in which direction we're moving
        if (_nextDirection.x > _previousDirection.x)
        {
            _currentDirection.x += Time.deltaTime * animationSpeed;
            _currentDirection.x = Mathf.Clamp(_currentDirection.x, _previousDirection.x, _nextDirection.x);
            if(_currentDirection.x >= _nextDirection.x)
                _previousDirection.x = _nextDirection.x;
        }
        else if(_nextDirection.x < _previousDirection.x)
        {
            _currentDirection.x -= Time.deltaTime * animationSpeed;
            _currentDirection.x = Mathf.Clamp(_currentDirection.x, _nextDirection.x, _previousDirection.x);
            if (_currentDirection.x <= _nextDirection.x)
                _previousDirection.x = _nextDirection.x;
        }

        // Lerp on Y depending in which direction we're moving
        if (_nextDirection.y > _previousDirection.y)
        {
            _currentDirection.y += Time.deltaTime * animationSpeed;
            _currentDirection.y = Mathf.Clamp(_currentDirection.y, _previousDirection.y, _nextDirection.y);
            if(_currentDirection.y >= _nextDirection.y)
                _previousDirection.y = _nextDirection.y;
        }
        else if (_nextDirection.y < _previousDirection.y)
        {
            _currentDirection.y -= Time.deltaTime * animationSpeed;
            _currentDirection.y = Mathf.Clamp(_currentDirection.y, _nextDirection.y, _previousDirection.y);
            if (_currentDirection.y <= _nextDirection.y)
                _previousDirection.y = _nextDirection.y;
        }
    }
}