using System.Reflection;
using NLog;
using Sandbox.Game.Entities.Blocks;
using Torch.Managers.PatchManager;
using VRageMath;

namespace DataValidateFix
{
    [PatchShim]
    public static class MySensorBlockPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _fieldMin;
        private static FieldInfo _fieldMax;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MySensorBlock __instance)
        {
            _fieldMin.GetSync<Vector3>(__instance).ValueChangedInRange(
                new Vector3(-__instance.MaxRange),
                new Vector3(-0.1f)
            );
            _fieldMax.GetSync<Vector3>(__instance).ValueChangedInRange(
                new Vector3(0.1f),
                new Vector3(__instance.MaxRange)
            );
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MySensorBlock>(InitPatch);
                _fieldMin = typeof(MySensorBlock).GetPrivateFieldInfo("m_fieldMin");
                _fieldMax = typeof(MySensorBlock).GetPrivateFieldInfo("m_fieldMax");
            });
        }
    }
}