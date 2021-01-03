namespace DataValidateFix
{
    internal delegate void ActionRef<T>(ref T item);

    internal delegate bool ActionRefBool<T>(ref T value);
}