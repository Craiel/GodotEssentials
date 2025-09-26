namespace Craiel.Essentials.DB;

public interface IGameDataId
{
    public GameDataType Type { get; }
    public GameDataIdType IDType { get; }
}