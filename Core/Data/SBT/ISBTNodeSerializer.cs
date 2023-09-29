namespace Craiel.Essentials.Runtime.Data.SBT;

public interface ISBTNodeSerializer
{
    void Serialize(ISBTNode node);
    
    string GetData();
}
