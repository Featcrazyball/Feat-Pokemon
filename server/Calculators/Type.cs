using System;
using System.Collections.Generic;
using System.Linq;

namespace Models;

// This was generated using Co-pilot
public static class TypeChart
{
    // Main look-up table:   attackType  ➜  (defendType ➜ multiplier)
    public static readonly Dictionary<string, Dictionary<string, float>> _chart =
        new(StringComparer.OrdinalIgnoreCase)
    {
        ["Normal"]   = new(StringComparer.OrdinalIgnoreCase)
        { ["Rock"]=0.5f, ["Steel"]=0.5f, ["Ghost"]=0f },

        ["Fire"]     = new(StringComparer.OrdinalIgnoreCase)
        { ["Bug"]=2f, ["Steel"]=2f, ["Grass"]=2f, ["Ice"]=2f,
          ["Rock"]=0.5f, ["Fire"]=0.5f, ["Water"]=0.5f, ["Dragon"]=0.5f },

        ["Water"]    = new(StringComparer.OrdinalIgnoreCase)
        { ["Fire"]=2f, ["Ground"]=2f, ["Rock"]=2f,
          ["Water"]=0.5f, ["Grass"]=0.5f, ["Dragon"]=0.5f },

        ["Electric"] = new(StringComparer.OrdinalIgnoreCase)
        { ["Water"]=2f, ["Flying"]=2f,
          ["Electric"]=0.5f, ["Grass"]=0.5f, ["Dragon"]=0.5f,
          ["Ground"]=0f },

        ["Grass"]    = new(StringComparer.OrdinalIgnoreCase)
        { ["Water"]=2f, ["Ground"]=2f, ["Rock"]=2f,
          ["Fire"]=0.5f, ["Grass"]=0.5f, ["Poison"]=0.5f, ["Flying"]=0.5f,
          ["Bug"]=0.5f, ["Dragon"]=0.5f, ["Steel"]=0.5f },

        ["Ice"]      = new(StringComparer.OrdinalIgnoreCase)
        { ["Grass"]=2f, ["Ground"]=2f, ["Flying"]=2f, ["Dragon"]=2f,
          ["Fire"]=0.5f, ["Water"]=0.5f, ["Ice"]=0.5f, ["Steel"]=0.5f },

        ["Fighting"] = new(StringComparer.OrdinalIgnoreCase)
        { ["Normal"]=2f, ["Rock"]=2f, ["Steel"]=2f, ["Ice"]=2f, ["Dark"]=2f,
          ["Poison"]=0.5f, ["Flying"]=0.5f, ["Psychic"]=0.5f,
          ["Bug"]=0.5f, ["Fairy"]=0.5f, ["Ghost"]=0f },

        ["Poison"]   = new(StringComparer.OrdinalIgnoreCase)
        { ["Grass"]=2f, ["Fairy"]=2f,
          ["Poison"]=0.5f, ["Ground"]=0.5f, ["Rock"]=0.5f, ["Ghost"]=0.5f,
          ["Steel"]=0f },

        ["Ground"]   = new(StringComparer.OrdinalIgnoreCase)
        { ["Fire"]=2f, ["Electric"]=2f, ["Poison"]=2f, ["Rock"]=2f, ["Steel"]=2f,
          ["Grass"]=0.5f, ["Bug"]=0.5f, ["Flying"]=0f },

        ["Flying"]   = new(StringComparer.OrdinalIgnoreCase)
        { ["Grass"]=2f, ["Fighting"]=2f, ["Bug"]=2f,
          ["Electric"]=0.5f, ["Rock"]=0.5f, ["Steel"]=0.5f },

        ["Psychic"]  = new(StringComparer.OrdinalIgnoreCase)
        { ["Fighting"]=2f, ["Poison"]=2f,
          ["Psychic"]=0.5f, ["Steel"]=0.5f, ["Dark"]=0f },

        ["Bug"]      = new(StringComparer.OrdinalIgnoreCase)
        { ["Grass"]=2f, ["Psychic"]=2f, ["Dark"]=2f,
          ["Fire"]=0.5f, ["Fighting"]=0.5f, ["Poison"]=0.5f, ["Flying"]=0.5f,
          ["Ghost"]=0.5f, ["Steel"]=0.5f, ["Fairy"]=0.5f },

        ["Rock"]     = new(StringComparer.OrdinalIgnoreCase)
        { ["Fire"]=2f, ["Ice"]=2f, ["Flying"]=2f, ["Bug"]=2f,
          ["Fighting"]=0.5f, ["Ground"]=0.5f, ["Steel"]=0.5f },

        ["Ghost"]    = new(StringComparer.OrdinalIgnoreCase)
        { ["Psychic"]=2f, ["Ghost"]=2f,
          ["Dark"]=0.5f, ["Normal"]=0f },

        ["Dragon"]   = new(StringComparer.OrdinalIgnoreCase)
        { ["Dragon"]=2f, ["Steel"]=0.5f, ["Fairy"]=0f },

        ["Dark"]     = new(StringComparer.OrdinalIgnoreCase)
        { ["Psychic"]=2f, ["Ghost"]=2f,
          ["Fighting"]=0.5f, ["Dark"]=0.5f, ["Fairy"]=0.5f },

        ["Steel"]    = new(StringComparer.OrdinalIgnoreCase)
        { ["Rock"]=2f, ["Ice"]=2f, ["Fairy"]=2f,
          ["Fire"]=0.5f, ["Water"]=0.5f, ["Electric"]=0.5f, ["Steel"]=0.5f },

        ["Fairy"]    = new(StringComparer.OrdinalIgnoreCase)
        { ["Fighting"]=2f, ["Dragon"]=2f, ["Dark"]=2f,
          ["Fire"]=0.5f, ["Poison"]=0.5f, ["Steel"]=0.5f }
    };
}