using System.Reflection;
using NLog;
using Sandbox.Game.Entities;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyJumpDrivePatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _jumpDistanceRatio;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyJumpDrive __instance)
        {
            _jumpDistanceRatio.GetSync<float>(__instance).ValueChangedInRange(0, 1);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyJumpDrive>(InitPatch);
                _jumpDistanceRatio = typeof(MyJumpDrive).GetPrivateFieldInfo("m_jumpDistanceRatio");
            });
        }
    }
}