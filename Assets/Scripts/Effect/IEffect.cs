using UnityEngine;

namespace Effect
{
    public interface IEffect
    {
        void Play(Vector2 position);
        void Stop();
        EffectType EffectType { get; }
    }
}