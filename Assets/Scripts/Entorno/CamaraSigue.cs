using UnityEngine;

namespace RunningIsland.Entorno
{
    public class CamaraSigue : MonoBehaviour
    {
        [Header("Objetivo a Seguir")]
        [SerializeField] private Transform objetivo; 

        [Header("Configuración de Movimiento")]
        [Range(0f, 1f)]
        [SerializeField] private float suavizado = 0.125f; 
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); 

        private void LateUpdate()
        {
            if (objetivo == null) return;

           
            Vector3 posicionDeseada = objetivo.position + offset;

         
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);

     
            transform.position = posicionSuavizada;
        }
    }
}