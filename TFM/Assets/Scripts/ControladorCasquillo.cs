using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
  public class ControladorCasquillo : MonoBehaviour
  {
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
      StartCoroutine(Destruye());
    }

    private void OnCollisionEnter(Collision other)
    {
      _audioSource.Play();
    }

    IEnumerator Destruye()
    {
      yield return new WaitForSeconds(10);
      Destroy(gameObject);
    }
  }
}