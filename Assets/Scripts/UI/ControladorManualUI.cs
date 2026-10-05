using UnityEngine;
using TMPro;
using System.Collections;
using RunningIsland.Datos;

namespace RunningIsland.UI
{
    public class ControladorManualUI : MonoBehaviour
    {
        [SerializeField] private GameObject panelManual;
        [SerializeField] private TextMeshProUGUI textoContenidoNota;
        [SerializeField] private TextMeshProUGUI textoAutorEpoca;
        [SerializeField] private float velocidadTexto = 0.03f;

        private Coroutine _corrutinaEfecto;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                AlternarEstadoManual();
            }
        }

        public void AlternarEstadoManual()
        {
            if (panelManual == null) return;

            bool estaActivo = !panelManual.activeSelf;
            panelManual.SetActive(estaActivo);

            if (estaActivo)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                if (_corrutinaEfecto != null) StopCoroutine(_corrutinaEfecto);
            }
        }

        public void CargarYMostrarNota(NotaSuperviviente datosNota)
        {
            if (datosNota == null || textoContenidoNota == null || textoAutorEpoca == null) return;

            if (!panelManual.activeSelf)
            {
                panelManual.SetActive(true);
                Time.timeScale = 0f;
            }

            textoAutorEpoca.text = datosNota.AutorEpoca;

            if (_corrutinaEfecto != null) StopCoroutine(_corrutinaEfecto);
            _corrutinaEfecto = StartCoroutine(RutinaEfectoTerminal(datosNota.ContenidoTexto));
        }

        private IEnumerator RutinaEfectoTerminal(string textoCompleto)
        {
            textoContenidoNota.text = "";
            
            foreach (char letra in textoCompleto)
            {
                textoContenidoNota.text += letra;
                yield return new WaitForSecondsRealtime(velocidadTexto);
            }
        }
    }
}