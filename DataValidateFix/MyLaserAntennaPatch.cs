using System.Reflection;
using NLog;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    //TODO this patch cause crash
    //[PatchShim]
    public static class MyLaserAntennaPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _range;
        private static float _infiniteRange;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyLaserAntenna __instance)
        {
            var definition = __instance.BlockDefinition;
            var maxRange = definition.MaxRange >= 0 ? definition.MaxRange : _infiniteRange;
            _range.GetSync<float>(__instance).ValueChangedInRange(1, maxRange);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyLaserAntenna>(InitPatch);
                _range = typeof(MyLaserAntenna).GetPrivateFieldInfo("m_range");
                _infiniteRange = typeof(MyLaserAntenna).GetPrivateConstValue<float>("INFINITE_RANGE");
            });
        }
    }
}