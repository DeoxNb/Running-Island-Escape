using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class SeguridadDatos
{
    private readonly byte[] claveSecreta = Encoding.UTF8.GetBytes("ClaveSeguraJuego1234567890123456");
    private readonly byte[] vectorInicializacion = Encoding.UTF8.GetBytes("VectorJuego12345");

    public string EncriptarTexto(string textoPlano)
    {
        using (Aes encriptadorAes = Aes.Create())
        {
            encriptadorAes.Key = claveSecreta;
            encriptadorAes.IV = vectorInicializacion;
            ICryptoTransform encriptador = encriptadorAes.CreateEncryptor(encriptadorAes.Key, encriptadorAes.IV);

            using (MemoryStream flujoMemoria = new MemoryStream())
            {
                using (CryptoStream flujoCriptografico = new CryptoStream(flujoMemoria, encriptador, CryptoStreamMode.Write))
                {
                    using (StreamWriter escritorFlujo = new StreamWriter(flujoCriptografico))
                    {
                        escritorFlujo.Write(textoPlano);
                    }
                }
                return Convert.ToBase64String(flujoMemoria.ToArray());
            }
        }
    }
}