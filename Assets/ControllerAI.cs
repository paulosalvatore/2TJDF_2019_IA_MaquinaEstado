using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerAI : MonoBehaviour
{
    public enum Estados
    {
        ESPERAR,
        PATRULHAR,
        PERSEGUIR
    }

    private Estados estadoAtual;

    private Transform alvo;

    private NavMeshAgent navMeshAgent;

    private Transform player;

    // Estado: Esperar
    [Header("Estado: Esperar")]
    public float tempoEsperar = 2f;
    private float tempoEsperando = 0f;

    // Estado: Patrulhar
    [Header("Estado: Patrulhar")]
    public Transform waypoint1;
    public Transform waypoint2;
    private Transform waypointAtual;
    public float distanciaMinimaWaypoint = 1f;
    private float distanciaWaypointAtual;

    // Estado: Perseguir
    [Header("Estado: Perseguir")]
    public float campoVisao = 5f;
    private float distanciaJogador;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        waypointAtual = waypoint1;

        Esperar();
    }

    private void Update()
    {
        ChecarEstados();
    }

    private void ChecarEstados()
    {
        if (estadoAtual != Estados.PERSEGUIR && PossuiVisaoJogador())
        {
            Perseguir();

            return;
        }

        switch (estadoAtual)
        {
            case Estados.ESPERAR:
                if (EsperouTempoSuficiente())
                {
                    Patrulhar();
                }
                else
                {
                    alvo = transform;
                }

                break;

            case Estados.PATRULHAR:
                if (PertoWaypointAtual())
                {
                    Esperar();

                    AlternarWaypoint();
                }
                else
                {
                    alvo = waypointAtual;
                }

                break;

            case Estados.PERSEGUIR:
                if (!PossuiVisaoJogador())
                {
                    Esperar();
                }
                else
                {
                    alvo = player;
                }

                break;
        }

        navMeshAgent.destination = alvo.position;
    }

    #region ESPERAR

    private void Esperar()
    {
        estadoAtual = Estados.ESPERAR;
        tempoEsperando = Time.time;
    }

    private bool EsperouTempoSuficiente()
    {
        return tempoEsperando + tempoEsperar <= Time.time;
    }

    #endregion ESPERAR

    #region PATRULHAR

    private void Patrulhar()
    {
        estadoAtual = Estados.PATRULHAR;
    }

    private bool PertoWaypointAtual()
    {
        distanciaWaypointAtual = Vector3.Distance(
            transform.position,
            waypointAtual.position
        );

        return distanciaWaypointAtual <= distanciaMinimaWaypoint;
    }

    private void AlternarWaypoint()
    {
        waypointAtual = (waypointAtual == waypoint1) ? waypoint2 : waypoint1;
    }

    #endregion PATRULHAR

    #region PERSEGUIR

    private void Perseguir()
    {
        estadoAtual = Estados.PERSEGUIR;
    }

    private bool PossuiVisaoJogador()
    {
        distanciaJogador = Vector3.Distance(
            transform.position,
            player.position
        );

        return distanciaJogador <= campoVisao;
    }

    #endregion PERSEGUIR
}