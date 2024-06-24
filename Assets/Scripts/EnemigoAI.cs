using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // Asegúrate de incluir esto

public class EnemigoAI : MonoBehaviour
{
    public NavMeshAgent agente;
    public Transform jugador;
    public LayerMask esSuelo, esJugador;

    public float vida;

    // Recorrido normal
    public Vector3 caminarPunto;
    bool caminarPuntoSet;

    public float caminarPuntoRango;

    // Atacar
    public float tiempoEntreAtaque;
    bool atacando;

    public GameObject proyectile;

    // Estados
    public float rangoVision, rangoAtaque;
    public bool jugadorEnRangoVision, jugadorEnRangoAtaque;

    private enum Estado { Idle, Patrullar, Perseguir, Atacar }
    private Estado estadoActual;
    private bool jugadorSeMovio;
    private Vector3 posicionInicialJugador;

    private void Awake()
    {
        jugador = GameObject.Find("Jugador").transform;

        // Intenta obtener el componente NavMeshAgent
        agente = GetComponent<NavMeshAgent>();

        if (agente == null)
        {
            Debug.LogError("No se encontró el componente NavMeshAgent en el objeto " + gameObject.name);
        }
        estadoActual = Estado.Idle;
        posicionInicialJugador = jugador.position;
        jugadorSeMovio = false;
    }

    private void Update()
    {
        if (Vector3.Distance(posicionInicialJugador, jugador.position) > 0.1f)
        {
            jugadorSeMovio = true;
        }

        jugadorEnRangoVision = Physics.CheckSphere(transform.position, rangoVision, esJugador);
        jugadorEnRangoAtaque = Physics.CheckSphere(transform.position, rangoAtaque, esJugador);

        // Manejo de estados
        switch (estadoActual)
        {
            case Estado.Idle:
                Idle();
                break;
            case Estado.Patrullar:
                Recorrer();
                break;
            case Estado.Perseguir:
                PerseguirJugador();
                break;
            case Estado.Atacar:
                AtacarJugador();
                break;
        }

        // Cambiar de estado basado en la distancia al jugador
        if (estadoActual != Estado.Atacar)
        {
            if (jugadorEnRangoAtaque && jugadorEnRangoVision)
                estadoActual = Estado.Atacar;
            else if (jugadorEnRangoVision)
                estadoActual = Estado.Perseguir;
            else if (jugadorSeMovio)
                estadoActual = Estado.Patrullar;
            else
                estadoActual = Estado.Idle; // Cambia al estado Idle si el jugador no se ha movido
        }
        else
        {
            // Si el jugador ya no está en rango de ataque, volver a patrullar
            if (!jugadorEnRangoAtaque)
            {
                if (jugadorEnRangoVision)
                    estadoActual = Estado.Perseguir;
                else
                    estadoActual = Estado.Patrullar;
            }
        }
    }

    private void Idle()
    {
        // Mantener al enemigo en su posición actual
        agente.SetDestination(transform.position);
    }

    private void Recorrer()
    {
        if (!caminarPuntoSet) BuscaCamino();

        if (caminarPuntoSet)
            agente.SetDestination(caminarPunto);

        Vector3 distanciaCaminarPunto = transform.position - caminarPunto;

        // Llega al punto
        if (distanciaCaminarPunto.magnitude < 1f)
            caminarPuntoSet = false;
    }

    private void BuscaCamino()
    {
        float randomX = Random.Range(-caminarPuntoRango, caminarPuntoRango);
        float randomZ = Random.Range(-caminarPuntoRango, caminarPuntoRango);

        caminarPunto = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(caminarPunto, -transform.up, 2f, esSuelo))
            caminarPuntoSet = true;
    }

    private void PerseguirJugador()
    {
        agente.SetDestination(jugador.position);
    }

    private void AtacarJugador()
    {
        agente.SetDestination(transform.position);
        transform.LookAt(jugador);

        if (!atacando)
        {
            Rigidbody rb = Instantiate(proyectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            atacando = true;
            Invoke(nameof(ReinicioAtaque), tiempoEntreAtaque);
        }
    }

    private void ReinicioAtaque()
    {
        atacando = false;
    }

    public void RecibirDaño(int daño)
    {
        vida -= daño;

        if (vida <= 0) Invoke(nameof(DestruirEnemigo), 0.5f);
    }

    private void DestruirEnemigo()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);
    }

    private void OnColliderEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //SceneManager.LoadScene("SceneFinal"); // Cambia "SceneFinal" por el nombre de la escena que quieres cargar.
        }
    }
}
