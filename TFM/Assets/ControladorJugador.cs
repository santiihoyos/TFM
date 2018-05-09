using System;
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

  [SerializeField] private GameObject[] prefabsImpactos;

  // Use this for initialization
  void Start()
  {
    _controller = GetComponent<CharacterController>();
  }

  private void FixedUpdate()
  {
    var isCorriendo = Mathf.Abs(_controller.velocity.x) > 5 || Mathf.Abs(_controller.velocity.z) > 5;
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

    if (!isCorriendo && Input.GetButtonDown("Fire1"))
    {
      animadorArma.SetTrigger("disparando");

      if (Physics.Raycast(ray, out hit, 100) && hit.collider.sharedMaterial != null) //Disparo choca!!!
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