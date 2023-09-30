namespace Craiel.Essentials.Runtime.Data.SBT;

public interface ISBTNodeDeserializer
{
    void Deserialize(string data);

    T GetData<T>() where T : ISBTNode;
}