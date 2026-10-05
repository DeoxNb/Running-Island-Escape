using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RunningIsland.Datos;

namespace RunningIsland.UI
{
    [DisallowMultipleComponent]
    public class BotonSeleccionPersonaje : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Base de Datos")]
        [SerializeField, Tooltip("Arrastra aquí el archivo PersonajeData de tu carpeta DatosCreados")]
        private PersonajeData datosPersonaje;

        [Header("Componentes de Interfaz")]
        [SerializeField] private Image imagenContornoHighlight;
        [SerializeField] private RectTransform rectTransformAnimacion;

        [Header("Ajustes de Animación")]
        [SerializeField] private float escalaZoomHover = 1.08f;
        [SerializeField] private float velocidadEscalado = 10f;

        private Vector3 _escalaOriginal;
        private Vector3 _escalaObjetivo;
        private bool _esElSeleccionadoFijo = false;

        public string NombrePersonaje => datosPersonaje != null ? datosPersonaje.NombrePersonaje : "Desconocido";
        public string DescripcionHabilidad => datosPersonaje != null ? datosPersonaje.DescripcionHabilidad : "Sin descripción";

        private void Awake()
        {
            if (rectTransformAnimacion == null)
            {
                rectTransformAnimacion = GetComponent<RectTransform>();
            }

            _escalaOriginal = (rectTransformAnimacion != null) ? rectTransformAnimacion.localScale : transform.localScale;
            _escalaObjetivo = _escalaOriginal;
        }

        private void Start()
        {
            if (imagenContornoHighlight != null && !_esElSeleccionadoFijo)
            {
                imagenContornoHighlight.enabled = false;
            }
        }

        private void Update()
        {
            if (rectTransformAnimacion != null)
            {
                rectTransformAnimacion.localScale = Vector3.Lerp(rectTransformAnimacion.localScale, _escalaObjetivo, Time.deltaTime * velocidadEscalado);
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, _escalaObjetivo, Time.deltaTime * velocidadEscalado);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _escalaObjetivo = _escalaOriginal * escalaZoomHover;

            if (imagenContornoHighlight != null)
            {
                imagenContornoHighlight.enabled = true;
            }

            if (GestorSeleccionUI.Instancia != null && datosPersonaje != null)
            {
                GestorSeleccionUI.Instancia.MostrarDatosEnPantalla(datosPersonaje.NombrePersonaje, datosPersonaje.DescripcionHabilidad);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _escalaObjetivo = _escalaOriginal;

            if (imagenContornoHighlight != null && !_esElSeleccionadoFijo)
            {
                imagenContornoHighlight.enabled = false;
            }

            if (GestorSeleccionUI.Instancia != null)
            {
                GestorSeleccionUI.Instancia.RestablecerDatosAlFondo();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (datosPersonaje == null || string.IsNullOrEmpty(datosPersonaje.IdPersonaje)) return;

            PlayerPrefs.SetString(CamposPartida.PERSONAJE_SELECCIONADO, datosPersonaje.IdPersonaje);
            PlayerPrefs.Save();

            if (GestorSeleccionUI.Instancia != null)
            {
                GestorSeleccionUI.Instancia.FijarPersonajeSeleccionado(this);
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene("Juego");
        }

        public void EncenderContornoSeleccion()
        {
            _esElSeleccionadoFijo = true;
            if (imagenContornoHighlight != null)
            {
                imagenContornoHighlight.enabled = true;
                imagenContornoHighlight.color = Color.green;
            }
        }

        public void ApagarContornoSeleccion()
        {
            _esElSeleccionadoFijo = false;
            if (imagenContornoHighlight != null)
            {
                imagenContornoHighlight.enabled = false;
                imagenContornoHighlight.color = Color.white;
            }
        }
    }
}