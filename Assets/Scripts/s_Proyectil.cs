using UnityEngine;

public class s_Proyectil : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private LayerMask esSuelo;


    private void OnCollisionEnter(Collision collision)
    {
        s_JugadorVida playerHealth = collision.gameObject.GetComponent<s_JugadorVida>();
        if (playerHealth)
        {
            playerHealth.GetDamage(damage);
            Destroy(gameObject);
            Debug.Log("hit", playerHealth);
        }
        else
        {
            Destroy(gameObject, 5f);
        }
    }
}