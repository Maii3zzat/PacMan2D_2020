using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pacman : MonoBehaviour
{

    public float velocity = 10.0f;
    public GameObject PacMan;
    public Vector2 dir; //direction
    private Vector2 nextDir;
    private node nodePrv;
    private node nodeCurr;
    private node nodeTarg;

  
    void Start()
    {

        node n = GetNodeInPos(transform.localPosition);
        if (n != null)
        {
            nodeCurr = n;
            Debug.Log(nodeCurr);
        }
        dir = Vector2.up;
        changePos(dir);

    }

    void Update()
    {

        Key();

        Move();

    }

    void Key()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            changePos(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changePos(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            changePos(Vector2.up); 
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            changePos(Vector2.down); 
        }
    }

    node possibleDirections(Vector2 dir) //the possible directions in which pacman can go to 
    {
        node goTo = null; //goes to a neighbor node
        for (int i = 0; i < nodeCurr.neighbors.Length; i++)
        {
            if (nodeCurr.possibleDirections[i] == dir)
            {
                goTo = nodeCurr.neighbors[i];
            }
        }
        return goTo;
    }


    void changePos(Vector2 p)
    {
        if (p != dir)
        {
            nextDir = p;
        }
        if (!(nodeCurr == null))
        {
            node movetoNode = possibleDirections(p);
            if (movetoNode != null)
            {
                dir = p;
                nodeTarg = movetoNode;
                nodePrv = nodeCurr;
                nodeCurr = null;
            }

        }
    }

    void Move()
    {
        if (nodeTarg != nodeCurr && nodeTarg != null)
        {
            if (gotOverTarget())
            {
                nodeCurr = nodeTarg;
                transform.localPosition = nodeCurr.transform.position;
                node moveToNode = possibleDirections(nextDir);
                if (moveToNode != null)
                {
                    dir = nextDir;
                }
                if (moveToNode == null)
                {
                    moveToNode = possibleDirections(dir);

                }
                if (moveToNode != null)
                {
                    nodeTarg = moveToNode;
                    nodePrv = nodeCurr;
                    nodeCurr = null;
                }
                else
                {
                    dir = Vector2.zero;
                }

            }
            else
            {
                transform.localPosition = transform.localPosition + ((Vector3)(dir * velocity) * Time.deltaTime);
            }
        }

    }

    node GetNodeInPos(Vector2 position)
    {
        GameObject tile = GameObject.Find("TheGame").GetComponent<PacmanGame>().platform[(int)position.x, (int)position.y];
        if (tile != null)
        {
            return tile.GetComponent<node>();
        }
        return null;
    }

    bool gotOverTarget()
    {
        float nToTarget = lgthFromNode(nodeTarg.transform.localPosition);
        float nToSelf = lgthFromNode(transform.localPosition);
        return nToSelf > nToTarget;
    }


    float lgthFromNode(Vector2 target)
    {
        Vector2 v = target - (Vector2)nodePrv.transform.position;
        return v.sqrMagnitude;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pinky"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
