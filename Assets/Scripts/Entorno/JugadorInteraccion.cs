using UnityEngine;
using RunningIsland.Datos;

namespace RunningIsland.Entidades
{
    public class JugadorInteraccion : MonoBehaviour
    {
        [SerializeField] private float radioInteraccion = 2f;
        [SerializeField] private LayerMask capaInteractuable;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IntentarInteractuar();
            }
        }

        private void IntentarInteractuar()
        {
            Collider2D[] colisionadores = Physics2D.OverlapCircleAll(transform.position, radioInteraccion, capaInteractuable);
            
            float distanciaMinima = float.MaxValue;
            Entorno.ObjetoRecogible objetoMasCercano = null;

            foreach (Collider2D colisionador in colisionadores)
            {
                Entorno.ObjetoRecogible objeto = colisionador.GetComponent<Entorno.ObjetoRecogible>();
                if (objeto != null)
                {
                    float distancia = Vector2.Distance(transform.position, colisionador.transform.position);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        objetoMasCercano = objeto;
                    }
                }
            }

            if (objetoMasCercano != null)
            {
                objetoMasCercano.Recoger();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radioInteraccion);
        }
    }
}