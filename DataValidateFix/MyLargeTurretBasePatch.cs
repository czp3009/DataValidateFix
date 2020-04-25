using System.Reflection;
using NLog;
using Sandbox.Game.Weapons;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyLargeTurretBasePatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _shootingRange;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyLargeTurretBase __instance)
        {
            _shootingRange.GetSync<float>(__instance).ValueChangedInRange(0, __instance.BlockDefinition.MaxRangeMeters);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyLargeTurretBase>(InitPatch);
                _shootingRange = typeof(MyLargeTurretBase).GetPrivateFieldInfo("m_shootingRange");
            });
        }
    }
}