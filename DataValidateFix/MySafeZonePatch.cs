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
        private static readonly Vector3 MinSize = new Vector3(MySafeZone.MIN_RADIUS * 2);
        private static readonly Vector3 MaxSize = new Vector3(MySafeZone.MAX_RADIUS * 2);

        // ReSharper disable once InconsistentNaming
        private static void InitInternalPatch(ref MyObjectBuilder_SafeZone ob)
        {
            ob.Size = Vector3.Clamp(ob.Size, MinSize, MaxSize);
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