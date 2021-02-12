namespace Game.Scripts
{
    public interface IWeapon
    {
        bool IsSelected { get; }
        bool IsInUse { get; }
        void SetSelectedState(bool value);
        void Use();
    }
}
