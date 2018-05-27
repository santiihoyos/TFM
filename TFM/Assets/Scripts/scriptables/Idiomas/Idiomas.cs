using UnityEngine;

namespace scriptables
{
  [CreateAssetMenu(menuName = "Scriptables/Idiomas/ListaIdiomas")]
  public class Idiomas: ScriptableObject
  {
    public Idioma[] IdiomasDisponibles;
  }
}