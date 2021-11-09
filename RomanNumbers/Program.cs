
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

RomanToArabConverter converter = new RomanToArabConverter();
string number = "IX";
Assert.AreEqual(converter.Convert(number), 9, "Konversion 9 falsch");

number = "MCDXCII";
Assert.AreEqual(converter.Convert(number), 1492, "Konversion 1492 falsch");

ArabToRomanConverter converter2 = new ArabToRomanConverter();
int number2 = 9;
Assert.AreEqual(converter2.Convert(number2), "IX", "Konversion 9 falsch");

number2 = 1492;
Assert.AreEqual(converter2.Convert(number2), "MCDXCII", "Konversion 1492 falsch");

number2 = 1494;
Assert.AreEqual(converter2.Convert(number2), "MCDXCIV", "Konversion 1494 falsch");

number2 = 1498;
Assert.AreEqual(converter2.Convert(number2), "MCDXCVIII", "Konversion 1498 falsch");

number2 = 8;
Assert.AreEqual(converter2.Convert(number2), "VIII", "Konversion 8 falsch");

number2 = 6;
Assert.AreEqual(converter2.Convert(number2), "VI", "Konversion 6 falsch");

number2 = 4;
Assert.AreEqual(converter2.Convert(number2), "IV", "Konversion 5 falsch");

Assert.AreEqual(Helper.IsValidRomanNumber("IX"), true, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("XI"), true, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("IV"), true, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("VIII"), true, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("IIX"), false, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("MCDXCII"), true, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("MCDXCIV"), true, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("MCDXCVIII"), true, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("IC"), false, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("VD"), false, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("XM"), false, "Falsch");
Assert.AreEqual(Helper.IsValidRomanNumber("CCXXVV"), false, "Falsch");

public static class Helper
{
    private static int OccursConnectedTimes(string toCheck, int position)
    {
        int cnt = 0;
        int pos = position;

        char atPosition = toCheck[pos];
        char nextPosition;

        while (pos < toCheck.Length)
        {
            nextPosition = toCheck[pos];
            if (nextPosition != atPosition)
            {
                break;
            }

            cnt++;
            pos++;
        }

        return cnt;
    }

    public static bool IsValidRomanNumber(string toCheck)
    {
        char toRemember = 'M';

        for (int i = 0; i < toCheck.Length; i++)
        {
            char atPosition = toCheck[i];

            // (1) Is this char smaller than the one we remembered?
            if (!IsSmallerOrEqualThan(atPosition, toRemember))
                return false;

            // (2) How many times to the right does this char occur?
            int cnt = OccursConnectedTimes(toCheck, i);

            // (2a) > thanAllowed? -> return false
            if (cnt > HowOftenAllowedInRow(atPosition))
                return false;

            // (2b) == 1 -> 
            if (cnt == 1)
            {
                // (i) is next char larger?
                if (i == toCheck.Length - 1)
                    return true;

                if (!IsSmallerOrEqualThan(atPosition, toCheck[i+1]))
                {
                    // no -> set toRemember to this char
                    toRemember = atPosition;
                }
                else
                {
                    // yes ->
                    //   (ii) is it allowed to be this char?
                    if (!IsCharAllowedBeforeOtherCharForSubtraction(atPosition, toCheck[i + 1]))
                    {
                        // no -> return false
                        return false;
                    }
                    else
                    {
                        // yes ->go to next char of next char, set toRemember to next char
                        toRemember = toCheck[i + 1];
                        i++;
                    }
                }
            }

            // (2c) <= thanAllowed? -> go to last occurence of this char, set toRemember to this char
            if (cnt <= HowOftenAllowedInRow(atPosition) && cnt != 1)
            {
                toRemember = atPosition;
                i += cnt - 1;
            }
        }

        return true;
    }

    public static bool IsCharAllowedBeforeOtherCharForSubtraction(char left, char right)
    {
        if (!IsGrundziffer(left))
        {
            return false;
        }

        switch (left)
        {
            case 'I':
                if (right == 'V' || right == 'X')
                {
                    return true;
                }
                return false;
            case 'X':
                if (right == 'L' || right == 'C')
                {
                    return true;
                }
                return false;
            case 'C':
                if (right == 'D' || right == 'M')
                {
                    return true;
                }
                return false;
            case 'M':
                return false;
            default:
                throw new Exception($"Unknown roman char {left}!");
        }
    }

