using System.Reflection;
using NLog;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyRadioAntennaPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _radius;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyRadioAntenna __instance)
        {
            var definition = (MyRadioAntennaDefinition) __instance.BlockDefinition;
            _radius.GetSync<float>(__instance).ValueChangedInRange(1, definition.MaxBroadcastRadius);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyRadioAntenna>(InitPatch);
                _radius = typeof(MyRadioAntenna).GetPrivateFieldInfo("m_radius");
            });
        }
    }
}