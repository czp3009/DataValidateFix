using System.Reflection;
using NLog;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyOreDetectorPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _range;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyOreDetector __instance)
        {
            // value is percentage
            _range.GetSync<float>(__instance).ValueChangedInRange(0, 100);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyOreDetector>(InitPatch);
                _range = typeof(MyOreDetector).GetPrivateFieldInfo("m_range");
            });
        }
    }
}