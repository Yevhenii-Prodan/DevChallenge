﻿using System.Collections.Generic;

namespace Sc.DevChallenge.Application.Models.ResultModels
{
    public class BaseErrorModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
    }
}