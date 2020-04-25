using NLog;
using Sandbox;
using Sandbox.Game.Entities.Blocks;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyPistonBasePatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyPistonBase __instance)
        {
            var blockDefinition = __instance.BlockDefinition;

            __instance.Velocity.ValueChangedInRange(-blockDefinition.MaxVelocity, blockDefinition.MaxVelocity);

            var minimum = blockDefinition.Minimum;
            var maximum = blockDefinition.Maximum;
            __instance.MaxLimit.ValueChangedInRange(minimum, maximum);
            __instance.MinLimit.ValueChangedInRange(minimum, maximum);

            var maxImpulseAxis = MySandboxGame.Config.ExperimentalMode
                ? float.MaxValue
                : blockDefinition.UnsafeImpulseThreshold;
            __instance.MaxImpulseAxis.ValueChangedInRange(100, maxImpulseAxis);
            __instance.MaxImpulseNonAxis.ValueChangedInRange(100, maxImpulseAxis);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() => { patchContext.PatchInit<MyPistonBase>(InitPatch); });
        }
    }
}