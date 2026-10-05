using UnityEngine;
using RunningIsland.Datos;
using RunningIsland.Entidades;
using RunningIsland.Gestores;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class ZonaEscapePortal : MonoBehaviour
    {
        [SerializeField] private Sprite portalFase1_SinObjetos;
        [SerializeField] private Sprite portalFase2_EstatuaBosque;
        [SerializeField] private Sprite portalFase3_EstatuaNieve;
        [SerializeField] private Sprite portalFase4_Activo;
        
        [SerializeField] private ItemData itemEstatuaBosque;
        [SerializeField] private ItemData itemEstatuaNieve;
        [SerializeField] private ItemData itemEstatuaVolcan;

        private bool _tieneBosque = false;
        private bool _tieneNieve = false;
        private bool _tieneVolcan = false;
        private bool _jugadorEnRango = false;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void Start()
        {
            if (_spriteRenderer != null && portalFase1_SinObjetos != null)
            {
                _spriteRenderer.sprite = portalFase1_SinObjetos;
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

            if (!_tieneBosque && itemEstatuaBosque != null && InventarioJugador.Instance.TieneItem(itemEstatuaBosque.IdItem))
            {
                InventarioJugador.Instance.ConsumirItem(itemEstatuaBosque.IdItem, 1);
                _tieneBosque = true;
                if (_spriteRenderer != null && portalFase2_EstatuaBosque != null) _spriteRenderer.sprite = portalFase2_EstatuaBosque;
                huboProgreso = true;
            }
            else if (_tieneBosque && !_tieneNieve && itemEstatuaNieve != null && InventarioJugador.Instance.TieneItem(itemEstatuaNieve.IdItem))
            {
                InventarioJugador.Instance.ConsumirItem(itemEstatuaNieve.IdItem, 1);
                _tieneNieve = true;
                if (_spriteRenderer != null && portalFase3_EstatuaNieve != null) _spriteRenderer.sprite = portalFase3_EstatuaNieve;
                huboProgreso = true;
            }
            else if (_tieneBosque && _tieneNieve && !_tieneVolcan && itemEstatuaVolcan != null && InventarioJugador.Instance.TieneItem(itemEstatuaVolcan.IdItem))
            {
                InventarioJugador.Instance.ConsumirItem(itemEstatuaVolcan.IdItem, 1);
                _tieneVolcan = true;
                if (_spriteRenderer != null && portalFase4_Activo != null) _spriteRenderer.sprite = portalFase4_Activo;
                huboProgreso = true;
            }

            if (_tieneBosque && _tieneNieve && _tieneVolcan && huboProgreso)
            {
                if (GestorEscapes.Instancia != null)
                {
                    GestorEscapes.Instancia.CompletarEscape("Portal");
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