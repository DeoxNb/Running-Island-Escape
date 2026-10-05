using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RunningIsland.UI
{
    public class ValidadorNombreUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputNombre;
        [SerializeField] private Button botonContinuar;
        [SerializeField] private CanvasGroup grupoPersonajes;
        [SerializeField] private CanvasGroup grupoFormulario; 

        private void Start()
        {
           
            grupoPersonajes.interactable = false;
            grupoPersonajes.blocksRaycasts = false;
            grupoPersonajes.alpha = 0.3f;

            botonContinuar.interactable = false;

            inputNombre.onValueChanged.AddListener(VerificarTexto);
            botonContinuar.onClick.AddListener(ConfirmarNombre);
            inputNombre.onSubmit.AddListener(delegate { ConfirmarNombre(); });
        }

        private void VerificarTexto(string textoEscrito)
        {
            botonContinuar.interactable = !string.IsNullOrWhiteSpace(textoEscrito);
        }

        private void ConfirmarNombre()
        {
            if (!string.IsNullOrWhiteSpace(inputNombre.text))
            {
                
                PlayerPrefs.SetString("NombreJugador", inputNombre.text);
                PlayerPrefs.Save();

            
                grupoFormulario.alpha = 0f;
                grupoFormulario.blocksRaycasts = false;
                grupoFormulario.interactable = false;

                grupoPersonajes.interactable = true;
                grupoPersonajes.blocksRaycasts = true;
                grupoPersonajes.alpha = 1f;
            }
        }
    }
}