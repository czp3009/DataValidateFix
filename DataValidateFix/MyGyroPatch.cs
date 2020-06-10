using System.Reflection;
using NLog;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;
using VRage.Game;
using VRageMath;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyGyroPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _gyroDefinition;
        private static FieldInfo _gyroOverrideVelocity;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyGyro __instance)
        {
            var max = ((MyGyroDefinition) _gyroDefinition.GetValue(__instance)).CubeSize == MyCubeSize.Small
                ? MyGridPhysics.GetSmallShipMaxAngularVelocity()
                : MyGridPhysics.GetLargeShipMaxAngularVelocity();

            _gyroOverrideVelocity.GetSync<Vector3>(__instance).ValueChangedInRange(
                new Vector3(-max, -max, -max),
                new Vector3(max, max, max)
            );
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyGyro>(InitPatch);
                _gyroDefinition = typeof(MyGyro).GetPrivateFieldInfo("m_gyroDefinition");
                _gyroOverrideVelocity = typeof(MyGyro).GetPrivateFieldInfo("m_gyroOverrideVelocity");
            });
        }
    }
}