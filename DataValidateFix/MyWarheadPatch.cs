using System.Reflection;
using NLog;
using Sandbox.Game.Entities.Cube;
using Torch.Managers.PatchManager;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyWarheadPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static int _maxCountdownMs;
        private static FieldInfo _countdownMs;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MyWarhead __instance)
        {
            _countdownMs.GetSync<int>(__instance).TypedValueChangedFirst(sync =>
            {
                if (sync.Value > _maxCountdownMs)
                {
                    sync.Value = _maxCountdownMs;
                }
            });
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                patchContext.PatchInit<MyWarhead>(InitPatch);
                _countdownMs = typeof(MyWarhead).GetPrivateFieldInfo("m_countdownMs");
                _maxCountdownMs = typeof(MyWarhead).GetPrivateConstValue<int>("MAX_COUNTDOWN") * 1000;
            });
        }
    }
}