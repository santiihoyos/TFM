using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Arma")]
public class Arma : ScriptableObject
{
  public string Nombre;
  public float Alcance;
  public int BalasPorCargador;
  public bool EsAutomatica;
  public GameObject PrefabCasquillo;
  public AudioClip SonidoDisparo;
  public AudioClip SonidoCasquillo;
  public AudioClip SonidoRecarga;
  public float SegundosCadencia;
}