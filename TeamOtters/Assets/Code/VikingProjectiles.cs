using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "Projectile")]
public class VikingProjectiles : ScriptableObject {

    public GameObject m_projectile;
    [HideInInspector]
    public string m_prefabLoadPath;
    public float m_force;
    public float m_damage;

	// Use this for initialization
	void Start ()
    {
        m_prefabLoadPath = m_projectile.name;
	}

    public void InstantiateProjecte ()
    {

    }
    


    IEnumerator SelfDestroyProjectileTimer()
    {
        yield return new WaitForSeconds(10f);
        SelfDestroyer();
    }
	
	private void SelfDestroyer ()
    {
        Destroy(this);
    }
}
