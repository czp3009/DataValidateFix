using System;
using System.Linq;
using System.Reflection;
using Sandbox.Game.Entities;
using Torch.Managers.PatchManager;
using VRage.Game;
using VRage.Sync;

namespace DataValidateFix
{
    internal static class ReflectionExtensions
    {
        internal static FieldInfo GetPrivateFieldInfo(this Type type, string fieldName) =>
            type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance) ??
            throw new MissingFieldException(type.Name, fieldName);

        internal static T GetPrivateConstValue<T>(this Type type, string fieldName) =>
            (T) (type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static) ??
                 throw new MissingFieldException(type.Name, fieldName)).GetValue(null);

        internal static MethodInfo GetMethod(this Type type, string methodName, params Type[] types) =>
            type.GetMethod(methodName, types) ?? throw new MissingMethodException(type.Name, methodName);

        internal static T GetValue<T>(this FieldInfo fieldInfo, object obj) where T : class =>
            (T) fieldInfo.GetValue(obj);

        internal static Sync<T, SyncDirection.BothWays> GetSync<T>(this FieldInfo fieldInfo, object obj) =>
            (Sync<T, SyncDirection.BothWays>) fieldInfo.GetValue(obj);

        internal static void TypedValueChanged<T>(
            this Sync<T, SyncDirection.BothWays> sync,
            Action<Sync<T, SyncDirection.BothWays>> action)
        {
            sync.ValueChanged += syncBase => action((Sync<T, SyncDirection.BothWays>) syncBase);
        }

        private static readonly FieldInfo ValueChangedEventInfo = typeof(SyncBase).GetPrivateFieldInfo("ValueChanged");

        internal static void TypedValueChangedFirst<T>(
            this Sync<T, SyncDirection.BothWays> sync,
            Action<Sync<T, SyncDirection.BothWays>> action)
        {
            var @event = (MulticastDelegate) ValueChangedEventInfo.GetValue(sync);
            var invocationList = @event.GetInvocationList().Cast<Action<SyncBase>>().ToList();
            invocationList.ForEach(it => sync.ValueChanged -= it);
            sync.ValueChanged += syncBase => action((Sync<T, SyncDirection.BothWays>) syncBase);
            invocationList.ForEach(it => sync.ValueChanged += it);
        }

        internal static void ValueChangedInRange<T>(
            this Sync<T, SyncDirection.BothWays> sync,
            T min, T max) where T : IComparable
        {
            sync.TypedValueChangedFirst(it =>
            {
                var value = it.Value;
                if (value.CompareTo(max) > 0) sync.Value = max;
                else if (value.CompareTo(min) < 0) sync.Value = min;
            });
        }

        internal static void PatchInit<T>(this PatchContext patchContext, Action<T> patch)
        {
            var init = typeof(T).GetMethod(
                "Init",
                typeof(MyObjectBuilder_CubeBlock), typeof(MyCubeGrid)
            );
            patchContext.GetPattern(init).Suffixes.Add(patch.Method);
        }
    }
}