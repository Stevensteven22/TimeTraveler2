using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services
{
    public interface ILargeModelService
    {
        string GetAccessToken();
        Puzzle GeneratePuzzle(string theme);
    }
}
