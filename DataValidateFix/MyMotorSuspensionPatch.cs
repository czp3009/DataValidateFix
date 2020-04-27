using System.Reflection;
using NLog;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyMotorSuspensionPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _maxSteerAngle;
        private static FieldInfo _power;
        private static FieldInfo _strenth; //keen typo
        private static FieldInfo _height;
        private static FieldInfo _friction;
        private static FieldInfo _speedLimit;
        private static FieldInfo _propulsionOverride;
        private static FieldInfo _steeringOverride;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyMotorSuspension __instance)
        {
            var blockDefinition = __instance.BlockDefinition;

            _maxSteerAngle.GetSync<float>(__instance).ValueChangedInRange(0, blockDefinition.MaxSteer);
            _power.GetSync<float>(__instance).ValueChangedInRange(0, 1);
            _strenth.GetSync<float>(__instance).ValueChangedInRange(0, 1);
            _height.GetSync<float>(__instance)
                .ValueChangedInRange(blockDefinition.MinHeight, blockDefinition.MaxHeight);
            _friction.GetSync<float>(__instance).ValueChangedInRange(0, 1);
            _speedLimit.GetSync<float>(__instance).ValueChangedInRange(0, 360);
            _propulsionOverride.GetSync<float>(__instance).ValueChangedInRange(-1, 1);
            _steeringOverride.GetSync<float>(__instance).ValueChangedInRange(-1, 1);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyMotorSuspension>(InitPatch);
                _maxSteerAngle = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_maxSteerAngle");
                _power = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_power");
                _strenth = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_strenth");
                _height = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_height");
                _friction = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_friction");
                _speedLimit = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_speedLimit");
                _propulsionOverride = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_propulsionOverride");
                _steeringOverride = typeof(MyMotorSuspension).GetPrivateFieldInfo("m_steeringOverride");
            });
        }
    }
}