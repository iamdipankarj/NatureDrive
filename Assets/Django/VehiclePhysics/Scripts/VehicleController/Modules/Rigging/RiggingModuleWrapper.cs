using System;

namespace Django.VehiclePhysics.Modules.Rigging
{
    /// <summary>
    ///     MonoBehaviour wrapper for Rigging module.
    /// </summary>
    [Serializable]
    public partial class RiggingModuleWrapper : ModuleWrapper
    {
        public RiggingModule module = new RiggingModule();


        public override VehicleComponent GetModule()
        {
            return module;
        }


        public override void SetModule(VehicleComponent module)
        {
            this.module = module as RiggingModule;
        }
    }
}