using UnityEngine;

namespace RunningIsland.Datos
{
    [CreateAssetMenu(fileName = "NuevoItem", menuName = "Running Island/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Identificación")]
        [SerializeField] private string idItem;
        [SerializeField] private string nombreItem;

        [Header("Visuales")]
        [SerializeField, Tooltip("El sprite que se mostrará en el inventario del HUD.")]
        private Sprite iconoItem;

        public string IdItem => idItem;
        public string NombreItem => nombreItem;
        public Sprite IconoItem => iconoItem;
    }
}