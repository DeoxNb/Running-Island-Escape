using UnityEngine;
using RunningIsland.Entidades;
using RunningIsland.Gestores;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ZonaEscapeBunker : MonoBehaviour
    {
        [SerializeField] private float tiempoEstarQuietoNecesario = 3f;

        private bool _jugadorDentroLava = false;
        private float _cronometroQuieto = 0f;
        private Vector3 _ultimaPosicionJugador;
        private Transform _transformJugador;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void Update()
        {
            if (!_jugadorDentroLava || _transformJugador == null) return;

            if (Vector3.Distance(_transformJugador.position, _ultimaPosicionJugador) < 0.01f)
            {
                _cronometroQuieto += Time.deltaTime;
                if (_cronometroQuieto >= tiempoEstarQuietoNecesario)
                {
                    _jugadorDentroLava = false;
                    EvaluarAccesoBunker();
                }
            }
            else
            {
                _cronometroQuieto = 0f;
                _ultimaPosicionJugador = _transformJugador.position;
            }
        }

        private void EvaluarAccesoBunker()
        {
            int notaVolcan = PlayerPrefs.GetInt("Nota_Descubierta_NotaFuturista", 0);
            int notaJungla = PlayerPrefs.GetInt("Nota_Descubierta_NotaMaya", 0);
            int notaPlaya = PlayerPrefs.GetInt("Nota_Descubierta_NotaPirata", 0);
            int notaNieve = PlayerPrefs.GetInt("Nota_Descubierta_NotaVikingo", 0);

            if (notaVolcan == 1 && notaJungla == 1 && notaPlaya == 1 && notaNieve == 1)
            {
                if (GestorEscapes.Instancia != null)
                {
                    GestorEscapes.Instancia.CompletarEscape("Bunker");
                }
            }
            else
            {
                if (_transformJugador != null && _transformJugador.TryGetComponent<SaludJugador>(out var salud))
                {
                    salud.RecibirDaño(salud.VidaMaxima);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D colisionador)
        {
            if (colisionador.CompareTag("Player"))
            {
                _jugadorDentroLava = true;
                _transformJugador = colisionador.transform;
                _ultimaPosicionJugador = _transformJugador.position;
                _cronometroQuieto = 0f;
            }
        }

        private void OnTriggerExit2D(Collider2D colisionador)
        {
            if (colisionador.CompareTag("Player"))
            {
                _jugadorDentroLava = false;
                _transformJugador = null;
            }
        }
    }
}