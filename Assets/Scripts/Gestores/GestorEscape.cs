using UnityEngine;
using RunningIsland.Datos;

namespace RunningIsland.Gestores
{
    public class GestorEscapes : MonoBehaviour
    {
        public static GestorEscapes Instancia { get; private set; }

        [SerializeField] private string nombreEscenaFinal = "PantallaFinal";

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

        public void CompletarEscape(string viaEscape)
        {
            PlayerPrefs.SetString(CamposPartida.ESTADO_FINAL, "Victoria");
            PlayerPrefs.SetString(CamposPartida.VIA_ESCAPE, viaEscape);

            if (viaEscape == "Barca")
            {
                PlayerPrefs.SetInt(CamposProgreso.FINAL_BARCA_CONSEGUIDO, 1);
            }
            else if (viaEscape == "Helicoptero")
            {
                PlayerPrefs.SetInt(CamposProgreso.FINAL_HELICOPTERO_CONSEGUIDO, 1);
            }
            else if (viaEscape == "Portal")
            {
                PlayerPrefs.SetString(CamposPartida.ESTADO_FINAL, "VictoriaVerdadera");
                PlayerPrefs.SetInt(CamposProgreso.FINAL_PORTAL_CONSEGUIDO, 1);
            }
            else if (viaEscape == "Bunker")
            {
                PlayerPrefs.SetString(CamposPartida.ESTADO_FINAL, "SecretoRevelado");
                PlayerPrefs.SetInt(CamposProgreso.FINAL_BUNKER_CONSEGUIDO, 1);
            }

            PlayerPrefs.Save();

            if (GestorDatos.instancia != null)
            {
                GestorDatos.instancia.RegistrarPartida(PlayerPrefs.GetString(CamposPartida.ESTADO_FINAL));
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(nombreEscenaFinal);
        }
    }
}