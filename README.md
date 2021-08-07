# DataValidateFix
**Deprecated!** Keen fixed all exploits.

A [Torch](https://torchapi.net/) plugin which fix several data validate issue for [SpaceEngineers](https://store.steampowered.com/app/244850/Space_Engineers/) server.

The SpaceEngineers server NOT validate some important two-way sync variable received from client.

For example, open the file located in `SpaceEngineers\Content\Data\CubeBlocks\CubeBlocks_Automation.sbc`(XML) and search for text `MyObjectBuilder_SensorBlockDefinition`.

Modify `MaxRange` of this BlockDefinition then save it.

Use this modified client to connect to dedicated server, you will find that you can set `LeftExtend`, `RightExtend` and other options in SensorBlock UI to very large value.

Those data NOT ONLY display on client, server actually accept them.

Use ProgramBlock to get data from SensorBlock with [`Sandbox.ModAPI.Ingame.IMySensorBlock.DetectedEntities`](https://github.com/malware-dev/MDK-SE/wiki/Sandbox.ModAPI.Ingame.IMySensorBlock.DetectedEntities).

Player can get as many grids' position as he want by increase the `MaxRange` in XML.

This plugin add extra validate logic in server to void the problem above.

# Current fixed block
* MySensorBlock(LeftExtend, RightExtend, BottomExtend, TopExtend, BackExtend, FrontExtend)

* MyWarhead(Countdown)

* MyLargeTurretBase(ShootingRange)

* MyOreDetector(Range)

* MyMechanicalConnectionBlockBase(SafetyDetach)

* MyPistonBase(MaxVelocity, MaxLimit, MinLimit, MaxImpulseAxis, MaxImpulseNonAxis)

* MyMotorStator(Torque, BrakingTorque, TargetVelocity)

* SafeZone(Box size)(modified client will display the fake border until rejoin to server)

* MyMotorSuspension(MaxSteerAngle, Power, Strength, Height, Friction, SpeedLimit, PropulsionOverride, SteeringOverride)

* MyThrust(ThrustOverride)

* MyJumpDrive(JumpDistanceRatio)

* MyGyro(OverrideVelocity)

* MyBeacon(Radius)

* MyRadioAntenna(Radius)

* MyLightingBlock(Radius, Falloff, BlinkInterval, BlinkLength, BlinkOffset, Intensity, LightOffset)

# TODO blocks
* MyLaserAntenna(this patch cause crash, i don't know why)
* Sound(i don't find the block class)

# Script fix
## NaN value

If script in ProgramBlock try to pass float.NaN to property of those fixed block, value of that property will be reset
to minimum legal value.

For example, player run such code in game:

```c#
(GridTerminalSystem.GetBlockWithName("GyroName") as IMyGyro).Pitch = float.NaN;
```

`Pitch` will be reset to `-30`(depend on BlockDefinition in server side).

## Enum out of range

Enums in c# are not checked, player can crash server by in-game script:

```c#
(GridTerminalSystem.GetBlockWithName("BatteryName") as IMyBatteryBlock).ChargeMode = (ChargeMode) 3;
```

With this plugin, `IMyBattertyBlock.ChargeMode` will not be changed if the Enum underlying value(max to 2) not valid.

For more Enum in game please create an issue on Github to tell me.

# Projector fix

~~If player use a modified blueprint in Projector, items in some block inventory will NOT be clear. Player can create
items for free.~~

~~This plugin current fix(If you find more block in blueprint can use this bug, please create an issue on Github):~~

~~* OxygenGenerator(All ice in inventory will be clear)~~

~~* JumpDrive(Stored power will be reset to zero)~~

~~* GatlingGun, MissileLauncher(All ammo in inventory will be clear)~~

(Keen fixed it)

# Note
The game will automatically correct the wrong data when loading world. If the player created illegal data before, Those data will be cleared after the server restarts(to nearest legal value). So no more step need to be done, just install this plugin and restart server.

# License
Apache 2.0
