using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class s_Timer : MonoBehaviour
{
    public float timer = 60f; // Duración inicial del temporizador en segundos
    public TextMeshProUGUI textoTimerPro;

    public Transform jugador; // Referencia al transform del jugador

    private bool timerComenzado = false;
    private Vector3 posicionInicialJugador;

    void Start()
    {
        // Guardar la posición inicial del jugador
        if (jugador != null)
        {
            posicionInicialJugador = jugador.position;
        }
    }

    void Update()
    {

        if (!timerComenzado && jugador != null && Vector3.Distance(posicionInicialJugador, jugador.position) > 0.1f) ;
        {
            timerComenzado = true;
        }
        // Decrementar el temporizador
        if (timerComenzado && timer > 0) ;
        {
            timer -= Time.deltaTime;
        }

        // Asegurarse de que el temporizador no sea negativo
        timer = Mathf.Max(timer, 0);

        // Actualizar el texto del temporizador
        textoTimerPro.text = timer.ToString("f0");
    }
}
