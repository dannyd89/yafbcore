# yafbcore
Yet another flattiverse bot .NET core library from Daniel Draghici

# Requirements
- .NET Standard 2.0
- Visual Studio 2017 or any IDE which supports C# 7.0 syntax

# TODO
Below is a list of current known issues/features who are set to be worked on

## Map.cs
- Need to think about the BeginLock/EndLock semantics.

## MapSection.cs
- enlargeArray() code needs benchmarking with and without Array.Copy
- Sorting of all arrays needs to be done efficiently (What sort criteria? etc.)
-- Still- and AgingUnits done, PlayerUnits still not clear how to sort

## Ship.cs
- Need to think about the move and shoot algorithms, should work with commands which are queueable

# TODO Today
- Continue the Ship class
