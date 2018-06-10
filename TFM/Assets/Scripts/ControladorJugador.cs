using System;
using CustomEvents;
using DefaultNamespace;
using Scriptables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ControladorJugador : MonoBehaviour
{
  public Animator animadorArma;

  [SerializeField] private GameObject[] prefabsImpactos;
  public Arma[] Armas;
  public int[] BalasDeArmasPoseidas;
  public byte ArmaSeleccionada;
  public bool EstaCorriendo;
  public bool Bloqueado;
  public int CargadorActual;
  public float VidaMaxima;
  public float StaminaMaxima;

  private FirstPersonController _firstPersonController;
  private CharacterController _controller;
  private AudioSource _audioSource;
  private float _cuentaCadencia;
  private bool _disparar;
  private int _balasDisparadasCargadorActual;
  private bool _recargando;
  private bool _invocadoNecesitaRecarga;
  private float _vidaActual;
  private float _staminaActual;

  [SerializeField] private Image _mirilla;

  [Tooltip("Evento que indica que es necesario meter balas al cargador, notifica las balas que se poseen.")]
  public ValueIntChangeEvent NecesitaRecargaEvent;

  public UnityEvent RecargadoEvent;
  public ValueFloatChangeEvent VidaChangeEvent;
  public ValueFloatChangeEvent StaminaChangeEvent;

  // Use this for initialization
  void Start()
  {
    if (NecesitaRecargaEvent == null)
    {
      NecesitaRecargaEvent = new ValueIntChangeEvent();
    }

    if (RecargadoEvent == null)
    {
      RecargadoEvent = new UnityEvent();
    }

    _controller = GetComponent<CharacterController>();
    _audioSource = GetComponent<AudioSource>();
    ArmaSeleccionada = 2;

    _vidaActual = VidaMaxima;
    _staminaActual = StaminaMaxima;
    VidaChangeEvent.Invoke(_vidaActual);
    StaminaChangeEvent.Invoke(_staminaActual);

    _firstPersonController = GetComponent<FirstPersonController>();
  }

  /// <summary>
  /// Escucha el evento de final de recarga de los ArmaController
  /// </summary>
  public void OnEndReload()
  {
    _recargando = false;
  }

  public void LanzarCasquillo(Transform posicionDeLanzado)
  {
    var casquillo = Instantiate(Armas[ArmaSeleccionada].PrefabCasquillo, posicionDeLanzado.position, posicionDeLanzado.rotation);
    casquillo.GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * 5, ForceMode.Impulse);
    casquillo.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right * 100, ForceMode.VelocityChange);
  }

  private void Update()
  {
    if (Bloqueado)
      return;
    
    if (_cuentaCadencia >= 0)
    {
      _cuentaCadencia -= Time.deltaTime;
    }

    if (Input.GetKeyDown(KeyCode.R))
    {
      Recarga();
    }
  }

  private void FixedUpdate()
  {
    ControlAnimacionMovimientos();
    ControlDisparo();
  }

  private void Recarga()
  {
    if (Bloqueado)
      return;

    var animacionActual = animadorArma.GetCurrentAnimatorClipInfo(0)[0].clip;
    if (animacionActual.name != "shot")
    {
      _recargando = true;
      if (CargadorActual > 0)
      {
        BalasDeArmasPoseidas[ArmaSeleccionada] += CargadorActual;
      }

      if (BalasDeArmasPoseidas[ArmaSeleccionada] >= Armas[ArmaSeleccionada].BalasPorCargador)
      {
        var balasMaximas = Armas[ArmaSeleccionada].BalasPorCargador;
        CargadorActual = balasMaximas;
        BalasDeArmasPoseidas[ArmaSeleccionada] -= balasMaximas;
      }
      else
      {
        CargadorActual = BalasDeArmasPoseidas[ArmaSeleccionada];
        BalasDeArmasPoseidas[ArmaSeleccionada] = 0;
      }

      if (CargadorActual > 0)
      {
        _audioSource.PlayOneShot(Armas[ArmaSeleccionada].SonidoRecarga);
        animadorArma.SetTrigger("recarga");
        _invocadoNecesitaRecarga = false;
      }

      RecargadoEvent.Invoke();
    }
  }

  private void ControlAnimacionMovimientos()
  {
    EstaCorriendo = Mathf.Abs(_controller.velocity.x) > 6.2 || Mathf.Abs(_controller.velocity.z) > 6.2;

    if (_staminaActual <= 0)
    {
      _firstPersonController.m_RunSpeed = 5;
    }
    else if (_staminaActual > 20)
    {
      _firstPersonController.m_RunSpeed = 10;
    }

    if (EstaCorriendo && _staminaActual >= 0)
    {
      _staminaActual = _staminaActual <= 0 ? 0 : _staminaActual - Time.deltaTime * 10;

      StaminaChangeEvent.Invoke(_staminaActual);
    }
    else if (_staminaActual <= StaminaMaxima)
    {
      _staminaActual = _staminaActual > StaminaMaxima ? StaminaMaxima : _staminaActual + Time.deltaTime * 5;
      StaminaChangeEvent.Invoke(_staminaActual);
    }

    animadorArma.SetBool("corriendo", EstaCorriendo);
  }

  private void ControlDisparo()
  {
    if (Bloqueado)
      return;
    
    var arma = Armas[ArmaSeleccionada];
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    Debug.DrawRay(ray.origin, ray.direction * arma.Alcance, Color.green);

    bool impactandoRayo = Physics.Raycast(ray, out hit, arma.Alcance);

    _mirilla.color = impactandoRayo && hit.collider.CompareTag("disparable") ? Color.red : Color.cyan;

    _disparar = arma.EsAutomatica ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

    var animacionActual = animadorArma.GetCurrentAnimatorClipInfo(0)[0].clip.name;

    if (CargadorActual > 0 && !EstaCorriendo && _cuentaCadencia <= 0 && _disparar && !_recargando && animacionActual != "run")
    {
      _cuentaCadencia = arma.SegundosCadencia;
      animadorArma.SetTrigger("disparando");
      _audioSource.PlayOneShot(arma.SonidoDisparo);
      CargadorActual--;

      if (impactandoRayo)
      {
        GameObject impacto = null;
        switch (hit.collider.tag)
        {
          case "metal":
            impacto = Instantiate(prefabsImpactos[0], hit.point, hit.transform.rotation);
            break;
          case "madera":
            impacto = Instantiate(prefabsImpactos[1], hit.point, hit.transform.rotation);
            break;
          case "carne":
            impacto = Instantiate(prefabsImpactos[2], hit.point, hit.transform.rotation);
            break;
          case "roca":
            impacto = Instantiate(prefabsImpactos[3], hit.point, hit.transform.rotation);
            break;
        }

        print("IMPACTO CON: " + hit.transform.gameObject.name);

        var quitable = hit.transform.gameObject.GetComponent<IQuitableVida>();
        if (quitable != null)
        {
          quitable.QuitaVida(10);
        }

        if (impacto != null)
        {
          impacto.transform.LookAt(gameObject.transform);
        }
      }
    }
    else
    {
      if (!_invocadoNecesitaRecarga && CargadorActual == 0)
      {
        NecesitaRecargaEvent.Invoke(BalasDeArmasPoseidas[ArmaSeleccionada]);
        _invocadoNecesitaRecarga = true;
      }
    }
  }
}