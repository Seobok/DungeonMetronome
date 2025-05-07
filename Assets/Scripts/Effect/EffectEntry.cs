using UnityEngine;

namespace Effect
{
    [System.Serializable]
    public class EffectEntry
    {
        public EffectType _type;
        public GameObject _prefab;
        public int _preloadCount = 3;
    }
}