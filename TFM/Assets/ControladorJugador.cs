using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityStandardAssets.Characters.FirstPerson;

public class ControladorJugador : MonoBehaviour
{
  public Animator animadorArma;
  private CharacterController _controller;

  // Use this for initialization
  void Start()
  {
    _controller = GetComponent<CharacterController>();
  }

  // Update is called once per frame
  void Update()
  {    

  }

  private void FixedUpdate()
  {
    var isCorriendo = Mathf.Abs(_controller.velocity.x) > 5 || Mathf.Abs(_controller.velocity.z) > 5;
    
    if (!isCorriendo && Input.GetButtonDown("Fire1"))
    {
      animadorArma.SetTrigger("disparando");
    }

    animadorArma.SetBool("corriendo", isCorriendo);
  }
}