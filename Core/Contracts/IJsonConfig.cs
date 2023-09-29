namespace Craiel.Essentials.Runtime.Contracts;

public interface IJsonConfig<T>
{
    T Current { get; set; }

    bool Load(ManagedFile file);
    bool Save(ManagedFile file = null);

    void Reset();
}

