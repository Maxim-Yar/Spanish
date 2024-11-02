using System.Globalization;
using Humanizer;

namespace Spanish;

internal class Spanish
{
    private static void Main()
    {
        int number = GetNumber();
        string gender = GetGender();
        var genderType = gender == "m" ? GrammaticalGender.Masculine : GrammaticalGender.Feminine;

        Console.WriteLine($"Program version: {GetSpanishNameOfNumber(number, gender)}");
        Console.WriteLine($"Humanizer version: {number.ToWords(genderType, new CultureInfo("es-ES_tradnl"))}");
    }

    private static int GetNumber()
    {
        int number;
        const int minNumber = 0;
        const int maxNumber = 1000000000;
        do
        {
            Console.Write("Enter the number from zero to billion:  ");
        }
        while (!(int.TryParse(Console.ReadLine(), out number) && (number >= minNumber) && (number <= maxNumber)));
        return number;
    }

    private static string GetGender()
    {
        string[] allGenders = ["m", "f"];
        string gender;
        do
        {
            Console.Write("Enter the used gender \"m\" or \"f\":  ");
            gender = Console.ReadLine() ?? string.Empty;
        }
        while (!allGenders.Contains(gender));
        return gender;
    }

    private static string GetSpanishNameOfNumber(int number, string gender)
    {
        const int separatorNumberPeriod = 1000;

        string nameNumberUnits = GetNameNumberUnits(number % separatorNumberPeriod, gender, true, false);
        string nameNumberThousands = GetNameNumberThousands((number / separatorNumberPeriod) % separatorNumberPeriod, gender);
        string nameNumberMillions = GetNameNumberMillions(number / separatorNumberPeriod / separatorNumberPeriod, gender);
        string spanishNameOfNumber = $"{nameNumberMillions} {nameNumberThousands} {nameNumberUnits}";

        if (number % separatorNumberPeriod == 0 && number > 0)
        {
            spanishNameOfNumber = $"{nameNumberMillions} {nameNumberThousands}";
        }

        return RemoveDoublicateSpace(spanishNameOfNumber);
    }

    private static string GetNameNumberUnits(int number, string gender, bool isUnits, bool isMillions)
    {
        const int separatorNumberPeriod = 100;
        if (number % separatorNumberPeriod == 0 && number > 0)
        {
            return GetNameOfHundreds(number / separatorNumberPeriod, gender, isMillions, number / separatorNumberPeriod == 1);
        }

        return $"{GetNameOfHundreds(number / separatorNumberPeriod, gender, isMillions, false)}" +
            $" {GetNameTwoDigitNumber(number % separatorNumberPeriod, gender, isUnits, isMillions)}";
    }

    private static string GetNameTwoDigitNumber(int number, string gender, bool isUnits, bool isMillions)
    {
        string[] listTenName = { "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        string[] listTwoDigitNumberUpTwentyName = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciseis", "diecisiete", "dieciocho", "diecinueve" };
        if (number < 10)
        {
            return GetListNamesOfDigits(gender, isUnits, isMillions)[number];
        }

        if (number < 20)
        {
            return listTwoDigitNumberUpTwentyName[number - 10];
        }

        if (number % 10 == 0)
        {
            return listTenName[(number / 10) - 2];
        }

        if (number < 30)
        {
            return $"{listTenName[0][0..5]}i{GetListNamesOfDigits(gender, isUnits, isMillions)[number % 10]}";
        }

        return $"{listTenName[(number / 10) - 2]} y {GetListNamesOfDigits(gender, isUnits, isMillions)[number % 10]}";
    }

    private static string GetNameOfHundreds(int number, string gender, bool isMillions, bool isOneHundred)
    {
        string hundred = isOneHundred ? "cien" : "ciento";
        string[] beginnigOfName = { "doscient", "trescient", "cuatrocient", "quinient", "seiscient", "setecient", "ochocient", "novecient" };
        string endName = gender == "f" && !isMillions ? "as" : "os";
        return number <= 1 ? hundred[0..(hundred.Length * number)] : $"{beginnigOfName[number - 2]}{endName}";
    }

    private static string GetNameNumberThousands(int number, string gender)
    {
        const string thousand = "mil";
        return number <= 1 ? thousand[0..(thousand.Length * number)] : $"{GetNameNumberUnits(number, gender, false, false)} {thousand}";
    }

    private static string GetNameNumberMillions(int number, string gender)
    {
        const string million = "millon";
        if (number <= 1)
        {
            return number == 1 ? $"{GetNameNumberUnits(number, gender, false, true)} {million}" : string.Empty;
        }

        return number == 1000 ? $"mil {million}es" : $"{GetNameNumberUnits(number, gender, false, true)} {million}es";
    }

    private static string RemoveDoublicateSpace(string incorrectString)
    {
        return incorrectString.Trim()
            .Replace("  ", " ", StringComparison.Ordinal)
            .Replace("  ", " ", StringComparison.Ordinal);
    }

    private static string[] GetListNamesOfDigits(string gender, bool isUnits, bool isMillions)
    {
        string one = !isMillions && gender == "f" ? "una" : isUnits ? "uno" : "un";
        return ["cero", one, "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve"];
    }
}
