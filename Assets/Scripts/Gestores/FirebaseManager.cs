using UnityEngine;
using Firebase;
using Firebase.Firestore;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseFirestore db;

    void Start()
    {
       
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            DependencyStatus status = task.Result; 
            if (status == DependencyStatus.Available)
            {
                
                db = FirebaseFirestore.DefaultInstance;
                UnityEngine.Debug.Log("Firebase inicializado correctamente."); 
            }
            else
            {
                UnityEngine.Debug.LogError($"No se pudieron resolver las dependencias de Firebase: {status}");
            }
        });
    }

    
    public async Task GuardarPartidaEnNube(string nombreJugador, string resultado, int tiempo)
    {
        if (db == null) return;

      
        var datosPartida = new System.Collections.Generic.Dictionary<string, object>
        {
            { "jugador", nombreJugador },
            { "resultado", resultado }, 
            { "tiempoSupervivencia", tiempo },
            { "fecha", System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") }
        };

        try
        {
           
            await db.Collection("partidas").AddAsync(datosPartida);
            UnityEngine.Debug.Log("PartidaSubidaFirebase: Datos guardados con éxito.");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError($"Error al subir la partida: {e.Message}");
        }
    }
}