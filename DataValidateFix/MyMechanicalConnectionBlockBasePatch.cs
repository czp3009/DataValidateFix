using System.Reflection;
using NLog;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyMechanicalConnectionBlockBasePatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _safetyDetach;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyMechanicalConnectionBlockBase __instance)
        {
            var blockDefinition = (MyMechanicalConnectionBlockBaseDefinition) __instance.BlockDefinition;
            _safetyDetach.GetSync<float>(__instance).ValueChangedInRange(
                blockDefinition.SafetyDetachMin,
                blockDefinition.SafetyDetachMax
            );
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyMechanicalConnectionBlockBase>(InitPatch);
                _safetyDetach = typeof(MyMechanicalConnectionBlockBase).GetPrivateFieldInfo("m_safetyDetach");
            });
        }
    }
}