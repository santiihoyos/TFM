using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using Scriptables;
using UnityEngine;
using UnityEngine.Audio;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameController : MonoBehaviour
{
  public AudioClip[] SonidosDeFondo;

  [Tooltip("Posicions desde las que se van a ir generando los zombies.")]
  public GameObject[] Spawmers;

  [Tooltip("Los prefabs de los zombie para generar de forma aleatoria en los spawmers")]
  public GameObject[] ZombiesPrefabs;

  public int RondaActual;
  public Ronda[] Rondas;
  public ValueIntChangeEvent OnRondaChange;
  public GameObject Player;

  private AudioSource _audioSourceGeneral;
  private int _sonidoDeFondoActual;
  private AudioMixer _audioMixer;
  private List<GameObject> _zombiesVivos = new List<GameObject>();
  private int _zombiesVivosCount;

  // Use this for initialization

  void Start()
  {
    _audioSourceGeneral = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (!_audioSourceGeneral.isPlaying && SonidosDeFondo.Length > 0 && RondaActual > 0)
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

  private IEnumerator CambiaDeRonda()
  {
    Player.GetComponent<ControladorJugador>().Bloqueado = false;
    RondaActual++;
    if (OnRondaChange != null)
    {
      OnRondaChange.Invoke(RondaActual);
    }

    yield return new WaitForSeconds(5f);

    _zombiesVivosCount = Rondas[RondaActual - 1].TotalZombies;
    StartCoroutine(SpawmeaZombies());
  }

  private IEnumerator SpawmeaZombies()
  {
    var counter = 0;
    do
    {
      var spawnPosition = Spawmers[Random.Range(0, Spawmers.Length)];
      var zombie = Instantiate(ZombiesPrefabs[Random.Range(0, ZombiesPrefabs.Length)], spawnPosition.transform.position, Quaternion.identity);
      zombie.GetComponent<AICharacterControl>().target = Player.transform;
      zombie.GetComponent<ControlZombie>().OnDeadEvent.AddListener(OnZombieMuereListener);
      counter++;
      _zombiesVivos.Add(zombie);
      yield return new WaitForSeconds(Rondas[RondaActual - 1].DelaySpawmeoZombies);
    } while (counter < Rondas[RondaActual - 1].TotalZombies);
  }

  private void OnZombieMuereListener()
  {
    _zombiesVivosCount--;
    if (_zombiesVivosCount <= 0)
    {
      StartCoroutine(CambiaDeRonda());
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    print("Iniciando rondas!");
    if (RondaActual <= 0 && other.gameObject.CompareTag("Player"))
    {
      StartCoroutine(CambiaDeRonda());
    }
  }
}