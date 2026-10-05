using UnityEngine;
using TMPro; 

public class GestorRequisitos : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoUI; 

   
    public void MostrarRequisitos(string mensaje)
    {
        if (textoUI != null)
        {
            textoUI.text = mensaje;
            textoUI.gameObject.SetActive(true); 
        }
    }


    public void OcultarRequisitos()
    {
        if (textoUI != null)
        {
            textoUI.gameObject.SetActive(false);
        }
    }
}