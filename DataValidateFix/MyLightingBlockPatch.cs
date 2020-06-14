using System.Reflection;
using NLog;
using Sandbox.Game.Entities.Blocks;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyLightingBlockPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static FieldInfo _lightRadius;
        private static FieldInfo _lightFalloff;
        private static FieldInfo _blinkIntervalSeconds;
        private static FieldInfo _blinkLength;
        private static FieldInfo _blinkOffset;
        private static FieldInfo _intensity;
        private static FieldInfo _lightOffset;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyLightingBlock __instance)
        {
            var definition = __instance.BlockDefinition;
            _lightRadius.GetSync<float>(__instance).ValueChangedInRange(
                __instance.IsReflector ? definition.LightReflectorRadius.Min : definition.LightRadius.Min,
                __instance.IsReflector ? definition.LightReflectorRadius.Max : definition.LightRadius.Max
            );
            _lightFalloff.GetSync<float>(__instance).ValueChangedInRange(
                definition.LightFalloff.Min,
                definition.LightFalloff.Max
            );
            _blinkIntervalSeconds.GetSync<float>(__instance).ValueChangedInRange(
                definition.BlinkIntervalSeconds.Min,
                definition.BlinkIntervalSeconds.Max
            );
            _blinkLength.GetSync<float>(__instance).ValueChangedInRange(
                definition.BlinkLenght.Min,
                definition.BlinkLenght.Max
            );
            _blinkOffset.GetSync<float>(__instance).ValueChangedInRange(
                definition.BlinkOffset.Min,
                definition.BlinkOffset.Max
            );
            _intensity.GetSync<float>(__instance).ValueChangedInRange(
                definition.LightIntensity.Min,
                definition.LightIntensity.Max
            );
            _lightOffset.GetSync<float>(__instance).ValueChangedInRange(
                definition.LightOffset.Min,
                definition.LightOffset.Max
            );
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyLightingBlock>(InitPatch);
                _lightRadius = typeof(MyLightingBlock).GetPrivateFieldInfo("m_lightRadius");
                _lightFalloff = typeof(MyLightingBlock).GetPrivateFieldInfo("m_lightFalloff");
                _blinkIntervalSeconds = typeof(MyLightingBlock).GetPrivateFieldInfo("m_blinkIntervalSeconds");
                _blinkLength = typeof(MyLightingBlock).GetPrivateFieldInfo("m_blinkLength");
                _blinkOffset = typeof(MyLightingBlock).GetPrivateFieldInfo("m_blinkOffset");
                _intensity = typeof(MyLightingBlock).GetPrivateFieldInfo("m_intensity");
                _lightOffset = typeof(MyLightingBlock).GetPrivateFieldInfo("m_lightOffset");
            });
        }
    }
}