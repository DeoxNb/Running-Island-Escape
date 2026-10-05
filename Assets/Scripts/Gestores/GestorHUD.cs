using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using RunningIsland.Datos;

namespace RunningIsland.UI
{
    public class GestorHUD : MonoBehaviour
    {
        public static GestorHUD instancia { get; private set; }

        [SerializeField] private Image imagenBarraSalud;
        [SerializeField] private Sprite[] estadosSaludSprites;

        [SerializeField] private Transform contenedorCasillas;
        [SerializeField] private GameObject prefabCasillaInventario;

        [SerializeField] private CanvasGroup grupoPanelDaño;
        [SerializeField] private float duracionFlashDaño = 0.4f;

        private List<Image> _imagenesDeItemsEnCasillas = new List<Image>();
        private Coroutine _rutinaFlash;

        private void Awake()
        {
            if (instancia == null) instancia = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            InicializarCuadriculaInventario();
        }

        private void InicializarCuadriculaInventario()
        {
            if (contenedorCasillas != null && prefabCasillaInventario != null)
            {
                foreach (Transform hijo in contenedorCasillas)
                {
                    Destroy(hijo.gameObject);
                }

                _imagenesDeItemsEnCasillas.Clear();

                for (int i = 0; i < 10; i++)
                {
                    GameObject nuevaCasilla = Instantiate(prefabCasillaInventario, contenedorCasillas);
                    Image[] imagenes = nuevaCasilla.GetComponentsInChildren<Image>();
                    if (imagenes.Length > 1)
                    {
                        Image imagenIcono = imagenes[1];
                        imagenIcono.enabled = false;
                        _imagenesDeItemsEnCasillas.Add(imagenIcono);
                    }
                }
            }
        }

        public void ActualizarSaludVisual(int saludActual, int saludMaxima)
        {
            if (imagenBarraSalud != null && estadosSaludSprites != null && estadosSaludSprites.Length > 0)
            {
                float porcentaje = (float)saludActual / saludMaxima;
                int indiceAproximado = Mathf.RoundToInt((estadosSaludSprites.Length - 1) * (1f - porcentaje));
                int indiceSeguro = Mathf.Clamp(indiceAproximado, 0, estadosSaludSprites.Length - 1);
                imagenBarraSalud.sprite = estadosSaludSprites[indiceSeguro];
            }
        }

        public void ActualizarInventarioVisual(List<ItemData> listaItems)
        {
            foreach (Image img in _imagenesDeItemsEnCasillas)
            {
                img.enabled = false;
            }

            int limite = Mathf.Min(listaItems.Count, _imagenesDeItemsEnCasillas.Count);
            for (int i = 0; i < limite; i++)
            {
                if (listaItems[i] != null && listaItems[i].IconoItem != null)
                {
                    _imagenesDeItemsEnCasillas[i].sprite = listaItems[i].IconoItem;
                    _imagenesDeItemsEnCasillas[i].enabled = true;
                }
            }
        }

        public void EfectoRecibirDaño()
        {
            if (grupoPanelDaño != null)
            {
                if (_rutinaFlash != null) StopCoroutine(_rutinaFlash);
                _rutinaFlash = StartCoroutine(RutinaFlashDaño());
            }
        }

        private System.Collections.IEnumerator RutinaFlashDaño()
        {
            float tiempo = 0f;
            while (tiempo < duracionFlashDaño / 2f)
            {
                tiempo += Time.deltaTime;
                grupoPanelDaño.alpha = Mathf.Lerp(0f, 1f, tiempo / (duracionFlashDaño / 2f));
                yield return null;
            }
            tiempo = 0f;
            while (tiempo < duracionFlashDaño / 2f)
            {
                tiempo += Time.deltaTime;
                grupoPanelDaño.alpha = Mathf.Lerp(1f, 0f, tiempo / (duracionFlashDaño / 2f));
                yield return null;
            }
            grupoPanelDaño.alpha = 0f;
        }
    }
}