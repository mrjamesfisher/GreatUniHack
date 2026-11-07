
using System;
using System.Collections.Generic;

namespace LifeFlip;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("================================");
        Console.WriteLine("            LIFEFLIP");
        Console.WriteLine("   Plan your money. Flip your future.");
        Console.WriteLine("================================\n");

        // USER DETAILS
        Console.Write("Enter your name: ");
        string name = Console.ReadLine() ?? "User";

        Console.Write("Enter your occupation: ");
        string occupation = Console.ReadLine() ?? "Not specified";

        // FINANCIAL INFORMATION
        // Salaried users enter their usual monthly take-home pay.
        // Non-salaried users enter their earnings for the last 12 months.
        Console.Write("\nAre you salaried? (Y/N): ");
        string salariedChoice = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

        while (salariedChoice != "Y" && salariedChoice != "N")
        {
            Console.Write("Please enter Y for salaried or N for not salaried: ");
            salariedChoice = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();
        }

        decimal income;

        if (salariedChoice == "Y")
        {
            Console.Write("\nEnter your monthly take-home salary (£): ");
            income = ReadAmount();
        }
        else
        {
            income = GetVariableMonthlyIncome();
        }

        Console.WriteLine($"\nMonthly income used by LIFEFLIP: £{income:F2}");

        Console.Write("Enter current general savings (£): ");
        decimal currentSavings = ReadAmount();

        Console.Write("Enter monthly bills and essentials (£): ");
        decimal bills = ReadAmount();

        if (bills > income)
        {
            Console.WriteLine("\nYour bills exceed your monthly income.");
            Console.WriteLine("You need to review your expenses before creating a savings plan.");
            return;
        }

        // BUDGET CALCULATIONS
        decimal remainingIncome = income - bills;

        // Suggested starting allocation
        decimal savingsPercentage = 40;
        decimal goalsPercentage = 30;
        decimal spendingPercentage = 30;

        decimal monthlySavings =
            remainingIncome * savingsPercentage / 100;

        decimal monthlyGoalBudget =
            remainingIncome * goalsPercentage / 100;

        decimal personalSpending =
            remainingIncome * spendingPercentage / 100;

        // LISTS TO STORE MULTIPLE GOALS
        List<string> goalNames = new List<string>();
        List<decimal> goalCosts = new List<decimal>();
        List<decimal> goalSavings = new List<decimal>();
        List<int> goalDeadlines = new List<int>();
        List<decimal> goalMonthlyRequirements = new List<decimal>();

        decimal totalMonthlyGoalRequirements = 0;

        // MAIN MENU
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n================================");
            Console.WriteLine("          LIFEFLIP MENU");
            Console.WriteLine("================================");
            Console.WriteLine("1. Add a future goal");
            Console.WriteLine("2. Save for a period of time");
            Console.WriteLine("3. Finish and view summary");
            Console.WriteLine("================================");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddGoal(
                        goalNames,
                        goalCosts,
                        goalSavings,
                        goalDeadlines,
                        goalMonthlyRequirements,
                        ref totalMonthlyGoalRequirements,
                        remainingIncome,
                        ref monthlySavings,
                        ref monthlyGoalBudget,
                        ref personalSpending,
                        ref savingsPercentage,
                        ref goalsPercentage,
                        ref spendingPercentage
                    );
                    break;

                case "2":
                    SaveForPeriod(
                        currentSavings,
                        monthlySavings,
                        monthlyGoalBudget,
                        totalMonthlyGoalRequirements
                    );
                    break;

                case "3":
                    DisplaySummary(
                        name,
                        occupation,
                        income,
                        bills,
                        remainingIncome,
                        currentSavings,
                        monthlySavings,
                        monthlyGoalBudget,
                        personalSpending,
                        savingsPercentage,
                        goalsPercentage,
                        spendingPercentage,
                        goalNames,
                        goalCosts,
                        goalSavings,
                        goalDeadlines,
                        goalMonthlyRequirements,
                        totalMonthlyGoalRequirements
                    );

                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please choose 1, 2 or 3.");
                    break;
            }
        }
    }

    // CASE 1: ADD A FUTURE GOAL
    static void AddGoal(
        List<string> goalNames,
        List<decimal> goalCosts,
        List<decimal> goalSavings,
        List<int> goalDeadlines,
        List<decimal> goalMonthlyRequirements,
        ref decimal totalMonthlyGoalRequirements,
        decimal remainingIncome,
        ref decimal monthlySavings,
        ref decimal monthlyGoalBudget,
        ref decimal personalSpending,
        ref decimal savingsPercentage,
        ref decimal goalsPercentage,
        ref decimal spendingPercentage)
    {
        Console.WriteLine("\n--- Create a Future Goal ---");

        Console.Write("What are you saving for? ");
        string goalName = Console.ReadLine() ?? "My Goal";

        while (string.IsNullOrWhiteSpace(goalName))
        {
            Console.Write("Please enter a goal name: ");
            goalName = Console.ReadLine() ?? "My Goal";
        }

        Console.Write("How much will it cost (£)? ");
        decimal goalCost = ReadAmount();

        while (goalCost <= 0)
        {
            Console.Write("The cost must be greater than £0. Enter again: ");
            goalCost = ReadAmount();
        }

        Console.Write("How much have you already saved towards it (£)? ");
        decimal goalSaved = ReadAmount();

        while (goalSaved > goalCost)
        {
            Console.WriteLine("Savings cannot exceed the goal cost.");
            Console.Write("Enter the amount saved again (£): ");
            goalSaved = ReadAmount();
        }

        Console.Write("How many months until your deadline? ");
        int deadline = ReadPositiveInt();

        // CALCULATE GOAL REQUIREMENTS
        decimal remainingGoalAmount = goalCost - goalSaved;

        decimal requiredMonthlySaving =
            remainingGoalAmount / deadline;

        // STORE GOAL
        goalNames.Add(goalName);
        goalCosts.Add(goalCost);
        goalSavings.Add(goalSaved);
        goalDeadlines.Add(deadline);
        goalMonthlyRequirements.Add(requiredMonthlySaving);

        totalMonthlyGoalRequirements += requiredMonthlySaving;

        // DISPLAY GOAL RESULTS
        Console.WriteLine("\n================================");
        Console.WriteLine("       FUTURE GOAL RESULTS");
        Console.WriteLine("================================");

        Console.WriteLine($"Goal: {goalName}");
        Console.WriteLine($"Target cost: £{goalCost:F2}");
        Console.WriteLine($"Already saved: £{goalSaved:F2}");
        Console.WriteLine($"Remaining amount: £{remainingGoalAmount:F2}");
        Console.WriteLine($"Deadline: {deadline} months");
        Console.WriteLine($"Required monthly saving: £{requiredMonthlySaving:F2}");

        Console.WriteLine($"\nTotal required for all goals: £{totalMonthlyGoalRequirements:F2}");
        Console.WriteLine($"Available goal budget: £{monthlyGoalBudget:F2}");

        // CHECK AFFORDABILITY
        while (totalMonthlyGoalRequirements > monthlyGoalBudget)
        {
            decimal extraGoalMoneyNeeded =
                totalMonthlyGoalRequirements - monthlyGoalBudget;

            Console.WriteLine("\nYour combined goals exceed your current allocation.");

            Console.WriteLine(
                $"Additional monthly goal funding needed: £{extraGoalMoneyNeeded:F2}");

            if (goalsPercentage > 0)
            {
                decimal extraIncomeNeeded =
                    extraGoalMoneyNeeded / (goalsPercentage / 100);

                Console.WriteLine(
                    $"Estimated additional monthly income needed: £{extraIncomeNeeded:F2}");
            }
            else
            {
                Console.WriteLine(
                    "You currently allocate 0% of your income to future goals.");
            }

            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. Change my budget allocation");
            Console.WriteLine("2. Extend my savings duration");
            Console.WriteLine("3. Delete or postpone a goal");
            Console.WriteLine("4. Keep my goals as they are");

            Console.Write("\nChoose an option: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                // OPTION 1: CHANGE BUDGET ALLOCATION
                case "1":
                    Console.WriteLine("\n--- Change Budget Allocation ---");

                    Console.WriteLine(
                        $"Current savings allocation: {savingsPercentage}%");

                    Console.WriteLine(
                        $"Current goals allocation: {goalsPercentage}%");

                    Console.WriteLine(
                        $"Current personal spending allocation: {spendingPercentage}%");

                    Console.Write("\nWhat percentage would you like to allocate to goals? ");

                    decimal newGoalsPercentage = ReadPercentage();

                    decimal remainingPercentage = 100 - newGoalsPercentage;

                    // Keep the previous savings-to-spending balance
                    decimal previousSavingsAndSpending =
                        savingsPercentage + spendingPercentage;

                    goalsPercentage = newGoalsPercentage;

                    if (previousSavingsAndSpending > 0)
                    {
                        savingsPercentage =
                            remainingPercentage *
                            savingsPercentage /
                            previousSavingsAndSpending;

                        spendingPercentage =
                            remainingPercentage - savingsPercentage;
                    }
                    else
                    {
                        savingsPercentage = remainingPercentage / 2;
                        spendingPercentage = remainingPercentage - savingsPercentage;
                    }

                    // RECALCULATE MONTHLY ALLOCATIONS
                    monthlySavings =
                        remainingIncome * savingsPercentage / 100;

                    monthlyGoalBudget =
                        remainingIncome * goalsPercentage / 100;

                    personalSpending =
                        remainingIncome * spendingPercentage / 100;

                    Console.WriteLine("\nYour budget has been updated.");

                    Console.WriteLine(
                        $"General savings: {savingsPercentage:F1}% (£{monthlySavings:F2})");

                    Console.WriteLine(
                        $"Future goals: {goalsPercentage:F1}% (£{monthlyGoalBudget:F2})");

                    Console.WriteLine(
                        $"Personal spending: {spendingPercentage:F1}% (£{personalSpending:F2})");

                    if (totalMonthlyGoalRequirements <= monthlyGoalBudget)
                    {
                        Console.WriteLine("\nYour goals are now affordable!");
                    }
                    else
                    {
                        Console.WriteLine("\nYour goals still exceed your budget.");
                    }

                    break;

                // OPTION 2: EXTEND SAVINGS DURATION
                case "2":
                    if (goalNames.Count == 0)
                    {
                        Console.WriteLine("You have no goals to extend.");
                        break;
                    }

                    Console.WriteLine("\n--- Your Current Goals ---");

                    for (int i = 0; i < goalNames.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {goalNames[i]}");
                    }

                    Console.Write("\nEnter the number of the goal to extend: ");
                    int goalNumber = ReadPositiveInt();

                    while (goalNumber > goalNames.Count)
                    {
                        Console.Write("Invalid goal number. Try again: ");
                        goalNumber = ReadPositiveInt();
                    }

                    int index = goalNumber - 1;

                    Console.Write("Enter the new deadline in months: ");
                    int newDeadline = ReadPositiveInt();

                    goalDeadlines[index] = newDeadline;

                    decimal newRemainingAmount =
                        goalCosts[index] - goalSavings[index];

                    decimal newMonthlySaving =
                        newRemainingAmount / newDeadline;

                    // UPDATE TOTAL MONTHLY REQUIREMENTS
                    totalMonthlyGoalRequirements -=
                        goalMonthlyRequirements[index];

                    goalMonthlyRequirements[index] = newMonthlySaving;

                    totalMonthlyGoalRequirements += newMonthlySaving;

                    Console.WriteLine(
                        $"\nYour new monthly saving for {goalNames[index]} is £{newMonthlySaving:F2}");

                    if (totalMonthlyGoalRequirements <= monthlyGoalBudget)
                    {
                        Console.WriteLine("Your goals are now affordable!");
                    }
                    else
                    {
                        Console.WriteLine("Your goals still exceed your budget.");
                    }

                    break;

                // OPTION 3: DELETE OR POSTPONE A GOAL
                case "3":
                    if (goalNames.Count == 0)
                    {
                        Console.WriteLine("You have no goals to delete or postpone.");
                        break;
                    }

                    Console.WriteLine("\n--- Your Current Goals ---");

                    for (int i = 0; i < goalNames.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {goalNames[i]}");
                    }

                    Console.Write("\nEnter the number of the goal: ");
                    int selectedNumber = ReadPositiveInt();

                    while (selectedNumber > goalNames.Count)
                    {
                        Console.Write("Invalid goal number. Try again: ");
                        selectedNumber = ReadPositiveInt();
                    }

                    int selectedIndex = selectedNumber - 1;

                    Console.WriteLine("\n1. Delete goal");
                    Console.WriteLine("2. Postpone goal");
                    Console.Write("Choose an option: ");

                    string deleteOrPostpone = Console.ReadLine() ?? "";

                    if (deleteOrPostpone == "1")
                    {
                        totalMonthlyGoalRequirements -=
                            goalMonthlyRequirements[selectedIndex];

                        goalNames.RemoveAt(selectedIndex);
                        goalCosts.RemoveAt(selectedIndex);
                        goalSavings.RemoveAt(selectedIndex);
                        goalDeadlines.RemoveAt(selectedIndex);
                        goalMonthlyRequirements.RemoveAt(selectedIndex);

                        Console.WriteLine("Goal successfully deleted.");
                    }
                    else if (deleteOrPostpone == "2")
                    {
                        totalMonthlyGoalRequirements -=
                            goalMonthlyRequirements[selectedIndex];

                        goalDeadlines[selectedIndex] = 0;
                        goalMonthlyRequirements[selectedIndex] = 0;

                        Console.WriteLine(
                            "Goal postponed. It will no longer count towards your monthly goal budget.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid option.");
                    }

                    break;

                // OPTION 4: KEEP GOALS UNCHANGED
                case "4":
                    Console.WriteLine(
                        "Your goals have been kept unchanged.");

                    return;

                default:
                    Console.WriteLine(
                        "Invalid option. Please choose 1, 2, 3 or 4.");

                    break;
            }
        }

        if (totalMonthlyGoalRequirements <= monthlyGoalBudget)
        {
            Console.WriteLine(
                "\nYour current goals fit within your monthly goal budget!");
        }
    }

    // CASE 2: SAVE FOR A PERIOD OF TIME
    static void SaveForPeriod(
        decimal currentSavings,
        decimal monthlySavings,
        decimal monthlyGoalBudget,
        decimal totalMonthlyGoalRequirements)
    {
        Console.WriteLine("\n--- Save for a Period of Time ---");

        Console.Write("How many months would you like to save for? ");
        int savingPeriod = ReadPositiveInt();

        decimal unallocatedGoalBudget =
            Math.Max(0, monthlyGoalBudget - totalMonthlyGoalRequirements);

        decimal projectedGeneralSavings =
            currentSavings + monthlySavings * savingPeriod;

        decimal projectedExtraGoalSavings =
            unallocatedGoalBudget * savingPeriod;

        Console.WriteLine("\n================================");
        Console.WriteLine("       YOUR SAVINGS FORECAST");
        Console.WriteLine("================================");

        Console.WriteLine($"Saving period: {savingPeriod} months");

        Console.WriteLine(
            $"Starting general savings: £{currentSavings:F2}");

        Console.WriteLine(
            $"Monthly general savings: £{monthlySavings:F2}");

        Console.WriteLine(
            $"Projected general savings: £{projectedGeneralSavings:F2}");

        Console.WriteLine("--------------------------------");

        Console.WriteLine(
            $"Monthly future-goal budget: £{monthlyGoalBudget:F2}");

        Console.WriteLine(
            $"Already committed to goals: £{totalMonthlyGoalRequirements:F2}");

        Console.WriteLine(
            $"Unallocated goal budget: £{unallocatedGoalBudget:F2}");

        Console.WriteLine(
            $"Unallocated goal savings over {savingPeriod} months: £{projectedExtraGoalSavings:F2}");

        Console.WriteLine("--------------------------------");

        Console.WriteLine(
            $"Projected general savings plus unallocated goal money: £{projectedGeneralSavings + projectedExtraGoalSavings:F2}");

        Console.WriteLine(
            "\nThis forecast assumes your income and expenses remain unchanged.");

        Console.WriteLine(
            "It does not include money already committed to individual goals.");
    }

    // CASE 3: DISPLAY FINAL SUMMARY
    static void DisplaySummary(
        string name,
        string occupation,
        decimal income,
        decimal bills,
        decimal remainingIncome,
        decimal currentSavings,
        decimal monthlySavings,
        decimal monthlyGoalBudget,
        decimal personalSpending,
        decimal savingsPercentage,
        decimal goalsPercentage,
        decimal spendingPercentage,
        List<string> goalNames,
        List<decimal> goalCosts,
        List<decimal> goalSavings,
        List<int> goalDeadlines,
        List<decimal> goalMonthlyRequirements,
        decimal totalMonthlyGoalRequirements)
    {
        Console.WriteLine("\n================================");
        Console.WriteLine("       YOUR LIFEFLIP SUMMARY");
        Console.WriteLine("================================");

        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Occupation: {occupation}");
        Console.WriteLine($"Monthly income: £{income:F2}");
        Console.WriteLine($"Monthly bills: £{bills:F2}");
        Console.WriteLine($"Remaining income: £{remainingIncome:F2}");

        Console.WriteLine("\n--- Monthly Allocation ---");

        Console.WriteLine(
            $"General savings ({savingsPercentage:F1}%): £{monthlySavings:F2}");

        Console.WriteLine(
            $"Future goals ({goalsPercentage:F1}%): £{monthlyGoalBudget:F2}");

        Console.WriteLine(
            $"Personal spending ({spendingPercentage:F1}%): £{personalSpending:F2}");

        Console.WriteLine("\n--- Your Goals ---");

        if (goalNames.Count == 0)
        {
            Console.WriteLine("You have not added any future goals.");
        }
        else
        {
            for (int i = 0; i < goalNames.Count; i++)
            {
                Console.WriteLine($"\nGoal: {goalNames[i]}");
                Console.WriteLine($"Cost: £{goalCosts[i]:F2}");
                Console.WriteLine($"Already saved: £{goalSavings[i]:F2}");

                if (goalDeadlines[i] == 0)
                {
                    Console.WriteLine("Status: Postponed");
                }
                else
                {
                    Console.WriteLine($"Deadline: {goalDeadlines[i]} months");

                    Console.WriteLine(
                        $"Required per month: £{goalMonthlyRequirements[i]:F2}");

                    if (goalSavings[i] >= goalCosts[i])
                    {
                        Console.WriteLine("Status: Goal reached!");
                    }
                    else
                    {
                        Console.WriteLine("Status: Active");
                    }
                }
            }

            Console.WriteLine(
                $"\nTotal monthly goal savings required: £{totalMonthlyGoalRequirements:F2}");

            if (totalMonthlyGoalRequirements <= monthlyGoalBudget)
            {
                Console.WriteLine(
                    "All active goals are affordable within your allocation.");
            }
            else
            {
                Console.WriteLine(
                    "Your active goals exceed your current allocation.");
            }
        }

        Console.WriteLine("\n================================");
        Console.WriteLine("       THANK YOU FOR USING");
        Console.WriteLine("            LIFEFLIP");
        Console.WriteLine("================================");
    }

    // VALIDATE MONEY INPUT
    static decimal ReadAmount()
    {
        decimal amount;

        while (!decimal.TryParse(Console.ReadLine(), out amount) ||
               amount < 0)
        {
            Console.Write("Enter a valid amount (£0 or more): ");
        }

        return amount;
    }

    // VALIDATE POSITIVE MONTH INPUT
    static int ReadPositiveInt()
    {
        int months;

        while (!int.TryParse(Console.ReadLine(), out months) ||
               months <= 0)
        {
            Console.Write("Enter a valid whole number greater than 0: ");
        }

        return months;
    }

    // VALIDATE PERCENTAGE INPUT
    static decimal ReadPercentage()
    {
        decimal percentage;

        while (!decimal.TryParse(Console.ReadLine(), out percentage) ||
               percentage < 0 ||
               percentage > 100)
        {
            Console.Write("Enter a percentage between 0 and 100: ");
        }

        return percentage;
    }

    // CALCULATE INCOME FOR NON-SALARIED USERS
    // Only the average is shown to the user. Other statistics are calculated
    // internally and can support future income-volatility features.
    static decimal GetVariableMonthlyIncome()
    {
        Console.WriteLine("\n--- Your Earnings Over the Last 12 Months ---");
        Console.WriteLine("Enter your total take-home earnings for each month.");

        List<decimal> monthlyEarnings = new List<decimal>();
        decimal totalIncome = 0;

        for (int month = 1; month <= 12; month++)
        {
            Console.Write($"Month {month} earnings (£): ");
            decimal earnings = ReadAmount();

            monthlyEarnings.Add(earnings);
            totalIncome += earnings;
        }

        // AVERAGE (MEAN): this is the income figure used by LIFEFLIP.
        decimal averageIncome = totalIncome / monthlyEarnings.Count;

        // SORTED VALUES: used for median, range, minimum and maximum.
        List<decimal> sortedEarnings = new List<decimal>(monthlyEarnings);
        sortedEarnings.Sort();

        // MEDIAN
        decimal medianIncome =
            (sortedEarnings[5] + sortedEarnings[6]) / 2;

        // MODE: there may be more than one mode, or no repeated mode.
        Dictionary<decimal, int> frequencies =
            new Dictionary<decimal, int>();

        foreach (decimal amount in monthlyEarnings)
        {
            if (frequencies.ContainsKey(amount))
            {
                frequencies[amount]++;
            }
            else
            {
                frequencies[amount] = 1;
            }
        }

        int highestFrequency = 0;

        foreach (int frequency in frequencies.Values)
        {
            if (frequency > highestFrequency)
            {
                highestFrequency = frequency;
            }
        }

        List<decimal> modeValues = new List<decimal>();

        if (highestFrequency > 1)
        {
            foreach (KeyValuePair<decimal, int> item in frequencies)
            {
                if (item.Value == highestFrequency)
                {
                    modeValues.Add(item.Key);
                }
            }
        }

        // RANGE, LOWEST AND HIGHEST MONTHLY EARNINGS
        decimal lowestIncome = sortedEarnings[0];
        decimal highestIncome = sortedEarnings[sortedEarnings.Count - 1];
        decimal incomeRange = highestIncome - lowestIncome;

        // POPULATION VARIANCE AND STANDARD DEVIATION
        decimal squaredDifferencesTotal = 0;

        foreach (decimal amount in monthlyEarnings)
        {
            decimal difference = amount - averageIncome;
            squaredDifferencesTotal += difference * difference;
        }

        decimal variance = squaredDifferencesTotal / monthlyEarnings.Count;
        decimal standardDeviation =
            (decimal)Math.Sqrt((double)variance);

        // Keep these statistics internal; do not print them to the user.
        // They are retained here for future LIFEFLIP analysis.
        _ = medianIncome;
        _ = modeValues;
        _ = incomeRange;
        _ = lowestIncome;
        _ = highestIncome;
        _ = variance;
        _ = standardDeviation;

        Console.WriteLine("\n================================");
        Console.WriteLine("       YOUR INCOME RESULTS");
        Console.WriteLine("================================");
        Console.WriteLine($"Your average monthly income is: £{averageIncome:F2}");
        Console.WriteLine("LIFEFLIP will use this average to create your budget and savings plan.");

        return averageIncome;
    }
}


