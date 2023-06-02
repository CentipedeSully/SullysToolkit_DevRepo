using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SullysToolkit
{
    public static class DieRoller
    {
        //Static Commands
        public static int RollDie(int numberOfSides)
        {
            if (numberOfSides > 0)
                return Random.Range(1, numberOfSides + 1);
            else
            {
                Debug.LogWarning($"Warning: invalid die size {numberOfSides} provided to DieRoller. Returning 1");
                return 1;
            }
        }
    }
}

