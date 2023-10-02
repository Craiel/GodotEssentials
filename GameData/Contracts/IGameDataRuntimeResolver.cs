﻿namespace Craiel.Essentials.Contracts;

using System.Collections.Generic;

public interface IGameDataRuntimeResolver
{
    GameDataId GetRuntimeId(GameDataRuntimeRefBase runtimeRef);

    T Get<T>(GameDataId dataId);

    bool GetAll<T>(IList<T> target);
}