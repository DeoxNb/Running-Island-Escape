using UnityEngine;
using RunningIsland.Entidades;
using RunningIsland.Entorno;

namespace RunningIsland.Ambiental
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FuegoFisico : MonoBehaviour
    {
        [SerializeField] private float dañoPorPulso = 8f;
        [SerializeField] private float intervaloDaño = 0.5f;
        [SerializeField] private Sprite fase1FuegoPequeño;
        [SerializeField] private Sprite fase2FuegoVivo;
        [SerializeField] private SpriteRenderer spriteRendererComponente;

        private float _cronometroPulso = 0f;
        private bool _jugadorEnContacto = false;
        private SaludJugador _saludJugador;
        private ControladorJugador _controladorJugador;

        private void Awake()
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
        }

        private void Update()
        {
            if (!_jugadorEnContacto || _saludJugador == null) return;

            float factorMitigacion = 1f;
            if (_controladorJugador != null && _controladorJugador.PersonajeActivo != null)
            {
                factorMitigacion = _controladorJugador.PersonajeActivo.MultiplicadorMitigacionFuego;
            }

            _cronometroPulso += Time.deltaTime;
            if (_cronometroPulso >= intervaloDaño)
            {
                _cronometroPulso = 0f;
                int dañoFinal = Mathf.RoundToInt(dañoPorPulso * factorMitigacion);
                _saludJugador.RecibirDaño(dañoFinal);
            }
        }

        public void CambiarFaseVisual(int fase)
        {
            if (spriteRendererComponente == null) return;

            if (fase == 1 && fase1FuegoPequeño != null)
            {
                spriteRendererComponente.sprite = fase1FuegoPequeño;
            }
            else if (fase == 2 && fase2FuegoVivo != null)
            {
                spriteRendererComponente.sprite = fase2FuegoVivo;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _jugadorEnContacto = true;
                _saludJugador = collision.GetComponent<SaludJugador>();
                _controladorJugador = collision.GetComponent<ControladorJugador>();
                _cronometroPulso = intervaloDaño;
            }
            else if (collision.TryGetComponent<ObjetoRecogible>(out var objeto))
            {
                objeto.QuemarPorCompleto();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _jugadorEnContacto = false;
                _saludJugador = null;
                _controladorJugador = null;
            }
        }
    }
}