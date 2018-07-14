using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //public static GameController Instance = null;

    //ORiginal
    // public static GameController Instance { get; private set; }

    private static GameController instance;

    public BoundaryHolder boundaryHolder;
    public GameObject player;
    public PhaseManager phaseManager;
    public CameraBehaviourManager cameraManager;
    public SnapPositionController snapPositionController;
    public PlayerUI playerUI;

    public PlayerComponents playerComponents;
    
    public PlayerData playerData;
    public AudioManager audioManager;
    /*
    public GameObject vikingPrefab;
    public GameObject valkyriePrefab;
    public GameObject scorePrefab;
    public GameObject transformParticles;
    */
    

    public GameObject logicLayer;
    public RumbleManager rumbleManager;
    private SnapablePlayers snapablePlayers;

    public ScoreManager m_scoreManager;


    public int numberOfPlayers = 4; //could this number read from number of plugged in controllers or menu in case we want to support more than 4 players?
    public bool playerStartViking = true;
    public float startGameDuration = 1.7f;

    /*public int bounceHit = 10;
    public int firstReachGoal = 10;
    public int carryingBonus = 20;
    public int hitOpponent = 10;*/

    public float snapGridZ = 0f;

    //used when creating the new players automatically
    private List<GameObject> m_players = new List<GameObject>();
    private PlayerData m_currentPlayerData;
    private PlayerSpawnPoint[] m_playerSpawnPoints;
    private GameObject m_tempGameObject;
    [HideInInspector]
    public int m_currentPhaseState;

    public static GameController Instance
    {
        // get { return instance ?? (instance = Instantiate(Resources.Load("GameController", typeof(GameObject)) as GameObject; }
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            // First time creation, sets first Instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }       
        else
        {
            // Keep original instance
            Destroy(gameObject);
        }

        CreatePlayers();

        SceneManager.LoadScene("MainGame_Level", LoadSceneMode.Additive); 

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
            //m_currentPlayerData.playerIndicatorText = myNewPlayer.GetComponentInChildren<Text>();

            //instantiates children (viking, valkyrie and score prefabs)
            GameObject myNewVikingCharacter = Instantiate(playerComponents.vikingPrefab, myNewPlayer.transform);
            GameObject myNewValkyrieCharacter = Instantiate(playerComponents.valkyriePrefab, myNewPlayer.transform);
            //GameObject myNewPlayerScoreUI = Instantiate(playerComponents.scorePrefab, myNewPlayer.transform);
            GameObject myTransformParticles = Instantiate(playerComponents.transformParticles, myNewPlayer.transform);
            myNewPlayer.GetComponent<VikingValkyrieSwitch>().m_transformParticles = myTransformParticles;
            phaseManager.m_players[i] = myNewPlayer.GetComponent<PlayerData>();
			m_scoreManager.m_players[i] = myNewPlayer.GetComponent<PlayerData>();      

            snapPositionController.m_positionsZ.Add(myNewVikingCharacter.gameObject);
            snapPositionController.m_positionsZ.Add(myNewValkyrieCharacter.gameObject);
            m_players.Add(myNewPlayer);
        }

    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        SetPlayerInactiveOnStart();

    }

    void SetPlayerInactiveOnStart()
    {
        foreach (GameObject player in m_players)
        {
            player.SetActive(false);
            StartCoroutine(StartGameDuration(startGameDuration));
        }
    }

    IEnumerator StartGameDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetPlayersActive();

    }

    void SetPlayersActive()
    {
        foreach (GameObject player in m_players)
        {
            player.SetActive(true);
        }
    }


}

    // Menu scene swap logic here? Check out https://www.youtube.com/watch?v=CPKAgyp8cno