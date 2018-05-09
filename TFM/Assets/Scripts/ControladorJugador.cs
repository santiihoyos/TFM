using UnityEngine;
using UnityEngine.UI;

public class ControladorJugador : MonoBehaviour
{
  public Animator animadorArma;

  [SerializeField] private GameObject[] prefabsImpactos;
  public Arma[] Armas;
  public Arma ArmaSeleccionada;
  
  private CharacterController _controller;
  private AudioSource _audioSource;

  [SerializeField] private Image _mirilla;
  
  // Use this for initialization
  void Start()
  {
    _controller = GetComponent<CharacterController>();
    _audioSource = GetComponent<AudioSource>();

    ArmaSeleccionada = Armas[2];

  }

  private void FixedUpdate()
  {
    var isCorriendo = Mathf.Abs(_controller.velocity.x) > 5 || Mathf.Abs(_controller.velocity.z) > 5;
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    Debug.DrawRay(ray.origin, ray.direction * ArmaSeleccionada.Alcance, Color.green);

    var impacto = Physics.Raycast(ray, out hit, ArmaSeleccionada.Alcance);
    if (impacto && hit.collider.CompareTag("disparable"))
    {
      _mirilla.color = Color.red;
    }
    else
    {
      _mirilla.color = Color.cyan;
    }

    if (!isCorriendo && Input.GetButtonDown("Fire1") && animadorArma.GetCurrentAnimatorClipInfo(0)[0].clip.name != "run")
    {
      animadorArma.SetTrigger("disparando");
      _audioSource.PlayOneShot(ArmaSeleccionada.SonidoDisparo);

      if (impacto && hit.collider.sharedMaterial != null) //Disparo choca!!!
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

    animadorArma.SetBool("corriendo", isCorriendo);
  }
}