using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Challenge
{
    int X { get; }
    int Current { get; }
    string Name { get; }
    int Score { get; }
}
