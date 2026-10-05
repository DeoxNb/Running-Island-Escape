using UnityEngine;
using System.IO;

public class GestorEstadisticas : MonoBehaviour
{
 
    private string rutaGuardado = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "RunningIslandLogs");

    public void GuardarPartida(string desenlace, string nombre, string personaje, string ruta, float tiempo)
    {
        if (!Directory.Exists(rutaGuardado)) Directory.CreateDirectory(rutaGuardado);

        var datos = new
        {
            desenlace = desenlace,
            fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            nombreJugador = nombre,
            personaje = personaje,
            rutaEscape = ruta,
            tiempoJugado = tiempo
        };

        string json = JsonUtility.ToJson(datos, true);
        string nombreArchivo = $"Partida_{System.DateTime.Now.ToString("yyyyMMdd_HHmmss")}.json";
        File.WriteAllText(Path.Combine(rutaGuardado, nombreArchivo), json);
    }
}