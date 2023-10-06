namespace Craiel.Essentials.Contracts;

using System.Collections.Generic;
using GameData;

public interface IGameDataResolver
{
    GameDataId GetId(GameDataRefBase runtimeRef);

    T Get<T>(GameDataId dataId);

    bool GetAll<T>(IList<T> target);
}