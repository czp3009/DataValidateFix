using NLog;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyMotorStatorPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyMotorStator __instance)
        {
            var blockDefinition = (MyMotorStatorDefinition) __instance.BlockDefinition;

            __instance.Torque.ValueChangedInRange(0, blockDefinition.MaxForceMagnitude);
            __instance.BrakingTorque.ValueChangedInRange(0, blockDefinition.MaxForceMagnitude);
            __instance.TargetVelocity.ValueChangedInRange(
                -__instance.MaxRotorAngularVelocity,
                __instance.MaxRotorAngularVelocity
            );
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() => { patchContext.PatchInit<MyMotorStator>(InitPatch); });
        }
    }
}