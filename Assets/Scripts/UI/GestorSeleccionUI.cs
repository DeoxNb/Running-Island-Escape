using UnityEngine;
using TMPro;

namespace RunningIsland.UI
{
    public class GestorSeleccionUI : MonoBehaviour
    {
        public static GestorSeleccionUI Instancia { get; private set; }

        [Header("Textos del Menú Global")]
        [SerializeField] private TextMeshProUGUI textoNombreGlobal;
        [SerializeField] private TextMeshProUGUI textoHabilidadGlobal;

        private BotonSeleccionPersonaje _botonSeleccionadoActivo;

        private void Awake()
        {
            if (Instancia == null)
            {
                Instancia = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void MostrarDatosEnPantalla(string nombre, string descripcion)
        {
            if (textoNombreGlobal != null) textoNombreGlobal.text = nombre;
            if (textoHabilidadGlobal != null) textoHabilidadGlobal.text = descripcion;
        }

        public void RestablecerDatosAlFondo()
        {
            if (_botonSeleccionadoActivo != null)
            {
                MostrarDatosEnPantalla(_botonSeleccionadoActivo.NombrePersonaje, _botonSeleccionadoActivo.DescripcionHabilidad);
            }
            else
            {
                if (textoNombreGlobal != null) textoNombreGlobal.text = "SELECCIONA";
                if (textoHabilidadGlobal != null) textoHabilidadGlobal.text = "Pasa el cursor sobre un superviviente para ver sus rasgos.";
            }
        }

        public void FijarPersonajeSeleccionado(BotonSeleccionPersonaje botonPresionado)
        {
            if (_botonSeleccionadoActivo != null && _botonSeleccionadoActivo != botonPresionado)
            {
                _botonSeleccionadoActivo.ApagarContornoSeleccion();
            }

            _botonSeleccionadoActivo = botonPresionado;
            
            if (_botonSeleccionadoActivo != null)
            {
                _botonSeleccionadoActivo.EncenderContornoSeleccion();
                MostrarDatosEnPantalla(_botonSeleccionadoActivo.NombrePersonaje, _botonSeleccionadoActivo.DescripcionHabilidad);
            }
        }
    }
}