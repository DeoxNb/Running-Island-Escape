using UnityEngine;
using System.Collections;
using RunningIsland.Datos;
using RunningIsland.Entidades;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class ObjetoRecogible : MonoBehaviour
    {
        public enum TipoObjeto { Madera, Tela, Metal, Cuerda, Bateria, Radio, Transmisor, EstatuaBosque, EstatuaNieve, EstatuaVolcan, NotaMapa }
        
        [SerializeField] private TipoObjeto tipo;
        [SerializeField] private ItemData datosItem;
        [SerializeField] private float fuerzaRebote = 0.15f;
        [SerializeField] private float duracionRebote = 0.2f;

        private Vector3 _escalaOriginal;
        private Coroutine _coroutineRebote;
        private bool _destruidoPorFuego = false;

        public TipoObjeto Tipo => tipo;
        public ItemData DatosItem => datosItem;

        private void Awake()
        {
            _escalaOriginal = transform.localScale;
        }

        public void Recoger()
        {
            if (_destruidoPorFuego) return;

            if (InventarioJugador.Instance != null && datosItem != null)
            {
                InventarioJugador.Instance.RegistrarItem(datosItem);
            }
            Destroy(gameObject);
        }

        public void QuemarPorCompleto()
        {
            _destruidoPorFuego = true;
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (_coroutineRebote != null) StopCoroutine(_coroutineRebote);
                _coroutineRebote = StartCoroutine(DoMinirebote());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                transform.localScale = _escalaOriginal;
            }
        }

        private IEnumerator DoMinirebote()
        {
            float timer = 0f;
            Vector3 escalaObjetivo = _escalaOriginal + new Vector3(fuerzaRebote, fuerzaRebote, 0f);

            while (timer < duracionRebote / 2f)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(_escalaOriginal, escalaObjetivo, timer / (duracionRebote / 2f));
                yield return null;
            }

            timer = 0f;
            while (timer < duracionRebote / 2f)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(escalaObjetivo, _escalaOriginal, timer / (duracionRebote / 2f));
                yield return null;
            }
            transform.localScale = _escalaOriginal;
        }
    }
}