using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool charging = false;
    private float stoppingDistance;
    [SerializeField] private float windupTime = .5f;
    [SerializeField] private float chargeMult = 2;
    [SerializeField] private Collider attackHitbox;
    [SerializeField] private AudioClip chargeSFX;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stoppingDistance = agent.stoppingDistance;
    }

    private void FixedUpdate()
    {
        if(!charging) agent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        if(Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance && !charging)
        {
            charging = true;
            StartCoroutine(_charge());
        }
    }

    private IEnumerator _charge()
    {
        yield return new WaitForSeconds(windupTime);
        // Debug.Log("charge");
        AudioManager.Instance.PlaySound("SFX", 0.5f, chargeSFX);
        attackHitbox.enabled = true;
        agent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        agent.speed *= chargeMult;
        agent.acceleration *= chargeMult;
        agent.stoppingDistance = 0;
        yield return new WaitForSeconds(windupTime * 2);
        attackHitbox.enabled = false;
        agent.speed /= chargeMult;
        agent.acceleration /= chargeMult;
        agent.stoppingDistance = stoppingDistance;
        charging = false;
    }
}
