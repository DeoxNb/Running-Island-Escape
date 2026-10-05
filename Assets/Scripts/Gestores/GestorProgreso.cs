using UnityEngine;
using RunningIsland.Datos;

namespace RunningIsland.Gestores
{
    public class GestorProgreso : MonoBehaviour
    {
        public void GuardarLogProgresoLocal(string viaCompletada)
        {
            switch (viaCompletada)
            {
                case "Barca":
                    PlayerPrefs.SetInt(CamposProgreso.FINAL_BARCA_CONSEGUIDO, 1);
                    break;
                case "Helicoptero":
                    PlayerPrefs.SetInt(CamposProgreso.FINAL_HELICOPTERO_CONSEGUIDO, 1);
                    break;
                case "Portal":
                    PlayerPrefs.SetInt(CamposProgreso.FINAL_PORTAL_CONSEGUIDO, 1);
                    break;
                case "Bunker":
                    PlayerPrefs.SetInt(CamposProgreso.FINAL_BUNKER_CONSEGUIDO, 1);
                    break;
            }
            PlayerPrefs.Save();
        }

        public bool ValidarRutaMisticaDesbloqueada()
        {
            return PlayerPrefs.GetInt(CamposProgreso.FINAL_BARCA_CONSEGUIDO, 0) == 1 && 
                   PlayerPrefs.GetInt(CamposProgreso.FINAL_HELICOPTERO_CONSEGUIDO, 0) == 1;
        }

        public bool ValidarRutaSecretaDesbloqueada()
        {
            return PlayerPrefs.GetInt(CamposProgreso.FINAL_BARCA_CONSEGUIDO, 0) == 1 && 
                   PlayerPrefs.GetInt(CamposProgreso.FINAL_HELICOPTERO_CONSEGUIDO, 0) == 1 && 
                   PlayerPrefs.GetInt(CamposProgreso.FINAL_PORTAL_CONSEGUIDO, 0) == 1;
        }

        public void ResetearProgresoTotal()
        {
            PlayerPrefs.DeleteKey(CamposProgreso.FINAL_BARCA_CONSEGUIDO);
            PlayerPrefs.DeleteKey(CamposProgreso.FINAL_HELICOPTERO_CONSEGUIDO);
            PlayerPrefs.DeleteKey(CamposProgreso.FINAL_PORTAL_CONSEGUIDO);
            PlayerPrefs.DeleteKey(CamposProgreso.FINAL_BUNKER_CONSEGUIDO);
            PlayerPrefs.Save();
        }
    }
}