using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    //RESPAWNER
    private int player1Timer = 100;
    private int player2Timer = 100;

    private bool player1Alive;
    private bool player2Alive;
    [SerializeField]
    private Rigidbody2D player1Prefab;
    [SerializeField]
    private Rigidbody2D player2Prefab;

    // Update is called once per frame
    void FixedUpdate()
    {
        player1Alive = false;
        player2Alive = false;

        //get player objects that exist in scene and mark them alive
        PlayerControls[] allPlayers = UnityEngine.Object.FindObjectsOfType<PlayerControls>();
        foreach (PlayerControls player in allPlayers)
        {
            if (player.playerNumber == 1)
            {
                player1Alive = true;
            }
            if (player.playerNumber == 2)
            {
                player2Alive = true;
            }
        }

        //if a player is not alive, spawn them after timer
        if (!player1Alive)
        {
            player1Timer -= 1;
            if (player1Timer <=0)
            {
                Rigidbody2D instance = Object.Instantiate(player1Prefab, new Vector3(-4f,0f,0f), new Quaternion(0f, 0f, -0.7f, 0.7f)) as Rigidbody2D;
                player1Timer = 100;
            }
        }
        if (!player2Alive)
        {
            player2Timer -= 1;
            if (player2Timer <= 0)
            {
                Rigidbody2D instance = Object.Instantiate(player2Prefab, new Vector3(4f,0f,0f), new Quaternion(0f, 0f, 0.7f, 0.7f)) as Rigidbody2D;
                player2Timer = 100;
            }
        }

    }
}
