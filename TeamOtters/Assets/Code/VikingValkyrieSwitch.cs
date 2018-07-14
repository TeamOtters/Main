using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VikingValkyrieSwitch : MonoBehaviour {

    public bool m_startViking = true;
    public float m_particleDuration = 2f;

    private GameObject m_vikingCharacter;
    private GameObject m_valkyrieCharacter;
    private PlayerData m_playerData;
    internal bool m_isValkyrie;
    private Transform m_parentTransform;
    private GameObject m_scoreText;
    internal GameObject m_transformParticles;
    private GameObject m_particleSpawnPointViking;
    private GameObject m_particleSpawnPointValkyrie;

    private bool m_FirstTimeTransforming;

	// Use this for initialization
	void Start ()
    {
        m_FirstTimeTransforming = true;

        //initialize all components
        if (transform.childCount != 0)
        {
            //gets character objects and creating a spawn point for the particle on each gameobject
            m_vikingCharacter = GetComponentInChildren(typeof(VikingController), true).gameObject;
            
            m_particleSpawnPointViking = new GameObject("particleSpawnPointViking");
            m_particleSpawnPointViking.transform.parent = m_vikingCharacter.transform;
            m_particleSpawnPointViking.transform.position = m_vikingCharacter.GetComponent<SpriteRenderer>().bounds.center;

            m_valkyrieCharacter = GetComponentInChildren(typeof(ValkyrieController), true).gameObject;
            m_particleSpawnPointValkyrie = new GameObject("particleSpawnPointValkyrie");
            m_particleSpawnPointValkyrie.transform.parent = m_valkyrieCharacter.transform;
            m_particleSpawnPointValkyrie.transform.position = m_valkyrieCharacter.transform.Find("body_sprite").GetComponent<SpriteRenderer>().bounds.center;

            //rest of the components
            m_playerData = GetComponent<PlayerData>();
            //m_scoreText = GetComponentInChildren<Text>().gameObject.transform.parent.gameObject;
            m_parentTransform = this.gameObject.transform;
        }
        else
        {
            Debug.Log("Player gameobject does not have its children! Something is very wrong");
        }

        SwitchToViking();

        //sets starting state according to startViking variable
        StartCoroutine(LateStart());

    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (m_startViking)
        {
            SwitchToViking();

        }
        else
        {
            SwitchToValkyrie();
        }

    }
    // Switch to Viking
    public void SwitchToViking()
    {
        //Debug.Log("Switching");
        //Activating viking
        m_isValkyrie = false;
        if (m_vikingCharacter != null && m_valkyrieCharacter!=null)
        {
            // Awful awful code the valkyrie and viking have opposite facing directions xD
            bool myFacingDirection;
            myFacingDirection = m_valkyrieCharacter.GetComponent<ValkyrieController>().m_isFacingRight;
            m_vikingCharacter.GetComponent<VikingController>().SetFacingDirection(!myFacingDirection);

            m_vikingCharacter.SetActive(true);
            //m_vikingCharacter.GetComponent<VikingRespawn>().StopRespawn();
            m_vikingCharacter.GetComponent<VikingController>().StunnedCooldown();
            //childing all relevant objects to their positions
            m_vikingCharacter.transform.parent = m_parentTransform;
            m_valkyrieCharacter.transform.parent = m_vikingCharacter.transform;
            //m_scoreText.transform.parent = m_vikingCharacter.transform;
            m_transformParticles.transform.parent = m_particleSpawnPointViking.transform;
            m_transformParticles.transform.position= m_particleSpawnPointViking.transform.position;
            //transformation effect
            TransformationEffect();
            //deactivate valkyrie
            m_valkyrieCharacter.GetComponent<ValkyrieController>().DropPickup();
            m_valkyrieCharacter.SetActive(false);
        }


    }

    private void Update() // this is only for debug purposes, nothing should need to live in here!
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(m_isValkyrie)
            {
                SwitchToViking();
            }
            else
            {
                SwitchToValkyrie();
            }
        }
    }

    // Switch to Valkyrie
    public void SwitchToValkyrie()
    {
        //Debug.Log("Switching");
        //Activating valkyrie, childing viking to valkyrie and deactivating viking
        m_isValkyrie = true;
        if (m_vikingCharacter != null && m_valkyrieCharacter != null)
        {          
            m_valkyrieCharacter.SetActive(true);
            //childing all relevant objects to their positions
            m_valkyrieCharacter.transform.parent = m_parentTransform;
            m_vikingCharacter.GetComponent<VikingRespawn>().StopRespawn();
            m_vikingCharacter.transform.parent = m_valkyrieCharacter.transform;
            //m_scoreText.transform.parent = m_valkyrieCharacter.transform;
            m_transformParticles.transform.parent = m_particleSpawnPointValkyrie.transform;
            m_transformParticles.transform.position = m_particleSpawnPointValkyrie.transform.position;

            // Awful awful code the valkyrie and viking have opposite facing directions xD
            bool myFacingDirection;
            myFacingDirection = m_vikingCharacter.GetComponent<VikingController>().m_turnedLeft;

            if (m_FirstTimeTransforming && myFacingDirection == false)
            {
                m_valkyrieCharacter.GetComponent<ValkyrieController>().ForceFacingDirectionToBeRight();
                m_FirstTimeTransforming = false;
            }
            else
                m_valkyrieCharacter.GetComponent<ValkyrieController>().SetFacingDirection(!myFacingDirection); //this is the worst thing Ive ever done in my life Im so sorry

            //transformation effect
            TransformationEffect();
            //deactivate viking
            m_vikingCharacter.SetActive(false);
        }
        
    }

    void TransformationEffect()
    {
        ParticleSystem [] particles = m_transformParticles.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
            StartCoroutine(StopParticles(particle, m_particleDuration));
        }
    }

    IEnumerator StopParticles(ParticleSystem particle, float duration)
    {
        yield return new WaitForSeconds(duration);
        particle.Stop();
    }


}
