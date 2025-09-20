using System;

public interface IInitializable
{
    InitializeOrder Order { get; }
    void Initialize();
}