using UnityEngine;
using RunningIsland.Datos;
using RunningIsland.Entidades;

namespace RunningIsland.Entorno
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ItemFisico : MonoBehaviour
    {
        [Header("Datos")]
        [SerializeField, Tooltip("Arrastra el ItemData correspondiente")] 
        private ItemData datosDelItem;

        [Header("Controles")]
        [SerializeField] private KeyCode teclaRecogida = KeyCode.E;

        private bool _jugadorEnRango = false;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void Update()
        {
            if (_jugadorEnRango && Input.GetKeyDown(teclaRecogida))
            {
                RecogerItem();
            }
        }

        private void RecogerItem()
        {
            if (InventarioJugador.Instance != null && datosDelItem != null)
            {
                InventarioJugador.Instance.RegistrarItem(datosDelItem);
                Debug.Log($"[Inventario] Recogido: {datosDelItem.NombreItem}");
            }
            
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D otroColisionador)
        {
            if (otroColisionador.CompareTag("Player"))
            {
                _jugadorEnRango = true;
            }
        }

        private void OnTriggerExit2D(Collider2D otroColisionador)
        {
            if (otroColisionador.CompareTag("Player"))
            {
                _jugadorEnRango = false;
            }
        }
    }
}