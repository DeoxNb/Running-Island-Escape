using UnityEngine;
using UnityEngine.SceneManagement;
using RunningIsland.Datos;

namespace RunningIsland.UI
{
    [DisallowMultipleComponent]
    public class MenuSeleccionUI : MonoBehaviour
    {
        public void SeleccionarPersonajePorId(string idPersonaje)
        {
            PlayerPrefs.SetString(CamposPartida.PERSONAJE_SELECCIONADO, idPersonaje);
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(CamposPartida.ESCENA_JUEGO);
        }

        public void BotonVolverAlMenu()
        {
            SceneManager.LoadScene(CamposPartida.ESCENA_INICIO);
        }
    }
}