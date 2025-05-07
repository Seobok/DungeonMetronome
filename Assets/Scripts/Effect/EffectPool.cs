using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Effect
{
    public class EffectPool
    {
        private readonly List<EffectEntry> _entries;
        private readonly IObjectResolver _resolver;  
        private readonly Dictionary<EffectType, Queue<IEffect>> _pools = new();
        private readonly Dictionary<EffectType, GameObject> _prefabs = new();
        
        
        public EffectPool(List<EffectEntry> entries, IObjectResolver resolver)
        {
            _resolver = resolver;
            _entries = entries;
        }

        public void Init()
        {
            foreach (var entry in _entries)
            {
                var queue = new Queue<IEffect>();
                _pools[entry._type] = queue;
                _prefabs[entry._type] = entry._prefab;

                for (int i = 0; i < entry._preloadCount; i++)
                {
                    IEffect effect = CreateEffect(entry._type);
                    queue.Enqueue(effect);
                    ((MonoBehaviour) effect).gameObject.SetActive(false);
                }
            }
        }
        
        public IEffect GetEffect(EffectType type)
        {
            if (!_pools.ContainsKey(type))
            {
                Debug.LogError($"이펙트 {type} 은 등록되지 않았습니다!");
                return null;
            }

            var queue = _pools[type];
            if (queue.Count > 0)
                return queue.Dequeue();

            return CreateEffect(type);
        }
        
        public void ReturnEffect(EffectType type, IEffect effect)
        {
            ((MonoBehaviour) effect).gameObject.SetActive(false);
            _pools[type].Enqueue(effect);
        }

        private IEffect CreateEffect(EffectType type)
        {
            var prefab = _prefabs[type];
            var obj = GameObject.Instantiate(prefab);
            _resolver.InjectGameObject(obj);    //DI 주입
            var effect = obj.GetComponent<IEffect>();
            
            if(effect == null)
                Debug.LogError($"{type} 이펙트가 IEffect가 아닙니다!");
            
            return effect;
        }
    }
}