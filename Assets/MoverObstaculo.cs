using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObstaculo : MonoBehaviour
{
    public Vector3 sentidoMovimento = Vector3.left;
    public float velocidade = 2f;
    public float delayInverterMovimento = 2f;
    private int direcao = 1;

    private void Awake()
    {
        InvokeRepeating(
            nameof(InverterDirecao),
            delayInverterMovimento,
            delayInverterMovimento
        );
    }

    private void Update()
    {
        transform.Translate(
            sentidoMovimento * direcao * velocidade * Time.deltaTime,
            Space.World
        );
    }

    private void InverterDirecao()
    {
        direcao *= -1;
    }
}