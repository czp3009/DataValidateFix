

//keen fixed projector inventory problem
// namespace DataValidateFix
// {
//     [PatchShim]
//     public static class MyProjectorBasePatch
//     {
//         private static readonly Logger Log = LogManager.GetCurrentClassLogger();
//
//         private static void SetNewBlueprintPatch(ref List<MyObjectBuilder_CubeGrid> gridBuilders)
//         {
//             gridBuilders.SelectMany(it => it.CubeBlocks)
//                 .ForEach(it =>
//                 {
//                     switch (it)
//                     {
//                         //note: inventory can be null in proto
//                         //note: don't set inventory to null since other plugin may not check it
//                         case MyObjectBuilder_OxygenGenerator oxygenGenerator:
//                             oxygenGenerator.Inventory?.Clear();
//                             break;
//                         case MyObjectBuilder_TurretBase turretBase:
//                             turretBase.Inventory?.Clear();
//                             break;
//                         case MyObjectBuilder_SmallGatlingGun gatlingGun:
//                             gatlingGun.Inventory?.Clear();
//                             break;
//                         case MyObjectBuilder_SmallMissileLauncher missileLauncher:
//                             missileLauncher.Inventory?.Clear();
//                             break;
//                         case MyObjectBuilder_JumpDrive jumpDrive:
//                             jumpDrive.StoredPower = 0;
//                             break;
//                     }
//                 });
//         }
//
//         public static void Patch(PatchContext patchContext)
//         {
//             Log.TryPatching(() =>
//             {
//                 var setNewBlueprint = typeof(MyProjectorBase)
//                     .GetMethod("SetNewBlueprint", BindingFlags.Instance | BindingFlags.NonPublic);
//                 patchContext.GetPattern(setNewBlueprint).Prefixes
//                     .Add(((ActionRef<List<MyObjectBuilder_CubeGrid>>) SetNewBlueprintPatch).Method);
//             });
//         }
//     }
// }