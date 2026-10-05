using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RunningIsland.UI
{
    public class SelectorPersonaje : MonoBehaviour
    {
        [Header("Referencias UI")]
        [SerializeField] private TMP_InputField inputNombre;
        [SerializeField] private Button botonContinuar;
        [SerializeField] private TextMeshProUGUI tituloSeleccion;

        [Header("Grupos (Asegúrate que tengan CanvasGroup)")]
        [SerializeField] private CanvasGroup grupoFormulario;
        [SerializeField] private CanvasGroup grupoPersonajes;
        [SerializeField] private CanvasGroup grupoTextosExtra;

        private void Start()
        {
            tituloSeleccion.text = "¿CÓMO TE LLAMAS?";

          
            SetGroupState(grupoPersonajes, false); 
            SetGroupState(grupoFormulario, true);  
            SetGroupState(grupoTextosExtra, false);

            botonContinuar.interactable = false;

            inputNombre.onValueChanged.AddListener(texto => botonContinuar.interactable = !string.IsNullOrWhiteSpace(texto));
            botonContinuar.onClick.AddListener(ConfirmarNombre);
            inputNombre.onSubmit.AddListener(delegate { ConfirmarNombre(); });
        }

        private void ConfirmarNombre()
        {
            if (!string.IsNullOrWhiteSpace(inputNombre.text))
            {
                
                PlayerPrefs.SetString("NombreJugador", inputNombre.text);
                PlayerPrefs.Save();

               
                SetGroupState(grupoFormulario, false);

             
                SetGroupState(grupoPersonajes, true);
                SetGroupState(grupoTextosExtra, true); 

                tituloSeleccion.text = "SELECCIONA PERSONAJE";
            }
        }

        private void SetGroupState(CanvasGroup cg, bool activo)
        {
            cg.alpha = activo ? 1f : 0f;
            cg.interactable = activo;
            cg.blocksRaycasts = activo;
        }
    }
}