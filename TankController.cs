using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// TankController:
/// -
/// </summary>
public class TankController : MonoBehaviour
{    
    // PRIVATE VARS THAT WILL BE DEFINED FROM playerManager.PlayersList.PlayerInfo in Start:
    // Axis Names/Values = for movement
    private string m_HorizontalAxisName;
    private string m_VerticalAxisName;
    private string m_FireButton;            
    // SPEED of self & shell
    private float m_Speed;
    private float m_ProjectileSpeed;    
    // other private vars
    private Rigidbody2D m_RigidBody;        
    private GameObject m_ShellPrefab;       

    // THESE VARS WILL BE ASSIGNED IN ProcessInput:
    private float m_HorizontalInputValue;   // Use returned Axis input vals for movement
    private float m_VerticalInputValue;
    public int m_teamNumber;

    static System.Random rnd;
    private GameObject target;
    private double xToTarget;
    private double yToTarget;

    private int cooldown = 50;

    public void setShellPrefab(GameObject shellPrefab)
    {
        m_ShellPrefab = shellPrefab;
    }

    public void setSpeed(float speed)
    {
        m_Speed = speed;
    }

    public void setProjectileSpeed(float speed)
    {
        m_ProjectileSpeed = speed;
    }

    public void setTeamNumber(int teamNumber)
    {
        m_teamNumber = teamNumber;
    }


    void Start()       
    {
        m_RigidBody = this.GetComponent<Rigidbody2D>();
        rnd = new System.Random();
    }



    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        if (!target)
        {
            AcquireTarget();
        }
    }



    private void FixedUpdate()
    {
        m_HorizontalInputValue = 0;
        m_VerticalInputValue = 0;

        if (target)
        {
            MoveTowardsTarget();
        }

        Move();
        Turn();
        if (target)
        {
            if (Math.Abs(yToTarget) < 0.5 || Math.Abs(xToTarget) < 0.5)
            {
                Fire();
            }
        }
        

        if (cooldown < 50)
        {
            cooldown -= 1;
        }
        if (cooldown <= 0)
        {
            cooldown = 50;
        }
    }



    /// <summary>
    /// Move:
    /// -Handles movement of tank by setting velocity
    /// -Called in FixedUpdate
    /// </summary>
    void Move()
    {
        Vector2 movement = new Vector2(m_HorizontalInputValue, m_VerticalInputValue);   
        m_RigidBody.velocity = movement * m_Speed;      
    }

    /// <summary>
    /// MoveTowardsTarget:
    /// -from target's position, calculate shortest move into firing position
    /// -returns either a horizontal or vertical movement
    /// </summary>
    void MoveTowardsTarget()
    {
        xToTarget = target.transform.position.x - this.transform.position.x;
        yToTarget = target.transform.position.y - this.transform.position.y;

        if (Math.Abs(xToTarget) < Math.Abs(yToTarget) && Math.Abs(xToTarget) > 0.5 || Math.Abs(yToTarget) < 0.5)
        {
            if (xToTarget > 0)
            {
                m_HorizontalInputValue = 1;
            }
            else
            {
                m_HorizontalInputValue = -1;
            }
        }
        if (Math.Abs(yToTarget) < Math.Abs(xToTarget) && Math.Abs(yToTarget) > 0.5 || Math.Abs(xToTarget) < 0.5)
        {
            if (yToTarget > 0)
            {
                m_VerticalInputValue = 1;
            }
            else
            {
                m_VerticalInputValue = -1;
            }
        }
    }



    /// <summary>
    /// Turn:
    /// -Turns player in direction of movement by setting 
    ///  the gameObject's transform's rotation to specific Quarternion.Euler things
    /// </summary>
    void Turn()
    {
        if (m_HorizontalInputValue > 0)         // input value of 1 = move right
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);   // -90 in the Z axis is rotating right
        }
        else if (m_HorizontalInputValue < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (m_VerticalInputValue > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (m_VerticalInputValue < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }



    /// <summary>
    /// OnCollisionEnter2D:
    /// -Stop when hit wall (thats it)
    /// --colliding with shell is handled in Shell's script
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            m_RigidBody.velocity = new Vector2(0f, 0f);
        }
    }



    /// <summary>
    /// Fire:
    /// -instantiates the shell object and set its velocity (so it travels right away)
    /// </summary>
    void Fire()
    {
        if (cooldown == 50)
        {
            cooldown -= 1;
            GameObject shell = Instantiate(m_ShellPrefab, m_RigidBody.transform.position, m_RigidBody.transform.rotation) as GameObject;
            shell.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
            shell.GetComponent<Rigidbody2D>().velocity = transform.up * m_ProjectileSpeed;      // transform."up" = green arrow (aka where facing)
            shell.GetComponent<Shell>().teamNumber = m_teamNumber;
        }

    }

    /// <summary>
    /// AcquireTarget:
    /// -from list of gameObjects, create list of enemies as potential targets
    /// </summary>
    void AcquireTarget()
    {
        GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        List<GameObject> enemies = new List<GameObject>();
        
        foreach (GameObject obj in gameObjects)
        {
            if(obj.GetComponent<PlayerController>() && obj.GetComponent<PlayerController>().m_teamNumber != m_teamNumber)
            {
                enemies.Add(obj);
            }
        }

        if (enemies.Count > 0)
        {
            int r = rnd.Next(enemies.Count);
            target = enemies[r];
        }
    }
}
