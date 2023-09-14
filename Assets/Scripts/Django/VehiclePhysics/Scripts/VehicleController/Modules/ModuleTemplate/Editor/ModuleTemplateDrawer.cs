#if UNITY_EDITOR
using Django.NUI;
using UnityEditor;
using UnityEngine;

namespace Django.VehiclePhysics2.Modules.ModuleTemplate
{
    [CustomPropertyDrawer(typeof(ModuleTemplate))]
    public partial class ModuleTemplateDrawer : ComponentNUIPropertyDrawer
    {
        public override bool OnNUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!base.OnNUI(position, property, label))
            {
                return false;
            }

            ModuleTemplate moduleTemplate = SerializedPropertyHelper.GetTargetObjectOfProperty(property) as ModuleTemplate;
            if (moduleTemplate == null)
            {
                drawer.EndProperty();
                return false;
            }

            drawer.Field("floatExample");

            drawer.BeginSubsection("Subsection Example");
            drawer.ReorderableList("listExample");
            drawer.EndSubsection();

            drawer.EndProperty();
            return true;
        }
    }
}

#endif
