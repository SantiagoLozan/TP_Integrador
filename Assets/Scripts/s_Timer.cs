using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class s_Timer : MonoBehaviour
{
    public float timer = 60f; // Duración inicial del temporizador en segundos
    public TextMeshProUGUI textoTimerPro;

    public Transform jugador; // Referencia al transform del jugador

    private bool timerComenzado = false;
    private Vector3 posicionAnteriorJugador;

    void Start()
    {
        // Guardar la posición inicial del jugador
        if (jugador != null)
        {
            posicionAnteriorJugador = jugador.position;
        }

    }

    void Update()
    {
        if (jugador != null)
        {
            // Verificar si el jugador se está moviendo
            float distancia = Vector3.Distance(posicionAnteriorJugador, jugador.position);
            //Debug.Log("Distancia recorrida: " + distancia);

            if (distancia > 0.01f)
            {
                timerComenzado = true;
                //  Debug.Log("Jugador se está moviendo.");
                posicionAnteriorJugador = jugador.position;
            }
            else
            {
                timerComenzado = false;
                // Debug.Log("Jugador está quieto.");
            }
        }

        // Decrementar el temporizador si ha comenzado
        if (timerComenzado && timer > 0)
        {
            timer -= Time.deltaTime;
            // Debug.Log("Temporizador decrementado: " + timer);
        }

        // Asegurarse de que el temporizador no sea negativo
        timer = Mathf.Max(timer, 0);

        // Actualizar el texto del temporizador
        textoTimerPro.text = timer.ToString("f0");

        if (timer == 0)
        {
            SceneManager.LoadScene("SceneGanador");
        }
    }
}