    public static string BuildStringBeforeMiddleAfter(char beforeOrAfter, char middle, bool isBefore, int howOftenMiddle)
    {
        StringBuilder sb = new ();

        if (isBefore)
        {
            sb.Append(beforeOrAfter);
        }

        for (int i = 0; i < howOftenMiddle; i++)
        {
            sb.Append(middle);
        }

        if (!isBefore)
        {
            sb.Append(beforeOrAfter);
        }

        return sb.ToString();
    }

    public static bool IsGrundziffer(char roman)
    {
        return (roman == 'I' || roman == 'X' || roman == 'C' || roman == 'M');
    }

    public static bool IsZwischenziffer(char roman)
    {
        return (roman == 'V' || roman == 'L' || roman =='D');
    }

    public static bool IsSmallerOrEqualThan(char a, char b)
    {
        switch (a)
        {
            case 'I':
                return true;
            case 'V':
                if (b != 'I')
                    return true;
                return false;
            case 'X':
                if (b != 'I' && b != 'V')
                    return true;
                return false;
            case 'L':
                if (b != 'I' && b != 'V' && b != 'X')
                    return true;
                return false;
            case 'C':
                if (b == 'C' || b == 'D' || b == 'M')
                    return true;
                return false;
            case 'D':
                if (b == 'D' || b == 'M')
                    return true;
                return false;
            case 'M':
                if (b == 'M')
                    return true;
                return false;
            default:
                throw new Exception($"Invalid roman number {a}");
        }
    }

    public static int HowOftenAllowedInRow(char roman)
    {
        switch (roman)
        {
            case 'I':
                return 3;
            case 'V':
                return 1;
            case 'X':
                return 3;
            case 'L':
                return 1;
            case 'C':
                return 3;
            case 'D':
                return 1;
            case 'M':
                return 3;
            default:
                throw new Exception($"Invalid roman char: {roman}");
        }
    }

    public static char GetRomanCharForNumber(int decimalNumber)
    {
        switch (decimalNumber)
        {
            case 1:
                return 'I';
            case 5:
                return 'V';
            case 10:
                return 'X';
            case 50:
                return 'L';
            case 100:
                return 'C';
            case 500:
                return 'D';
            case 1000:
                return 'M';
            default:
                throw new Exception($"Invalid number: {decimalNumber}");
        }
    }

    public static int GetNumberForRomanChar(char roman)
    {
        switch (roman)
        {
            case 'I':
                return 1;
            case 'V':
                return 5;
            case 'X':
                return 10;
            case 'L':
                return 50;
            case 'C':
                return 100;
            case 'D':
                return 500;
            case 'M':
                return 1000;
            default:
                throw new Exception($"Invalid roman char: {roman}");
        }
    }

    public static char GetNextLargerRomanNumber(char roman)
    {
        switch (roman)
        {
            case 'I':
                    return 'V';
            case 'V':
                    return 'X';
            case 'X':
                    return 'L';
            case 'L':
                    return 'C';
            case 'C':
                    return 'D';
            case 'D':
                    return 'M';
            case 'M':
                throw new Exception($"No larger roman char available than: {roman}");
            default:
                throw new Exception($"Invalid roman char: {roman}");
        }
    }
}

public class RomanToArabConverter
{
    private bool IsLeftSmallerOrEqualThanRight(char left, char right)
    {
        return Helper.GetNumberForRomanChar(left) <= Helper.GetNumberForRomanChar(right);
    }


    private bool IsLeftSmallerThanRight(char left, char right)
    {
        return Helper.GetNumberForRomanChar(left) < Helper.GetNumberForRomanChar(right);
    }

