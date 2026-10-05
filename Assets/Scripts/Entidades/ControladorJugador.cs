using UnityEngine;
using System.Collections;
using RunningIsland.Datos;

namespace RunningIsland.Entidades
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
    public class ControladorJugador : MonoBehaviour
    {
        [Header("Configuración Dash")]
        [SerializeField] private float fuerzaDash = 15f;
        [SerializeField] private float duracionDash = 0.2f;
        [SerializeField] private float cooldownDash = 1f;

        [SerializeField] private PersonajeData[] catalogoPersonajes;

        private PersonajeData _personajeActivo;
        private Vector2 _direccionMovimiento;
        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private bool _estaHaciendoDash = false;
        private bool _puedeHacerDash = true;

        public PersonajeData PersonajeActivo => _personajeActivo;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _rb.gravityScale = 0f;
            _rb.freezeRotation = true;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void Start()
        {
            CargarPersonajeSeleccionado();
        }

        private void Update()
        {
            if (_estaHaciendoDash) return;

            float entradaX = Input.GetAxisRaw("Horizontal");
            float entradaY = Input.GetAxisRaw("Vertical");
            _direccionMovimiento = new Vector2(entradaX, entradaY).normalized;

            if (_animator != null)
            {
                _animator.SetFloat("VelocidadX", _direccionMovimiento.x);
                _animator.SetFloat("VelocidadY", _direccionMovimiento.y);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && _puedeHacerDash && _direccionMovimiento != Vector2.zero)
            {
                StartCoroutine(EjecutarDash());
            }
        }

        private void FixedUpdate()
        {
            if (_personajeActivo != null && !_estaHaciendoDash)
            {
                Vector2 nuevaPosicion = _rb.position + _direccionMovimiento * _personajeActivo.VelocidadMovimiento * Time.fixedDeltaTime;
                _rb.MovePosition(nuevaPosicion);
            }
        }

        private IEnumerator EjecutarDash()
        {
            _estaHaciendoDash = true;
            _puedeHacerDash = false;

            _rb.linearVelocity = _direccionMovimiento * fuerzaDash;

            yield return new WaitForSeconds(duracionDash);

            _rb.linearVelocity = Vector2.zero;
            _estaHaciendoDash = false;

            yield return new WaitForSeconds(cooldownDash);
            _puedeHacerDash = true;
        }

        private void CargarPersonajeSeleccionado()
        {
            string idSalvado = PlayerPrefs.GetString(CamposPartida.PERSONAJE_SELECCIONADO, "Agente");
            _personajeActivo = System.Array.Find(catalogoPersonajes, p => p.IdPersonaje == idSalvado);

            if (_personajeActivo == null && catalogoPersonajes.Length > 0)
            {
                _personajeActivo = catalogoPersonajes[0];
            }

            if (_personajeActivo != null)
            {
                if (_animator != null) _animator.runtimeAnimatorController = _personajeActivo.ControladorAnimaciones;
                if (_spriteRenderer != null) _spriteRenderer.sprite = _personajeActivo.SkinVisualMapa;

                if (TryGetComponent<SaludJugador>(out var salud))
                {
                    salud.InicializarVida(_personajeActivo.VidaMaxima);
                }
            }
        }
    }
}

