using System.Reflection;
using NLog;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Torch.Managers.PatchManager;
using VRage;
using VRageMath;

namespace DataValidateFix
{
    [PatchShim]
    public static class MySafeZonePatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static Vector3 _minSize = new Vector3(MySafeZone.MIN_RADIUS * 2);
        private static Vector3 _maxSize = new Vector3(MySafeZone.MAX_RADIUS * 2);

        // ReSharper disable once InconsistentNaming
        private static void InitInternalPatch(ref MyObjectBuilder_SafeZone ob)
        {
            var size = ob.Size;
            if (!size.IsInsideInclusive(ref _minSize, ref _maxSize))
            {
                ob.Size = size.Clamp(ref _minSize, ref _maxSize);
            }
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                var initInternal = typeof(MySafeZone)
                    .GetMethod("InitInternal", BindingFlags.Instance | BindingFlags.NonPublic);
                patchContext.GetPattern(initInternal).Prefixes
                    .Add(((ActionRef<MyObjectBuilder_SafeZone>) InitInternalPatch).Method);
            });
        }
    }
}