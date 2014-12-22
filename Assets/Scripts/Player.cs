using UnityEngine;
using System.Collections;

enum PlayerStates
{
    Idle,
    MovingLeft,
    MovingRight
}

public class Player : MonoBehaviour 
{
    public float distance = 1.0f;
    public float speed = 0.1f;

    Vector2 target, position;
    PlayerStates state = PlayerStates.Idle;

    TileMap map { get { return FindObjectOfType<TileMap>(); } }
    bool moving
    {
        get
        {
            return 
                state == PlayerStates.MovingLeft || 
                state == PlayerStates.MovingRight;
        }
    }
    bool inputLeft { get { return Input.GetAxis("Horizontal") < 0; } }
    bool inputRight { get { return Input.GetAxis("Horizontal") > 0; } }

    void Start()
    {
        target = transform.position;
        position = transform.position;
    }

    void Update()
    {
        if (!moving)
        {
            if (inputLeft)
            {
                state = PlayerStates.MovingLeft;
                target.x -= distance;
            }
            if (inputRight)
            {
                state = PlayerStates.MovingRight;
                target.x += distance;
                if (map.IsTileSolid(Mathf.RoundToInt(target.x), Mathf.RoundToInt(position.x)))
                {

                }
            }
        }

        if (moving)
        {
            if (state == PlayerStates.MovingLeft)
            {
                position.x -= speed * Time.deltaTime;

                if (position.x < target.x)
                {
                    state = PlayerStates.Idle;
                    if (!inputLeft)
                    {
                        position.x = target.x;
                    }
                }
            }
            if (state == PlayerStates.MovingRight)
            {
                position.x += speed * Time.deltaTime;

                if (position.x > target.x)
                {
                    state = PlayerStates.Idle;
                    if (!inputRight)
                    {
                        position.x = target.x;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(position.x, position.y);
    }
}