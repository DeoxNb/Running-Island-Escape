using RunningIsland.Datos;
using RunningIsland.Entidades;
using System.Collections.Generic;
using UnityEngine;

namespace RunningIsland.Entorno
{
    public class RequisitoInventario : MonoBehaviour, IRequisitoEscape
    {
        [Header("Configuración de Requisitos")]
        [SerializeField] private List<ItemData> itemsRequeridos = new List<ItemData>();

        [Header("Retroalimentación")]
        [SerializeField, TextArea(2, 4)] private string mensajeFallo = "No tienes los objetos necesarios.";

        public bool CumpleRequisito()
        {
            if (InventarioJugador.Instance == null)
            {
                return false;
            }

            return InventarioJugador.Instance.TieneTodosLosItems(itemsRequeridos);
        }

        public string ObtenerMensajeFallo()
        {
            return mensajeFallo;
        }
    }
}