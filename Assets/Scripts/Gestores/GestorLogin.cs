using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace RunningIsland.UI
{
    public class GestorLogin : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputNombre;

        public void GuardarNombreYContinuar()
        {
            if (!string.IsNullOrEmpty(inputNombre.text))
            {
                PlayerPrefs.SetString("NombreJugador", inputNombre.text);
                PlayerPrefs.Save();
                SceneManager.LoadScene("SeleccionPersonaje");
            }
            else
            {
                Debug.Log("Introduce un nombre primero");
            }
        }
    }
}