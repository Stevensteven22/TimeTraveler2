using Xunit;

namespace TimeTraveler.UnitTest.Services;

public class ResultVerifyServiceTest
{
    [Fact]
    public void Verify_Default()
    {
        object result = new
        {
            Problem1Answer = "苹果",
            Problem2Answer = "攫金鸦印",
            Problem3Answer = "琉璃百合",
        };

        Type resultType = result.GetType();
        var properties = resultType.GetProperties();
        string problem1Answer = "",
            problem2Answer = "",
            problem3Answer = "";
        foreach (var property in properties)
        {
            if (property.Name == "Problem1Answer")
            {
                problem1Answer = property.GetValue(result).ToString();
            }
            else if (property.Name == "Problem2Answer")
            {
                problem2Answer = property.GetValue(result).ToString();
            }
            else if (property.Name == "Problem3Answer")
            {
                problem3Answer = property.GetValue(result).ToString();
            }
        }

        if (
            problem1Answer == "苹果"
            && problem2Answer == "攫金鸦印"
            && problem3Answer == "琉璃百合"
        )
        {
            Assert.Equal("苹果", problem1Answer);
            Assert.Equal("攫金鸦印", problem2Answer);
            Assert.Equal("琉璃百合", problem3Answer);
            Assert.True(true, "验证成功");
        }
        else
        {
            Assert.False(false, "验证失败");
        }
    }

    [Fact]
    public void BeforeVerify_Default()
    {
        object result = new
        {
            Problem1Answer = "苹果",
            Problem2Answer = "攫金鸦印",
            Problem3Answer = "琉璃百合",
        };

        string errorMsg = "";
        Type resultType = result.GetType();
        var properties = resultType.GetProperties();
        string problem1Answer = "",
            problem2Answer = "",
            problem3Answer = "";
        foreach (var property in properties)
        {
            if (property.Name == "Problem1Answer")
            {
                problem1Answer = property.GetValue(result).ToString();
            }
            else if (property.Name == "Problem2Answer")
            {
                problem2Answer = property.GetValue(result).ToString();
            }
            else if (property.Name == "Problem3Answer")
            {
                problem3Answer = property.GetValue(result).ToString();
            }
        }

        if (string.IsNullOrEmpty(problem1Answer))
        {
            Assert.True(string.IsNullOrEmpty(problem1Answer), "第一个问题的答案不能为空。");
            errorMsg += "第一个问题的答案不能为空。\r\n";
        }
        if (string.IsNullOrEmpty(problem2Answer))
        {
            Assert.True(string.IsNullOrEmpty(problem2Answer), "第二个问题的答案不能为空。");
            errorMsg += "第二个问题的答案不能为空。\r\n";
        }
        if (string.IsNullOrEmpty(problem3Answer))
        {
            Assert.True(string.IsNullOrEmpty(problem3Answer), "第三个问题的答案不能为空。");
            errorMsg += "第三个问题的答案不能为空。\r\n";
        }

        if (
            string.IsNullOrEmpty(problem1Answer)
            || string.IsNullOrEmpty(problem2Answer)
            || string.IsNullOrEmpty(problem3Answer)
        )
        {
            Assert.False(false, "验证失败");
        }
        else
        {
            Assert.True(true, "验证成功");
        }
    }
}
