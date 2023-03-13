namespace CSFramework.Core
{
    public interface IPresettable
    { 
        public PresettableCategory Category { get; }
    }
    public interface IPresettable<TPreset>: IPresettable
        where TPreset: IPreset
    {
        public TPreset Preset { get; }
    }
}
