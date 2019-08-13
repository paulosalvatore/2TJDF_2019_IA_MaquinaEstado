using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerAI : MonoBehaviour
{
    public enum Estados
    {
        ESPERAR,
        PATRULHAR
    }

    private Estados estadoAtual;

    private Transform alvo;

    private NavMeshAgent navMeshAgent;

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

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        waypointAtual = waypoint1;

        Esperar();
    }

    private void Update()
    {
        ChecarEstados();
    }

    private void ChecarEstados()
    {
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
}