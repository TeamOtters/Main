using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public BoundaryHolder boundaryHolder;
    public GameObject player;
    public PhaseManager phaseManager;
    public CameraBehaviourManager cameraManager;
    public SnapPositionController snapPositionController;
    public PlayerData playerData;
    public GameObject vikingPrefab;
    public GameObject valkyriePrefab;
    public GameObject scorePrefab;
    public GameObject logicLayer;

    public RumbleManager rumbleManager;
    private SnapablePlayers snapablePlayers;

    public ScoreManager m_scoreManager;


    public int numberOfPlayers = 4; //could this number read from number of plugged in controllers or menu in case we want to support more than 4 players?
    public bool playerStartViking = true;
    public int bounceHit = 10;
    public int firstReachGoal = 10;
    public int hitOpponent = 10;

    public float snapGridZ = 0f;

    //used when creating the new players automatically
    private GameObject[] m_players;
    private PlayerData m_currentPlayerData;
    private PlayerSpawnPoint[] m_playerSpawnPoints;
    private GameObject m_tempGameObject;

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

        CreatePlayers();

    }

    // A function that creates a complete player for each player in "number of players" int
   void CreatePlayers()
    {
        m_playerSpawnPoints = GetComponentsInChildren<PlayerSpawnPoint>();
        m_tempGameObject = new GameObject("MyTempGameObject");

        for(int i =0; i<numberOfPlayers; i++)
        {
            //creates and names the new player parent gameobject
            GameObject myNewPlayer = new GameObject("Player_" + (i + 1));

            //assigns the new player to the player variable for the boundaries - THIS WILL NEED AN UPDATE DEPENDING ON HOW WE CHOOSE TO DO SCREEN BOUNDARIES
            player = myNewPlayer;

            //assigns it to the logic layer
            myNewPlayer.transform.parent = logicLayer.transform;

            //sets the transform of the new player gameobject
            var hasSetTransform = false;

            //shuffles the order of the spawn points to randomize the location of each player
            for (int j = 0; j < m_playerSpawnPoints.Length; j++)
            {
                int rnd = Random.Range(0, m_playerSpawnPoints.Length);
                m_tempGameObject = m_playerSpawnPoints[rnd].gameObject;
                m_playerSpawnPoints[rnd] = m_playerSpawnPoints[j];
                m_playerSpawnPoints[j] = m_tempGameObject.GetComponent<PlayerSpawnPoint>();
            }

            //chooses a spawnpoint out of the available spawnpoints and places the player there
            foreach (PlayerSpawnPoint currentPlayerSpawnPoint in m_playerSpawnPoints)
            {
                if(!currentPlayerSpawnPoint.isOccupied && !hasSetTransform)
                {
                    myNewPlayer.transform.position = currentPlayerSpawnPoint.transform.position;
                    currentPlayerSpawnPoint.isOccupied = true;
                    hasSetTransform = true;

                }
            }


            //adding the necessary components to the player parent
            myNewPlayer.AddComponent<PlayerData>();
            myNewPlayer.AddComponent<VikingValkyrieSwitch>();

            //initializing the necessary variables for each component
            myNewPlayer.GetComponent<VikingValkyrieSwitch>().m_startViking = playerStartViking;
            m_currentPlayerData = myNewPlayer.GetComponent<PlayerData>();
            m_currentPlayerData.m_PlayerIndex = i + 1;
            m_currentPlayerData.m_CurrentScore = 0;
            m_currentPlayerData.scoreText = myNewPlayer.GetComponentInChildren<Text>();

            //instantiates children (viking, valkyrie and score prefabs)
            GameObject myNewVikingCharacter = Instantiate(vikingPrefab, myNewPlayer.transform);
            GameObject myNewValkyrieCharacter = Instantiate(valkyriePrefab, myNewPlayer.transform);
            GameObject myNewPlayerScoreUI = Instantiate(scorePrefab, myNewPlayer.transform);
            phaseManager.m_players[i] = myNewPlayer.GetComponent<PlayerData>();
			m_scoreManager.m_players[i] = myNewPlayer.GetComponent<PlayerData>();      

            snapPositionController.m_positionsZ.Add(myNewVikingCharacter.gameObject);
            snapPositionController.m_positionsZ.Add(myNewValkyrieCharacter.gameObject);
        }

    }



    }

    // Menu scene swap logic here? Check out https://www.youtube.com/watch?v=CPKAgyp8cno