    private bool IsApplySubtractionRule(char left, char right)
    {
        if (left == 'I' || left == 'X' || left == 'C')
        {
            if (IsLeftSmallerThanRight(left, right))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsApplyAdditionRule(char left, char right)
    {
        return IsLeftSmallerOrEqualThanRight(right, left);
    }

    public int Convert(string romanNumber)
    {
        var sum = 0;

        for (int i = 0; i < romanNumber.Length - 1; i++)
        {
            var place = romanNumber[i];
            var rightToPlace = romanNumber[i + 1];

            var isApplySubtraction = IsApplySubtractionRule(place, rightToPlace);
            var isApplyAddition = IsApplyAdditionRule(place, rightToPlace);

            var valuePlace = Helper.GetNumberForRomanChar(place);
            var valueRightToPlace = Helper.GetNumberForRomanChar(rightToPlace);

            if (isApplyAddition)
            {
                sum += valuePlace;
            }
            else
            {
                if (isApplySubtraction)
                {
                    sum -= valuePlace;
                }
                else
                {
                    throw new Exception($"Keine passende Regel gefunden fuer {romanNumber}, i={i} i+1={i + 1}");
                }
            }

            if (i == romanNumber.Length - 2)
            {
                sum += valueRightToPlace;
            }
        }

        return sum;
    }
}

public class ArabToRomanConverter
{
    private int GetLargestDivisor(int number)
    {
        var divisor = 1000;
        int result, remainder;

        while (divisor > 0)
        {
            result = number / divisor;
            remainder = number % divisor;

            if (result >= 1)
                break;

            divisor /= 10;
        }

        return divisor;
    }

    private bool IsSubtraction(int nextLargerDivisor, int number)
    {
        bool isSubtraction = true;
        if (nextLargerDivisor <= number)
        {
            isSubtraction = false;
        }

        return isSubtraction;
    }

    private int HowOftenUseSmallerNumber(int largerDivisor, int divisor, int howOftenFitsDivisor)
    {
        return Math.Abs((largerDivisor - divisor * howOftenFitsDivisor) / divisor);
    }

    private string GetRomanNumberPartMixedLettersNecessary(char romanLetterForDivisor, int divisor, int howOftenFitsDivisor, int howOftenAllowedDivisor, int number)
    {
        char nextLargerRomanNumber = Helper.GetNextLargerRomanNumber(romanLetterForDivisor);
        int nextLargerDivisor = Helper.GetNumberForRomanChar(nextLargerRomanNumber);

        int howOftenSmallerRomanNumber = HowOftenUseSmallerNumber(nextLargerDivisor, divisor, howOftenFitsDivisor);
        if (howOftenSmallerRomanNumber <= howOftenAllowedDivisor)
        {
            // Addition or Subtraction?
            bool isSubtraction = IsSubtraction(nextLargerDivisor, number);
            return Helper.BuildStringBeforeMiddleAfter(nextLargerRomanNumber, romanLetterForDivisor, !isSubtraction, howOftenSmallerRomanNumber);
        }
        else
        {
            char nextNextLargerRomanNumber = Helper.GetNextLargerRomanNumber(nextLargerRomanNumber);
            int nextNextLargerDivisor = Helper.GetNumberForRomanChar(nextNextLargerRomanNumber);

            howOftenSmallerRomanNumber = HowOftenUseSmallerNumber(nextNextLargerDivisor, divisor, howOftenFitsDivisor);

            return Helper.BuildStringBeforeMiddleAfter(nextNextLargerRomanNumber, romanLetterForDivisor, isBefore: false, howOftenSmallerRomanNumber);
        }
    }

    private string GetRomanNumberPart(int divisor, int howOftenFitsDivisor, int number)
    {
        StringBuilder sb = new();

        char romanLetterForDivisor = Helper.GetRomanCharForNumber(divisor);

        // check the rules for the letter: how often is it allowed to appear?
        int howOftenAllowed = Helper.HowOftenAllowedInRow(romanLetterForDivisor);
        if (howOftenFitsDivisor <= howOftenAllowed)
        {
            for (int i = 0; i < howOftenFitsDivisor; i++)
            {
                sb.Append(romanLetterForDivisor);
            }
        }
        else
        {
            // a mix of letters is necessary, either use a subtraction or an addition
            sb.Append(GetRomanNumberPartMixedLettersNecessary(romanLetterForDivisor, divisor, howOftenFitsDivisor, howOftenAllowed, number));              
        }

        return sb.ToString();
    }

    private (string, int) GetRomanNumberPartAndRemainder(int number)
    {
        int largestDivisor = GetLargestDivisor(number);
        int howOftenFitsDivisor = number / largestDivisor;
        int remainder = number % largestDivisor;

        string romanNumber = GetRomanNumberPart(largestDivisor, howOftenFitsDivisor, number);

        return (romanNumber, remainder);
    }

    public string Convert(int decimalNumber)
    {
        StringBuilder sb = new ();

        int tmp = decimalNumber;
        while (tmp > 0)
        {
            (string, int) nextPart = GetRomanNumberPartAndRemainder(tmp);
            sb.Append(nextPart.Item1);
            tmp = nextPart.Item2;
        }

        return sb.ToString();
    }
}

