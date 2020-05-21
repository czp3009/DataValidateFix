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

            _gyroOverrideVelocity.GetSync<Vector3>(__instance).TypedValueChangedFirst(sync =>
            {
                var value = sync.Value;
                var targetX = value.X;
                var targetY = value.Y;
                var targetZ = value.Z;
                var invalid = false;

                if (value.X < -max || value.X > max)
                {
                    targetX = MathHelper.Clamp(value.X, -max, max);
                    invalid = true;
                }

                if (value.Y < -max || value.Y > max)
                {
                    targetY = MathHelper.Clamp(value.Y, -max, max);
                    invalid = true;
                }

                if (value.Z < -max || value.Z > max)
                {
                    targetZ = MathHelper.Clamp(value.Z, -max, max);
                    invalid = true;
                }

                if (float.IsNaN(value.X))
                {
                    targetX = 0;
                    invalid = true;
                }

                if (float.IsNaN(value.Y))
                {
                    targetY = 0;
                    invalid = true;
                }

                if (float.IsNaN(value.Z))
                {
                    targetZ = 0;
                    invalid = true;
                }

                if (invalid)
                {
                    sync.Value = new Vector3(targetX, targetY, targetZ);
                }
            });
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