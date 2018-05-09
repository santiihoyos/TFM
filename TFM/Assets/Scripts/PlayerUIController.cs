using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
  [SerializeField] private Text _avisoCargador;

  // Use this for initialization
  void Start()
  {
    _avisoCargador.enabled = false;
  }

  public void Recargado()
  {
    _avisoCargador.enabled = false;
  }

  public void OnNecesariaRecarga(int balasQueQuedan)
  {
    if (balasQueQuedan <= 0)
    {
      _avisoCargador.text = "No quedan balas!";
    }
    else
    {
      _avisoCargador.text = "Recarga!";
    }

    _avisoCargador.enabled = true;
  }
}