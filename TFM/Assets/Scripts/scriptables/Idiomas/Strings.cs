using System.Linq;
using scriptables;
using UnityEngine;

public class Strings : MonoBehaviour
{
  public Idioma IdiomaActual;
  public Idiomas Idiomas;

  public static Strings Resolver;  

  private void Awake()
  {
    Resolver = this;
  }

  public string GetString(string key)
  {
    var result = IdiomaActual.strings.First(str => str.Key == key);
    return result.String;
  }

  public void CargaIdioma(string denominacion)
  {
    IdiomaActual = Idiomas.IdiomasDisponibles.First(idm => idm.Denominacion == denominacion);
  }

  private void OnDestroy()
  {
    Resolver = null;
  }
}