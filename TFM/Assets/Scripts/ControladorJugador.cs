using UnityEngine;
using UnityEngine.UI;

public class ControladorJugador : MonoBehaviour
{
  public Animator animadorArma;

  [SerializeField] private GameObject[] prefabsImpactos;
  public Arma[] Armas;
  public Arma ArmaSeleccionada;
  public bool EstaCorriendo;

  private CharacterController _controller;
  private AudioSource _audioSource;
  private float _cuentaCadencia;
  private bool _disparar;

  [SerializeField] private Image _mirilla;

  // Use this for initialization
  void Start()
  {
    _controller = GetComponent<CharacterController>();
    _audioSource = GetComponent<AudioSource>();

    ArmaSeleccionada = Armas[2];
  }

  private void Update()
  {
    if (_cuentaCadencia >= 0)
    {
      _cuentaCadencia -= Time.deltaTime;
    }
  }

  private void FixedUpdate()
  {
    ControlAnimacionMovimientos();
    ControlDisparo();
  }

  private void ControlAnimacionMovimientos()
  {
    EstaCorriendo = Mathf.Abs(_controller.velocity.x) > 8 || Mathf.Abs(_controller.velocity.z) > 8;
    animadorArma.SetBool("corriendo", EstaCorriendo);
  }

  private void ControlDisparo()
  {
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    Debug.DrawRay(ray.origin, ray.direction * ArmaSeleccionada.Alcance, Color.green);

    bool impactandoRayo = Physics.Raycast(ray, out hit, ArmaSeleccionada.Alcance);
    
    _mirilla.color = impactandoRayo && hit.collider.CompareTag("disparable") ? Color.red : Color.cyan;
    
    _disparar = ArmaSeleccionada.EsAutomatica ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

    if (!EstaCorriendo && _cuentaCadencia <= 0 && _disparar && animadorArma.GetCurrentAnimatorClipInfo(0)[0].clip.name != "run")
    {
      _cuentaCadencia = ArmaSeleccionada.SegundosCadencia;
      animadorArma.SetTrigger("disparando");
      _audioSource.PlayOneShot(ArmaSeleccionada.SonidoDisparo);

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
  }
}