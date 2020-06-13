using System.Reflection;
using NLog;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public class MyBeaconPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _radius;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyBeacon __instance)
        {
            var definition = (MyBeaconDefinition) __instance.BlockDefinition;
            _radius.GetSync<float>(__instance).ValueChangedInRange(1, definition.MaxBroadcastRadius);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyBeacon>(InitPatch);
                _radius = typeof(MyBeacon).GetPrivateFieldInfo("m_radius");
            });
        }
    }
}