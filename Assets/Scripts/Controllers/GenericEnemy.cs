using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class GenericEnemy : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent navMesh;
    private bool podeAtacar;
    PlayerController play = new PlayerController();

    private void Awake()
    {
        transform.tag = "Enemy";

    }


    // Start is called before the first frame update
    void Start()
    {
        podeAtacar = true;
        player = GameObject.FindWithTag("Player");
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMesh.destination = player.transform.position;

        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
        {
            Atacar(40);
        }
        
    }

    void Atacar(int damage)
    {
        if (podeAtacar == true)
        {
            StartCoroutine("TempoDeAtaque");
            player.GetComponent<PlayerController>().playerLife -= damage;
        }
    }

    IEnumerator TempoDeAtaque()
    {
        podeAtacar = false;
        yield return new WaitForSeconds(5);
        podeAtacar = true;
    }
}
