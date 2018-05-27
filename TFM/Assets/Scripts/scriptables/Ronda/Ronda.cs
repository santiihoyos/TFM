using UnityEngine;

namespace Scriptables
{
  [CreateAssetMenu(menuName = "Scriptables/Ronda")]
  public class Ronda : ScriptableObject
  {
    public int TotalZombies;
    public float DelaySpawmeoZombies;
    public bool EsRondaFinal;
  }
}
