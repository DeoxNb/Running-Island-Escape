using UnityEngine;
using System.Linq;
using RunningIsland.Datos;
using RunningIsland.Entidades;
using RunningIsland.Gestores;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class ZonaEscapeBarca : MonoBehaviour
    {
        [SerializeField] private Sprite barcaFase1_SinObjetos;
        [SerializeField] private Sprite barcaFase2_ConMadera;
        [SerializeField] private Sprite barcaFase3_ConCuerda;
        [SerializeField] private Sprite barcaFase4_Activa;

        [SerializeField] private ItemData itemMadera;
        [SerializeField] private ItemData itemCuerda;
        [SerializeField] private ItemData itemTela;

        private bool _tieneMadera = false;
        private bool _tieneCuerda = false;
        private bool _tieneTela = false;
        private bool _jugadorEnRango = false;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void Start()
        {
            if (_spriteRenderer != null && barcaFase1_SinObjetos != null)
            {
                _spriteRenderer.sprite = barcaFase1_SinObjetos;
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

            bool tieneMaderaInventario = InventarioJugador.Instance.ListaItems.Any(item => item != null && itemMadera != null && item.IdItem == itemMadera.IdItem);
            bool tieneCuerdaInventario = InventarioJugador.Instance.ListaItems.Any(item => item != null && itemCuerda != null && item.IdItem == itemCuerda.IdItem);
            bool tieneTelaInventario = InventarioJugador.Instance.ListaItems.Any(item => item != null && itemTela != null && item.IdItem == itemTela.IdItem);

            if (!_tieneMadera && itemMadera != null && tieneMaderaInventario)
            {
                InventarioJugador.Instance.ConsumirItem(itemMadera.IdItem, 1);
                _tieneMadera = true;
                if (_spriteRenderer != null && barcaFase2_ConMadera != null) _spriteRenderer.sprite = barcaFase2_ConMadera;
                huboProgreso = true;
            }
            else if (_tieneMadera && !_tieneCuerda && itemCuerda != null && tieneCuerdaInventario)
            {
                InventarioJugador.Instance.ConsumirItem(itemCuerda.IdItem, 1);
                _tieneCuerda = true;
                if (_spriteRenderer != null && barcaFase3_ConCuerda != null) _spriteRenderer.sprite = barcaFase3_ConCuerda;
                huboProgreso = true;
            }
            else if (_tieneMadera && _tieneCuerda && !_tieneTela && itemTela != null && tieneTelaInventario)
            {
                InventarioJugador.Instance.ConsumirItem(itemTela.IdItem, 1);
                _tieneTela = true;
                if (_spriteRenderer != null && barcaFase4_Activa != null) _spriteRenderer.sprite = barcaFase4_Activa;
                huboProgreso = true;
            }

            if (_tieneMadera && _tieneCuerda && _tieneTela && huboProgreso)
            {
                if (GestorEscapes.Instancia != null)
                {
                    GestorEscapes.Instancia.CompletarEscape("Barca");
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