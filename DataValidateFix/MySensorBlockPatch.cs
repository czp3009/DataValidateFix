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
            var fieldMinMin = new Vector3(-__instance.MaxRange);
            var fieldMinMax = new Vector3(-0.1f);
            var fieldMaxMin = new Vector3(0.1f);
            var fieldMaxMax = new Vector3(__instance.MaxRange);

            _fieldMin.GetSync<Vector3>(__instance).TypedValueChangedFirst(sync =>
            {
                var fieldMin = sync.Value;
                if (!fieldMin.IsInsideInclusive(ref fieldMinMin, ref fieldMinMax))
                {
                    __instance.FieldMin = fieldMin.Clamp(ref fieldMinMin, ref fieldMinMax);
                }
            });

            _fieldMax.GetSync<Vector3>(__instance).TypedValueChangedFirst(sync =>
            {
                var fieldMax = sync.Value;
                if (!fieldMax.IsInsideInclusive(ref fieldMaxMin, ref fieldMaxMax))
                {
                    __instance.FieldMax = fieldMax.Clamp(ref fieldMaxMin, ref fieldMaxMax);
                }
            });
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