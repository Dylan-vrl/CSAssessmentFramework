namespace CSFramework.Core
{
    public interface IPresettable
    {
        public PresettableCategory GetCategory();
    }
    public interface IPresettable<TPreset>: IPresettable
        where TPreset: IPreset
    {
        public TPreset Preset { get; }
    }
}
