using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions; 
using System.Text.RegularExpressions;

public class AuthManager : MonoBehaviour
{
    [Header("Interfaz")]
    public TMP_InputField inputUsuario;
    public TMP_InputField inputPassword;
    public TextMeshProUGUI textoFeedback;

    private FirebaseAuth auth;
    private string sufijoOculto = "@runningisland.local";
    private bool moduloListo = false;

    void Start()
    {
  
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                moduloListo = true;

                if (auth.CurrentUser != null)
                {
                    textoFeedback.text = "Sesión recuperada. Listo para jugar.";
                }
            }
            else
            {
                textoFeedback.text = "Error interno de base de datos.";
            }
        });
    }

 
    public void ProcesarEntrada()
    {
        if (!moduloListo)
        {
            textoFeedback.text = "Cargando servicios...";
            return;
        }

        string nombreUsuario = inputUsuario.text.Trim();
        string password = inputPassword.text;

        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
        {
            textoFeedback.text = "Rellena ambos campos.";
            return;
        }

        if (!ValidarPasswordSegura(password))
        {
            textoFeedback.text = "La contraseña debe tener mínimo 6 caracteres, una mayúscula, una minúscula y un número.";
            return;
        }

        string correoFalso = nombreUsuario + sufijoOculto;
        textoFeedback.text = "Conectando...";

       
        auth.SignInWithEmailAndPasswordAsync(correoFalso, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Firebase.FirebaseException ex = task.Exception?.GetBaseException() as Firebase.FirebaseException;
                if (ex != null)
                {
                    AuthError errorCode = (AuthError)ex.ErrorCode;

                    if (errorCode == AuthError.UserNotFound)
                    {
                        textoFeedback.text = "Registrando nuevo usuario...";

                      
                        auth.CreateUserWithEmailAndPasswordAsync(correoFalso, password).ContinueWithOnMainThread(regTask =>
                        {
                            if (regTask.IsCanceled || regTask.IsFaulted)
                            {
                                textoFeedback.text = "Error al crear la cuenta.";
                            }
                            else
                            {
                                textoFeedback.text = "¡Cuenta creada! Entrando...";
                              
                            }
                        });
                    }
                    else if (errorCode == AuthError.WrongPassword)
                    {
                        textoFeedback.text = "Contraseña incorrecta.";
                    }
                    else
                    {
                        textoFeedback.text = "Error de conexión.";
                    }
                }
            }
            else
            {
                textoFeedback.text = "¡Login exitoso! Bienvenido.";
             
            }
        });
    }

    private bool ValidarPasswordSegura(string pass)
    {
        string patron = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
        return Regex.IsMatch(pass, patron);
    }
}