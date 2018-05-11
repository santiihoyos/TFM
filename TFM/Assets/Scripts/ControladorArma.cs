using System;
using UnityEngine;
using UnityEngine.Events;

public class ControladorArma : MonoBehaviour
{
  public Arma Scriptable;
  
  [Serializable]
  public class LanzaCasquilloEvent : UnityEvent<Transform> {}
  public LanzaCasquilloEvent OnLanzaCasquillo;
  
  public UnityEvent OnEndReload;
  private Animator _animator;

  [SerializeField] private Transform _posicionLanzamientoasquillo;

  // Use this for initialization
  void Start()
  {
    if (OnEndReload == null)
    {
      OnEndReload = new UnityEvent();
      OnLanzaCasquillo = new LanzaCasquilloEvent();
    }

    _animator = GetComponent<Animator>();
    foreach (var clip in _animator.runtimeAnimatorController.animationClips)
    {
      switch (clip.name)
      {
        case "reload":
          clip.AddEvent(new AnimationEvent
          {
            functionName = "OnEndReloadListener",
            intParameter = 0,
            time = 2
          });
          break;
        case "shoot":
          clip.AddEvent(new AnimationEvent
          {
            functionName = "LanzaCasquilloListener",
            intParameter = 0,
            time = 0.1f
          });
          break;
      }
    }
  }

  void OnEndReloadListener(int param)
  {
    OnEndReload.Invoke();
  }

  void LanzaCasquilloListener()
  {
    print("Lanza casquillo");
    OnLanzaCasquillo.Invoke(_posicionLanzamientoasquillo);
  }
}