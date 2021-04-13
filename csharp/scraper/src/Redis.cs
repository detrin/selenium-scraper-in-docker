using System;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace scraper
{
    public class RedisConnectionFactory
    {
        private static readonly Lazy<ConnectionMultiplexer> Connection;
 
        private static readonly string REDIS_CONNECTIONSTRING = "redis";
 
        static RedisConnectionFactory()
        {
 
            var options = ConfigurationOptions.Parse(REDIS_CONNECTIONSTRING);
 
            Connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
        }
 
        public static ConnectionMultiplexer GetConnection() => Connection.Value;
    }

    public class RedisDataAgent
    {
        private static IDatabase _database;
        public RedisDataAgent()
        {
            var connection = RedisConnectionFactory.GetConnection();
 
            _database = connection.GetDatabase();
        }
 
        public string GetStringValue(string key)
        {
            return _database.StringGet(key);
        }
 
        public void SetStringValue(string key, string value)
        {
            _database.StringSet(key, value);
        }
 
        public void DeleteStringValue(string key)
        {
            _database.KeyDelete(key);
        }
    }
}
