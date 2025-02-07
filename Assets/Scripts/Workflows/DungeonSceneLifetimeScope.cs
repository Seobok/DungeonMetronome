using Map;
using Unit;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Workflows
{
    public class DungeonSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<DungeonSceneWorkflow>(Lifetime.Scoped);

            builder.Register<Dungeon>(Lifetime.Scoped);
            builder.Register<UnitManager>(Lifetime.Scoped);
        }
    }
}
