using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaremaFollowObj : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [Header("FlipRotationStats")]
    [SerializeField] private float _FlipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;
    private PlayerMovement_Kdx _player;
    private bool _isFacingRight;

    private void Awake()
    {
        _player = _playerTransform.gameObject.GetComponent<PlayerMovement_Kdx>();
        _isFacingRight = _player.isFacingRight;
    }
    private void Update()
    {
        transform.position = _player.transform.position;
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _FlipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation,endRotationAmount, elapsedTime/_FlipYRotationTime);
            transform.rotation = Quaternion.Euler(0f,yRotation,0f);
            
            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if (_isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
}
