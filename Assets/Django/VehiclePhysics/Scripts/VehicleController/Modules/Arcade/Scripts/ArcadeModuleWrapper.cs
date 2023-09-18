using System;

namespace Django.VehiclePhysics.Modules.ArcadeModule
{
    /// <summary>
    ///     MonoBehaviour wrapper for example module.
    /// </summary>
    [Serializable]
    public partial class ArcadeModuleWrapper : ModuleWrapper
    {
        public ArcadeModule module = new ArcadeModule();

        public override VehicleComponent GetModule()
        {
            return module;
        }

        public override void SetModule(VehicleComponent module)
        {
            this.module = module as ArcadeModule;
        }
    }
}