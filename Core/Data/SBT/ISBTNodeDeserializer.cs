namespace Craiel.Essentials.Runtime.Data.SBT;

public interface ISBTNodeDeserializer
{
    T GetData<T>() where T : ISBTNode;
}