using System;

namespace NetPOC.Backend.Domain.Enums
{
    [Flags]
    public enum FlagEscolaridade
    {
        Infantil = 1,
        Fundamental = 2,
        Médio = 3,
        Superior = 4
    }
}