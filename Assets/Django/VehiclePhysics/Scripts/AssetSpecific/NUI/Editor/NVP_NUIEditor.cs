#if UNITY_EDITOR
namespace Django.NUI
{
    /// <summary>
    ///     Django Vehicle Physics specific NUI Editor.
    /// </summary>
    public class NVP_NUIEditor : NUIEditor
    {
        public override bool OnInspectorNUI()
        {
            if (!base.OnInspectorNUI())
            {
                return false;
            }

            drawer.documentationBaseURL = "http://nwhvehiclephysics.com/doku.php/";
            return true;
        }
    }
}

#endif
