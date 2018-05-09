using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ControladorJugador : MonoBehaviour
{
  public Animator animadorArma;

  [SerializeField] private GameObject[] prefabsImpactos;
  public Arma[] Armas;
  public int[] BalasDeArmasPoseidas;
  public byte ArmaSeleccionada;
  public bool EstaCorriendo;

  private CharacterController _controller;
  private AudioSource _audioSource;
  private float _cuentaCadencia;
  private bool _disparar;
  private int _balasDisparadasCargadorActual;
  private bool _recargando;
  public int CargadorActual;

  [SerializeField] private Image _mirilla;

  [Tooltip("Evento que indica que es necesario meter balas al cargador, notifica las balas que se poseen.")]
  public EventNecesitaRecarga NecesitaRecargaEvent;

  public UnityEvent RecargadoEvent;

  [System.Serializable]
  public class EventNecesitaRecarga : UnityEvent<int>
  {
  }

  // Use this for initialization
  void Start()
  {
    print("intancehash" + this.GetHashCode());
    
    if (NecesitaRecargaEvent == null)
    {
      NecesitaRecargaEvent = new EventNecesitaRecarga();
    }

    if (RecargadoEvent == null)
    {
      RecargadoEvent = new UnityEvent();
    }

    _controller = GetComponent<CharacterController>();
    _audioSource = GetComponent<AudioSource>();
    ArmaSeleccionada = 2;
  }

  private void Update()
  {
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
      animadorArma.SetTrigger("recarga");
    }

    RecargadoEvent.Invoke();
  }

  private void ControlAnimacionMovimientos()
  {
    EstaCorriendo = Mathf.Abs(_controller.velocity.x) > 8 || Mathf.Abs(_controller.velocity.z) > 8;
    animadorArma.SetBool("corriendo", EstaCorriendo);
  }

  private void ControlDisparo()
  {
    var arma = Armas[ArmaSeleccionada];
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    Debug.DrawRay(ray.origin, ray.direction * arma.Alcance, Color.green);

    bool impactandoRayo = Physics.Raycast(ray, out hit, arma.Alcance);

    _mirilla.color = impactandoRayo && hit.collider.CompareTag("disparable") ? Color.red : Color.cyan;

    _disparar = arma.EsAutomatica ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

    var animacionActual = animadorArma.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    _recargando = animacionActual == "reload";

    if (CargadorActual > 0 && !EstaCorriendo && _cuentaCadencia <= 0 && _disparar && !_recargando && animacionActual != "run")
    {
      _cuentaCadencia = arma.SegundosCadencia;
      animadorArma.SetTrigger("disparando");
      _audioSource.PlayOneShot(arma.SonidoDisparo);
      CargadorActual--;

      if (impactandoRayo && hit.collider.sharedMaterial != null)
      {
        switch (hit.collider.sharedMaterial.name)
        {
          case "Metal":
            Instantiate(prefabsImpactos[0], hit.point, hit.transform.rotation);
            break;
          case "Wood":
            Instantiate(prefabsImpactos[1], hit.point, hit.transform.rotation);
            break;
          case "Meat":
            Instantiate(prefabsImpactos[2], hit.point, hit.transform.rotation);
            break;
          case "Stone":
            Instantiate(prefabsImpactos[3], hit.point, hit.transform.rotation);
            break;
        }
      }
    }
    else
    {
      if (CargadorActual == 0)
      {
        NecesitaRecargaEvent.Invoke(BalasDeArmasPoseidas[ArmaSeleccionada]);
      }
    }
  }
}