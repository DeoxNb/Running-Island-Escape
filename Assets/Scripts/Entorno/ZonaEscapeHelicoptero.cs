using UnityEngine;
using RunningIsland.Datos;
using RunningIsland.Entidades;
using RunningIsland.Gestores;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class ZonaEscapeHelicoptero : MonoBehaviour
    {
        [SerializeField] private Sprite helipuertoFase1_SinObjetos;
        [SerializeField] private Sprite helipuertoFase2_ConBateria;
        [SerializeField] private Sprite helipuertoFase3_ConRadio;
        [SerializeField] private Sprite helipuertoFase4_Activo;
        
        [SerializeField] private ItemData itemBateria;
        [SerializeField] private ItemData itemRadio;
        [SerializeField] private ItemData itemTransmisor;

        private bool _tieneBateria = false;
        private bool _tieneRadio = false;
        private bool _tieneTransmisor = false;
        private bool _jugadorEnRango = false;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void Start()
        {
            if (_spriteRenderer != null && helipuertoFase1_SinObjetos != null)
            {
                _spriteRenderer.sprite = helipuertoFase1_SinObjetos;
            }
        }

        private void Update()
        {
            if (_jugadorEnRango && Input.GetKeyDown(KeyCode.E))
            {
                ProcesarEntrega();
            }
        }

        private void ProcesarEntrega()
        {
            if (InventarioJugador.Instance == null) return;

            bool huboProgreso = false;

            if (!_tieneBateria && itemBateria != null && InventarioJugador.Instance.TieneItem(itemBateria.IdItem))
            {
                InventarioJugador.Instance.ConsumirItem(itemBateria.IdItem, 1);
                _tieneBateria = true;
                if (_spriteRenderer != null && helipuertoFase2_ConBateria != null) _spriteRenderer.sprite = helipuertoFase2_ConBateria;
                huboProgreso = true;
            }
            else if (_tieneBateria && !_tieneRadio && itemRadio != null && InventarioJugador.Instance.TieneItem(itemRadio.IdItem))
            {
                InventarioJugador.Instance.ConsumirItem(itemRadio.IdItem, 1);
                _tieneRadio = true;
                if (_spriteRenderer != null && helipuertoFase3_ConRadio != null) _spriteRenderer.sprite = helipuertoFase3_ConRadio;
                huboProgreso = true;
            }
            else if (_tieneBateria && _tieneRadio && !_tieneTransmisor && itemTransmisor != null && InventarioJugador.Instance.TieneItem(itemTransmisor.IdItem))
            {
                InventarioJugador.Instance.ConsumirItem(itemTransmisor.IdItem, 1);
                _tieneTransmisor = true;
                if (_spriteRenderer != null && helipuertoFase4_Activo != null) _spriteRenderer.sprite = helipuertoFase4_Activo;
                huboProgreso = true;
            }

            if (_tieneBateria && _tieneRadio && _tieneTransmisor && huboProgreso)
            {
                if (GestorEscapes.Instancia != null)
                {
                    GestorEscapes.Instancia.CompletarEscape("Helicoptero");
                }
            }
            else if (!huboProgreso)
            {
                if (UI.GestorHUD.instancia != null)
                {
                    UI.GestorHUD.instancia.EfectoRecibirDaño();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D colisionador)
        {
            if (colisionador.CompareTag("Player"))
            {
                _jugadorEnRango = true;
            }
        }

        private void OnTriggerExit2D(Collider2D colisionador)
        {
            if (colisionador.CompareTag("Player"))
            {
                _jugadorEnRango = false;
            }
        }
    }
}