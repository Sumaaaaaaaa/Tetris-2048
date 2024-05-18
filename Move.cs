using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Move : MonoBehaviour
{
    private float _time;
    private float _fullTime;
    private bool _isMoving;
    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isMoving)
        {
            _time += Time.deltaTime;
            transform.position = Vector3.Lerp(_originalPosition, _targetPosition, _time / _fullTime);
            if(_time >= _fullTime)
            {
                transform.position = _targetPosition;
                _isMoving = false;
            }
        }
    }
    public void move(Vector2 value, float time)
    {
        _isMoving = true;

        _originalPosition = transform.position;
        _targetPosition = transform.position + new Vector3(value.x, value.y, 0);

        _fullTime = time;
        _time = 0;
    }
}
