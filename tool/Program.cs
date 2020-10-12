using HGV.Basilius;
using HGV.Basilius.Client;
using System;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HGV.Hyperstone.Images
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var metaClient = new MetaClient();
            var heroes = metaClient.GetHeroes();
            var abilities = metaClient.GetAbilities();
            var items = metaClient.GetItems();

            Directory.CreateDirectory(@"images\heroes\banner");
            Directory.CreateDirectory(@"images\heroes\profile");
            Directory.CreateDirectory(@"images\abilities");
            Directory.CreateDirectory(@"images\items");
            
            foreach (var hero in heroes)
            {
                try
                {
                    var img = hero.Key.Replace("npc_dota_hero_", "");
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/{img}_vert.jpg?v=6120190";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\heroes\profile\{hero.Id}.jpg"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to Download Hero profile {hero.Name} ");
                }
            }

            foreach (var hero in heroes)
            {
                try
                {
                    var img = hero.Key.Replace("npc_dota_hero_", "");
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/{img}_full.png?v=6120190";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\heroes\banner\{hero.Id}.jpg"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception)
                {
                     Console.WriteLine($"Failed to Download hero banner {hero.Name} ");
                }
            }

            foreach (var ability in abilities)
            {
                try
                {
                    var img = ability.Key;
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/abilities/{img}_hp1.png?v=6120190";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\abilities\{ability.Id}.jpg"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception)
                {
                     Console.WriteLine($"Failed to Download Ability {ability.Name}");
                }
            }

            foreach (var item in items)
            {
                try
                {
                    var img = item.Key.Replace("item_", "");
                    var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/{img}_lg.png";
                    var stream = await httpClient.GetStreamAsync(url);
                    using (var fileStream = File.Create($@"images\items\{item.Id}.jpg"))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                catch (Exception)
                {
                     Console.WriteLine($"Failed to Download Item {item.Name}");
                }
            }
        }
    }
}
