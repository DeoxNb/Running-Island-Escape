
using RunningIsland.Datos;
using UnityEngine;

public class RepositorioPartidas
{
   

    public void GuardarPartida(DatosPartida datosSubida)
    {
       
        SeguridadDatos seguridad = new SeguridadDatos();
        datosSubida.nombreJugador = seguridad.EncriptarTexto(datosSubida.nombreJugador);


        Debug.Log("Lógica de encriptación ejecutada. (Subida a RealtimeDB desactivada para migración a Firestore).");
    }
}