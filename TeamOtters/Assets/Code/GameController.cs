using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public BoundaryHolder boundaryHolder;
    public GameObject player;
    public PhaseManager phaseManager;
    public GameObject cameraHolder;

    public int bounceHit = 10;
    public int firstReachGoal = 10;
    public int hitOpponent = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            // First time creation, sets first Instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }       
        else
        {
            // Keep original instance
            Destroy(gameObject);
        }
    }

    // Menu scene swap logic here? Check out https://www.youtube.com/watch?v=CPKAgyp8cno
}