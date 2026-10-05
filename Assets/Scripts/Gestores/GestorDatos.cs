using System;
using System.IO;
using System.Collections.Generic; 
using UnityEngine;
using RunningIsland.Datos;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore; 
using Firebase.Extensions;

public class GestorDatos : MonoBehaviour
{
    public static GestorDatos instancia;
    private string nombreArchivoCSV = "datos_globales.csv";
    private string rutaCompletaCSV;
    private float contadorTiempo;
    private FirebaseAuth autenticacion;
    private FirebaseFirestore db; 
    private HerramientasExtras herramientas;

    private void Awake()
    {
        if (instancia == null) instancia = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        rutaCompletaCSV = Path.Combine(UnityEngine.Application.dataPath, "../", nombreArchivoCSV);
        contadorTiempo = Time.time;
        herramientas = new HerramientasExtras();

        CrearArchivoLocal();
        InicializarFirebase();
    }

    private void CrearArchivoLocal()
    {
        if (!File.Exists(rutaCompletaCSV))
        {
            File.WriteAllText(rutaCompletaCSV, "Nombre,Personaje,Ruta,Desenlace,Tiempo\n");
        }
    }

    private void InicializarFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(tarea => {
            if (tarea.Result == DependencyStatus.Available)
            {
                autenticacion = FirebaseAuth.DefaultInstance;
                db = FirebaseFirestore.DefaultInstance;
                IniciarSesionSilenciosa();
            }
        });
    }

    private void IniciarSesionSilenciosa()
    {
        autenticacion.SignInAnonymouslyAsync().ContinueWithOnMainThread(tareaAutenticacion => {
            if (tareaAutenticacion.IsFaulted || tareaAutenticacion.IsCanceled)
            {
                UnityEngine.Debug.LogError("ErrorAutenticacion");
                return;
            }
            UnityEngine.Debug.Log("AutenticacionCorrecta");
        });
    }

    public void RegistrarPartida(string desenlaceActual)
    {
        string nombreJugadorActual = PlayerPrefs.GetString("NombreJugador", "Anonimo");
        string personajeActual = PlayerPrefs.GetString(CamposPartida.PERSONAJE_SELECCIONADO, "Agente");
        string rutaActual = PlayerPrefs.GetString(CamposPartida.VIA_ESCAPE, "Ninguna");
        float tiempoTranscurrido = Time.time - contadorTiempo;

        int indicePartida = PlayerPrefs.GetInt("TotalPartidas", 0) + 1;
        PlayerPrefs.SetInt("TotalPartidas", indicePartida);
        PlayerPrefs.SetString($"Partida_{indicePartida}_Nombre", nombreJugadorActual);
        PlayerPrefs.SetString($"Partida_{indicePartida}_Personaje", personajeActual);
        PlayerPrefs.SetString($"Partida_{indicePartida}_Ruta", rutaActual);
        PlayerPrefs.SetString($"Partida_{indicePartida}_Desenlace", desenlaceActual);
        PlayerPrefs.SetFloat($"Partida_{indicePartida}_Tiempo", tiempoTranscurrido);
        PlayerPrefs.Save();

        string nuevaLineaCSV = $"{nombreJugadorActual},{personajeActual},{rutaActual},{desenlaceActual},{tiempoTranscurrido:F2}\n";
        File.AppendAllText(rutaCompletaCSV, nuevaLineaCSV);
        UnityEngine.Debug.Log("Partida registrada localmente con éxito (CSV y PlayerPrefs).");

        
        if (db != null && autenticacion != null && autenticacion.CurrentUser != null)
        {
           
            var datosSubida = new Dictionary<string, object>
            {
                { "nombreJugador", nombreJugadorActual },
                { "personaje", personajeActual },
                { "rutaEscape", rutaActual },
                { "desenlace", desenlaceActual },
                { "tiempoJugado", tiempoTranscurrido },
                { "fecha", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") }
            };

            
            db.Collection("partidas").AddAsync(datosSubida).ContinueWithOnMainThread(tareaSubida =>
            {
                if (tareaSubida.IsCompleted && !tareaSubida.IsFaulted)
                {
                    UnityEngine.Debug.Log("¡Datos subidos a Firestore correctamente!");
                }
                else
                {
                    UnityEngine.Debug.LogError("Error al subir a Firestore: " + tareaSubida.Exception);
                }
            });
        }
    }

    public void EjecutarExportacion()
    {
        herramientas.ExportarHistorialLocal();
    }
}