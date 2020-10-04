using System;
using NLog;
using Torch.Managers.PatchManager;
using System.Reflection;
using Sandbox.Game.Entities;
using Sandbox.ModAPI.Ingame;

namespace DataValidateFix
{
    [PatchShim]
    public class MyBatteryBlockPatch
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly MethodInfo chargeMode = typeof(MyBatteryBlock).GetProperty(nameof(MyBatteryBlock.ChargeMode), BindingFlags.Instance | BindingFlags.Public).SetMethod;
        private static readonly MethodInfo chargeModePatch = typeof(MyBatteryBlockPatch).GetMethod(nameof(ChargeModePatchMethod), BindingFlags.Static | BindingFlags.Public);

        public static bool ChargeModePatchMethod(MyBatteryBlock __instance, ChargeMode value)
        {
            if (!Enum.IsDefined(typeof(ChargeMode), value))
                return false;
            return true;
        }

        public static void Patch(PatchContext context)
        {
            context.GetPattern(chargeMode).Prefixes.Add(chargeModePatch);
        }
    }
}