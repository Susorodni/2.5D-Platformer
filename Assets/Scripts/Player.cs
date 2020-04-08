using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _gravity = 1.0f;
    [SerializeField] private float _jumpHeight = 15.0f;
    private float _yVelocity;
    private bool _canDoubleJump = false;
    private CharacterController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        transform.position = new Vector3(0, 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = direction * _speed;

        if (_controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_canDoubleJump)
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
            }

            _yVelocity -= _gravity;
        }

        velocity.y = _yVelocity;
        _controller.Move(velocity * Time.deltaTime);
    }
}
