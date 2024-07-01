﻿namespace App.UI.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<ErrorResponseItem> Errors { get; set; }
    }

    public class ErrorResponseItem
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
