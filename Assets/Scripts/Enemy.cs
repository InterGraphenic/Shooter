using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Vector3 target;
    public GameObject player;
    bool firstTime = true;
    public float maxHealth;
    float health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        bool patrol = true;
        if(player != null){
            Vector3 v = player.transform.position - transform.position;
            v.Normalize();
            if(Vector3.Dot(v, transform.forward) >= 0.707f)
            {
                if(!Physics.Linecast(transform.position, player.transform.position, LayerMask.GetMask("Wall"))){
                    patrol = false;
                    target = player.transform.position;
                }           
            }
        }  
        if(patrol)
        {
            Debug.Log(target);
            Vector3 targethorizontal = new Vector3(target.x, 0, target.z);
            Vector3 poshorizontal = new Vector3(transform.position.x, 0, transform.position.z);
            if(firstTime || ((targethorizontal - poshorizontal).magnitude < 0.1f && Mathf.Abs(target.y - transform.position.y) < 1f))
            {
                Vector3 randpos = transform.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                if(NavMesh.SamplePosition(randpos, out _, 1f, NavMesh.AllAreas))
                {
                    firstTime = false;
                    target = randpos;
                } 
            }
        }
        // Debug.Log((target - transform.position).magnitude);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target;
        Debug.DrawLine(transform.position, target, Color.red);
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            health -= 10f;
            Flash();
        }
    }

    void Flash()
    {
        foreach(Flash flash in GetComponentsInChildren<Flash>())
        {
            flash.StartFlash();
        }
    }
}
