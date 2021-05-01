using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanGame : MonoBehaviour
{
    private static int h = 40; //platform height 16
    private static int w = 60; //platform width 40
    public GameObject[,] platform = new GameObject[w, h]; //this stores positions of gameobjects on the platform
    void Start()
    {
        Object[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)); //array of all of our objects
        foreach (GameObject x in allObjects) //iterating over our objects 
        {
            Vector2 position = x.transform.position;
            if (x.tag != "player")
            {
                platform[Mathf.Abs((int)position.x), Mathf.Abs((int)position.y)] = x;
            }
            else
            {
                Debug.Log("player at: " + position);
            }
        }
    }

    void Update()
    {

    }


}
