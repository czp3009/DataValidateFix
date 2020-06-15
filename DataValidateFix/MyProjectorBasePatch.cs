using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Blocks;
using Torch.Managers.PatchManager;
using VRage;
using VRage.Game;

namespace DataValidateFix
{
    [PatchShim]
    public static class MyProjectorBasePatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static void SetNewBlueprintPatch(ref List<MyObjectBuilder_CubeGrid> gridBuilders)
        {
            gridBuilders.SelectMany(it => it.CubeBlocks)
                .ForEach(it =>
                {
                    switch (it)
                    {
                        case MyObjectBuilder_OxygenGenerator oxygenGenerator:
                            oxygenGenerator.Inventory = null;
                            break;
                        case MyObjectBuilder_JumpDrive jumpDrive:
                            jumpDrive.StoredPower = 0;
                            break;
                    }
                });
        }

        public static void Patch(PatchContext patchContext)
        {
            Log.TryPatching(() =>
            {
                var setNewBlueprint = typeof(MyProjectorBase)
                    .GetMethod("SetNewBlueprint", BindingFlags.Instance | BindingFlags.NonPublic);
                patchContext.GetPattern(setNewBlueprint).Prefixes
                    .Add(((ActionRef<List<MyObjectBuilder_CubeGrid>>) SetNewBlueprintPatch).Method);
            });
        }
    }
}