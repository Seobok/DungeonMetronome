using System.Collections.Generic;
using Effect;
using Map;
using Unit;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Workflows
{
    public class DungeonSceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private List<EffectEntry> _effectEntries;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_effectEntries);
            
            builder.Register<EffectPool>(Lifetime.Singleton);
            builder.Register<Dungeon>(Lifetime.Singleton);
            builder.Register<UnitManager>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<DungeonSceneWorkflow>();
        }
    }
}
