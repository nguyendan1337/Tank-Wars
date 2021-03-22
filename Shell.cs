using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shell:
/// -Takes care of whatever you hit
/// </summary>
public class Shell : MonoBehaviour
{
    private PlayerManager playerManager;
    private Rigidbody2D m_Body;                     // the current shell's rigid body
    public int teamNumber;



    void Start()
    {
        m_Body = this.GetComponent<Rigidbody2D>();

        playerManager = FindObjectOfType<PlayerManager>();
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")           // die if hit wall
        {
            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "Target")    // die & destroy target if hit target
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
        //if shell collides with enemy player or another shell
        else if (col.gameObject.tag == "Player" && col.GetComponent<PlayerController>().m_teamNumber != teamNumber
            || col.gameObject.tag == "Shell")
        {
            Destroy(gameObject);    // destroy bullet
            Destroy(col.gameObject);
        }
    }

}
