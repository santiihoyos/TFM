using CustomEvents;
using Scriptables;
using UnityEngine;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
  public AudioClip[] SonidosDeFondo;
  
  [Tooltip("Posicions desde las que se van a ir generando los zombies.")]
  public GameObject[] Spawmers;

  private AudioSource _audioSourceGeneral;
  private int _sonidoDeFondoActual;
  private AudioMixer _audioMixer;

  public int RondaActual;
  public Ronda[] Rondas;
  public ValueIntChangeEvent OnRondaChange;

  // Use this for initialization

  void Start()

  {
    _audioSourceGeneral = GetComponent<AudioSource>();
  }
  // Update is called once per frame

  private void Update()
  {
    if (!_audioSourceGeneral.isPlaying && SonidosDeFondo.Length > 0)
    {
      if (_sonidoDeFondoActual + 1 >= SonidosDeFondo.Length)
      {
        _sonidoDeFondoActual = 0;
      }
      else
      {
        _sonidoDeFondoActual += 1;
      }

      _audioSourceGeneral.clip = SonidosDeFondo[_sonidoDeFondoActual];
      _audioSourceGeneral.Play();
    }
  }
}