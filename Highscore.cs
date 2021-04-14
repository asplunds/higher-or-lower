using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace HigherOrLower
{
    public class Highscore
    {
        private List<HighscoreData> data;
        private readonly string location;
        public Highscore(string location)
        {
            this.location = location;

            // Try to read previous data
            try
            {
                var stored = JsonConvert.DeserializeObject<FinalData>(File.ReadAllText(location));

                // Set the RAM highscore to the file highscore
                data = stored.Highscores;
            } catch(Exception e)
            {
                data = new List<HighscoreData>();
            }
        }

        // Appends a new entry into the highscore data
        public Highscore AddData(string name, int score)
        {
            HighscoreData hsData = new HighscoreData
            {
                Name = name,
                Score = score
            };

            data.Add(hsData);

            return this;
        }

        public Highscore WriteData()
        {
            FinalData finalData = new FinalData
            {
                Highscores = data
            };

            File.WriteAllText(location, JsonConvert.SerializeObject(finalData).ToString());

            return this;
        }

        public Highscore Display()
        {
            new Menu("Highscores")
                .DisplayBox();

            // Sort entries by score
            data.Sort((x, y) => y.Score - x.Score);

            // Get the longest name
            var dataSortedByNameLength = data
                .Select((x, i) => new KeyValuePair<HighscoreData, int>(x, i))
                .OrderBy(x => x.Key.Name.Length)
                .ToList();

            // List all sorted entries
            for (int i = 0; i < data.Count; i++)
            {
                var entry = data[i];
                
                Console.WriteLine($"{i + 1}. {data[i].Name}: {data[i].Score}");
            }
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();

            return this;
        }
    }
    class FinalData
    {
        public List<HighscoreData> Highscores { get; set; }
    }
    public class HighscoreData
    {
        public int Score { get; set; }
        public string Name { get; set; }
    }
}
