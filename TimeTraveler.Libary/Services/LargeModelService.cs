using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RestSharp;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services;

public class LargeModelService : ILargeModelService
{
    const string API_KEY = "dCa7niTOlzuoTDpp99I6Lgh6";
    const string SECRET_KEY = "eRlq4eQ1PfFerHCKgHHX35oUZyXMefl8";

    private const string TokenUrl = "https://aip.baidubce.com/oauth/2.0/token";
    private const string ApiUrl = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/completions_pro";

    // 获取 AccessToken
    public string GetAccessToken()
    {
        var client = new RestClient(TokenUrl);
        var request = new RestRequest(Method.POST);
        request.AddParameter("grant_type", "client_credentials");
        request.AddParameter("client_id", API_KEY);
        request.AddParameter("client_secret", SECRET_KEY);

        IRestResponse response = client.Execute(request);
        if (!response.IsSuccessful)
        {
            throw new Exception($"获取AccessToken失败: {response.ErrorMessage}");
        }

        var result = JsonConvert.DeserializeObject<dynamic>(response.Content);
        return result.access_token.ToString();
    }

    // 生成谜题
    public Puzzle GeneratePuzzle(string theme)
    {
        var accessToken = GetAccessToken();
        var client = new RestClient($"{ApiUrl}?access_token={accessToken}");
        var request = new RestRequest(Method.POST);
        request.AddHeader("Content-Type", "application/json");

        var body = new
        {
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = $"根据主题'{theme}'生成一个谜题，并给出三个选项，格式为A:选项一 B：选项二 C：选项三 正确答案：  解释： "
                }
            }
        };

        request.AddJsonBody(body);
        IRestResponse response = client.Execute(request);

        if (!response.IsSuccessful)
        {
            throw new Exception($"生成谜题失败: {response.ErrorMessage}");
        }

        return ParseResponse(response.Content);
    }

    // 解析 API 返回的 JSON 并返回 Puzzle 对象
    static Puzzle ParseResponse(string responseContent)
{
    try
    {
        // 假设返回的数据结构中含有 result 字段
        var response = JsonConvert.DeserializeObject<dynamic>(responseContent);

        // 提取谜题数据
        string result = response.result;

        // 分解返回内容，假设格式为：
        // 谜题描述 A:选项1 B:选项2 C:选项3 正确答案: B 解释: ...
        string[] parts = result.Split(new string[] { "正确答案：" }, StringSplitOptions.None);
        string questionAndOptions = parts[0].Trim();
        string correctAnswerAndExplanation = parts.Length > 1 ? parts[1].Trim() : "无解释";

        // 提取正确答案和解释
        string correctAnswer = ExtractCorrectAnswer(correctAnswerAndExplanation);
        string explanation = ExtractExplanation(correctAnswerAndExplanation, correctAnswer);

        // 提取问题和选项
        string question = ExtractQuestion(questionAndOptions);
        string[] options = ExtractOptions(questionAndOptions);

        // 创建并返回 Puzzle 对象
        return new Puzzle
        {
            Question = question,
            Options = options,
            CorrectAnswer = correctAnswer,
            explanation = explanation
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine($"解析返回数据时发生错误：{ex.Message}");
        return null;
    }
}

// 提取选项的方法
static string[] ExtractOptions(string questionAndOptions)
{
    // 假设选项的格式为 "A:选项1 B:选项2 C:选项3"
    var optionMatches = Regex.Matches(questionAndOptions, @"[A-Z]:[^A-Z]+(?=\s[A-Z]:|$)");
    return optionMatches.Cast<Match>().Select(m => m.Value.Trim()).ToArray();
}

// 提取问题的方法
static string ExtractQuestion(string questionAndOptions)
{
    // 假设问题在选项之前，用正则提取选项部分之前的内容
    var match = Regex.Match(questionAndOptions, @"^(.*?)(?=\s[A-Z]:)");
    return match.Success ? match.Groups[1].Value.Trim() : questionAndOptions;
}

// 提取正确答案的方法
static string ExtractCorrectAnswer(string correctAnswerAndExplanation)
{
    // 正确答案一般是冒号后的单个字母
    var match = Regex.Match(correctAnswerAndExplanation, @"^[A-Z]");
    return match.Success ? match.Value.Trim() : "";
}

// 提取解释的方法
static string ExtractExplanation(string correctAnswerAndExplanation, string correctAnswer)
{
    // 解释是正确答案之后的部分
    return correctAnswerAndExplanation.Substring(correctAnswer.Length).Trim();
}
        
}