using System.Reflection;
using NLog;
using Sandbox.Game.Entities;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyThrustPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _thrustOverride;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyThrust __instance)
        {
            _thrustOverride.GetSync<float>(__instance).ValueChangedInRange(0, 100);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyThrust>(InitPatch);
                _thrustOverride = typeof(MyThrust).GetPrivateFieldInfo("m_thrustOverride");
            });
        }
    }
}