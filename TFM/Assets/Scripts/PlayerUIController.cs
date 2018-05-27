using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{

  [SerializeField] private GameObject _mira;
  [SerializeField] private GameObject _panelJuego;
  [SerializeField] private GameObject _panelRonda;
  [SerializeField] private GameObject _panelVidaBaja;
  [SerializeField] private Text _contadorVida;
  [SerializeField] private Text _contadorStamina;
  [SerializeField] private Text _avisoCargador;
  private Coroutine corutineRecarga;
  
  
  // Use this for initialization
  void Start()
  {
    _avisoCargador.enabled = false;
  }

  public void Recargado()
  {
    print("Recargando en ui!");
    _avisoCargador.gameObject.SetActive(false);
    StopCoroutine(corutineRecarga);
    StartCoroutine(CambioDeRonda());
  }

  public void OnNecesariaRecarga(int balasQueQuedan)
  {
    if (balasQueQuedan <= 0)
    {
      _avisoCargador.text = Strings.Resolver.GetString("noBalas");
    }
    else
    {
      _avisoCargador.text = Strings.Resolver.GetString("recarga");
    }

    _avisoCargador.gameObject.SetActive(true);
    print("activando desactivaror!");
    corutineRecarga = StartCoroutine(AvisoRecargaPalpito());
  }

  public void OnRondaChangeListener(int rondaActual)
  {
    
  }

  //activa y desactiva el text del aviso de recarga cada 1s
  IEnumerator AvisoRecargaPalpito()
  {
    while (true)
    {
      yield return new WaitForSeconds(1f);
      _avisoCargador.enabled = !_avisoCargador.enabled; 
    }
  }

  IEnumerator CambioDeRonda()
  {
    _panelRonda.SetActive(true);
    _panelJuego.SetActive(false);
    _mira.SetActive(false);
    yield return new WaitForSeconds(3f);
    _panelJuego.SetActive(true);
    _panelRonda.SetActive(false);
    _mira.SetActive(true);
  }

  public void OnVidaChangeListener(float newVidaPercent)
  {
    
  }

  public void OnStaminaChangeListener(float newStaminaPercent)
  {
    _contadorStamina.text = newStaminaPercent.ToString("000");
  }
}