using UnityEngine;
using UnityEngine.Events;

public class ControladorArma : MonoBehaviour
{
  public UnityEvent OnEndReload;
  private Animator _animator;

  // Use this for initialization
  void Start()
  {
    if (OnEndReload == null)
    {
      OnEndReload = new UnityEvent();
    }

    _animator = GetComponent<Animator>();
    foreach (var clip in _animator.runtimeAnimatorController.animationClips)
    {
      if (clip.name == "reload")
      {
        clip.AddEvent(new AnimationEvent
        {
          functionName = "OnEndReloadListener",
          intParameter = 0,
          time = 2
        });
      }
    }
  }

  void OnEndReloadListener(int param)
  {
    OnEndReload.Invoke();
  }
}