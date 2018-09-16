using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour {

    [Header ("Settings")]
    public bool m_startInPhaseOne = true;
    public float m_playerTransformationDuration = 0.2f;
    public float m_phaseChangeDuration = 2f;
    public float m_phase2Duration = 10f;
    public float m_characterUpwardsMoveSpeed = 1.2f;

    [Header ("Objects")]
    public BouncingBall m_bouncingBall;
    public GameObject m_dragon;
    public GameObject m_gatesOpening;
    public GameObject m_Level;

    [Header("UI")]
    public Phase2UI m_phase2UI;
    public Canvas m_Phase1UI;
    public Canvas m_Results;
    public Button m_restart;

    [HideInInspector]
    internal List<PlayerData> m_players = new List<PlayerData>();
    //public PlayerData[] m_players;
    internal bool m_isInPhaseOne = true;
    private List<int> m_playerScores = new List<int>();
    private bool m_phaseSet = false;
    private RumbleManager m_rumbleManager;
    private GameController m_gameController;
    private ScoreManager m_scoreManager;
    internal bool m_phaseTransformationActive = false;
    private bool m_isWaiting = false;

    // for celebration
    internal bool m_isCelebratingActive = false;
    internal bool m_isCelebratingWaiting = false;
    public Transform m_destinationScreenPos;
    private GameObject winnerCharacter;
    public bool m_hasReachedValhalla;

    void Start ()
    {
        m_Phase1UI.gameObject.SetActive(true);

        m_gameController = GameController.Instance;
        m_rumbleManager = m_gameController.rumbleManager;
        m_scoreManager = m_gameController.m_scoreManager;

        for (int i = 0; i < m_gameController.numberOfPlayers; i++)
        {
            m_playerScores.Add(0);
        }
        
        float x = (m_gameController.boundaryHolder.playerBoundary.Left + m_gameController.boundaryHolder.playerBoundary.Right) / 2;
        float y = m_gameController.boundaryHolder.playerBoundary.Up - 1f;

        //m_destinationScreenPos.position = new Vector3(x, y, 0);     

        //allows the devs to set the starting phase
        if (m_startInPhaseOne == true)
        {
            PhaseOneSetup();
        }
        else
        {
            PhaseTwoSetup();
        }
        m_dragon.SetActive(false);
        m_gatesOpening.SetActive(false);
    }

    void Update()
    {
        CheckPhaseSwitch();
        if (m_isWaiting)
        {
            foreach (PlayerData player in m_players)
            {
                GameObject viking = player.GetComponentInChildren(typeof(VikingController), true).gameObject;
                viking.transform.Translate(Vector3.up * m_characterUpwardsMoveSpeed * Time.deltaTime);
            }
        }


        // TODO - Steph started the celebration. Viking winner lift up
        CheckCelebrationSwitch();
        if (m_isCelebratingWaiting)
        {
            int playerIndex = m_scoreManager.m_ranks[0].playerIndex;
            foreach (PlayerData player in m_players)
            {
                if (m_players[playerIndex - 1].m_PlayerIndex == m_scoreManager.m_ranks[0].playerIndex)
                {
                    // For now we assume the winner is a viking is on its own    
                    if (player.GetComponentInChildren<Rigidbody>().tag == "Viking")
                    {
                        GameObject winnerCharacter = player.GetComponentInChildren(typeof(VikingController), true).gameObject;

                        winnerCharacter.transform.position = Vector3.Lerp(winnerCharacter.transform.position, m_destinationScreenPos.position, m_characterUpwardsMoveSpeed * Time.deltaTime);
                                                
                        if (winnerCharacter.transform.position.y == m_destinationScreenPos.position.y)
                        {
                            m_hasReachedValhalla = true;
                        }
                    }
                }
                else
                {
                    // if the players are not the highest, they should play sad anims                    
                }
            }
        }
        
    }


    // the condition for phase 2 switch - ball health and debug command
    private void CheckPhaseSwitch()
    {        
        if ((!m_bouncingBall.m_isAlive && !m_phaseSet)|| Input.GetKeyDown(KeyCode.P))
        {
            m_phaseSet = true;
            StartCoroutine(PhaseChangeStart(m_phaseChangeDuration));
        }
        
    }

    // the condition for phase 2 switch - ball health and debug command
    private void CheckCelebrationSwitch()
    {
        // TODO If camera has panned into position, trigger the valhalla gates effects and then start the celebration.

        if (m_gameController.goalLine.m_hasReachedValhalla == true || (Input.GetKeyDown(KeyCode.Q)))
        {
            m_gameController.cameraManager.SetCelebrateState(true);
            m_phaseSet = true;
            StartCoroutine(CelebrationPhaseStart(5));
        }
    }

    //phase 1 setup - called on start
    void PhaseOneSetup()
    {
        m_dragon.SetActive(false);
        m_gatesOpening.SetActive(false);
        m_isInPhaseOne = true;
        foreach(PlayerData player in m_players)
        {
            //Accesses the Valkyrie/Viking switch in all players and does the switch to viking
            var mySwitchScript = player.gameObject.GetComponent<VikingValkyrieSwitch>();
            if (mySwitchScript!=null && mySwitchScript.m_startViking == true)
            {
                mySwitchScript.SwitchToViking();
            }
        }
        //respawns the bouncing ball in phase 1 if we want
        if(!m_bouncingBall.m_isAlive)
        {
            m_bouncingBall.Respawn();
            Debug.Log("Bouncing ball respawn triggered");
        }
    }

    IEnumerator CelebrationPhaseStart(float duration)
    {
        m_isCelebratingWaiting = true;
        m_isCelebratingActive = true;

        GameObject viking;
        //GameObject valkyrie;

        foreach (PlayerData player in m_players)
        {
            if (player.GetComponentInChildren<Rigidbody>().tag == "Viking")
            {
                viking = player.GetComponentInChildren(typeof(VikingController), true).gameObject;
                viking.GetComponent<VikingController>().enabled = false;
            }
        }

        yield return new WaitForSeconds(duration);
        m_gatesOpening.SetActive(true);
        m_isCelebratingWaiting = false;
        m_isCelebratingActive = false;
        StartCoroutine(Wait());
    }

    //Character Transformation and Character Transformation Delay loops around so that each character gets spawned together with their UI element in a delay.
    IEnumerator CharacterAscending(int i)
    {
        int playerIndex = m_scoreManager.m_ranks[i].playerIndex;
        GameObject viking;

        foreach (PlayerData player in m_players)
        {
            if (player.GetComponentInChildren<Rigidbody>().tag == "Viking")
            {
                viking = m_players[playerIndex - 1].GetComponentInChildren(typeof(VikingController), true).gameObject;
                viking.GetComponent<VikingController>().enabled = true;
            }
        }

        // Once viking has reached destination, trigger the scoreboard
        StartCoroutine(Wait());
        yield return null;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
        m_Results.gameObject.SetActive(true);
        m_Results.GetComponentInChildren<Results>().ShowResults();
        m_restart.Select();    
    }

    //dramatic moment before phase 2 start
    IEnumerator PhaseChangeStart(float duration)
    {
        m_isWaiting = true;
        m_phaseTransformationActive = true;
        m_dragon.SetActive(true);
        m_rumbleManager.PhaseOneShake();
        GameObject viking;
        foreach (PlayerData player in m_players)
        {
            viking = player.GetComponentInChildren(typeof(VikingController),true).gameObject;
            viking.GetComponent<VikingController>().enabled = false;

            //StartCoroutine(MoveViking(viking));
        }
        
        yield return new WaitForSeconds(duration);
        m_isWaiting = false;
        //m_dragon.SetActive(false);
        m_phaseTransformationActive = false;
        StartCoroutine(CharacterTransformation(0));
    }

    //Character Transformation and Character Transformation Delay loops around so that each character gets spawned together with their UI element in a delay.
    IEnumerator CharacterTransformation(int i)
    {
        int playerIndex = m_scoreManager.m_ranks[i].playerIndex;
        var mySwitchScript = m_players[playerIndex - 1].gameObject.GetComponent<VikingValkyrieSwitch>();
        if (m_players[playerIndex-1].m_PlayerIndex != m_scoreManager.m_ranks[0].playerIndex) // if the players are not the highest, they should change into valkyries
        {
            if (mySwitchScript != null)
            {
                // valkyrie transform camera shake + rumble
                mySwitchScript.SwitchToValkyrie();

            }
        }
        else
        {
            mySwitchScript.SwitchToViking();
        }
        GameObject viking = m_players[playerIndex - 1].GetComponentInChildren(typeof(VikingController), true).gameObject;
        viking.GetComponent<VikingController>().enabled = true;

        m_phase2UI.ActivateScoreboard(i);
        StartCoroutine(CharacterTransformationDelay(i));
        yield return null;
    }

    IEnumerator CharacterTransformationDelay(int i)
    {
        Debug.Log("Delay");
        yield return new WaitForSeconds(m_playerTransformationDuration);
        if (i < m_players.Count -1)
        {
            i++;
            StartCoroutine(CharacterTransformation(i));
        }
        else
        {
            PhaseTwoSetup();
        }
    }

    //Set the two characters with highest score to Valkyries
    void PhaseTwoSetup()
    {
        Debug.Log("set up phase 2!");
        m_phaseSet = true;//prevents phase switch from happening every frame
        m_phase2UI.ShowPrompt();
        m_isInPhaseOne = false; //for other scripts to listen to
        GameController.Instance.m_currentPhaseState = 2;

        m_phase2UI.HidePrompt();
        GameController.Instance.cameraManager.SetRaceState(true);

    }

    //Set the two characters with highest score to Valkyries
    void CelebrationSetup()
    {
        Debug.Log("set up celebration!");
        m_phaseSet = true;//prevents phase switch from happening every frame

        //Hide GUI and wait for celebration

        //GameController.Instance.m_currentPhaseState = 2;
        //GameController.Instance.cameraManager.SetRaceState(true);

    }

}
