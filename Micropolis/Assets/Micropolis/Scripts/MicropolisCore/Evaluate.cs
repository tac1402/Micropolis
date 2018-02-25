﻿using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Evaluate city
        /// TODO Handle lack of voting explicitly
        /// </summary>
        public void cityEvaluation()
        {
            if (totalPop > 0)
            {
                // TODO
                getAssessedValue();
                doPopNum();
                // doProblems
                // getScore
                doVotes(); // How well is the mayor doing?
                changeEval();
            }
            else
            {
                evalInit();
                cityYes = 50; // No population => no voting. Let's say 50/50
                changeEval();
            }
        }

        /// <summary>
        /// Initialize evaluation variables
        /// </summary>
        public void evalInit()
        {
            cityYes = 0;
            cityPop = 0;
            cityPopDelta = 0;
            cityAssessedValue = 0;
            cityClass = CityClass.CC_VILLAGE;
            cityScore = 500;
            cityScoreDelta = 0;
            // TOOD problemVotes
            // TODO problemOrder
        }

        /// <summary>
        /// Assess value of the city.
        /// TODO Make function return the value, or change the name of the function.
        /// </summary>
        public void getAssessedValue()
        {
            long z;

            z = roadTotal * 5;
            z += railTotal * 10;
            z += policeStationPop * 1000;
            z += fireStationPop * 1000;
            z += hospitalPop * 400;
            z += stadiumPop * 3000;
            z += seaportPop * 5000;
            z += airportPop * 10000;
            z += coalPowerPop * 3000;
            z += nuclearPowerPop * 6000;

            cityAssessedValue = z * 1000;
        }

        /// <summary>
        /// Compute city population and city classification.
        /// </summary>
        public void doPopNum()
        {
            long oldCityPop = cityPop;

            cityPop = getPopulation();

            if (oldCityPop == -1)
            {
                oldCityPop = cityPop;
            }

            cityPopDelta = cityPop - oldCityPop;
            cityClass = getCityClass(cityPop);
        }

        /// <summary>
        /// Compute city population.
        /// </summary>
        /// <returns></returns>
        public long getPopulation()
        {
            long pop = (resPop + (comPop + indPop) * 8L) * 20L;
            return pop;
        }

        /// <summary>
        /// Compute average traffic in the city.
        /// </summary>
        /// <returns>Value representing how large the traffic problem is.</returns>
        public short getTrafficAverage()
        {
            // TODO
            return 0;
        }

        /// <summary>
        /// Compute severity of unemployment
        /// </summary>
        /// <returns>Value representing the severity of unemployment problems</returns>
        public short getUnemployment()
        {
            short b = (short) ((comPop + indPop) * 8);

            if (b == 0)
            {
                return 0;
            }

            // Ratio total people / working. At least 1.
            float r = ((float)resPop) / b;

            b = (short)((r - 1) * 255); // (r - 1) is the fraction unemployed people
            return Math.Min(b, (short)255);
        }

        /// <summary>
        /// Compute severity of fire
        /// </summary>
        /// <returns>Value representing the severity of fire problems</returns>
        public short getFireSeverity()
        {
            return (short) Math.Min(firePop * 5, 255);
        }

        /// <summary>
        /// Vote whether the mayor is doing a good job
        /// </summary>
        public void doVotes()
        {
            int z;

            cityYes = 0;

            for (z = 0; z < 100; z++)
            {
                if (getRandom(1000) < cityScore)
                {
                    cityYes++;
                }
            }
        }

        /// <summary>
        /// Push new score to the user
        /// </summary>
        public void doScoreCard()
        {
            callback("update", "s", "evaluation");
        }

        /// <summary>
        /// Request that new score is displayed to the user.
        /// </summary>
        public void changeEval()
        {
            evalChanged = true;
        }

        /// <summary>
        /// Update the score after being requested.
        /// </summary>
        public void scoreDoer()
        {
            if (evalChanged)
            {
                doScoreCard();
                evalChanged = false;
            }
        }

        /// <summary>
        /// Return number of problem in the city.
        /// </summary>
        /// <returns>Number of problems.</returns>
        public int countProblems()
        {
            // TODO
            return 0;
        }

        /// <summary>
        /// Return the index of the i-th worst problem.
        /// </summary>
        /// <param name="i">Number of the problem.</param>
        /// <returns>
        /// Index into the #problemOrder table of the i-th problem. 
        /// Returns -1 if such a problem does not exist.
        /// </returns>
        public int getProblemNumber(int i)
        {
            // TODO
            return 0;
        }

        /// <summary>
        /// Return number of votes to solve the i-th worst problem.
        /// </summary>
        /// <param name="i">Number of the problem.</param>
        /// <returns>
        /// Number of votes to solve the i-th worst problem.
        /// Returns -1 if such a problem does not exist.
        /// </returns>
        public int getProblemVotes(int i)
        {
            // TODO
            return 0;
        }

        /// <summary>
        /// Classify the city based on its population.
        /// </summary>
        /// <param name="cityPopulation">Number of people in the city.</param>
        /// <returns>City classification.</returns>
        private CityClass getCityClass(long cityPopulation)
        {
            CityClass cityClassification = CityClass.CC_VILLAGE;

            if (cityPopulation > 2000)
            {
                cityClassification = CityClass.CC_TOWN;
            }
            if (cityPopulation > 10000)
            {
                cityClassification = CityClass.CC_CITY;
            }
            if (cityPopulation > 50000)
            {
                cityClassification = CityClass.CC_CAPITAL;
            }
            if (cityPopulation > 100000)
            {
                cityClassification = CityClass.CC_METROPOLIS;
            }
            if (cityPopulation > 500000)
            {
                cityClassification = CityClass.CC_MEGALOPOLIS;
            }

            return cityClassification;
        }
    }
}
