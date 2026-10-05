using UnityEngine;

namespace RunningIsland.Datos
{
    [CreateAssetMenu(fileName = "NuevaNota", menuName = "Running Island/Nota Superviviente")]
    public class NotaSuperviviente : ScriptableObject
    {
        [Header("Identificación")]
        [SerializeField] private string idNota;
        [SerializeField] private string autorEpoca;

        [Header("Contenido de la Pista")]
        [SerializeField, TextArea(4, 8)] private string contenidoTexto;

        [Header("Estado")]
        [SerializeField] private bool descubierta;

        public string IdNota => idNota;
        public string AutorEpoca => autorEpoca;
        public string ContenidoTexto => contenidoTexto;
        public bool Descubierta => descubierta;

        public void InicializarNota()
        {
            descubierta = false;
        }

        public void MarcarComoDescubierta()
        {
            descubierta = true;
        }
    }
}