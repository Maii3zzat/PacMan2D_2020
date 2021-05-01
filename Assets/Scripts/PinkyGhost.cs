using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PinkyGhost : MonoBehaviour
{

    public float velocity = 8f;
    private GameObject pacman;
    private node nodePrev;
    private node nodeCurrent;
    private node nodeTarget;
    private Vector2 direction; 

    void Start()
    {
        pacman = GameObject.FindGameObjectWithTag("player");
        node n = GetNodeInPos(transform.localPosition);
        if (n != null)
        {
            nodeCurrent = n;
        }
        direction = Vector2.right;
        nodePrev = nodeCurrent;
        Vector2 pacmanPos = pacman.transform.position;
        Vector2 tileTarg = new Vector2(Mathf.RoundToInt(pacmanPos.x), Mathf.RoundToInt(pacmanPos.y));
        nodeTarget = GetNodeInPos(tileTarg);

    }

    void Update()
    {
        
        PinkyMoves();
    }

    void PinkyMoves()
    {
        if (nodeTarget != null && nodeTarget != nodeCurrent)
        {
            if (gotOverTarget())
            {
                nodeCurrent = nodeTarget;
                transform.localPosition = nodeCurrent.transform.position;
                nodeTarget = chooseNextNode();
                nodePrev = nodeCurrent;
                nodeCurrent = null;
            }
            else
            {
             
                transform.localPosition = transform.localPosition + ((Vector3)(direction * velocity) * Time.deltaTime);
            }
        }
    }


    node chooseNextNode()
    {
        Vector2 tileTarg = Vector2.zero;
        //getting pacman pos to set the target for the ghost but we will change that when we get to the AI 
        Vector2 pacmanPos = pacman.transform.position;
        tileTarg = new Vector2(Mathf.RoundToInt(pacmanPos.x), Mathf.RoundToInt(pacmanPos.y));
        node moveTo = null;
        node[] nodesFound = new node[4];  //ndoe neighbours
        Vector2[] nodesFoundDirc = new Vector2[4];
        int nodeCounter = 0;
        for (int i = 0; i < nodeCurrent.neighbors.Length; i++)
        {
                nodesFound[nodeCounter] = nodeCurrent.neighbors[i];
                nodesFoundDirc[nodeCounter] = nodeCurrent.possibleDirections[i];
                nodeCounter++;
        }
        if (nodesFound.Length == 1)
        {
          
            moveTo = nodesFound[0];
            direction = nodesFoundDirc[0];
        }
        if (nodesFound.Length > 1)
        {
       
            float leastDistance = 1000000f;
            for (int i = 0; i < nodesFound.Length; i++)
            {
             
                if (nodesFoundDirc[i] != Vector2.zero)
                {
                  
                    float distance = getDistance(nodesFound[i].transform.localPosition, tileTarg);
                    if (distance < leastDistance)
                    {
                   
                        leastDistance = distance;
                        moveTo = nodesFound[i];
                        direction = nodesFoundDirc[i];
                    }
                }
            }
        }
        return moveTo;
    }

    node GetNodeInPos(Vector2 position)
    {
        GameObject tile = GameObject.Find("TheGame").GetComponent<PacmanGame>().platform[(int)position.x, (int)position.y];
        if (tile != null)
        {
            if (tile.GetComponent<node>() != null)
            {
                return tile.GetComponent<node>();
            }
        }
        return null;
    }


       float LgthFromNode(Vector2 posTarget)
       {
        Vector2 v = posTarget - (Vector2)nodePrev.transform.position;
        return v.sqrMagnitude;
       }

      bool gotOverTarget()
      {
        float nToTarget = LgthFromNode(nodeTarget.transform.localPosition);
        float nToSelf = LgthFromNode(transform.localPosition);
        return nToSelf > nToTarget;
      }

       float getDistance(Vector2 posA, Vector2 posB)
       {
           float dx = posA.x - posB.x;
           float dy = posA.y - posB.y;
           float dist = Mathf.Sqrt(dx * dx + dy * dy);

           return dist;
       }

}
