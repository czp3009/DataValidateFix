using System;
using NLog;
using Sandbox.Game.Entities;
using Sandbox.ModAPI.Ingame;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public class MyBatteryBlockPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static bool SetChargeModePatch(ref ChargeMode value)
        {
            return Enum.IsDefined(typeof(ChargeMode), value);
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                var setChargeMode = typeof(MyBatteryBlock).GetPublicPropertyInfo("ChargeMode").SetMethod;
                patchContext.GetPattern(setChargeMode).Prefixes
                    .Add(((ActionRefBool<ChargeMode>) SetChargeModePatch).Method);
            });
        }
    }
}