﻿namespace NetPOC.Backend.Domain
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }
    
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}