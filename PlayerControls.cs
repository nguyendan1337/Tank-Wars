using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Module used to move the player")]
    private MovementModule movementModule;
    private Vector2 lastMove;
    [SerializeField]
    [Tooltip("Module used by the player to fire the gun")]
    private GunModule gunModule;
    [SerializeField]
    [Tooltip("Minimum number of seconds that must pass between firing bullets")]
    private float fireRate;
    // Time that has elapsed since the gun last fired
    private float timeSinceLastFire;

    public int playerNumber;
    private KeyCode UP;
    private KeyCode DOWN;
    private KeyCode LEFT;
    private KeyCode RIGHT;
    private KeyCode FIRE;

    // Start is called before the first frame update
    void Start()
    {

        timeSinceLastFire = fireRate;

        if (playerNumber == 1)
        {
            UP = KeyCode.W;
            DOWN = KeyCode.S;
            LEFT = KeyCode.A;
            RIGHT = KeyCode.D;
            FIRE = KeyCode.Space;
        }
        if (playerNumber == 2)
        {
            UP = KeyCode.UpArrow;
            DOWN = KeyCode.DownArrow;
            LEFT = KeyCode.LeftArrow;
            RIGHT = KeyCode.RightArrow;
            FIRE = KeyCode.Return;
        }
    }

    void Update()
    {
        timeSinceLastFire += Time.deltaTime;

        // If spacebar is pressed, and time since last fire has exceeded min time between bullets, fire the gun
        if (Input.GetKey(FIRE) && timeSinceLastFire > fireRate)
        {
            gunModule.Shoot();

            // No time has elapsed since last fire of the gun, since the gun just fired
            timeSinceLastFire = 0f;
        }
    }


    private void FixedUpdate()
    {
        // +1 is right
        // -1 is left
        //float horizontal = Input.GetAxisRaw("Horizontal");
        float horizontal = 0;
        if (Input.GetKey(LEFT))
        {
            horizontal = -1;
        }
        else if (Input.GetKey(RIGHT))
        {
            horizontal = +1;
        }

        // +1 is up
        // -1 is down
        // Only set vertical by input if player is not moving horizontal
        float vertical = 0;
        if (Mathf.Abs(horizontal) < 0.1)
        {
            if (Input.GetKey(UP))
            {
                vertical = +1;
            }
            else if (Input.GetKey(DOWN))
            {
                vertical = -1;
            }
        }

        movementModule.Move(new Vector2(horizontal, vertical));
    }

    public MovementModule getMovement()
    {
        return movementModule;
    }
}
