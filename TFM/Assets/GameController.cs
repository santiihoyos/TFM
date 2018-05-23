using System.Collections;
 
using System.Collections.Generic;
 
using UnityEngine;
 
using UnityEngine.Audio;
 

 
public class GameController : MonoBehaviour
 
{
	public AudioClip[] SonidosDeFondo;
 

 
	private AudioSource _audioSourceGeneral;
 
	private int _sonidoDeFondoActual;
 
	private AudioMixer _audioMixer;
 
  
 
	// Use this for initialization
 
	void Start ()
 
	{
 
		_audioSourceGeneral = GetComponent<AudioSource>();
 
	}
 
  
 
	// Update is called once per frame
 
	void Update () {
 

 
		if (!_audioSourceGeneral.isPlaying)
 
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
