using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBodyFilter : IComparable
{
    int Precedence { get; }

    void ApplyFilter(Body2d body);
    void ApplyIvertedFilter(Body2d body);
}