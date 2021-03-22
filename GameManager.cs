using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameManager:
/// -Configure tank stats here
/// -Attatched to the Game Manager GameObject
/// -Tanks created from this will have these stats
/// </summary>
public class GameManager : MonoBehaviour
{
    public int numTeams;
    public int tanksPerTeam;
    public GameObject tankPrefab;
    public float tankSpeed;
    public float bulletSpeed;
    public Sprite tankSprite;             
    public GameObject shellPrefab;          
    public GameObject teamsTextField;
    public GameObject tanksTextField;
    public GameObject errorMessage;
    public InputField teamsInputField;
    public InputField tanksInputField;



    public void setNumTeams()
    {
        numTeams = Int32.Parse(teamsTextField.GetComponent<Text>().text);
        Debug.Log(numTeams);
    }

    public void setTanksPerTeam(string newTanksPerTeam)
    {
        tanksPerTeam = Int32.Parse(tanksTextField.GetComponent<Text>().text);
        Debug.Log(tanksPerTeam);
    }

    //instantiates user's requested number of teams and tanks per team
    //each team has a unique color
    public void beginGame()
    {
        if (numTeams > 6 || numTeams < 2 || tanksPerTeam < 1 || numTeams*tanksPerTeam > 500)
        {
            errorMessage.SetActive(true);
        }
        else
        {
            errorMessage.SetActive(false);
            GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in gameObjects)
            {
                if (obj.GetComponent<PlayerController>() || obj.GetComponent<Shell>())
                {
                    Destroy(obj);
                }
            }

            Color[] colors = { Color.red, Color.blue, Color.yellow, Color.green, Color.white, Color.black };

            for (int i = 0; i < numTeams; i++)
            {
                for (int l = 0; l < tanksPerTeam; l++)
                {
                    GameObject tank = Instantiate(tankPrefab, new Vector2(UnityEngine.Random.Range(-14.625f, 14.625f), UnityEngine.Random.Range(-6.824997f, 6.824997f)), Quaternion.identity) as GameObject;
                    tank.GetComponent<SpriteRenderer>().sprite = tankSprite;
                    tank.GetComponent<SpriteRenderer>().color = colors[i];
                    tank.GetComponent<PlayerController>().setShellPrefab(shellPrefab);
                    tank.GetComponent<PlayerController>().setSpeed(tankSpeed);
                    tank.GetComponent<PlayerController>().setProjectileSpeed(bulletSpeed);
                    tank.GetComponent<PlayerController>().setTeamNumber(i);
                }
            }
        }
    }

    

    void Start()
    {
        teamsInputField.text = numTeams.ToString();
        tanksInputField.text = tanksPerTeam.ToString();
    }
}