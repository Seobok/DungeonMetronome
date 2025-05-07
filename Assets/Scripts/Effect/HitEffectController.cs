using UnityEngine;
using VContainer;

namespace Effect
{
    [RequireComponent(typeof(Animator))]
    public class HitEffectController : MonoBehaviour, IEffect
    {
        public EffectType EffectType => EffectType.Hit;
        
        private Animator _animator;
        private EffectPool _effectPool;


        [Inject]
        public void Construct(EffectPool effectPool)
        {
            _effectPool = effectPool;
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void Play(Vector2 position)
        {
            transform.position = position;
            gameObject.SetActive(true);

            var clip = _animator.runtimeAnimatorController.animationClips[0];
            _animator.Play(clip.name, 0, 0f);
        }

        public void Stop()  //애니메이션 이벤트에서 호출됨
        {
            _effectPool.ReturnEffect(EffectType, this);
        }
    }
}