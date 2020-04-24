using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _gravity = 1.0f;
    [SerializeField] private float _jumpHeight = 15.0f;
    [SerializeField] private int _coins;
    [SerializeField] private int _lives = 3;
    private float _yVelocity;
    private bool _canDoubleJump = false;
    private Vector3 _startingPosition;
    private CharacterController _controller;
    private UIManager _uiManager;

    void Awake()
    {
        _startingPosition = transform.position;
    }
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL!");
        }

        _uiManager.UpdateLivesDisplay(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        // transform.SetPositionAndRotation(transform.position, Quaternion.identity);
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

    public void AddCoins()
    {
        _coins++;
        _uiManager.UpdateCoinDisplay(_coins);
    }

    public void Respawn()
    {
        transform.position = _startingPosition;
        _lives--;
        if (_lives <= 0)
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
        _uiManager.UpdateLivesDisplay(lives: _lives);
    }

    public bool IsFalling()
    {
        if (transform.position.y <= -7f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
