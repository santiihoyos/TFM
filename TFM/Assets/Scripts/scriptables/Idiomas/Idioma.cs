using System;
using UnityEngine;

namespace scriptables
{
  [Serializable]
  public class StringKey
  {
    public string Key;
    public string String;
  }

  
  [CreateAssetMenu(menuName = "Scriptables/Idiomas/Idioma")]
  public class Idioma : ScriptableObject
  {
    public string Denominacion;
    public StringKey[] strings;
  }
}