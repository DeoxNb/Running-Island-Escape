using UnityEngine;
using RunningIsland.Entidades;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class FocoFuego : MonoBehaviour
    {
        [SerializeField] private int dañoBase = 20;
        [SerializeField] private float tiempoEntreDaño = 1f;

        private float _temporizadorDaño;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void Update()
        {
            _temporizadorDaño += Time.deltaTime;
        }

        private void OnTriggerStay2D(Collider2D otroColisionador)
        {
            if (_temporizadorDaño >= tiempoEntreDaño && otroColisionador.CompareTag("Player"))
            {
                AplicarDañoAlJugador(otroColisionador.gameObject);
                _temporizadorDaño = 0f;
            }
        }

        private void AplicarDañoAlJugador(GameObject jugador)
        {
            if (jugador.TryGetComponent<SaludJugador>(out var salud) && jugador.TryGetComponent<ControladorJugador>(out var controlador))
            {
                float multiplicador = 1f;
                if (controlador.PersonajeActivo != null)
                {
                    multiplicador = controlador.PersonajeActivo.MultiplicadorMitigacionFuego;
                }

                int dañoFinal = Mathf.RoundToInt(dañoBase * multiplicador);
                salud.RecibirDaño(dañoFinal);
            }
        }
    }
}