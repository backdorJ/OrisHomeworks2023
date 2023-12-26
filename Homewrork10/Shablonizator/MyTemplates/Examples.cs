using System.Text.RegularExpressions;

namespace MyTemplates;

public class Examples
{
    public static string FirstExample(string name)
        => "Здравствуйте, @{name}, вы отчислены".Replace("@{name}", name);

    public string SecondExample(object obj)
    {
        string templateString = "Здравствуйте @{name} вы прописаны по адресу @{address}";

        string pattern = "@{([^}]*)}";
        var regex = new Regex(pattern);

        var matches = regex.Matches(templateString);

        var extractedValues = new Dictionary<string, object>();

        foreach (Match match in matches)
        {
            var extractedValue = match.Groups[1].Value;
            var value = obj.GetType()?.GetProperty(extractedValue.Replace(extractedValue[0].ToString(),
                extractedValue[0].ToString().ToUpper()))?.GetValue(obj);
            
            extractedValues[extractedValue] = value;
        }

        return FormatTemplate(templateString, extractedValues);
    }

    public string ThirdExample(object? obj)
    {
        var templateString = "Здравствуйте, @{if(temperature >= 37)} @then{Выздоравливайте} @else{Прогульщица}";

        var pattern = @"@{if\(([^}]*)\)} @then{([^}]*)} @else{([^}]*)}";
        var regex = new Regex(pattern);

        var match = regex.Match(templateString);

        if (match.Success)
        {
            var condition = match.Groups[1].Value.Trim();
            var thenValue = match.Groups[2].Value.Trim();
            var elseValue = match.Groups[3].Value.Trim();

            if (obj is not null && obj.GetType().GetProperty("temperature") is not null)
            {
                var temperatureValue = (int)obj.GetType().GetProperty("temperature").GetValue(obj);
                var isConditionTrue = temperatureValue >= 37;

                return isConditionTrue ? thenValue : elseValue;
            }
        }
        
        return templateString;
    }

    public string FourExample(object obj)
    {
        var stringTemplate = "Здравствуйте, студенты группы @{group}." +
                             " \nВаши баллы по ОРИС:\n @for(item in students) {@{item.FIO} баллы: @{item.grade}}";

        string pattern = @"@{([^}]*)}";
        var regex = new Regex(pattern);

        Match match = regex.Match(stringTemplate);

        string group = null;
        if (match.Success)
        {
            group = match.Groups[1].Value;
        }

        var students = obj.GetType().GetProperty("students")?.GetValue(obj) as IEnumerable<object>;

        if (group != null && students != null)
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine($"Здравствуйте, студенты группы {group}. \nВаши баллы по ОРИС:");

            foreach (var student in students)
            {
                var fio = student.GetType().GetProperty("FIO")?.GetValue(student);
                var grade = student.GetType().GetProperty("grade")?.GetValue(student);

                if (fio != null && grade != null)
                {
                    result.AppendLine($"{fio} баллы: {grade}");
                }
            }

            return result.ToString();
        }

        return stringTemplate;
    }
    
    
    private string FormatTemplate(string template, Dictionary<string, object> values)
    {
        return values.Aggregate(template, 
            (current, pair) 
                => current.Replace("@{" + pair.Key + "}", pair.Value?.ToString() ?? ""));
    }
}