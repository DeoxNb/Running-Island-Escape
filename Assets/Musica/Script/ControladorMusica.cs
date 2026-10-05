using UnityEngine;

public class ControladorMusica : MonoBehaviour
{
    public static ControladorMusica instancia; // ESTA LÍNEA ES LA CLAVE

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
}