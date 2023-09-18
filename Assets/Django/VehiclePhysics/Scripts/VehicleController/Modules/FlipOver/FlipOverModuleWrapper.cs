using System;

namespace Django.VehiclePhysics.Modules.FlipOver
{
    /// <summary>
    ///     MonoBehaviour wrapper for FlipOver module.
    /// </summary>
    [Serializable]
    public partial class FlipOverModuleWrapper : ModuleWrapper
    {
        public FlipOverModule module = new FlipOverModule();


        public override VehicleComponent GetModule()
        {
            return module;
        }


        public override void SetModule(VehicleComponent module)
        {
            this.module = module as FlipOverModule;
        }

    }
}