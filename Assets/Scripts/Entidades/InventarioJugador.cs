using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using RunningIsland.Datos;
using RunningIsland.UI;

namespace RunningIsland.Entidades
{
    public class InventarioJugador : MonoBehaviour
    {
        public static InventarioJugador Instance { get; private set; }

        private List<ItemData> _listaItems = new List<ItemData>();
        
        public List<ItemData> ListaItems => _listaItems;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegistrarItem(ItemData datos)
        {
            if (datos == null) return;
            
            _listaItems.Add(datos);
            
            if (GestorHUD.instancia != null)
            {
                GestorHUD.instancia.ActualizarInventarioVisual(_listaItems);
            }
        }

        public bool TieneItem(string idItem)
        {
            if (string.IsNullOrEmpty(idItem)) return false;
            
            return _listaItems.Any(i => i != null && i.IdItem.ToLower() == idItem.ToLower());
        }

        public bool TieneTodosLosItems(List<ItemData> requeridos)
        {
            if (requeridos == null || requeridos.Count == 0) return true;

            List<ItemData> copiaMochila = new List<ItemData>(_listaItems);
            
            foreach (ItemData req in requeridos)
            {
                if (req == null) continue;
                
                ItemData encontrado = copiaMochila.FirstOrDefault(i => i != null && i.IdItem == req.IdItem);
                
                if (encontrado == null) return false;
                
                copiaMochila.Remove(encontrado);
            }
            
            return true;
        }

        public void ConsumirItem(string idItem, int cantidad)
        {
            if (string.IsNullOrEmpty(idItem) || cantidad <= 0) return;

            int elementosBorrados = 0;
            
            while (elementosBorrados < cantidad)
            {
                ItemData itemEncontrado = _listaItems.FirstOrDefault(item => item != null && item.IdItem.ToLower() == idItem.ToLower());
                
                if (itemEncontrado != null)
                {
                    _listaItems.Remove(itemEncontrado);
                }
                
                elementosBorrados++;
            }

            if (GestorHUD.instancia != null)
            {
                GestorHUD.instancia.ActualizarInventarioVisual(_listaItems);
            }
        }
    }
}