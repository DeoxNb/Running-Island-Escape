using UnityEngine;

namespace RunningIsland.Datos
{
    [CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "Running Island/Personaje Data")]
    public class PersonajeData : ScriptableObject
    {
        [SerializeField] private string idPersonaje;
        [SerializeField] private string nombrePersonaje;
        [SerializeField, TextArea(3, 5)] private string descripcionHabilidad;
        [SerializeField] private float velocidadMovimiento = 5f;
        [SerializeField] private float multiplicadorMitigacionFuego = 1f;
        [SerializeField] private int vidaMaxima = 100;
        [SerializeField] private RuntimeAnimatorController controladorAnimaciones;
        [SerializeField] private Sprite skinVisualMapa;

        public string IdPersonaje => idPersonaje;
        public string NombrePersonaje => nombrePersonaje;
        public string DescripcionHabilidad => descripcionHabilidad;
        public float VelocidadMovimiento => velocidadMovimiento;
        public float MultiplicadorMitigacionFuego => multiplicadorMitigacionFuego;
        public int VidaMaxima => vidaMaxima;
        public RuntimeAnimatorController ControladorAnimaciones => controladorAnimaciones;
        public Sprite SkinVisualMapa => skinVisualMapa;
    }
}