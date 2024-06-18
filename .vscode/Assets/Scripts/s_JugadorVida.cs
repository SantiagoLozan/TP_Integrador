using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class s_JugadorVida : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;

    void Start()
    {
        health = maxHealth;
        Debug.Log("Vida inicial del jugador: " + health); // Muestra la vida inicial en la consola.
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        Debug.Log("Vida del jugador después del daño: " + health); // Muestra la vida después de recibir daño.

        if (health <= 0)
        {
            Debug.Log("Jugador muerto. Cambiando a la escena de Menu.");

            // Verificar si la escena "Menu" está en la configuración de compilación antes de cargarla
            if (SceneManager.GetSceneByName("SceneFinal") != null)
            {
                SceneManager.LoadScene("SceneFinal");
            }

            else
            {
                Debug.Log("aaaa");
            }
        }
    }
}