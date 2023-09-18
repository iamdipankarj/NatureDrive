using System;

namespace Django.VehiclePhysics.Modules.ModuleTemplate
{
    /// <summary>
    ///     MonoBehaviour wrapper for example module.
    /// </summary>
    [Serializable]
    public partial class ModuleTemplateWrapper : ModuleWrapper
    {
        public ModuleTemplate module = new ModuleTemplate();

        public override VehicleComponent GetModule()
        {
            return module;
        }


        public override void SetModule(VehicleComponent module)
        {
            this.module = module as ModuleTemplate;
        }
    }
}