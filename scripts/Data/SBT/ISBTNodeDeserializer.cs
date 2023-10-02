namespace Craiel.Essentials.Data.SBT;

public interface ISBTNodeDeserializer
{
    T GetData<T>() where T : ISBTNode;
}