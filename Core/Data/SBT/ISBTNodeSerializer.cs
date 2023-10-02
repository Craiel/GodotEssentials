namespace Craiel.Essentials.Data.SBT;

public interface ISBTNodeSerializer
{
    void Serialize(ISBTNode node);
    
    string GetData();
}
