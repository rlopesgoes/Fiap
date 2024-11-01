using System.Text;

namespace Fiap_01;

public class Program
{
    private static readonly HttpClient Client = new();

    static async Task Main(string[] args)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        var numbers = "0123456789".ToCharArray();

        var combinations = GetCombinations(chars, numbers).ToList();

        var tasks = combinations.Select(async combination => {
            Console.WriteLine($"Testing combination: {combination}");
            var response = await SendRequest(combination);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Valid combination found: {combination}");
            }
        });

        await Task.WhenAll(tasks);

        Console.WriteLine("All tasks completed.");
    }

    static IEnumerable<string> GetCombinations(char[] chars, char[] numbers)
    {
        var combinations = new List<string>();

        foreach (var firstChar in chars)
        {
            foreach (var secondNum in numbers)
            {
                foreach (var thirdNum in numbers)
                {
                    foreach (var lastChar in chars)
                    {
                        combinations.Add($"{firstChar}{secondNum}{thirdNum}{lastChar}");
                    }
                }
            }
        }

        return combinations;
    }

    private static async Task<HttpResponseMessage> SendRequest(string combination)
    {
        var url = "https://fiapnet.azurewebsites.net/fiap";
        var json = $"{{\"Key\": \"{combination}\", \"grupo\":\"room4\"}}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(url, content);
        return response;
    }
}