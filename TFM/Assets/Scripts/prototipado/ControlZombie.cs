using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlZombie : MonoBehaviour
{
  
  private Animator _animador;
  private Rigidbody _rigidbody;
  
  // Use this for initialization
  private void Start()
  {
    _animador = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  private void Update()
  {
    var izquierdaDerecha = Input.GetAxis("Horizontal");
    var delanteAtras = Input.GetAxis("Vertical");
    _rigidbody.AddForce(Vector3.forward * delanteAtras * 20f, ForceMode.Acceleration);
  }
}