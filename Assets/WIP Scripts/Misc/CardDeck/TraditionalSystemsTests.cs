using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



namespace SullysToolkit
{
    /*
    public class Tester
    {
        void Main()
        {
            DieRollerTester.TestDieRoller();
        }
    }

    public static class CardDeckTester
    {
        //...
    }

    public static class DieRollerTester
    {
        //Core Test
        public static void TestDieRoller()
        {
            ProbabilityTest();
        }

        //Tests
        private static void ProbabilityTest()
        {
            //Roll a D20 1000 times
            //If a result of LessThan 1 OR GreaterThan 20 is detected, then the test fails

            string testName = "Probability Test";
            bool isTestPassed= true;
            LogTestStart(testName);

            int[] resultsArray = new int[1000];

            for (int i = 0; i < 1000; i++)
                resultsArray[i] = DieRoller.RollDie(20);

            foreach (int result in resultsArray)
            {
                if (result > 20 || result < 1)
                {
                    isTestPassed = false;
                    break;
                }
            }

            if (isTestPassed) LogTestSuccess(testName);
            else LogTestFailure(testName, "OutOfBounds value detected in sample set");

        }

        //Debug Utils
        private static void LogTestSuccess(string testName)
        {
            Console.WriteLine($"{testName} Passed");
        }

        private static void LogTestFailure(string testname, string failureDesc)
        {
            Console.WriteLine($"{testname} Failed: {failureDesc}");
        }

        private static void LogTestStart(string testName)
        {
            Console.WriteLine($"Entering {testName}");
        }
        
    }
    */
}
