
using ProjectMars_AdvanceTask_NUnit.Models;
using RazorEngine;
using System.Text.Json;

namespace ProjectMars_AdvanceTask_NUnit.Helpers
{
    public static class TestDataHelper
    {
        public static TestUserData GetUserData(string key)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Credential");
            string filePath = Path.Combine(basePath, $"{key}.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Test data file not found at: {filePath}");

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<TestUserData>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return data ?? throw new Exception($"Failed to deserialize test data for key: {key}");
        }


        public static SingleFieldTestData GetSingleFieldTestData(string folder, string key)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", folder);
            string filePath = Path.Combine(basePath, $"{key}.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Test data file not found at: {filePath}");

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<SingleFieldTestData>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return data ?? throw new Exception($"Failed to deserialize test data for key: {key}");
        }

        public static Languages GetLanguageData(string key)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Language");
            string filePath = Path.Combine(basePath, $"{key}.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Language test data file not found at: {filePath}");

            string json = File.ReadAllText(filePath);
            var languageData = JsonSerializer.Deserialize<Languages>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return languageData ?? throw new Exception($"Failed to deserialize language data for key: {key}");
        }

        public static Skill GetSkillData(string key)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Skill");
            string filePath = Path.Combine(basePath, $"{key}.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Skill test data file not found at: {filePath}");

            string json = File.ReadAllText(filePath);
            var skill = JsonSerializer.Deserialize<Skill>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return skill ?? throw new Exception($"Failed to deserialize skill data for key: {key}");
        }
     
        public static ShareSkillModel GetShareSkillData(string key)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "ShareSkill");
            string filePath = Path.Combine(basePath, $"{key}.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Certification test data file not found at: {filePath}");

            string json = File.ReadAllText(filePath);
            var shareSkill = JsonSerializer.Deserialize<ShareSkillModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return shareSkill ?? throw new Exception($"Failed to deserialize ShareSkill data for key: {key}");
        }

        public static SearchSkill GetSearchSkillData(string key)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "SearchSkill");
            string filePath = Path.Combine(basePath, $"{key}.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Search Share Skill test data file not found at: {filePath}");

            string json = File.ReadAllText(filePath);
            var searchSkill = JsonSerializer.Deserialize<SearchSkill>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return searchSkill ?? throw new Exception($"Failed to deserialize Search Skill parameter data for key: {key}");
        }

        public static NotificationData GetNotificationData(string key)
            {
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Notification");
                string filePath = Path.Combine(basePath, $"{key}.json");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Notification test data file not found at: {filePath}");

                string json = File.ReadAllText(filePath);
                var notificationData = JsonSerializer.Deserialize<NotificationData>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return notificationData ?? throw new Exception($"Failed to deserialize Notification data for key: {key}");
            }
        




    }
}
