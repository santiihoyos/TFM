using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityStandardAssets.Characters.ThirdPerson;

public class ControlZombie : MonoBehaviour, IQuitableVida
{
  public float Vida = 100;
  public GameObject MunicionPrefab;
  public GameObject VidaPrefab;
  public UnityEvent OnDeadEvent;


  private Animator _animador;
  private Rigidbody _rigidbody;
  private bool _alive = true;

  // Use this for initialization
  private void Start()
  {
    _animador = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  private void Update()
  {
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      _animador.SetBool("ataca", true);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      _animador.SetBool("ataca", false);
    }
  }

  public void QuitaVida(float cantidad)
  {
    Vida -= cantidad;
    if (Vida <= 0 && _alive)
    {
      _alive = false;
      GetComponent<AICharacterControl>().target = null;
      GetComponent<CapsuleCollider>().enabled = false;
      GetComponent<BoxCollider>().enabled = false;
      GetComponent<AICharacterControl>().enabled = false;
      GetComponent<ThirdPersonCharacter>().enabled = false;
      GetComponent<NavMeshAgent>().enabled = false;
      _animador.SetBool("ataca", false);
      _animador.SetTrigger("muere");
      if (OnDeadEvent != null)
      {
        OnDeadEvent.Invoke();
      }

      if (Random.Range(0, 100) > 0)
      {
        print("Spawmenado municion");
        var posicion = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Instantiate(MunicionPrefab, posicion, Quaternion.identity);
      }
    }
  }

  public IEnumerator OnEndDead()
  {
    yield return new WaitForSeconds(5f);
    Destroy(gameObject);
  }
}