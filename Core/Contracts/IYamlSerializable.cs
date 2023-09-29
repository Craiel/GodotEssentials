namespace Craiel.Essentials.Runtime.Contracts;

public interface IYamlSerializable
{
    void Serialize(YamlFluentSerializer serializer);
    void Deserialize(YamlFluentDeserializer deserializer);
}

