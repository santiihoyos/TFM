using System;
using UnityEngine.Events;

namespace CustomEvents
{
  [Serializable]
  public class ValueIntChangeEvent : UnityEvent<int>
  {
  }
  
  [Serializable]
  public class ValueStringChangeEvent : UnityEvent<int>
  {
  }
}